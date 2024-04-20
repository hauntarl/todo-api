
using ErrorOr;
using Todo.Models;
using Todo.ServiceErrors;

namespace Todo.Services.Tasks;

public class TaskService : ITaskService
{
    private static readonly Dictionary<Guid, TaskItem> _tasks = [];

    public async Task<ErrorOr<Created>> CreateTask(TaskItem item)
    {
        await Task.Run(() => _tasks.Add(item.Id, item));
        return Result.Created;
    }

    public async Task<ErrorOr<Deleted>> DeleteTask(Guid id)
    {
        await Task.Run(() => _tasks.Remove(id));
        return Result.Deleted;
    }

    public async Task<ErrorOr<TaskItem>> FetchTask(Guid id)
    {
        var item = await Task.Run(() => _tasks.GetValueOrDefault(id));
        return item != null ? item : Errors.TaskItem.NotFound;
    }

    public async Task<ErrorOr<List<TaskItem>>> FetchTasks()
    {
        return await Task.Run(() => _tasks.Values.ToList());
    }

    public async Task<ErrorOr<UpdatedTask>> UpdateTask(TaskItem item)
    {
        return await Task.Run(() =>
        {
            var isNewlyCreated = !_tasks.ContainsKey(item.Id);
            _tasks[item.Id] = item;
            return new UpdatedTask(isNewlyCreated);
        });
    }
}
