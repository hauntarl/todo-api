namespace Todo.Contracts.Tasks;

public record TaskResponse(
    Guid Id,
    string TaskDescription,
    DateTime CreatedDate,
    DateTime DueDate,
    bool Completed);