
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InterativaSystem.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
    
namespace InterativaSystem.Services
{
    public enum AccessTypes
    {
        User,
        Users,
        Score, 
        Question
    }
    public class WebService : GenericService
    {
        public string url;

        private IdsController _idsController;

        public string token { get; protected set; }

        public Dictionary<AccessTypes, string> accessPath;

        public delegate void WebObjectEvent(WebObject response);

        public event GenericController.SimpleEvent OnTokenGet;
        public event GenericController.StringEvent OnError;
        public event WebObjectEvent OnGet;
#if HAS_WEBSERVICE


        void Awake()
        {
            //Mandatory set for every service
            Type = ServicesTypes.WebService;

            accessPath = new Dictionary<AccessTypes, string>()
            {
                {AccessTypes.User, "user"},
                {AccessTypes.Users, "user"},
                {AccessTypes.Score, "score"},
                {AccessTypes.Question, "quiz"}
            };
        }

        protected override void OnStart()
        {
            base.OnStart();
            _idsController = _bootstrap.GetController(ControllerTypes.Id) as IdsController;
        }

        public int errorIdCall = 100;
        public void GetToken(AccessTypes accessTypes, Dictionary<string, string> headers, WebObjectEvent returnStringEvent)
        {
            StartCoroutine(_GetToken(accessTypes, headers, returnStringEvent));
        }
        protected IEnumerator _GetToken(AccessTypes accessTypes, Dictionary<string, string> headers, WebObjectEvent returnStringEvent)
        {
            var path = url + "/" + accessPath[accessTypes];


            var connection = UnityEngine.Networking.UnityWebRequest.Post(path, headers);
            //WWW connection = new WWW(path, form);

            yield return connection.Send();

            if (connection.isError)
            {
                Debug.LogError("Error: " + connection.error);
                _idsController.CallAction(errorIdCall);
                yield break;
            }

            var webres = JsonConvert.DeserializeObject<WebObject>(connection.downloadHandler.text);

            /*
            if (webres.err!= null && !string.IsNullOrEmpty(webres.err.error))
            {
                Debug.LogError("Error: " + webres.err.error);
                _idsController.CallAction(errorIdCall);
                if (OnError != null) OnError(webres.err.error);
                yield break;
            }
            /**/
            token = webres.token;

            returnStringEvent(webres);
            if (OnTokenGet != null) OnTokenGet();
            if (OnGet != null) OnGet(webres);
        }

        public void GetJson(AccessTypes accessTypes, WebObjectEvent returnStringEvent)
        {
            StartCoroutine(_GetJson(accessTypes, returnStringEvent));
        }
        protected IEnumerator _GetJson(AccessTypes accessTypes, WebObjectEvent returnStringEvent)
        {
            var path = url + "/" + accessPath[accessTypes];

            var headers = new Dictionary<string, string>();
            headers["Authorization"] = "Bearer " + token;

            WWW connection = new WWW(path, null, headers);

            yield return connection;
            object webres = null;

            switch (accessTypes)
            {
                case AccessTypes.Users:
                    try
                    {
                        webres = JsonConvert.DeserializeObject<List<WebUser>>(connection.text);
                    }
                    catch (Exception)
                    {
                        Debug.LogError("Error: ");
                    }

                    var users = webres as List<WebUser>;

                    if (webres == null)
                    {
                        webres = JsonConvert.DeserializeObject<WebObject>(connection.text);
                        var error = webres as WebObject;
                        Debug.LogError("Error: " + error.err.error);
                        _idsController.CallAction(errorIdCall);
                        if (OnError != null) OnError(error.err.error);
                        yield break;
                    }

                    var usersWebObj = users.Select(webUser => new WebObject(webUser)).ToList();
                    for (int webObject=0, n = usersWebObj.Count; webObject<n; webObject++)
                    {
                        returnStringEvent(usersWebObj[webObject]);
                        if (OnGet != null) OnGet(usersWebObj[webObject]);
                    }
                    break;
                case AccessTypes.User:
                    webres = JsonConvert.DeserializeObject<WebUser>(connection.text);
                    var user = webres as WebUser;
                    if (string.IsNullOrEmpty(user.err.error))
                    {
                        Debug.LogError("Error: " + user.err.error);
                        _idsController.CallAction(errorIdCall);
                        if (OnError != null) OnError(user.err.error);
                        yield break;
                    }
                    var userWebObj = new WebObject(user);
                    returnStringEvent(userWebObj);
                    if (OnGet != null) OnGet(userWebObj);
                    break;
                case AccessTypes.Score:
                    Debug.LogError("Not Implemented");
                    break;
                case AccessTypes.Question:
                    Debug.LogError("Not Implemented");
                    break;
                default:
                    Debug.LogError("Out of Arguments - Not Implemented");
                    break;
            }
        }

        protected IEnumerator _PostJson(AccessTypes accessTypes, WWWForm form)
        {
            yield return null;
        }
#endif

        public string GetUrl()
        {
            //TODO: salvar no playerprefs
            return url;
        }

        public void SetUrl(string value)
        {
            url = value;
        }
    }
    public class WebUser
    {
        public WebErr err;
        public string email;
        public string createdAt;
        public string updatedAt;
        public string id;
    }
    public class WebErr
    {
        public string error;
        public int status;
        public string summary;
    }
    public class WebObject
    {
        public WebObject() { }

        public WebObject(WebUser user)
        {
            this.user = user;
        }

        public WebUser user;
        public string token;
        public WebErr err;
    }
}