using Microsoft.AspNetCore.Identity;

namespace task_manager.Entities;

public class TaskEntity
{
    public int TaskId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Order { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public IdentityUser User { get; set; } = new IdentityUser();
    public DateTime CreateAt { get; set; }
    public List<StepEntity> Steps { get; set; } = [];
    public List<AttachmentEntity> Attachments { get; set; } = [];
}
