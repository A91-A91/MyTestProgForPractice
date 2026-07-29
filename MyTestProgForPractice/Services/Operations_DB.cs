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
                var values = await parser.ParseF(file);

                await DeleteOldResult(file.FileName);

                var result = CreateResult(file.FileName, values);

                await SaveResult(result);

                await SaveValues(values, result.Id);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task DeleteOldResult(string fileName)
        {
            var existingResult = await context.Results
                .Include(r => r.Values)
                .FirstOrDefaultAsync(r => r.FileName == fileName);

            if (existingResult == null)
                return;

            context.Values.RemoveRange(existingResult.Values);
            context.Results.Remove(existingResult);

            await context.SaveChangesAsync();
        }

        private Result CreateResult(string fileName, List<Value> values)
        {
            var averageValue = CalculateAverageValue(values);
            var averageExecTime = CalculateAverageExecutionTime(values);

            var minValue = values.Min(x => x.ValueData!.Value);
            var maxValue = values.Max(x => x.ValueData!.Value);

            var startDate = values.Min(x => x.Date!.Value);
            var endDate = values.Max(x => x.Date!.Value);

            var timeDelta = (endDate - startDate).TotalSeconds;

            var median = CalculateMedian(values);

            return new Result
            {
                FileName = fileName,
                AverageValue = averageValue,
                AverageExecTime = averageExecTime,
                MinValue = minValue,
                MaxValue = maxValue,
                MedianValue = median,
                StartDate = startDate,
                TimeDelta = timeDelta
            };
        }

        private double CalculateAverageValue(List<Value> values)
        {
            return values.Average(x => x.ValueData!.Value);
        }

        private double CalculateAverageExecutionTime(List<Value> values)
        {
            return values.Average(x => x.ExecutionTime!.Value);
        }

        private async Task SaveResult(Result result)
        {
            context.Results.Add(result);

            await context.SaveChangesAsync();
        }

        private async Task SaveValues(List<Value> values, int resultId)
        {
            foreach (var value in values)
            {
                value.ResultId = resultId;
            }

            context.Values.AddRange(values);

            await context.SaveChangesAsync();
        }

        private double CalculateMedian(List<Value> values)
        {
            var sorted = values
            .Select(v => v.ValueData!.Value)
            .OrderBy(v => v)
            .ToList();

            int count = sorted.Count;
            int middle = count / 2;

            if (count % 2 == 0)
                return (sorted[middle - 1] + sorted[middle]) / 2;

            return sorted[middle];
        }

        public async Task<List<Result>> GetResults(ResultDTO filter)
        {
            var query = context.Results.AsQueryable();

            query = FilterByFileName(query, filter);
            query = FilterByStartDate(query, filter);
            query = FilterByAverageValue(query, filter);
            query = FilterByAverageExecutionTime(query, filter);

            return await query.ToListAsync();
        }

        private IQueryable<Result> FilterByFileName(
        IQueryable<Result> query,
        ResultDTO filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.FileName))
            {
                query = query.Where(r =>
                    r.FileName!.Contains(filter.FileName));
            }

            return query;
        }

        private IQueryable<Result> FilterByStartDate(
        IQueryable<Result> query,
        ResultDTO filter)
        {
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

            return query;
        }
        private IQueryable<Result> FilterByAverageValue(
        IQueryable<Result> query,
        ResultDTO filter)
        {
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

            return query;
        }
        private IQueryable<Result> FilterByAverageExecutionTime(
          IQueryable<Result> query,
          ResultDTO filter)
         {
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

            return query;
        }


        public async Task<List<Value>> GetLastValues(string fileName)
        {
            var values = await context.Values
             .Where(v => v.Result != null && v.Result.FileName == fileName)
             .OrderByDescending(v => v.Date)
             .Take(10)
             .ToListAsync();

            return values
                .OrderBy(v => v.Date)
                .ToList();
        }
    }
}
