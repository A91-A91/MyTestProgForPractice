using System;
using System.Collections.Generic;

namespace MyTestProgForPractice.Models;

public partial class Value
{
    public int Id { get; set; }

    public DateTime? Date { get; set; }

    public double? Execution_time { get; set; }

    public double? ValueData { get; set; }

    public int? ResultId { get; set; }

    public virtual Result? Result { get; set; }
}
