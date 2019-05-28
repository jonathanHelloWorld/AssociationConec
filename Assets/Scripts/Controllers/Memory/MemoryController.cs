using UnityEngine;
using System.Collections;
using InterativaSystem.Controllers.Sound;
using InterativaSystem.Models;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using InterativaSystem.Views;
using System.Linq;
using InterativaSystem.Interfaces;
using InterativaSystem.Views.Grid;
using Interativa.Views.MemoryGame;

namespace InterativaSystem.Controllers
{
    [AddComponentMenu("ModularSystem/Controllers/Memory Controller")]
    public class MemoryController : GenericController, IContainer
    {
        ImagesDataBase _images;
        //ScoreData _score;

        PagesController _pagesController;
        ScoreController _scoreController;
        TimeController _timeController;
        SFXController _sfxController;
        RegisterController _registerController;

        ResourcesDataBase _resources;
        GridDatabase _gridDatabase;

        List<GridCanvas2D> _grids;
        int currentGrid = 0;

        public Transform Container { get; set; }

        bool gameAlreadyPrepared = false;
        bool gameWon = false;

        public event SimpleEvent RotateErrors, LockCards, UnlockCards, RotateCards, OnMemoryStart;
        public event IntEvent OnScore;

        public float gameStartDelay = 3f;
        public float gameDuration = 60f;
        public int qntMatches = 6;
        public int numAnswers = 0;
        public float rotationDuration;
        public bool hasTimer = false;
        public int defaultScoreCorrect;
        public int defaultScoreError;

        public List<CardBack> backs;

        [Space(10f)]
        [Header("Audios")]
        public string victorySfx = "VictoryMemory";
        public string gameOverSfx = "GameOver";

        [Space(10f)]
        [Header("Pages")]
        public int idVictory = 1;
        public int idDefeat = 2;

        float tempScore = 0f;

        List<MemoryCard> _cards;
        List<List<Sprite>> _cardsPool;

        Sprite _cardBack;

        List<MemoryCard> answers;
        int correctAnswers = 0;

        void Awake()
        {
            Type = ControllerTypes.MemoryGame;

            answers = new List<MemoryCard>();
        }

        protected override void OnStart()
        {
            base.OnStart();

            _pagesController = _bootstrap.GetController(ControllerTypes.Page) as PagesController;
            _scoreController = _bootstrap.GetController(ControllerTypes.Score) as ScoreController;
            _timeController = _bootstrap.GetController(ControllerTypes.Time) as TimeController;
            _sfxController = _bootstrap.GetController(ControllerTypes.SoundSFX) as SFXController;
            _registerController = _bootstrap.GetController(ControllerTypes.Register) as RegisterController;

            _resources = _bootstrap.GetModel(ModelTypes.Resources) as ResourcesDataBase;
            _images = _bootstrap.GetModel(ModelTypes.Images) as ImagesDataBase;
            _gridDatabase = _bootstrap.GetModel(ModelTypes.Grid) as GridDatabase;

            _bootstrap.AppStarted += () =>
            {
                _grids = new List<GridCanvas2D>();
                List<Models.GridLayout> gList = _gridDatabase.GetGridsByGame(Type);

                for (int i = 0; i < gList.Count; i++)
                {
                    _grids.Add(new GridCanvas2D(gList[i]));
                }
            };

            _cardsPool = new List<List<Sprite>>();
            _cards = new List<MemoryCard>();

            OnGamePrepare += GamePrepare;
            OnGameEnd += GameEnded;

            if(hasTimer)
                _timeController.GameTimeout += TimeOut;

            _cardsPool = _images.GetImagesByCategory(ImageCategory.MemoryCard, true);
            _cardBack = _images.GetImageByCategory(ImageCategory.MemoryBack);
        }

        void GamePrepare()
        {
            //_cards.Clear();
            _cardsPool.Shuffle();

            if(hasTimer)
                _timeController.GameTimeLimit = gameDuration;

            if (!gameAlreadyPrepared)
            {
                gameAlreadyPrepared = true;

                string cardPrefab = _resources.Prefabs.Find(x => x.category == PrefabCategory.MemoryCardPrefab).name;

                for (int i = 0; i < qntMatches * numAnswers; i++)
                {
                    GameObject gObj = Instantiate(Resources.Load<GameObject>(cardPrefab)) as GameObject;

                    if (Container != null)
                        gObj.transform.SetParent(Container);

                    gObj.GetComponent<MemoryCard>().Initialize();

                    _cards.Add(gObj.GetComponent<MemoryCard>());
                }
            }

            List<Vector2> list = new List<Vector2>();

            for(int i = 0; i < qntMatches; i++)
            {
                for (int j = 0; j < numAnswers; j++)
                {
                    Vector2 pos;

                    if (j < _cardsPool[i].Count)
                    {
                        if (_cardsPool[i].ElementAtOrDefault(j) != null)
                            pos = new Vector2(i, j);
                        else
                            pos = new Vector2(i, 0);
                    }
                    else
                        pos = new Vector2(i, 0);

                    list.Add(pos);
                }
            }

            list.Shuffle();

            for(int i = 0; i < _cards.Count; i++)
            {
                _cards[i].ResetCard();
                _cards[i].SetAttributes((int)list[i].x, _cardsPool[(int)list[i].x][(int)list[i].y], rotationDuration);

                /*if(_cards[i].cardFace.name.Contains("klm"))
                    _cb = backs.Find(x => x.id == "KLM").sprite;
                else if (_cards[i].cardFace.name.Contains("af"))
                    _cb = backs.Find(x => x.id == "AF").sprite;
                else if (_cards[i].cardFace.name.Contains("dl"))
                    _cb = backs.Find(x => x.id == "DL").sprite;
                else if (_cards[i].cardFace.name.Contains("gol"))
                    _cb = backs.Find(x => x.id == "GOL").sprite;
                */

                _cards[i].SetBack(_cardBack);
            }

            List<GridPiece> gridPieces = _grids[currentGrid].GetAllPieces();

            for (int i = 0; i < _cards.Count; i++)
            {
                _cards[i].transform.localPosition = gridPieces[i].position;
            }

            if (LockCards != null) LockCards();
        }

