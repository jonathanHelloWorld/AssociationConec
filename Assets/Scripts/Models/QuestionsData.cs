using System;
using System.Collections.Generic;
using InterativaSystem.Controllers;
using Newtonsoft.Json;
using SQLite4Unity3d;
using UnityEngine;

namespace InterativaSystem.Models
{
    [AddComponentMenu("ModularSystem/DataModel/ Questions")]
    public class QuestionsData : DataModel
    {
        public event GenericController.SimpleEvent OnQuestionsOver, OnNewQuestionReady;
        
        [Space]
        public bool RandomizeQuestions;

        [Space]
        public int ActualQuestion;
        public List<Question> Questions;

        [Space]
        public bool ResetAlreadyChosen = true;

        [HideInInspector]
        public List<int> QuestionsUsed;

        System.Random rnd;
        [HideInInspector]
        public int QuestionType = 0;

        QuizController _quizController;
#if HAS_TURNING
#endif

#if HAS_SQLITE3
#endif

        #region Initialization
        void Awake()
        {
            //Mandatory set for every model
            Type = ModelTypes.Questions;
        }
        protected override void OnStart()
        {
            GetReferences();

            if (!RandomizeQuestions)
                ActualQuestion = -1;

            _quizController = _bootstrap.GetController(ControllerTypes.Quiz) as QuizController;

            base.OnStart();
        }
        protected override void ResetData()
        {
            if (!OverrideDB)
                Questions = new List<Question>();

            QuestionsUsed = new List<int>();
        }
        protected override void GetReferences()
        {
            base.GetReferences();

#if HAS_SERVER
            if (GetDataFromServerOnConnect)
                _clientController.OnConnect += RequestQuestionsFromServer;
#endif
        }
        public override void CallReset()
        {
            base.CallReset();

            if (ResetAlreadyChosen)
                ResetChosen();
        }
        #endregion

        #region Questions Methods
        private void AddNewQuestion(Question question)
        {
            Questions.Add(question);
        }
        public void NextQuestion(int type)
        {
            var typeQuestions = Questions.FindAll(x => x.type == type);

            if (RandomizeQuestions)
            {
                /*rnd = new System.Random();
                var i = rnd.Next(0, typeQuestions.Count);

                while (QuestionsUsed.Contains(i) && typeQuestions[i].alreadyChosen)
                {
                    i = rnd.Next(0, typeQuestions.Count);

                    if (QuestionsUsed.Count >= Questions.Count || !typeQuestions[i].alreadyChosen)
                        break;
                }

                typeQuestions[i].alreadyChosen = true;
                ActualQuestion = Questions.FindIndex(x=>x == typeQuestions[i]);*/

                typeQuestions.Shuffle();

                for(int i = 0; i < typeQuestions.Count; i++)
                {
                    if (!QuestionsUsed.Contains(typeQuestions[i].id) && !typeQuestions[i].alreadyChosen)
                    {
                        ActualQuestion = typeQuestions[i].id;
                        QuestionsUsed.Add(ActualQuestion);
                        break;
                    }
                }
            }
            else
            {
                if (ActualQuestion < typeQuestions.Count - 1)
                    ActualQuestion++;
            }


            if (QuestionsUsed.Count >= typeQuestions.Count)
            {
                if (ResetAlreadyChosen)
                {
                    if (OnQuestionsOver != null) OnQuestionsOver();
                    return;
                }
                else
                {
                    ResetChosen();
                    NextQuestion(type);
                    return;
                }
            }

            Debug.Log("Next Question " + ActualQuestion);

            QuestionsUsed.Add(ActualQuestion);

            if (OnNewQuestionReady != null) OnNewQuestionReady();
        }
        public void NextQuestion()
        {
            if (RandomizeQuestions)
            {
                rnd = new System.Random();
                var i = rnd.Next(0,Questions.Count);

                while (QuestionsUsed.Contains(i) && Questions[i].alreadyChosen)
                {
                    i = rnd.Next(0, Questions.Count);

                    if(QuestionsUsed.Count >= Questions.Count)
                        break;
                }

                Questions[i].alreadyChosen = true;
                ActualQuestion = i;
            }
            else
            {
                if (ActualQuestion < Questions.Count-1)
                    ActualQuestion++;
            }

            if (QuestionsUsed.Count >= Questions.Count)
            {
                if (ResetAlreadyChosen)
                {
                    if (OnQuestionsOver != null) OnQuestionsOver();
                    return;
                }
                else
                {
                    ResetChosen();
                    NextQuestion();
                    return;
                }
            }

            QuestionsUsed.Add(ActualQuestion);

            if (OnNewQuestionReady != null) OnNewQuestionReady();
        }
        void ResetChosen()
        {
            ActualQuestion = -1;
            QuestionsUsed = new List<int>();

            for (int i = 0, n = Questions.Count; i < n; i++)
            {
                Questions[i].alreadyChosen = false;
            }
        }
        public void PreviousQuestion()
        {
            if (!RandomizeQuestions)
            {
                if (ActualQuestion > 0)
                    ActualQuestion --;
            }
            if (OnNewQuestionReady != null) OnNewQuestionReady();
        }
        public Question GetQuestion()
        {
            if (_quizController.HasTypes)
            {
                List<Question> list = Questions.FindAll(x => x.type == _quizController.type);
                return list.Find(x => x.id == ActualQuestion);
            }
            else
                return Questions[ActualQuestion];
        }
        public bool CheckAnswer(int i)
        {
            if (_quizController.HasTypes)
            {
                List<Question> list = Questions.FindAll(x => x.type == _quizController.type);
                return list.Find(x => x.id == ActualQuestion).alternatives.Exists(x => x.id == i && x.isRight);
            }
            else
                return Questions[ActualQuestion].alternatives.Exists(x=>x.id == i && x.isRight);
        }
        #endregion

