using System;
using System.Linq;
using InterativaSystem.Controllers;
using InterativaSystem.Controllers.Network;
using Newtonsoft.Json;
using UnityEngine;

namespace InterativaSystem.Models
{
    /// <summary>
    /// Base para todos os models, que são divididos em DataModels e InfoModels
    /// </summary>
    public class GenericModel : MonoBehaviour
    {
        /// <summary>
        /// Evento disparado no fim da inicialização do Model
        /// </summary>
        public event GenericController.SimpleEvent OnInitialize;
        /// <summary>
        /// Evento disparado quando model reseta
        /// </summary>
        public event GenericController.SimpleEvent Reset;

        /// <summary>
        /// Referencia para o gerenciador de IO
        /// </summary>
        protected IOController _IOController;
        
        /// <summary>
        /// Tipo do modelo, usado para serializar a partir do Bootstrap
        /// </summary>
        [HideInInspector]
        public ModelTypes Type;

        /// <summary>
        /// Referencia para o gerenciador do console
        /// </summary>
        protected ConsoleController _console;

        /// <summary>
        /// Referencia para o bootstrap
        /// </summary>
        protected Bootstrap _bootstrap;

        #region Network Parameters and Methods
#if HAS_SERVER
        /// <summary>
        /// Usada para saber se a aplicação é um servidor ou um cliente
        /// OBS: Usado apenas no servidor
        /// </summary>
        protected bool _isServer;
        /// <summary>
        /// referencia para o Controlador do cliente
        /// OBS: Usado apenas no servidor
        /// </summary>
        protected NetworkClientController _clientController;
        /// <summary>
        /// referencia para o Controlador do servidor
        /// OBS: Usado apenas no servidor
        /// </summary>
        protected NetworkServerController _serverController;
        /// <summary>
        /// referencia do tipo da classe para facilitar os metodos de Reflection
        /// OBS: Usado apenas no servidor
        /// </summary>
        protected Type _type;
        
        /// <summary>
        /// Metodo para devolver on controlador de network da aplicacao seja servidor ou cliente
        /// OBS: Usado apenas no servidor
        /// </summary>
        public NetworkController GetNetworkController()
        {
            if (_isServer)
                return _serverController;

            return _clientController;
        }
        
        /// <summary>
        /// Metodo que dispara o metodo trazido pelo evento do servidor ou cliente
        /// OBS: Usado apenas no servidor
        /// </summary>
        protected void CheckNetworCall(string method, string json)
        {
            var meth = _type.GetMethods().ToList();
            if (!meth.Exists(x => x.Name == method)) return;

            var param = new object[] { json };

            try
            {
                _type.GetMethod(method).Invoke(this, param);
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                Debug.LogWarning(this + " - " + method + "\n\n" + e.ToString());
#endif
            }
            DebugLog("Called: " + method + ", on " + this);
            /**/
        }
        
        /// <summary>
        /// Metodo que dispara o metodo trazido pelo evento do servidor ou cliente destinada para apenas um cliente
        /// OBS: Usado apenas no servidor
        /// </summary>
        protected void CheckNetworCallId(string id, string method, string json)
        {
            var meth = _type.GetMethods().ToList();
            if (!meth.Exists(x => x.Name == method)) return;

            var param = new object[] { id, json };

            try
            {
                _type.GetMethod(method).Invoke(this, param);
                DebugLog("Called: " + method + ", on " + this + ", and id:" + id);
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                Debug.LogWarning(this + " - " + method + " - " + id + "\n\n" + e.ToString());
#endif
            }
        }
#endif
        #endregion

        #region Initialization
        void Start()
        {
            //the child needs to define a type for the controller
            if (Type == ModelTypes.Null)
            {
                Debug.LogError("Type not set");
                return;
            }
            _bootstrap = Bootstrap.Instance;
            _bootstrap.SetModel(this);

            _bootstrap.InitializeModels += Initialize;
        }

        /// <summary>
        /// Metodo para a inicializacao do Model base disparada pelo bootstrap
        /// </summary>
        void Initialize()
        {
            _IOController = _bootstrap.GetController(ControllerTypes.IO) as IOController;

            try
            {
                _console = _bootstrap.GetController(ControllerTypes.Console) as ConsoleController;
            }
            catch (Exception)
            {
                Debug.LogError("There is no Console Controller in the scene");
            }

            _bootstrap.Reset += CallReset;

#if HAS_SERVER
            try
            {
                var server = _bootstrap.GetController(ControllerTypes.NetworkServer);
                _isServer = server != null;
            }
            catch (Exception)
            {
                throw;
            }
            _type = GetType();
            if (!_isServer)
            {
                _clientController = _bootstrap.GetController(ControllerTypes.NetworkClient) as NetworkClientController;
                _clientController.OnReceive += CheckNetworCall;
            }
            else
            {
                _serverController = _bootstrap.GetController(ControllerTypes.NetworkServer) as NetworkServerController;
                _serverController.OnReceive += CheckNetworCall;
                _serverController.OnIdReceive += CheckNetworCallId;
            }
#endif
            OnStart();
        }

        /// <summary>
        /// Metodo para a inicializacao dos Medels que herdam o GenericModel
        /// </summary>
        protected virtual void OnStart() { }

        /// <summary>
        /// Metodo usado para buscar referencias de outros Models ou Controllers
        /// </summary>
        protected virtual void GetReferences() { }
        #endregion

        /// <summary>
        /// Metodo que dispara o evento Reset
        /// </summary>
        public virtual void CallReset()
        {
            if (Reset != null) Reset();
        }

        /// <summary>
        /// Metodo que dispara o Debug para o Console caso exista e imprime no editor
        /// </summary>
        /// <param name="debug">Texto que será impresso</param>
        protected void DebugLog(string debug)
        {
            if (_bootstrap == null) return;

            if (_console == null && _bootstrap.IsEditor && _bootstrap.DebugOnEdtor)
            {
                Debug.Log(debug);
                return;
            }

            if (_console == null) return;

            _console.Print(debug);
        }

        /// <summary>
        /// Metodo que disara o evento imformando que o model ja terminou sua inicialização
        /// </summary>
        protected virtual void EndInitialization()
        {
            if (OnInitialize != null) OnInitialize();
        }
    }
}