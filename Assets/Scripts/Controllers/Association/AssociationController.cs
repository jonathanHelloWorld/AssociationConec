using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using InterativaSystem.Controllers.Sound;
using InterativaSystem.Models;
using InterativaSystem.Views.EnviromentComponents;
using TouchScript.Behaviors;
using TouchScript.Gestures;
using UnityEngine;

namespace InterativaSystem.Controllers
{
    [AddComponentMenu("ModularSystem/Controllers/Association Controller")]
    public class AssociationController : GenericController
    {
        public  SimpleEvent OnAssociation, OnAssociationFeedback;
        public IntEvent OnContentChosed, OnRoundChange;
        
        protected ScoreController _scoreController;
        protected TimeController _timeController;
        protected SoundController _SoundController;
        private QuestionsData _questionsData;
        protected ResourcesDataBase _resourcesData;
        private RegisterController _register;

        protected AssociationData _AssociationData;

        private List<AssociationObject> _associationObjects;
        private List<AssociationObjectContent> _associationObjectsContents;

        protected List<GameObject> instancedPieces; 

        public bool GenerateFromModel = true;
        public bool WaitToShowFeedback;

        public float DelayTime;

        public int WinPoints;
        public int AssociationRounds;

        public int AssociationsCount;
        public int AssociationsTrys = 10;

        public int StaticAssociated;

        public int MinimunIdRepeat = 1;

        private int associationsRounds;

        private int _nAssociated, _nAssocTrys;

        private int _qType;

        void Awake()
        {
            //Mandatory set for every controller
            Type = ControllerTypes.Association;

            _nAssocTrys = 0;
            _nAssociated = 0;
            _associationObjects = new List<AssociationObject>();
            _associationObjectsContents = new List<AssociationObjectContent>();
        }

        protected override void OnStart()
        {
            base.OnStart();

            _scoreController = _bootstrap.GetController(ControllerTypes.Score) as ScoreController;
            _register = _bootstrap.GetController(ControllerTypes.Register) as RegisterController;

            _resourcesData = _bootstrap.GetModel(ModelTypes.Resources) as ResourcesDataBase;

            _timeController = _bootstrap.GetController(ControllerTypes.Time) as TimeController;
            if (_timeController!=null)
                _timeController.GameTimeout += TimeOut;

            _SoundController = _bootstrap.GetController(ControllerTypes.SoundSFX) as SoundController;
            _questionsData = _bootstrap.GetModel(ModelTypes.Questions) as QuestionsData;
            _AssociationData = _bootstrap.GetModel(ModelTypes.Association) as AssociationData;
        }

        public override void PrepareGame()
        {
            if (GenerateFromModel)
            {
                GenereateAssociationFromModel();
                return;
            }

            if (_questionsData == null)
                _questionsData = _bootstrap.GetModel(ModelTypes.Questions) as QuestionsData;

            _questionsData.NextQuestion();


            if (_associationObjectsContents.Count == 0)
                RandomizeAssociationObjects();
            else
                RandomizeAssociationObjectsContents();

            base.PrepareGame();
        }

        public void PrepareGame(int questionIndex)
        {
            if (GenerateFromModel)
            {
                GenereateAssociationFromModel();
                return;
            }

            if (_questionsData == null)
                _questionsData = _bootstrap.GetModel(ModelTypes.Questions) as QuestionsData;

            _qType = questionIndex;
            _questionsData.ActualQuestion = questionIndex;



            if (_associationObjectsContents.Count == 0)
                RandomizeAssociationObjects();
            else
                RandomizeAssociationObjectsContents();

            base.PrepareGame();
        }

        private void GenereateAssociationFromModel()
        {
            var round = _AssociationData.GetActualRound();
            round.pieces = round.pieces.OrderByDescending(x => x.isStatic).ToList();

            AssociationsCount = round.pieces.Count(x => x.isStatic);

            DestroyPieces();
            instancedPieces = new List<GameObject>();

            for (int i = 0, n = round.pieces.Count; i < n; i++)
            {
                InstantiatePiece(round.pieces[i]);
            }


            _timeController.StartGameTimer();
            if (OnRoundChange != null) OnRoundChange(_AssociationData.GetRound());
        }

        void InstantiatePiece(AssociationPiece piece)
        {
            /*
            var types = new Type[]
            {
                typeof (RectTransform),
                typeof (Rigidbody),
                typeof (Transformer2D),
                typeof (PanGesture),
                typeof (AssociationObject)
            };
            /**/
            var temp = Instantiate(Resources.Load<GameObject>(_resourcesData.Prefabs.Find(x => x.category == PrefabCategory.AssociationObjectDynamic).name)); //new GameObject("Piece_" + piece.id + (piece.isStatic ? "_S" : "_D"), types);

            temp.transform.SetParent(_AssociationData.Root);

            temp.transform.localPosition = piece.pos;
            temp.transform.localEulerAngles = Vector3.zero;
            temp.transform.localScale = Vector3.one;

            instancedPieces.Add(temp);

            var ao = temp.GetComponent<AssociationObject>();
            ao.Initialize(piece);
        }

