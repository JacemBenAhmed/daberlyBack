namespace DaberlyProjet.Services
{
    public class PhotoUserService
    {
        public async Task<(string FCinPath, string BCinPath)> UploadFiles(IFormFile fCINFile, IFormFile bCINFile)
        {
            if (fCINFile == null || fCINFile.Length == 0 || bCINFile == null || bCINFile.Length == 0)
            {
                throw new ArgumentException("One or both files were not uploaded");
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fCINFileName = Path.GetFileName(fCINFile.FileName);
            var bCINFileName = Path.GetFileName(bCINFile.FileName);

            var fCINPath = Path.Combine(uploadsFolder, fCINFileName);
            var bCINPath = Path.Combine(uploadsFolder, bCINFileName);

            using (var fCINStream = new FileStream(fCINPath, FileMode.Create))
            {
                await fCINFile.CopyToAsync(fCINStream);
            }

            using (var bCINStream = new FileStream(bCINPath, FileMode.Create))
            {
                await bCINFile.CopyToAsync(bCINStream);
            }

            return (fCINFileName, bCINFileName);
        }
    }
}
