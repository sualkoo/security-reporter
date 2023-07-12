using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers
{
    public class SearchController : Controller
    {

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {

            // Process or save the uploaded file as needed

            return Ok("File uploaded successfully.");
        }
    }
}