        void DestroyPieces()
        {
            if (instancedPieces == null) return;
            for (int i = 0; i < instancedPieces.Count; i++)
            {
                Destroy(instancedPieces[i]);
            }
        }

        #region Getters
        #endregion

        #region Obsoletes
        [Obsolete]
        void RandomizeAssociationObjects()
        {
            var rnd = new System.Random();

            var maxtypes = _associationObjects.Max(x => x.Type);

            var alts = _questionsData.Questions[_questionsData.ActualQuestion].alternatives;

            var maxIds = alts.Max(x => x.id);

            for (int type = 0; type <= maxtypes; type++)
            {
                var choosed = new List<int>();
                var minIdCount = new List<int>();
                int idCount = 0;

                var assocs = _associationObjects.FindAll(x => x.Type == type && !x.IsStatic);

                for (int i = 0, n = assocs.Count; i < n; i++)
                {
                    var num = rnd.Next(0, alts.Count);

                    while (choosed.Contains(num) && choosed.Count < alts.Count || idCount <= maxIds && choosed.Count < alts.Count && alts[num].id != idCount || choosed.Count < alts.Count && minIdCount.Contains(num))
                    {
                        num = rnd.Next(0, alts.Count);
                    }

                    assocs[i].Id = alts[num].id;
                    assocs[i].QuestionId = num;

                    choosed.Add(num);
                    minIdCount.Add(num);

                    if (minIdCount.Count >= MinimunIdRepeat)
                    {
                        idCount++;

                        minIdCount = new List<int>();
                    }
                }
            }
        }

        [Obsolete]
        void RandomizeAssociationObjectsContents()
        {
            var rnd = new System.Random();

            var maxtypes = _questionsData.Questions.Max(x => x.type);

            int assocCount = 0;

            //Used for diferent games in pages
            for (int type = 0; type <= maxtypes; type++)
            {
                var alts = _questionsData.Questions[type].alternatives;
                var maxIds = alts.Max(x => x.id);

                var choosed = new List<int>();
                var choosedID = new List<int>();
                var minIdCount = new List<int>();

                var assocCs = _associationObjectsContents.FindAll(x => x.Type == type);

                // todo Criar todos por instanciacao
                var assocs = _associationObjects.FindAll(x => x.Type == type && !x.IsStatic);

                int idCount = 0;

                //Get Ids for every content
                for (int i = 0, qnt = assocCs.Count; i < qnt; i++)
                {
                    //Generate Id for content
                    var csid = rnd.Next(0, maxIds + 1);
                    while (choosedID.Contains(csid) && choosedID.Count < maxIds + 1)
                    {
                        csid = rnd.Next(0, maxIds + 1);
                    }
                    var csidCount = alts.FindAll(x => x.id == csid);
                    choosedID.Add(csid);

                    if (type == _qType)
                        if (OnContentChosed != null) OnContentChosed(csid);

                    idCount = csid;

                    assocCs[i].Id = csid;

                    //Get data for dinamic associations
                    for (int j = 0, n = csidCount.Count; j < n; j++)
                    {
                        assocCs[i].AddContent(csid);

                        if (type == _qType)
                        {
                            assocCount++;
                        }

                        var num = rnd.Next(0, alts.Count);
                        while (choosed.Contains(num) && choosed.Count < alts.Count ||
                                idCount <= maxIds && choosed.Count < alts.Count && alts[num].id != idCount ||
                                choosed.Count < alts.Count && minIdCount.Contains(num)
                               )
                        {
                            num = rnd.Next(0, alts.Count);
                        }


                        if (j < assocs.Count && assocs[j] != null)
                        {
                            assocs[j].Id = alts[num].id;
                            assocs[j].QuestionId = num;
                        }

                        choosed.Add(num);
                        minIdCount.Add(num);

                        /*
                        if (minIdCount.Count >= MinimunIdRepeat)
                        {
                            idCount++;

                            minIdCount = new List<int>();
                        }
                        /**/
                    }

                    for (int j = csidCount.Count-1; j >= 0; j--)
                    {
                        try
                        {
                            assocs.RemoveAt(j);
                        }
                        catch (Exception e)
                        {
                            Debug.Log(e);
                        }
                    }
                }

                for (int j = 0, n = assocs.Count; j < n; j++)
                {
                    var num = rnd.Next(0, alts.Count);

                    while (choosed.Contains(num) && choosed.Count < alts.Count)
                    {
                        num = rnd.Next(0, alts.Count);
                    }

                    assocs[j].Id = alts[num].id;
                    assocs[j].QuestionId = num;

                    choosed.Add(num);
                    minIdCount.Add(num);
                }
            }

            AssociationsCount = assocCount;
        }

