using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using Google.Cloud.Storage.V1;
using System.Text;
using Google;
using gcp_net_sample.ViewModels;
using Microsoft.Extensions.Configuration;

namespace gcp_net_sample.Controllers
{
    public class CloudStorageController : Controller
    {
        private readonly StorageClient _storage;
        private readonly IConfiguration _configuration;

        public CloudStorageController(IConfiguration configuration)
        {
            _storage = StorageClient.Create();
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new CloudStorageViewModel();

            try
            {

                //await _storage.DeleteObjectAsync(
                //     _configuration["bucketName"], _configuration["objectName"]);

                // Get the storage object.
                var storageObject =
                    await _storage.GetObjectAsync(_configuration["bucketName"], _configuration["objectName"]);

                // Download the storage object.
                MemoryStream m = new MemoryStream();
                await _storage.DownloadObjectAsync(
                     _configuration["bucketName"], _configuration["objectName"], m);
                m.Seek(0, SeekOrigin.Begin);
                byte[] content = new byte[m.Length];
                m.Read(content, 0, content.Length);
                model.Content = Encoding.UTF8.GetString(content);
            }
            catch (GoogleApiException e)
            when (e.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
            {
                // Does not exist yet.  No problem.
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(Form sendForm)
        {
            var model = new CloudStorageViewModel();
            // Take the content uploaded in the form and upload it to
            // Google Cloud Storage.

            await _storage.UploadObjectAsync(
                 _configuration["bucketName"], _configuration["objectName"], "text/plain",
                new MemoryStream(Encoding.UTF8.GetBytes(sendForm.Content)));


            model.Content = sendForm.Content;
            model.SavedNewContent = true;

            return View(model);
        }
    }
}