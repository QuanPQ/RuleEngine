using System;
using System.Collections.Generic;

namespace RuleEngine.Models;

public partial class Rule
{
    public string RuleId { get; set; } = null!;

    public string RuleName { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActived { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedUserId { get; set; }

    public DateTime? LastModifiedDate { get; set; }

    public Guid? LastModifiedUserId { get; set; }
}
