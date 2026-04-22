using System;
using System.Collections.Generic;

namespace RuleEngine.Models;

public partial class RuleExecutionLog
{
    public Guid RuleExecutionLog1 { get; set; }

    public Guid RuleAssignmentId { get; set; }

    public string? RequestId { get; set; }

    public string? Input { get; set; }

    public bool? ConditionResult { get; set; }

    public string? ActionsExecuted { get; set; }

    public string? ErrorMessage { get; set; }

    public int? ExecutionTimeMs { get; set; }

    public bool IsActived { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedUserId { get; set; }

    public DateTime? LastModifiedDate { get; set; }

    public Guid? LastModifiedUserId { get; set; }
}
