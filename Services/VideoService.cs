namespace DaberlyProjet.Services
{
    public class VideoService
    {
        private readonly IWebHostEnvironment _env;

        public VideoService(IWebHostEnvironment env)
        {
            _env = env;
        }


        public  void DeleteVideoFileByName(string fileName)
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
