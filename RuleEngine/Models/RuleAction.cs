using System;
using System.Collections.Generic;

namespace RuleEngine.Models;

public partial class RuleAction
{
    public string RuleActionId { get; set; } = null!;

    public string RuleId { get; set; } = null!;

    public string ActionType { get; set; } = null!;

    public int ActionOrder { get; set; }

    public string ActionJson { get; set; } = null!;

    public bool IsActived { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedUserId { get; set; }

    public DateTime? LastModifiedDate { get; set; }

    public Guid? LastModifiedUserId { get; set; }
}
