namespace Todo.Contracts.Task;

public record TaskResponse(
    Guid Id,
    string TaskDescription,
    DateTime CreatedDate,
    DateTime DueDate,
    bool Completed);