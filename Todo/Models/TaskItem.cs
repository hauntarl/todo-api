using ErrorOr;
using Todo.Contracts.Tasks;
using Todo.ServiceErrors;

namespace Todo.Models;

public class TaskItem
{
    public const int MinTaskDescriptionLength = 3;
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
    public static ErrorOr<TaskItem> Create(
        string taskDescription,
        DateTime dueDate,
        bool completed,
        Guid? id = null,
        DateTime? createdDate = null)
    {
        var _taskDescription = taskDescription.Trim();
        if (_taskDescription.Length is < MinTaskDescriptionLength)
        {
            return Errors.TaskItem.InvalidDescription;
        }

        return new TaskItem(
            id ?? Guid.NewGuid(),
            _taskDescription,
            createdDate ?? DateTime.UtcNow,
            dueDate,
            completed);
    }

    // * Factory method for creating a new task item from CreateTaskRequest
    public static ErrorOr<TaskItem> From(CreateTaskRequest request)
    {
        return Create(
            request.TaskDescription,
            request.DueDate,
            request.Completed);
    }

    // * Factory method for creating a new task item from UpdateTaskRequest
    public static ErrorOr<TaskItem> From(Guid id, UpdateTaskRequest request)
    {
        if (id != request.Id)
        {
            return Errors.TaskItem.InvalidId;
        }

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