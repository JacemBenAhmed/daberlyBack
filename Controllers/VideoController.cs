using DaberlyProjet.Data;
using DaberlyProjet.DTO;
using DaberlyProjet.Models;
using DaberlyProjet.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class VideoController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly PlaylistService _playlistService;
    private readonly VideoService _videoService;
    public VideoController(AppDbContext context , PlaylistService playlist , VideoService videoService)
    {
        _context = context;
        _playlistService = playlist;
        _videoService = videoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Video>>> GetVideos()
    {
        var videos = await _context.Videos.ToListAsync();
        return Ok(videos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Video>> GetVideo(int id)
    {
        var video = await _context.Videos.FindAsync(id);
        if (video == null)
            return NotFound();

        return Ok(video);
    }

    [HttpPost]
    public async Task<ActionResult<Video>> AddVideo([FromForm] VideoDTO videoDTO , IFormFile file)
    {

        if(videoDTO == null)
        {
            return BadRequest("video null");
        }

        if(await _playlistService.DetermineFileType(file) == "Image")
        {
            return BadRequest("file must be Video");
        }


        var playlistExists = await _context.Playlists.AnyAsync(p => p.Id == videoDTO.PlaylistId);

        if (!playlistExists)
            return BadRequest("Playlist not found.");


        var video = new Video
        {
            Titre = videoDTO.Titre,
            Description = videoDTO.Description,
            VideoUrl = await _playlistService.UploadPlaylistImage(file),
            PlaylistId=videoDTO.PlaylistId
        };

        _context.Videos.Add(video);


        await _context.SaveChangesAsync();
        return Ok(video);
    }

    [HttpPut("{playlistId}/videos/{videoId}")]
    public async Task<IActionResult> UpdateVideo(int playlistId, int videoId, [FromForm] VideoDTO videoDto, IFormFile? file)
    {
        var playlist = await _context.Playlists
            .Include(p => p.Videos)
            .FirstOrDefaultAsync(p => p.Id == playlistId);

        if (playlist == null)
            return NotFound("Playlist introuvable");

        var video = playlist.Videos.FirstOrDefault(v => v.Id == videoId);
        if (video == null)
            return NotFound("Vidéo introuvable");

        bool isModified = false;

        if (video.Titre != videoDto.Titre)
        {
            video.Titre = videoDto.Titre;
            isModified = true;
        }

        if (video.Description != videoDto.Description)
        {
            video.Description = videoDto.Description;
            isModified = true;
        }

        if (file != null && file.Length > 0)
        {
            

            var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PlaylistVideos", video.VideoUrl);

            if (System.IO.File.Exists(oldPath))
            {
                System.IO.File.Delete(oldPath);
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PlaylistImages");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            //var newFileName = Path.GetFileName(file.FileName);

            var originalName = Path.GetFileNameWithoutExtension(file.FileName);
            var extension = Path.GetExtension(file.FileName);
            var uniqueSuffix = Guid.NewGuid().ToString(); 
            var uniqueFileName = $"{originalName}_{uniqueSuffix}{extension}";



            var newPath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var stream = new FileStream(newPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            video.VideoUrl = uniqueFileName;
            isModified = true;
        }

        if (isModified)
        {
            await _context.SaveChangesAsync();
            return Ok("Vidéo mise à jour");
        }

        return Ok("Aucun changement détecté");
    }



    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVideo(int id)
    {
        var video = await _context.Videos.FindAsync(id);
        if (video == null)
            return NotFound();

         _videoService.DeleteVideoFileByName(video.VideoUrl);

        _context.Videos.Remove(video);
        await _context.SaveChangesAsync();

        return NoContent();
    }


}
