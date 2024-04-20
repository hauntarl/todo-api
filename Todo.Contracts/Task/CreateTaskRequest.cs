namespace Todo.Contracts.Task;

public record CreateTaskRequest(
    string TaskDescription,
    DateTime DueDate,
    bool Completed
);
