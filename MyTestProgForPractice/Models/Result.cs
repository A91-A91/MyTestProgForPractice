using System;
using System.Collections.Generic;

namespace MyTestProgForPractice.Models;

public partial class Result
{
    public int Id { get; set; }

    public string? FileName { get; set; }

    public double? TimeDelta { get; set; }

    public DateTimeOffset? StartDate { get; set; }

    public double? AverageExecTime { get; set; }

    public double? AverageValue { get; set; }

    public double? MedianValue { get; set; }

    public double? MaxValue { get; set; }

    public double? MinValue { get; set; }

    public virtual ICollection<Value> Values { get; set; } = new List<Value>();
}
