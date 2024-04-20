using Microsoft.AspNetCore.Mvc;
using Todo.Contracts.Task;

namespace Todo.Controllers;

public class TasksController : ApiController
{
    private readonly ILogger<TasksController> _logger;

    public TasksController(ILogger<TasksController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult FetchTasks(
        [FromQuery] bool? completed,
        [FromQuery(Name = "sort_by")] string? sortBy)
    {
        return Ok();
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult FetchTask(Guid id)
    {
        return Ok();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult CreateTask(CreateTaskRequest request)
    {
        return CreatedAtAction(
            actionName: nameof(FetchTask),
            routeValues: new { id = Guid.NewGuid() },
            value: request);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult UpdateTask(Guid id, UpdateTaskRequest request)
    {
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult DeleteBreakfast(Guid id)
    {
        return NoContent();
    }
}