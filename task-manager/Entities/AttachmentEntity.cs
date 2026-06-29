namespace task_manager.Entities
{
    public class AttachmentEntity
    {
        public Guid AttachmentId { get; set; }
        public int TaskId { get; set; }
        public TaskEntity? Task_Entity { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public int AttachmentOrder { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
