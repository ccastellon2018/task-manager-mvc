namespace task_manager.Entities;

public class StepEntity
{
    public Guid StepId { get; set; }
    public int TaskId { get; set; }
    public TaskEntity? Task_Entity { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool Done { get; set; }
    public int StepOrder { get; set; }
}
