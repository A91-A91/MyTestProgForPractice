using Microsoft.AspNetCore.Mvc;
using MyTestProgForPractice.Services;

namespace MyTestProgForPractice.Controllers
{
    [ApiController]
    [Route("api/results")]
    public class ResultsController : ControllerBase
    {
        private readonly Operations_DB resultService;

        public ResultsController(Operations_DB _resultService)
        {
            resultService = _resultService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null)
                return BadRequest("Файл не выбран.");

            await resultService.UploadCsv(file);

            return Ok("Файл успешно обработан.");
        }
    }
}
