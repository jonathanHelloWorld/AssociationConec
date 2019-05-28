using InterativaSystem.Controllers;

namespace InterativaSystem.Views.Events
{
    public interface IExecuteEvents
    {
        event GenericController.SimpleEvent DoOnEventStart, DoOnEventRepeat, DoOnEventEnd;
    }
}