        [Obsolete]
        public void AddAssociationObject(AssociationObject obj)
        {
            if (!_associationObjects.Contains(obj))
                _associationObjects.Add(obj);
            else
                Debug.LogError(obj.gameObject + " already exists in collection");
        }
        [Obsolete]
        public void AddAssociationObjectContent(AssociationObjectContent obj)
        {
            if (!_associationObjectsContents.Contains(obj))
                _associationObjectsContents.Add(obj);
            else
                Debug.LogError(obj.gameObject + " already exists in collection");
        }
        #endregion

        void OnWin()
        {
            _register.AddRegisterValue("GameResult", "win", false);
            _register.AddRegisterValue("GameTrys", _nAssocTrys.ToString(), false);
            Debug.Log("Win");
            CallAction(5);
            if (_scoreController != null)
                _scoreController.AddScore(WinPoints, _timeController.TimeSinceGameStart);
        }
        void OnLose()
        {
            _register.AddRegisterValue("GameResult", "lose", false);
            _register.AddRegisterValue("GameTrys", _nAssocTrys.ToString(), false);
            Debug.Log("Lose");
            CallAction(10);
            if (_scoreController != null)
                _scoreController.AddScore(0, _timeController.TimeSinceGameStart);
        }

        public override void StartGame()
        {
            base.StartGame();

            CallAction(0);

            //if (WaitToShowFeedback)
                //_nAssocTrys = 0;
        }

        protected void TimeOut()
        {
            if (GenerateFromModel)
            {
                associationsRounds++;

                if (associationsRounds >= AssociationRounds)
                {

                    associationsRounds = 0;
                    _nAssociated = 0;
                    _bootstrap.EndGame(this);
                }
                else if (_AssociationData.NewRound())
                {
                    Invoke("NextRound", DelayTime);
                }
                else
                {
                    _nAssociated = 0;
                    associationsRounds = 0;
                    _bootstrap.EndGame(this);
                }

                _timeController.StartGameTimer();
                OnLose();
                return;
            }


            OnLose();
            _bootstrap.EndGame(this);
        }

        protected override void CallReset()
        {
            base.CallReset();

            CallAction(0);
            associationsRounds = 0;
            _nAssociated = 0;
            _nAssocTrys = 0;
        }

        public void Associated(bool right)
        {
            if(!IsGameRunning) return;

            if (right)
            {
                _nAssociated++;

                if (!WaitToShowFeedback)
                    _SoundController.PlaySound("Right");
            }
            else
            {
                if (!WaitToShowFeedback)
                {
                    _nAssocTrys++;
                    _SoundController.PlaySound("Wrong");
                }
            }
            
            if (_nAssociated >= AssociationsCount || _nAssocTrys >= AssociationsTrys)
            {
                if (GenerateFromModel)
                {
                    associationsRounds++;

                    if (associationsRounds >= AssociationRounds)
                    {
                        if (!WaitToShowFeedback)
                        {
                            OnWin();
                            _bootstrap.EndGame(this);
                        }
                    }
                    else if (_AssociationData.NewRound())
                    {
                        Invoke("NextRound", DelayTime);
                        OnWin();
                    }
                    if (_nAssocTrys >= AssociationsTrys)
                    {
                        OnLose();
                        _bootstrap.EndGame(this);
                    }
                    else
                    {
                        OnWin();
                        _bootstrap.EndGame(this);
                    }
                }
                else
                {
                    _bootstrap.EndGame(this);
                }
            }

            //Debug.Log(right + ": " + _nAssociated.ToString("00") + "/" + AssociationsCount.ToString("00"));
            if (OnAssociation != null) OnAssociation();
        }
        
        public void WaitAssociated()
        {
            if(!WaitToShowFeedback) return;

            StaticAssociated++;
        }

        public void WaitReturn()
        {
            if (!WaitToShowFeedback) return;

            StaticAssociated--;
        }

        void NextRound()
        {
            _timeController.StartGameTimer();
            if (OnRoundChange != null) OnRoundChange(_AssociationData.GetRound());
            _nAssociated = 0;
            PrepareGame();
            StartGame();
        }

        public void SendFeedback()
        {
            if (OnAssociationFeedback != null) OnAssociationFeedback();
            _nAssocTrys++;

            Associated(false);
        }

        public int GetTrys()
        {
            return _nAssocTrys;
        }

        public int GetDefinedQuestion()
        {
            return _questionsData.ActualQuestion;
        }

        public string GetRoundTittle()
        {
            return _AssociationData.GetRoundTittle();
        }

        public string GetRoundTittle(bool isB)
        {
            return _AssociationData.GetRoundTittle(isB);
        }
    }
}