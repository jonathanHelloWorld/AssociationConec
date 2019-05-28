using InterativaSystem.Controllers.Run;
using InterativaSystem.Models;
using Newtonsoft.Json;
using SocketIO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace InterativaSystem.Services
{
    public class RunListenerSocketIOService : SocketIOService
    {
        RunListenerController _runController;
        WebService _webService;
        //CitiesData _cities;

        bool updateScore = true;

        protected override void OnAwake()
        {
            base.OnAwake();

            Type = ServicesTypes.SocketIO;
        }

        protected override void OnStart()
        {
            base.OnStart();

            //_cities = _bootstrap.GetModel(ModelTypes.City) as CitiesData;
            //_cities.OnSelectCity += GetScore;

            _socketManager.On("UpdateScore", UpdateScore);
            _socketManager.On("EndGame", EndGame);

            _runController = _controller as RunListenerController;
            _runController.OnGameStart += () => { StartCoroutine(_CheckEndGame()); };
            _runController.OnGameEnd += () => { updateScore = false; };

            _webService = _bootstrap.GetService(ServicesTypes.WebService) as WebService;

            StartCoroutine(_UpdateScore());
        }

        protected void UpdateScore(SocketIOEvent e)
        {
            /*if (_cities.GetSelectedCity().name == e.data["city"].str)
            {
                Debug.Log(e.data.ToString());
                _runController.UpdateScore(e.data["team"].str, int.Parse(e.data["score"].ToString()));
                CheckEndGame();
            }
            else
                Debug.Log("Not this city! Receiving " + e.data["city"].str + ", but it's " + _cities.GetSelectedCity().name + ".");*/
#if HAS_WEBSERVICE
            _webService.GetJsonRaw(AccessTypes.Users, "city/" + _cities.GetSelectedCity().id, (WebObject response) =>
            {
                if (response == null) return;

                List<RankingUser> team1 = new List<RankingUser>();
                List<RankingUser> team2 = new List<RankingUser>();

                JSONObject json = JSONObject.Create(response.rawJson);
                
                for(int i = 0; i < json.list.Count; i++)
                {
                    if (json.list[i]["team"]["name"].ToString().Replace("\"", "").Equals("Team 1"))
                        team1.Add(new RankingUser(json.list[i]["name"].ToString().Replace("\"", ""), int.Parse(json.list[i]["score"].ToString())));
                    else
                        team2.Add(new RankingUser(json.list[i]["name"].ToString().Replace("\"", ""), int.Parse(json.list[i]["score"].ToString())));
                }

                _runController.UpdateRanking(team1.OrderByDescending(x => x.score).ToList(), 0);
                _runController.UpdateRanking(team2.OrderByDescending(x => x.score).ToList(), 1);
            });
#endif
        }

        IEnumerator _UpdateScore()
        {
            while(updateScore)
            {
                yield return new WaitForSeconds(30f);

                GetScore(0);
            }
        }

        void GetScore(int cityId)
        {
            /*if (_cities.GetSelectedCity() != null)
                _webService.GetJsonRaw(AccessTypes.Score, _cities.GetSelectedCity().id, (WebObject response) => { Debug.Log("Score requested."); });*/
        }

        IEnumerator _CheckEndGame()
        {
            while (_runController.IsGameRunning)
            {
                yield return new WaitForSeconds(2f);

                CheckEndGame();
            }
        }

        void CheckEndGame()
        {
#if HAS_WEBSERVICE
            _webService.GetJsonRaw(AccessTypes.HasEnded, _cities.GetSelectedCity().id, (WebObject response) =>
            {
                Debug.Log("Checking if game has finished.");

                EndGame(new SocketIOEvent("EndGame", JSONObject.Create(response.rawJson)));
            });
#endif
        }

        void EndGame(SocketIOEvent e)
        {
            Debug.Log(bool.Parse(e.data.ToString()));

            if (bool.Parse(e.data.ToString()))
                _bootstrap.EndGame(_runController);
        }
    }
}