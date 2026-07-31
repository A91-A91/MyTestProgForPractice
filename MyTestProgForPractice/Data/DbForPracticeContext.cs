using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MyTestProgForPractice.Models;

namespace MyTestProgForPractice.Data;

public partial class DbForPracticeContext : DbContext
{

    public DbForPracticeContext()
    {

    }

    public DbForPracticeContext(DbContextOptions<DbForPracticeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Result> Results { get; set; }

    public virtual DbSet<Value> Values { get; set; }




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Results_pkey");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.AverageExecTime).HasColumnName("average_exec_time");
            entity.Property(e => e.AverageValue).HasColumnName("average_value");
            entity.Property(e => e.FileName).HasColumnName("fileName");
            entity.Property(e => e.MaxValue).HasColumnName("max_value");
            entity.Property(e => e.MedianValue).HasColumnName("median_value");
            entity.Property(e => e.MinValue).HasColumnName("min_value");
            entity.Property(e => e.StartDate)
                .HasColumnName("startDate");
            entity.Property(e => e.TimeDelta).HasColumnName("timeDelta");
        });

        modelBuilder.Entity<Value>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Values_pkey");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Execution_time).HasColumnName("execution_time");
            entity.Property(e => e.ResultId).HasColumnName("result_id");
            entity.Property(e => e.ValueData).HasColumnName("valueData");

            entity.HasOne(d => d.Result).WithMany(p => p.Values)
                .HasForeignKey(d => d.ResultId)
                .HasConstraintName("fk_result_id ");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
