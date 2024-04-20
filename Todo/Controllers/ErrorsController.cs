using Microsoft.AspNetCore.Mvc;

namespace Todo.Controllers;

public class ErrorsController: ApiController
{
    // * The UseExceptionHandler re-executes the request to the following route
    [HttpGet("/error")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Error() 
    {
        // Return a 500 status code (Internal server error)
        return Problem();
    }
}