        #region Serialization
        public override string SerializeDataBase()
        {
            return JsonConvert.SerializeObject(Questions, Formatting.Indented);
        }
        public override void DeserializeDataBase(string json)
        {
            //TextAsset tAsset = Resources.Load("questions") as TextAsset;
            Questions = JsonConvert.DeserializeObject<List<Question>>(json);
        }
        public override void UpdateDataBase(string folderData)
        {
            var data = JsonConvert.DeserializeObject<List<Question>>(folderData);

            for (int i = 0, n = data.Count; i < n; i++)
            {
                if (Questions.Exists(x => x.id == data[i].id && x.type == data[i].type))
                {
                    Questions.Find(x => x.id == data[i].id && x.type == data[i].type).alreadyChosen = data[i].alreadyChosen;
                    Questions.Find(x => x.id == data[i].id && x.type == data[i].type).alternatives = data[i].alternatives;
                    Questions.Find(x => x.id == data[i].id && x.type == data[i].type).title = data[i].title;
                    Questions.Find(x => x.id == data[i].id && x.type == data[i].type).weight = data[i].weight;
                }
                else
                {
                    Questions.Add(data[i]);
                }
            }
        }
        #endregion
        
        #region Networking specific methods
#if HAS_SERVER
        #region Server methods
        public void RequestQuestions(string json)
        {
            if (!_isServer) return;

            _serverController.SendMessageToAll("NetworkStartReceiveQuestions", "");

            for (int i = 0, n = Questions.Count; i < n; i++)
            {
                _serverController.SendMessageToAll("NetworkSetQuestions", JsonConvert.SerializeObject(Questions[i]));
            }

            _serverController.SendMessageToAll("NetworkSetQuestionsEnd", "");

            DebugLog("RequestQuestions - All Data Send");
        }
        #endregion
        #region Client methods
        public void RequestQuestionsFromServer()
        {
            if (_isServer) return;

            _clientController.SendMessageToServer("RequestQuestions", "");
        }

        #region Receive all data from server process
        public void NetworkStartReceiveQuestions(string json)
        {
            if (_isServer) return;

            Questions = new List<Question>();
        }
        public void NetworkSetQuestions(string json)
        {
            if (_isServer) return;

            AddNewQuestion(JsonConvert.DeserializeObject<Question>(json));
        }
        public void NetworkSetQuestionsEnd(string json)
        {
            if (_isServer) return;

            DebugLog("NetworkSetQuestions");
            _IOController.Save(this);
        }
        #endregion
        #endregion
        public void SendActualQuestion()
        {
            if (_isServer)
            {
                _serverController.SendMessageToAll("SetActualQuestion", JsonConvert.SerializeObject(ActualQuestion));
            }
            else
            {
                _clientController.SendMessageToServer("SetActualQuestion", JsonConvert.SerializeObject(ActualQuestion));
            }
        }
        public void SetActualQuestion(string json)
        {
            var ac = JsonConvert.DeserializeObject<int>(json);
            ActualQuestion = ac;

            if (OnNewQuestionReady != null) OnNewQuestionReady();
        }
#endif
        #endregion
    }


#if HAS_SQLITE3
    [System.Serializable]
    public class Question
    {
        public string title { get; set; }

        [PrimaryKey]
        public int id { get; set; }
        public bool alreadyChosen { get; set; }
        public int type { get; set; }

        public int weight { get; set; }

        [Ignore]
        public List<Alternatives> alternatives { get; set; }

        [Indexed]
        public int alternativesIndex { get; set; }


        [System.Serializable]
        public class Alternatives
        {
            public string title { get; set; }
            public int id { get; set; }
            [PrimaryKey]
            public int tableId { get; set; }
            [Indexed]
            public int referenceIndex { get; set; }
            public bool isRight { get; set; }
        }
    }
#else
    [System.Serializable]
    public class Question
    {
        public string title;
        public int id;
        public bool alreadyChosen;
        public int type;
        public int weight;
        public List<Alternatives> alternatives;

        [System.Serializable]
        public class Alternatives
        {
            public string title;
            public int id;
            public bool isRight;
        }
    }
#endif
}