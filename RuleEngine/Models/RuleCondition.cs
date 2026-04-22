using System;
using System.Collections.Generic;

namespace RuleEngine.Models;

public partial class RuleCondition
{
    public string RuleConditionId { get; set; } = null!;

    public string RuleId { get; set; } = null!;

    public string ConditionJson { get; set; } = null!;

    public bool IsActived { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedUserId { get; set; }

    public DateTime? LastModifiedDate { get; set; }

    public Guid? LastModifiedUserId { get; set; }
}
