using MyTestProgForPractice.Models;

namespace MyTestProgForPractice.Classes
{
    public class CsvParser
    {
        public async Task<List<Value>> ParseF(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new Exception("Файл пустой.");
        }
    }
}
