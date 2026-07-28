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
            try
            {
                if (file == null)
                    return BadRequest("Файл не выбран.");

                await operation.UploadCsv(file);

                return Ok("Файл успешно обработан.");
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    error = ex.Message
                });
            }
        }

        [HttpPost("filter")]
        public async Task<IActionResult> FilterResults(ResultDTO filter)
        {
            try
            {
                var results = await operation.GetResults(filter);
                if (!results.Any()) { return NotFound("Нет подходящих под условия данных!"); }
                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    error = ex.Message
                });
            }
        }
    }
}
