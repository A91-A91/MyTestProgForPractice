using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using MyTestProgForPractice.Classes;
using MyTestProgForPractice.Data;
using MyTestProgForPractice.DTO;
using MyTestProgForPractice.Models;
using MyTestProgForPractice.Services;
namespace MyTestProgForPractice.Services
{
    public class Operations_DB
    {
        private readonly DbForPracticeContext context;
        private readonly CsvParser parser;


        public Operations_DB(DbForPracticeContext context, CsvParser parser)
        {
            this.context = context;
            this.parser = parser;
        }
        public async Task UploadCsv(IFormFile file)
        {
            await using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                // Парсим CSV
                var values = await parser.ParseF(file);

                // Если файл с таким именем уже есть
                var existingResult = await context.Results
                    .Include(r => r.Values)
                    .FirstOrDefaultAsync(r => r.FileName == file.FileName);

                if (existingResult != null)
                {
                    context.Values.RemoveRange(existingResult.Values);
                    context.Results.Remove(existingResult);

                    await context.SaveChangesAsync();
                }

                // Вычисляем статистику
                var averageValue = values.Average(x => x.ValueData!.Value);
                var averageExecTime = values.Average(x => x.ExecutionTime!.Value);

                var minValue = values.Min(x => x.ValueData!.Value);
                var maxValue = values.Max(x => x.ValueData!.Value);

                var startDate = values.Min(x => x.Date!.Value);
                var endDate = values.Max(x => x.Date!.Value);

                var timeDelta = (endDate - startDate).TotalSeconds;

                var median = CalculateMedian(values);

                // Создаем Result
                var result = new Result
                {
                    FileName = file.FileName,
                    AverageValue = averageValue,
                    AverageExecTime = averageExecTime,
                    MinValue = minValue,
                    MaxValue = maxValue,
                    MedianValue = median,
                    StartDate = startDate,
                    TimeDelta = timeDelta
                };

                context.Results.Add(result); // добавляем результат 

                await context.SaveChangesAsync();

                // Привязываем Values к Result
                foreach (var value in values)
                {
                    value.ResultId = result.Id;
                }
                foreach (var v in values)
                {
                    Console.WriteLine($"Id = {v.Id}");
                }

                Console.WriteLine($"Количество объектов = {values.Count}");

                context.Values.AddRange(values);

                await context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private double CalculateMedian(List<Value> values)
        {
            var sorted = values
                .Select(v => v.ValueData)
                .OrderBy(v => v)
                .ToList();

            int count = sorted.Count;
            int middle = count / 2;

            if (count % 2 == 0)
                return (double)(sorted[middle - 1] + sorted[middle]) / 2;

            return (double)sorted[middle];
        }

        public async Task<List<Result>> GetResults(ResultDTO filter)
        {
            var query = context.Results.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.FileName))
            {
                query = query.Where(r =>
                    r.FileName!.Contains(filter.FileName));
            }

            if (filter.StartDateFrom.HasValue)
            {
                query = query.Where(r =>
                    r.StartDate >= filter.StartDateFrom.Value);
            }

            if (filter.StartDateTo.HasValue)
            {
                query = query.Where(r =>
                    r.StartDate <= filter.StartDateTo.Value);
            }

            if (filter.AverageValueFrom.HasValue)
            {
                query = query.Where(r =>
                    r.AverageValue >= filter.AverageValueFrom.Value);
            }

            if (filter.AverageValueTo.HasValue)
            {
                query = query.Where(r =>
                    r.AverageValue <= filter.AverageValueTo.Value);
            }

            if (filter.AverageExecutionTimeFrom.HasValue)
            {
                query = query.Where(r =>
                    r.AverageExecTime >= filter.AverageExecutionTimeFrom.Value);
            }

            if (filter.AverageExecutionTimeTo.HasValue)
            {
                query = query.Where(r =>
                    r.AverageExecTime <= filter.AverageExecutionTimeTo.Value);
            }

            return await query.ToListAsync();
        }
    }
}
