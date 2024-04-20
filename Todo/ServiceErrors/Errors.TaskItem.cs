using ErrorOr;
using static Todo.Models.TaskItem;

namespace Todo.ServiceErrors;

public static class Errors
{
    public static class TaskItem
    {
        public static Error NotFound => Error.NotFound(
            code: "TaskItem.NotFound",
            description: "Task not found");

        public static Error InvalidId => Error.Validation(
            code: "TaskItem.InvalidId",
            description: "Task id must be the same.");

        public static Error InvalidDescription => Error.Validation(
            code: "TaskItem.InvalidDescription",
            description: "Task description must be atleast " + 
            $"{MinTaskDescriptionLength} characters long");
    }
}