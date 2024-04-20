namespace Todo.Contracts.Tasks;

public record UpdateTaskRequest(
    Guid Id,
    string TaskDescription,
    DateTime CreatedDate,
    DateTime DueDate,
    bool Completed);
