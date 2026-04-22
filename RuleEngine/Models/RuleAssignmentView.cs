using System;
using System.Collections.Generic;

namespace RuleEngine.Models;

public partial class RuleAssignmentView
{
    public Guid RuleAssignmentId { get; set; }

    public Guid ApplicationId { get; set; }

    public string ApplicationName { get; set; } = null!;

    public string? Version { get; set; }

    public Guid RuleTargetId { get; set; }

    public string? TargetType { get; set; }

    public string TargetCode { get; set; } = null!;

    public string RuleId { get; set; } = null!;

    public string RuleName { get; set; } = null!;

    public string? Description { get; set; }

    public int Priority { get; set; }

    public bool StopOnFirstFail { get; set; }

    public bool EnableLog { get; set; }

    public bool IsActived { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedUserId { get; set; }

    public DateTime? LastModifiedDate { get; set; }

    public Guid? LastModifiedUserId { get; set; }
}
