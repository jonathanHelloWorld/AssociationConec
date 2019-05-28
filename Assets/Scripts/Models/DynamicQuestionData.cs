using System.Collections.Generic;
using InterativaSystem.Controllers;
using Newtonsoft.Json;
using UnityEngine;

namespace InterativaSystem.Models
{
    [AddComponentMenu("ModularSystem/DataModel/ Dynamic Questions")]
    public class DynamicQuestionData : DataModel
    {
        //public event GenericController.SimpleEvent OnQuestionsOver, OnNewQuestionReady;

        private DynamicQuizPointBased _dynamicQuiz;

        [HideInInspector]
        public int QuestionType = 0;

        public int ActualQuestion;

        public List<DynamicQuestion> Questions;

#if HAS_TURNING
#endif

#if HAS_SQLITE3
#endif

        void Awake()
        {
            //Mandatory set for every model
            Type = ModelTypes.DynamicQuestions;

            if (!OverrideDB)
                Questions = new List<DynamicQuestion>();
        }
        protected override void OnStart()
        {
            if (!OverrideDB && !_IOController.TryLoad(this))
            {
                _IOController.Save(this);
            }
            else if (OverrideDB)
            {
                _IOController.Save(this);
            }

            _dynamicQuiz = _bootstrap.GetController(ControllerTypes.DynamicQuiz) as DynamicQuizPointBased;
        }

        public override void CallReset()
        {
            base.CallReset();

            for (int i = 0, n = Questions.Count; i < n; i++)
            {
                Questions[i].alreadyChosen = false;
            }
        }
        
        public bool CheckAnswer(int i)
        {
            if (Questions.Exists(x => x.maxRange >= _dynamicQuiz.Points && x.minRange <= _dynamicQuiz.Points))
            {
                var quest = Questions.Find(x => x.maxRange >= _dynamicQuiz.Points && x.minRange <= _dynamicQuiz.Points);
                
                return quest.answer == i;
            }
            else
            {
                return false;
            }
        }

        //This methods will serialize an deserialize from a json data
        public override string SerializeDataBase()
        {
            return JsonConvert.SerializeObject(Questions, Formatting.Indented);
        }
        public override void DeserializeDataBase(string json)
        {
            Questions = JsonConvert.DeserializeObject<List<DynamicQuestion>>(json);
        }
        public override void UpdateDataBase(string folderData)
        {
            var data = JsonConvert.DeserializeObject<List<DynamicQuestion>>(folderData);

            for (int i = 0, n = data.Count; i < n; i++)
            {
                if (Questions.Exists(x => x.id == data[i].id && x.type == data[i].type))
                {
                    Questions.Find(x => x.id == data[i].id && x.type == data[i].type).title = data[i].title;
                    Questions.Find(x => x.id == data[i].id && x.type == data[i].type).weight = data[i].weight;
                    Questions.Find(x => x.id == data[i].id && x.type == data[i].type).alreadyChosen = data[i].alreadyChosen;
                    Questions.Find(x => x.id == data[i].id && x.type == data[i].type).minRange = data[i].minRange;
                    Questions.Find(x => x.id == data[i].id && x.type == data[i].type).maxRange = data[i].maxRange;
                    Questions.Find(x => x.id == data[i].id && x.type == data[i].type).answer = data[i].answer;
                }
                else
                {
                    Questions.Add(data[i]);
                }
            }
        }
        /**/
    }

    [System.Serializable]
    public class DynamicQuestion
    {
        public string title;
        public int id;
        public int type;
        public bool alreadyChosen;
        public int weight;
        public int minRange, maxRange;
        public int answer;
    }
}