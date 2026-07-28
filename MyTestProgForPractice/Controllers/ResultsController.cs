using Microsoft.AspNetCore.Mvc;
using MyTestProgForPractice.DTO;
using MyTestProgForPractice.Services;

namespace MyTestProgForPractice.Controllers
{
    [ApiController]
    [Route("api/results")]
    public class ResultsController : ControllerBase
    {
        private readonly Operations_DB operation;

        public ResultsController(Operations_DB _operation)
        {
            operation = _operation;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null)
                return BadRequest("Файл не выбран.");

            await operation.UploadCsv(file);

            return Ok("Файл успешно обработан.");
        }

        [HttpPost("filter")]
        public async Task<IActionResult> FilterResults(ResultDTO filter)
        {
            var results = await operation.GetResults(filter);

            return Ok(results);
        }
    }
}
