namespace DaberlyProjet.Services
{
    public class PlaylistService
    {
        private readonly IWebHostEnvironment _env;

        public PlaylistService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> UploadPlaylistImage(IFormFile file)
        {
            if (file == null)
            {
                throw new ArgumentException("One or both files were not uploaded");
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PlaylistImages");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }


            var originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
            var extension = Path.GetExtension(file.FileName);
            var uniqueSuffix = Guid.NewGuid().ToString(); 

            var uniqueFileName = $"{originalFileName}_{uniqueSuffix}{extension}";



            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }



            return uniqueFileName;
        }

        public async Task<string> DetermineFileType(IFormFile file)
        {
            var imageMimeTypes = new List<string> { "image/jpeg", "image/png", "image/gif", "image/bmp" };
            var videoMimeTypes = new List<string> { "video/mp4", "video/avi", "video/mkv", "video/webm" };

            var mimeType = file.ContentType;

            if (imageMimeTypes.Contains(mimeType))
            {
                return "Image";
            }
            else if (videoMimeTypes.Contains(mimeType))
            {
                return "Vidéo";
            }
            else
            {
                return "Type de fichier non pris en charge";
            }
        }

        public void DeleteImageFileByName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return;

            var folderPath = Path.Combine(_env.WebRootPath, "PlaylistImages");
            var filePath = Path.Combine(folderPath, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
