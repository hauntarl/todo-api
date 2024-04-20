
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

    public async Task<ErrorOr<List<TaskItem>>> FetchTasks(
        bool? filterByCompleted,
        string? sortBy)
    {
        return await Task.Run(() => {
            var items = _tasks.Values.ToList();
            // Filter tasks based on the completed parameter value
            if (filterByCompleted != null)
            {
                items.RemoveAll(item => item.Completed != filterByCompleted.Value);
            }

            // Sort the tasks based on the sort_by parameter value
            return Sort(items, sortBy);
         });
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

    private static List<TaskItem> Sort(List<TaskItem> items, string? sortBy)
    {
        return sortBy switch
        {
            "dueDate" => [.. items.OrderBy(item => item.DueDate)],
            "-dueDate" => [.. items.OrderByDescending(item => item.DueDate)],
            "createdDate" => [.. items.OrderBy(item => item.CreatedDate)],
            _ => [.. items.OrderByDescending(item => item.CreatedDate)],
        };
    }
}
