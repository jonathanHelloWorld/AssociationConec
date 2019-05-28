using UnityEngine;

namespace InterativaSystem.Controllers
{
    [AddComponentMenu("ModularSystem/Controllers/Console Controller")]
    public class ConsoleController : GenericController
    {
        public event StringEvent ConsolePrint;
        
        void Awake()
        {
            //Mandatory set for every controller
            Type = ControllerTypes.Console;
        }

        public void Print(string value)
        {
            if (ConsolePrint != null) ConsolePrint(value);
            Debug.Log(value);
        }
    }
}