using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RuleEngine.Models;

public partial class RuleEngineContext : DbContext
{
    public RuleEngineContext()
    {
    }

    public RuleEngineContext(DbContextOptions<RuleEngineContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Application> Applications { get; set; }

    public virtual DbSet<Rule> Rules { get; set; }

    public virtual DbSet<RuleAction> RuleActions { get; set; }

    public virtual DbSet<RuleAssignment> RuleAssignments { get; set; }

    public virtual DbSet<RuleAssignmentView> RuleAssignmentViews { get; set; }

    public virtual DbSet<RuleCondition> RuleConditions { get; set; }

    public virtual DbSet<RuleExecutionLog> RuleExecutionLogs { get; set; }

    public virtual DbSet<RuleTarget> RuleTargets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Application>(entity =>
        {
            entity.ToTable("Application");

            entity.HasIndex(e => e.ApplicationId, "IX_Application");

            entity.Property(e => e.ApplicationId).ValueGeneratedNever();
            entity.Property(e => e.ApplicationKey).HasMaxLength(128);
            entity.Property(e => e.ApplicationName).HasMaxLength(128);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())", "DF_Application_CreatedDate")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActived).HasDefaultValue(true, "DF_Application_IsActived");
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Version)
                .HasMaxLength(16)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Rule>(entity =>
        {
            entity.ToTable("Rule");

            entity.Property(e => e.RuleId)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())", "DF_Rule_CreatedDate")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(512);
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.RuleName).HasMaxLength(128);
        });

        modelBuilder.Entity<RuleAction>(entity =>
        {
            entity.ToTable("RuleAction");

            entity.Property(e => e.RuleActionId)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.ActionType)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())", "DF_RuleAction_CreatedDate")
                .HasColumnType("datetime");
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.RuleId)
                .HasMaxLength(64)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RuleAssignment>(entity =>
        {
            entity.ToTable("RuleAssignment");

            entity.Property(e => e.RuleAssignmentId).HasDefaultValueSql("(newid())", "DF_RuleAssignment_RuleAssignmentId");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())", "DF_RuleAssignment_CreatedDate")
                .HasColumnType("datetime");
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.RuleId)
                .HasMaxLength(64)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RuleAssignmentView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("RuleAssignmentView");

            entity.Property(e => e.ApplicationName).HasMaxLength(128);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(512);
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.RuleId)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.RuleName).HasMaxLength(128);
            entity.Property(e => e.TargetCode)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.TargetType)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.Version)
                .HasMaxLength(16)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RuleCondition>(entity =>
        {
            entity.ToTable("RuleCondition");

            entity.Property(e => e.RuleConditionId)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())", "DF_RuleCondition_CreatedDate")
                .HasColumnType("datetime");
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.RuleId)
                .HasMaxLength(64)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RuleExecutionLog>(entity =>
        {
            entity.HasKey(e => e.RuleExecutionLog1);

            entity.ToTable("RuleExecutionLog");

            entity.Property(e => e.RuleExecutionLog1)
                .ValueGeneratedNever()
                .HasColumnName("RuleExecutionLog");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())", "DF_RuleExecutionLog_CreatedDate")
                .HasColumnType("datetime");
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.RequestId)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.RuleAssignmentId).HasDefaultValueSql("(newid())", "DF_RuleExecutionLog_RuleAssignmentId");
        });

        modelBuilder.Entity<RuleTarget>(entity =>
        {
            entity.ToTable("RuleTarget");

            entity.Property(e => e.RuleTargetId).HasDefaultValueSql("(newid())", "DF_RuleTarget_RuleTargetId");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())", "DF_RuleTarget_CreatedDate")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActived).HasDefaultValue(true, "DF_RuleTarget_IsActived");
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.TargetCode)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.TargetType)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("PRODUCT, CHANNEL, REGION, CUSTOMER_SEGMENT, PERSON");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
