using Todo.Models;

namespace Todo.Services.Tasks;

public interface ITaskService
{
    void CreateTask(TaskItem item);

    void DeleteTask(Guid id);

    TaskItem FetchTask(Guid id);

    List<TaskItem> FetchTasks();

    void UpdateTask(TaskItem item);
}