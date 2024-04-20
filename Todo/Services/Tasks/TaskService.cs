
using Todo.Models;

namespace Todo.Services.Tasks;

public class TaskService : ITaskService
{
    private static readonly Dictionary<Guid, TaskItem> _tasks = new();

    public void CreateTask(TaskItem item)
    {
        _tasks.Add(item.Id, item);
    }

    public void DeleteTask(Guid id)
    {
        _tasks.Remove(id);
    }

    public TaskItem FetchTask(Guid id)
    {
        return _tasks[id];
    }

    public List<TaskItem> FetchTasks()
    {
        return [.. _tasks.Values];
    }

    public UpdatedTask UpdateTask(TaskItem item)
    {
        var isNewlyCreated = _tasks.ContainsKey(item.Id);
        _tasks[item.Id] = item;
        return new UpdatedTask(isNewlyCreated);
    }
}
