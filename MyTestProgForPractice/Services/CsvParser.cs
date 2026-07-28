using MyTestProgForPractice.Models;
using System.Globalization;

namespace MyTestProgForPractice.Classes
{
    public class CsvParser
    {
        public async Task<List<Value>> ParseF(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new Exception("Файл пустой.");

            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);

            var header = await reader.ReadLineAsync();

            if (header != "Date;ExecutionTime;Value")
                throw new Exception("Неверный формат заголовка CSV.");

            var values = new List<Value>();

            string? line;
            int rowNumber = 1;

            while ((line = await reader.ReadLineAsync()) != null)
            {
                rowNumber++;

                if (string.IsNullOrWhiteSpace(line))
                    throw new Exception($"Пустая строка №{rowNumber}.");

                var parts = line.Split(';');

                if (parts.Length != 3)
                    throw new Exception($"Неверное количество столбцов в строке №{rowNumber}.");

                if (!DateTime.TryParse(
                        parts[0].Trim(),
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.RoundtripKind,
                        out var date))
                {
                    throw new Exception($"Неверная дата в строке №{rowNumber}.");
                }

                if (date < new DateTime(2000, 1, 1))
                    throw new Exception($"Дата раньше 01.01.2000 в строке №{rowNumber}.");

                if (date > DateTime.UtcNow)
                    throw new Exception($"Дата позже текущей в строке №{rowNumber}.");

                if (!double.TryParse(
                        parts[1].Trim(),
                        NumberStyles.Float,
                        CultureInfo.InvariantCulture,
                        out var executionTime))
                {
                    throw new Exception($"Неверное время выполнения в строке №{rowNumber}.");
                }

                if (executionTime < 0)
                    throw new Exception($"ExecutionTime не может быть меньше 0. Строка №{rowNumber}.");

                if (!double.TryParse(
                        parts[2].Trim(),
                        NumberStyles.Float,
                        CultureInfo.InvariantCulture,
                        out var value))
                {
                    throw new Exception($"Неверное значение показателя в строке №{rowNumber}.");
                }

                if (value < 0)
                    throw new Exception($"Value не может быть меньше 0. Строка №{rowNumber}.");

                values.Add(new Value
                {
                    Date = date,
                    ExecutionTime = executionTime,
                    ValueData = value
                });

                if (values.Count > 10000)
                    throw new Exception("Количество строк превышает 10000.");
            }

            if (values.Count == 0)
                throw new Exception("Файл не содержит данных.");

            return values;
        }


    }
}
