using Microsoft.AspNetCore.Mvc;
using Todo.Contracts.Tasks;
using Todo.Models;
using Todo.Services.Tasks;

namespace Todo.Controllers;

public class TasksController : ApiController
{
    private readonly ILogger<TasksController> _logger;
    private readonly ITaskService _taskService; // Dependency Injection

    public TasksController(
        ILogger<TasksController> logger,
        ITaskService taskService)
    {
        _logger = logger;
        _taskService = taskService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult FetchTasks(
        [FromQuery] bool? completed,
        [FromQuery(Name = "sort_by")] string? sortBy)
    {
        // Fetch all tasks from the service
        var items = _taskService.FetchTasks();

        // Filter tasks based on the completed parameter value
        if (completed != null)
        {
            items.RemoveAll(item => item.Completed != completed.Value);
        }

        // Sort the tasks based on the sort_by parameter value
        items = Sort(items, sortBy);

        // Convert all tasks to TaskResponse objects.
        var response = items.ConvertAll(item => item.ToResponse());
        
        // Return 200 (OK) with a list of TaskResponse objects
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult FetchTask(Guid id)
    {
        // Fetch task for given id
        var item = _taskService.FetchTask(id);

        // Convert task to TaskResponse object
        var response = item.ToResponse();

        // Return 200 (OK) with a TaskResponse object
        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult CreateTask(CreateTaskRequest request)
    {
        // Create a TaskItem object from the given CreateTaskRequest
        var item = TaskItem.From(request);

        // Request the task service to create a new task from TaskItem
        _taskService.CreateTask(item);

        // Return 201 (Created) with location of the created task
        return CreatedAtFetchTask(item);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult UpdateTask(Guid id, UpdateTaskRequest request)
    {
        // Create a TaskItem object from the given Guid and UpdateTaskRequest
        var item = TaskItem.From(id, request);

        // Request the task service to update a new task from TaskItem
        var updateResult = _taskService.UpdateTask(item);

        // Return 201 (Created) if the task was newly created
        // Return 204 (No Content) if the task was updated
        return updateResult.IsNewlyCreated
        ? CreatedAtFetchTask(item)
        : NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult DeleteBreakfast(Guid id)
    {
        // Request task service to delete TaskItem object for given id
        _taskService.DeleteTask(id);

        // Return 204 (No Content)
        return NoContent();
    }

    public List<TaskItem> Sort(List<TaskItem> items, string? sortBy)
    {
        return sortBy switch
        {
            "dueDate" => [.. items.OrderBy(item => item.DueDate)],
            "-dueDate" => [.. items.OrderByDescending(item => item.DueDate)],
            "createdDate" => [.. items.OrderBy(item => item.CreatedDate)],
            _ => [.. items.OrderByDescending(item => item.CreatedDate)],
        };
    }

    // * Helper method that produces a 201 (Created) response with location of the created task
    private CreatedAtActionResult CreatedAtFetchTask(TaskItem item)
    {
        return CreatedAtAction(
            actionName: nameof(FetchTask),
            routeValues: new { id = item.Id },
            value: item.ToResponse());
    }
}
