using System;
using System.Collections.Generic;

namespace RuleEngine.Models;

public partial class RuleTarget
{
    public Guid RuleTargetId { get; set; }

    public Guid ApplicationId { get; set; }

    /// <summary>
    /// PRODUCT, CHANNEL, REGION, CUSTOMER_SEGMENT, PERSON
    /// </summary>
    public string? TargetType { get; set; }

    public string TargetCode { get; set; } = null!;

    public bool IsActived { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedUserId { get; set; }

    public DateTime? LastModifiedDate { get; set; }

    public Guid? LastModifiedUserId { get; set; }
}
