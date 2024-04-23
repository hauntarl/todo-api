
using System.Collections.Concurrent;
using ErrorOr;
using Todo.Models;
using Todo.ServiceErrors;

namespace Todo.Services.Tasks;

public class TaskService : ITaskService
{
    private static readonly ConcurrentDictionary<Guid, TaskItem> _tasks = [];

    public async Task<ErrorOr<Created>> CreateTask(TaskItem item)
    {
        return await Task.Run(() => _tasks.TryAdd(item.Id, item))
        ? Result.Created
        : Errors.TaskItem.Failure("Failed to create a new task");
    }

    public async Task<ErrorOr<Deleted>> DeleteTask(Guid id)
    {
        return await Task.Run(() => _tasks.TryRemove(id, out _))
        ? Result.Deleted
        : Errors.TaskItem.Failure("Failed to remove the task");
    }

    public async Task<ErrorOr<TaskItem>> FetchTask(Guid id)
    {
        var item = await Task.Run(() => { 
            _tasks.TryGetValue(id, out var item);
            return item;
        });
        return item != null ? item : Errors.TaskItem.NotFound;
    }

    public async Task<ErrorOr<List<TaskItem>>> FetchTasks(
        bool? filterByCompleted,
        string? sortBy)
    {
        var items = await Task.Run(() => _tasks.Values.ToList());
        if (items == null) {
            return new List<TaskItem>();
        }

        // Filter tasks based on the completed parameter value
        if (filterByCompleted != null)
        {
            items.RemoveAll(item => item.Completed != filterByCompleted.Value);
        }
        // Sort the tasks based on the sort_by parameter value
        return Sort(items, sortBy);
    }

    public async Task<ErrorOr<UpdatedTask>> UpdateTask(TaskItem item)
    {
        return await Task.Run(() =>
        {
            var isNewlyCreated = !_tasks.ContainsKey(item.Id);
            _tasks.AddOrUpdate(item.Id, item, (_, _) => item);
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
