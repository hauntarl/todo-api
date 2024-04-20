namespace Todo.Contracts.Tasks;

public record CreateTaskRequest(
    string TaskDescription,
    DateTime DueDate,
    bool Completed);
