namespace DaberlyProjet.Services
{
    public class AlbumService
    {
        public async Task<string> UploadFiles(IFormFile file)
        {
            if (file == null )
            {
                throw new ArgumentException("One or both files were not uploaded");
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

           
            var fileName = Path.GetFileName(file.FileName);

           
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var fCINStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fCINStream);
            }

           

            return fileName;
        }
    }
}
