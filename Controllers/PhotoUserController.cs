using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DaberlyProjet.Services;
using DaberlyProjet.Models;  
using DaberlyProjet.Data;  
using System.Threading.Tasks;
using DaberlyProjet.DTO;

namespace DaberlyProjet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoUserController : ControllerBase
    {
        private readonly PhotoUserService _photoUserService;
        private readonly AppDbContext _context;

        public PhotoUserController(PhotoUserService photoUserService, AppDbContext context)
        {
            _photoUserService = photoUserService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> UploadPhotos([FromForm] PhotoUserDTO dto)
        {
            try
            {
                var (fCINFileName, bCINFileName) = await _photoUserService.UploadFiles(dto.FCin, dto.BCin);

                var photoUser = new PhotoUser
                {
                    IdUser = dto.UserId,
                    FCin = fCINFileName,
                    BCin = bCINFileName
                };

                _context.PhotosUser.Add(photoUser);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Files uploaded successfully", data = photoUser });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error uploading files", error = ex.Message });
            }
        }
    }
}