        public override void StartGame()
        {
            base.StartGame();
            StartCoroutine(GameStart());
        }

        IEnumerator GameStart()
        {
            for (int i = 0; i < _cards.Count; i++)
            {
                _cards[i].StartCard();
            }

            yield return new WaitForSeconds(gameStartDelay);

            if (OnMemoryStart != null)
                OnMemoryStart();

            StartCoroutine(StartUnlock());
        }

        IEnumerator StartUnlock()
        {
            if(RotateCards != null)
                RotateCards();

            yield return new WaitForSeconds(rotationDuration);

            if(UnlockCards != null) UnlockCards();
        }

        void TimeOut()
        {
            _bootstrap.EndGame(this);

            _pagesController.GoToNextPage();
        }

        void GameEnded()
        {
            if (LockCards != null)
                LockCards();

            if (gameWon)
            {
                if (_sfxController != null)
                    _sfxController.PlaySound(victorySfx);

                _scoreController.AddScore((int)tempScore, _timeController.TimeSinceGameStart);
                //CallAction(idVictory);
            }
            else
            {
                if (_sfxController != null)
                    _sfxController.PlaySound(gameOverSfx);

                //CallAction(idDefeat);
                _scoreController.AddScore(0, _timeController.TimeSinceGameStart);
            }

            if (_registerController != null)
            {
                _registerController.AddRegisterValue("MemoryWon", gameWon.ToString(), false);
                _registerController.AddRegisterValue("MemoryScore", tempScore.ToString(), false);
            }
        }

        protected override void CallReset()
        {
            answers.Clear();

            gameWon = false;
            LockCards = null;
            UnlockCards = null;

            correctAnswers = 0;
            tempScore = 0f;

            if(_timeController != null)
                _timeController.GameTimeLimit = gameDuration;

            //if (ScoreToUpdate != null)
                //ScoreToUpdate("0");
        }

        void OpenPopup(int popup)
        {
            _timeController.Paused = true;

            CallAction(popup);
        }

        public void ClosePopup()
        {
            _timeController.Paused = false;

            CallAction(0);

            if (correctAnswers >= qntMatches)
                _pagesController.Invoke("GoToNextPage", 1f);
        }

        public bool CheckPaused()
        {
            return _timeController.Paused;
        }

        public void CheckCard(MemoryCard _card)
        {
            if (answers.Count >= numAnswers) return;

            if (answers.Count < numAnswers)
            {
                answers.Add(_card);
                RotateErrors += _card.Rotate;
            }

            if (answers.Count >= numAnswers)
            {
                if (LockCards != null) LockCards();

                bool notEqual = answers.Any(o => o.id != answers[0].id);

                if (!notEqual)
                {
                    for(int i = 0; i < answers.Count; i++)
                    {
                        MemoryCard card = answers[i].GetComponent<MemoryCard>();
                        card.RemoveEvents();
                    }

                    correctAnswers++;
                    tempScore += defaultScoreCorrect;

                    int pID = 0;

                    if (_card.cardFace.name.Contains("COBALT"))
                        pID = 1;
                    else if(_card.cardFace.name.Contains("CRUZE"))
                        pID = 2;
                    else if (_card.cardFace.name.Contains("EQUINOX"))
                        pID = 3;
                    else if (_card.cardFace.name.Contains("ONIX"))
                        pID = 4;
                    else if (_card.cardFace.name.Contains("PRISMA"))
                        pID = 5;
                    else if (_card.cardFace.name.Contains("SPIN"))
                        pID = 6;

                    OpenPopup(pID);

                    if (OnScore != null)
                        OnScore(correctAnswers);

                    ClearAnswers();
                }
                else
                {
                    tempScore += defaultScoreError;
                    Invoke("ErrorFeedback", rotationDuration + 0.5f);
                }

                if (_sfxController != null)
                {
                    if (!notEqual)
                    {
                        Debug.Log("SfxCerto");
                        _sfxController.PlaySound("Right");
                    }
                    else
                    {
                        Debug.Log("SfxErro");
                        _sfxController.PlaySound("Wrong");
                    }
                }

                //if(ScoreToUpdate != null)
                    //ScoreToUpdate(tempScore.ToString());
            }

            if (correctAnswers >= qntMatches)
            {
                gameWon = true;
                Invoke("EndGame", 2f);
            }
        }

        void ClearAnswers()
        {
            answers.Clear();
            RotateErrors = null;
            if(UnlockCards != null) UnlockCards();
        }

        void ErrorFeedback()
        {
            if(RotateErrors != null) RotateErrors();
            ClearAnswers();
        }
    }
}

[System.Serializable]
public class CardBack
{
    public string id;
    public Sprite sprite;
}