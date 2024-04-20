using Todo.Contracts.Tasks;

namespace Todo.Models;

public class TaskItem
{
    public Guid Id { get; }
    public string TaskDescription { get; }
    public DateTime CreatedDate { get; }
    public DateTime DueDate { get; }
    public bool Completed { get; }

    private TaskItem(
        Guid id,
        string taskDescription,
        DateTime createdDate,
        DateTime dueDate,
        bool completed)
    {
        Id = id;
        TaskDescription = taskDescription;
        CreatedDate = createdDate;
        DueDate = dueDate;
        Completed = completed;
    }

    // * Factory method for creating a new task item
    public static TaskItem Create(
        string taskDescription,
        DateTime dueDate,
        bool completed,
        Guid? id = null,
        DateTime? createdDate = null)
    {
        // TODO: Enforce constraints
        return new TaskItem(
            id ?? Guid.NewGuid(),
            taskDescription,
            createdDate ?? DateTime.UtcNow,
            dueDate,
            completed);
    }

    // * Factory method for creating a new task item from CreateTaskRequest
    public static TaskItem From(CreateTaskRequest request)
    {
        return Create(
            request.TaskDescription,
            request.DueDate,
            request.Completed);
    }

    // * Factory method for creating a new task item from UpdateTaskRequest
    public static TaskItem From(Guid id, UpdateTaskRequest request)
    {
        return Create(
            request.TaskDescription,
            request.DueDate,
            request.Completed,
            id,
            request.CreatedDate);
    }

    // * Helper method that maps TaskItem to TaskResponse
    public TaskResponse ToResponse()
    {
        return new TaskResponse(
            Id,
            TaskDescription,
            CreatedDate,
            DueDate,
            Completed);
    }
}