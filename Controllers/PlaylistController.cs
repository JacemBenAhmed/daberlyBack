using DaberlyProjet.Data;
using DaberlyProjet.DTO;
using DaberlyProjet.Models;
using DaberlyProjet.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DaberlyProjet.Services;
[ApiController]
[Route("api/[controller]")]
public class PlaylistController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly PlaylistService _playlistService;

    public PlaylistController(AppDbContext context , PlaylistService playlistService)
    {
        _context = context;
        _playlistService = playlistService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Playlist>>> GetPlaylists()
    {
        var playlists = await _context.Playlists
            .Include(p => p.Videos)
            .Select(p => new Playlist
            {
                Id = p.Id,
                Titre = p.Titre,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                Videos = p.Videos.Select(v => new Video
                {
                    Id = v.Id,
                    Titre = v.Titre,
                    Description = v.Description,
                    VideoUrl = v.VideoUrl
                }).ToList()
            })
            .ToListAsync();

        return Ok(playlists);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<PlaylistDTO>> GetPlaylist(int id)
    {
        var playlist = await _context.Playlists
            .Include(p => p.Videos)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (playlist == null)
            return NotFound();

        var playlistData = new Playlist
        {
           
            Titre = playlist.Titre,
            Description = playlist.Description,
            ImageUrl = playlist.ImageUrl,
            Videos = playlist.Videos.Select(v => new Video
            {
                Id=v.Id,
                Titre = v.Titre,
                Description = v.Description,
                VideoUrl = v.VideoUrl
            }).ToList()
        };

        return Ok(playlistData);
    }


    [HttpPost]
    public async Task<ActionResult<Playlist>> CreatePlaylist([FromForm]  PlaylistDTO playlistDTO , IFormFile file)
    {
        
        if(playlistDTO == null)
        {
            return BadRequest("playlist null");

        }

        if(file == null)
        {
            return BadRequest("file null");
        }

        if (await _playlistService.DetermineFileType(file) == "Vidéo")
        {
            return BadRequest("file must be image !");
        }

        var playlist = new Playlist
        {
            Titre = playlistDTO.Titre,
            Description = playlistDTO.Description,
            ImageUrl = await _playlistService.UploadPlaylistImage(file)
        };

        _context.Playlists.Add(playlist);
        await _context.SaveChangesAsync();
        return Ok(playlist);


    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePlaylist(int id, [FromBody] PlaylistDTO updatedDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var playlist = await _context.Playlists.FindAsync(id);

        if (playlist == null)
            return NotFound("Playlist non trouvée");

        playlist.Titre = updatedDto.Titre;
        playlist.Description = updatedDto.Description;

        await _context.SaveChangesAsync();

        return Ok("Playlist mise à jour avec succès");
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlaylist(int id)
    {
        var playlist = await _context.Playlists.FindAsync(id);
        if (playlist == null)
            return NotFound();

        _context.Playlists.Remove(playlist);

        _playlistService.DeleteImageFileByName(playlist.ImageUrl);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
