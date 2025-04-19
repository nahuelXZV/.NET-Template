using WebClient.Common;
using WebClient.Controllers;
using WebClient.Models;

namespace WebClient.Extensions;

public static class ControllerExtensions
{
    public static bool AddTempMessage(this MainController controller, string message, MessageType type)
    {
        var messages = controller.TempData.Get<List<MessageModel>>("Messages") ?? new List<MessageModel>();
        messages.Add(new MessageModel { Message = message, Type = type });
        controller.TempData.Set("Messages", messages);
        return true;
    }

    public static bool AddTempMessage(this MainController controller, Exception exception)
    {
        var message = exception.Message;
        if (exception.InnerException != null)
        {
            message += " " + exception.InnerException.Message;
        }

        return controller.AddTempMessage(message, MessageType.Error);
    }

    public static bool AddInfoTempMessage(this MainController controller, string message)
    {
        return controller.AddTempMessage(message, MessageType.Information);
    }

    public static bool AddSuccessTempMessage(this MainController controller, string message)
    {
        return controller.AddTempMessage(message, MessageType.Success);
    }

    public static bool AddErrorTempMessage(this MainController controller, string message)
    {
        return controller.AddTempMessage(message, MessageType.Error);
    }

    public static bool AddWarningTempMessage(this MainController controller, string message)
    {
        return controller.AddTempMessage(message, MessageType.Warning);
    }
}
