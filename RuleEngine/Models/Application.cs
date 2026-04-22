using System;
using System.Collections.Generic;

namespace RuleEngine.Models;

public partial class Application
{
    public Guid ApplicationId { get; set; }

    public string ApplicationKey { get; set; } = null!;

    public string ApplicationName { get; set; } = null!;

    public string? Version { get; set; }

    public bool IsActived { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedUserId { get; set; }

    public DateTime? LastModifiedDate { get; set; }

    public Guid? LastModifiedUserId { get; set; }
}
