using Google.Cloud.SecretManager.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace gcp_net_sample.Controllers
{
    public class SecretManagerController : Controller
    {
        private IConfiguration _configuration;

        public SecretManagerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> IndexAsync()
        {
            try
            {
                string projectid = _configuration["projectid"];
                string secretId = _configuration["secretId"];
                string secretVersionId = _configuration["secretVersionId"];

                Console.WriteLine($"projectid {projectid}");
                Console.WriteLine($"secretId {secretId}");
                Console.WriteLine($"secretVersionId {secretVersionId}");

                SecretManagerServiceClient client = SecretManagerServiceClient.Create();
                SecretVersionName secretVersionName = new SecretVersionName(projectid, secretId, secretVersionId);
                AccessSecretVersionResponse result = client.AccessSecretVersion(secretVersionName);
                return Content(result.Payload.Data.ToStringUtf8());
            }
            catch (Exception ex)
            {
                return Content(ex.StackTrace);
            }
        }
    }
}
