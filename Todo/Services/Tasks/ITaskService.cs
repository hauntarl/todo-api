using ErrorOr;
using Todo.Models;

namespace Todo.Services.Tasks;

public interface ITaskService
{
    Task<ErrorOr<Created>> CreateTask(TaskItem item);

    Task<ErrorOr<Deleted>> DeleteTask(Guid id);

    Task<ErrorOr<TaskItem>> FetchTask(Guid id);

    Task<ErrorOr<List<TaskItem>>> FetchTasks(bool? filterByCompleted, string? sortBy);

    Task<ErrorOr<UpdatedTask>> UpdateTask(TaskItem item);
}