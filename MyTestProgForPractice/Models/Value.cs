using System;
using System.Collections.Generic;

namespace MyTestProgForPractice.Models;

public partial class Value
{
    public int Id { get; set; }

    public DateTime? Date { get; set; }

    public double? ExecutionTime { get; set; }

    public double? Value1 { get; set; }

    public int? ResultId { get; set; }

    public virtual Result? Result { get; set; }
}
