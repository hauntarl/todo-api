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
    public async Task<IActionResult> FetchTasks(
        [FromQuery] bool? completed,
        [FromQuery(Name = "sort_by")] string? sortBy)
    {
        // Fetch all tasks from the service
        var taskItemsResult = await _taskService.FetchTasks(completed, sortBy);
        // If the result is an error, return a Problem with appropriate status code
        if (taskItemsResult.IsError)
        {
            return Problem(taskItemsResult.Errors);
        }

        // Get the tasks items from service result
        var items = taskItemsResult.Value;

        // Convert all tasks to TaskResponse objects.
        var response = items.ConvertAll(item => item.ToResponse());
        
        // Return 200 (OK) with a list of TaskResponse objects
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> FetchTask(Guid id)
    {
        // Fetch task for given id
        var taskResult = await _taskService.FetchTask(id);

        // Returns 200 (OK) with the TaskResponse object
        // Returns a Problem response with appropriate status code
        return taskResult.Match(
            item => Ok(item.ToResponse()),
            Problem);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTask(CreateTaskRequest request)
    {
        // Create a TaskItem object from the given CreateTaskRequest
        var itemResult = TaskItem.From(request);
        // If the result is an error, return a Problem with appropriate status code
        if (itemResult.IsError) 
        {
            return Problem(itemResult.Errors);
        }

        // Get the tasks item from the service result
        var item = itemResult.Value;

        // Request the task service to create a new task from TaskItem
        var createResult = await _taskService.CreateTask(item);
        
        // Returns 201 (Created) with location of the newly created task
        // Returns a Problem response with appropriate status code
        return createResult.Match(
            _ => CreatedAtFetchTask(item),
            Problem
        );
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateTask(Guid id, UpdateTaskRequest request)
    {
        // Create a TaskItem object from the given Guid and UpdateTaskRequest
        var itemResult = TaskItem.From(id, request);
        // If the result is an error, return a Problem with appropriate status code
        if (itemResult.IsError)
        {
            return Problem(itemResult.Errors);
        }
        
        // Get the tasks item from the service result
        var item = itemResult.Value;
        // Request the task service to update a new task from TaskItem
        var updateResult = await _taskService.UpdateTask(item);

        // Returns 201 (Created) if the task was newly created
        // Returns 204 (No Content) if the task was updated
        // Returns a Problem response with appropriate status code
        return updateResult.Match(
            status => status.IsNewlyCreated ? CreatedAtFetchTask(item) : NoContent(),
            Problem);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteBreakfast(Guid id)
    {
        // Request task service to delete TaskItem object for given id
        var deleteResult = await _taskService.DeleteTask(id);

        // Returns 204 (No Content) if task deleted successfully
        // Returns a Problem response with appropriate status code
        return deleteResult.Match(
            _ => NoContent(),
            Problem);
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
