using UnityEngine;
using System.Collections;
using InterativaSystem.Controllers;
using InterativaSystem.Models;
using InterativaSystem.Views.Grid;
using System.Collections.Generic;
using DG.Tweening;
using InterativaSystem.Interfaces;
using Interativa.Views.CandyGame;
using System.Linq;
using InterativaSystem.Enums;
using UnityEngine.UI;
using InterativaSystem.Controllers.Sound;
using TouchScript;

namespace InterativaSystem.Controllers
{
    [AddComponentMenu("ModularSystem/Controllers/Candy Controller")]
    public class CandyController : GenericController, IContainer
    {
        SFXController _sfxController;
        TimeController _timeController;
        ScoreController _scoreController;
        RegisterController _registerController;

        ResourcesDataBase _resources;
        ImagesDataBase _images;

        public VectorEvent CreateSpecial;
        public FloatEvent NoMovementsPopUp;
        public SimpleEvent KillGels, ShowGels;
        public StringEvent RemainingGelsText;

        [SerializeField]
        GridCanvas2D _grid;

        [Space(20f)]
        public bool randomGels = false;
        public int gelAmount = 8;
        public int piecePoints = 50;

        [Space(20f)]
        public int piecesMegaCount = 4;
        public int piecesSpecialCount = 3;
        public int piecesBreakCount = 2;

        [Space(20f)]
        public bool saveData = true;
        public float gameStartDelay = 2f;
        public float gameDuration = 120f;
        public int qntPieces = 8;
        public float fallDuration = 2f;
        public float moveDuration = 0.2f;
        public float breakDuration = 0.2f;
        public float hintDelay = 2f;
        float hintTime = 0f;

        [Space(10f)]
        public float pulseSelected = 0.8f;

        [Space(10f)]
        [Header("Result Page ID")]
        [SerializeField]
        int victoryPage = 1;
        [SerializeField]
        int timeOutPage = 2;

        [Header("Sounds")]
        public string pieceClickSfx = "Click";
        public string pieceMatchSfx = "Right";
        public string pieceMismatchSfx = "Wrong";
        public string pieceSpecialSfx = "CandySpecial";
        public string pieceMegaSfx = "CandyMega";
        public string pieceFallSfx = "CandyFall";
        public string victorySfx = "VictoryCandy";
        public string gameOverSfx = "GameOver";

        Vector3 pointPos = Vector3.zero;

        [HideInInspector]
        public ITouch touch;

        int count = 0;
        int comboCounter = 0;
        int gelsDestroyed = 0;
        int tempScore = 0;
        float launchFrom = 0f;

        [HideInInspector]
        public bool gameEnded = false;
        [HideInInspector]
        public bool gameStarted = false;
        [HideInInspector]
        public bool gameBuilt = false;
        [HideInInspector]
        public bool autoRunning = true;
        bool gameWon = false;

        [HideInInspector]
        public CandyComboText comboText;

        [HideInInspector]
        public bool isSelecting = false;

        IEnumerator autoCheck, destroyPieces, dropPieces;

        List<List<Sprite>> candyImgs;
        Sprite candySpecial;

        List<CandyEffect> destroyEffectPool;
        List<CandyEffect> verticalPool;
        List<CandyEffect> horizontalPool;
        List<CandyGel> gelPool;

        List<CandyPiece> hintPool;
        List<CandyPiece> megaPool;
        List<CandyPiece> deadPool;

        [HideInInspector]
        public Vector2 mouseStartPos;

        [HideInInspector]
        public Transform Container { get; set; }

        [HideInInspector]
        public CandyPiece candyStart, candyEnd;

        /*void Awake()
        {
            Type = ControllerTypes.CandyGame;
        }

        protected override void OnStart()
        {
            base.OnStart();

            _sfxController = _bootstrap.GetController(ControllerTypes.SoundSFX) as SFXController;
            _scoreController = _bootstrap.GetController(ControllerTypes.Score) as ScoreController;
            _timeController = _bootstrap.GetController(ControllerTypes.Time) as TimeController;
            _registerController = _bootstrap.GetController(ControllerTypes.Register) as RegisterController;

            _images = _bootstrap.GetModel(ModelTypes.Images) as ImagesDataBase;
            _resources = _bootstrap.GetModel(ModelTypes.Resources) as ResourcesDataBase;

            destroyEffectPool = new List<CandyEffect>();
            verticalPool = new List<CandyEffect>();
            horizontalPool = new List<CandyEffect>();

            hintPool = new List<CandyPiece>();
            megaPool = new List<CandyPiece>();
            deadPool = new List<CandyPiece>();

            candyImgs = new List<List<Sprite>>();

            List<Sprite> candies = _images.GetImagesByCategory(ImageCategory.CandyImage);
            List<Sprite> verts = _images.GetImagesByCategory(ImageCategory.CandyVertical);
            List<Sprite> hors = _images.GetImagesByCategory(ImageCategory.CandyHorizontal);

            candySpecial = _images.GetImageByCategory(ImageCategory.CandySpecial);

            for(int i = 0; i < candies.Count; i++)
            {
                candyImgs.Add(new List<Sprite>());

                candyImgs[i].Add(candies[i]);
                candyImgs[i].Add(verts[i]);
                candyImgs[i].Add(hors[i]);
            }

            OnGameStart += GameStart;
            OnGameEnd += GameEnded;
            _timeController.GameTimeout += GameTimeOut;

            _grid.Initialize();
        }

        public override void PrepareGame()
        {
            base.PrepareGame();

            candyImgs.Shuffle();

            if (!gameBuilt)
            {
                gameBuilt = true;

                if (!randomGels)
                    gelAmount = 0;

                if (Container != null)
                    Container.GetComponent<RectTransform>().sizeDelta = new Vector2((_grid.pieceSize.x + _grid.spacing.x) * _grid.size.x, (_grid.pieceSize.y + _grid.spacing.y) * _grid.size.y);

                string candyPrefab = _resources.Prefabs.Find(x => x.category == PrefabCategory.CandyPiece).name;
                string effectPrefab = _resources.Prefabs.Find(x => x.category == PrefabCategory.CandyDestroyEffect).name;

                launchFrom = ((_grid.size.y + 2) * (_grid.pieceSize.y + _grid.spacing.y)) / 2;
                int c = 0;

                for(int i = 0; i < _grid.pieces.Count; i++)
                {
                    for (int j = 0; j < _grid.pieces[i].Count; j++)
                    {
                        GridPiece gridPiece = _grid.pieces[i][j];

                        GameObject destroyEffect = Instantiate(Resources.Load<GameObject>(effectPrefab)) as GameObject;
                        destroyEffect.GetComponent<CandyEffect>().Prepare();

                        GameObject gObj = Instantiate(Resources.Load<GameObject>(candyPrefab)) as GameObject;

                        if (Container != null)
                        {
                            gObj.transform.SetParent(Container);
                            gObj.transform.localScale = Vector3.one;
                            gObj.transform.localRotation = Quaternion.identity;

                            destroyEffect.transform.SetParent(Container);
                            destroyEffect.transform.localScale = Vector3.one;
                            destroyEffect.transform.localRotation = Quaternion.identity;
                        }

                        gObj.GetComponent<CandyPiece>().uniqueId = c;
                        gObj.gameObject.name = "Piece-" + c++;
                        gObj.GetComponent<CandyPiece>().Initialize();
                        deadPool.Add(gObj.GetComponent<CandyPiece>());
                        destroyEffectPool.Add(destroyEffect.GetComponent<CandyEffect>());

                        if (!randomGels)
                        {
                            if (_grid.gridLayout[(int)gridPiece.coodinates.x].row[(int)gridPiece.coodinates.y] == GridPieceType.CandyGel)
                            {
                                gelAmount++;
                            }
                        }
                    }
                }

                for (int i = 0; i < _grid.size.x * 2; i++)
                {
                    GameObject gObj = Instantiate(Resources.Load<GameObject>(candyPrefab)) as GameObject;

                    if (Container != null)
                        gObj.transform.SetParent(Container);

                    gObj.GetComponent<CandyPiece>().uniqueId = c;
                    gObj.gameObject.name = "Piece-" + c++;
                    gObj.GetComponent<CandyPiece>().Initialize();
                    deadPool.Add(gObj.GetComponent<CandyPiece>());
                }

                string gelPrefab = _resources.Prefabs.Find(x => x.category == PrefabCategory.CandyGel).name;
                gelPool = new List<CandyGel>();

                for (int i = 0; i < gelAmount; i++)
                {
                    GameObject gObj = Instantiate(Resources.Load<GameObject>(gelPrefab)) as GameObject;
                    gObj.GetComponent<CandyGel>().Initialize();

                    gelPool.Add(gObj.GetComponent<CandyGel>());

                    if (Container != null)
                        gObj.transform.SetParent(Container);
                }

                string verticalPrefab = _resources.Prefabs.Find(x => x.category == PrefabCategory.CandyVertical).name;
                string horizontalPrefab = _resources.Prefabs.Find(x => x.category == PrefabCategory.CandyHorizontal).name;

                for (int i = 0; i < 10; i++)
                {
                    GameObject vertical = Instantiate(Resources.Load<GameObject>(verticalPrefab)) as GameObject;
                    vertical.GetComponentInChildren<CandyEffect>().Prepare();
                    verticalPool.Add(vertical.GetComponentInChildren<CandyEffect>());

                    GameObject horizontal = Instantiate(Resources.Load<GameObject>(horizontalPrefab)) as GameObject;
                    horizontal.GetComponentInChildren<CandyEffect>().Prepare();
                    horizontalPool.Add(horizontal.GetComponentInChildren<CandyEffect>());

                    if(Container != null)
                    {
                        vertical.transform.SetParent(Container);
                        vertical.transform.localScale = Vector3.one;
                        vertical.transform.localRotation = Quaternion.identity;

                        horizontal.transform.SetParent(Container);
                        horizontal.transform.localScale = Vector3.one;
                        horizontal.transform.localRotation = Quaternion.identity;
                    }
                }
            }
            else
            {
                candyStart = null;
                candyEnd = null;

                for(int i = 0; i < _grid.pieces.Count; i++)
                {
                    for(int j = 0; j < _grid.pieces[i].Count; j++)
                    {
                        GridPiece piece = _grid.pieces[i][j];

                        if (piece.objScript != null)
                        {
                            ((CandyPiece)piece.objScript).transform.DOKill(false);
                            ((CandyPiece)piece.objScript).KillPiece(launchFrom);

                            deadPool.Add(((CandyPiece)piece.objScript));
                            piece.objScript = null;
                        }
                    }
                }
            }

            int pos = 0;
            if (randomGels)
            {
                for (int i = 0; i < _grid.pieces.Count; i++)
                {
                    for(int j = 0; j < _grid.pieces[i].Count; j++)
                    {
                        if (_grid.pieces[i][j].type != GridPieceType.Blank)
                            _grid.pieces[i][j].type = GridPieceType.Normal;
                    }
                }

                while (true)
                {
                    int rngX = Random.Range(0, (int)_grid.size.x);
                    int rngY = Random.Range(0, (int)_grid.size.y);

                    if (_grid.pieces[rngX][rngY].type != GridPieceType.CandyGel && _grid.pieces[rngX][rngY].type != GridPieceType.Blank)
                    {
                        _grid.pieces[rngX][rngY].type = GridPieceType.CandyGel;
                        
                        gelPool[pos].Reset();
                        gelPool[pos].gameObject.SetActive(true);
                        gelPool[pos].gridPiece = _grid.pieces[rngX][rngY];
                        gelPool[pos].transform.localPosition = _grid.pieces[rngX][rngY].position;

                        pos++;
                        if (pos >= gelPool.Count)
                            break;
                    }
                }
            }
            else
            {
                GridPiece piece;
                CandyGel gel;

                for (int row = 0; row < _grid.pieces.Count; row++)
                {
                    for (int col = 0; col < _grid.pieces[row].Count; col++)
                    {
                        piece = _grid.pieces[row][col];

                        if (_grid.gridLayout[(int)piece.coodinates.x].row[(int)piece.coodinates.y] == GridPieceType.CandyGel)
                        {
                            while(true)
                            {
                                if (gelPool[pos].isDestroyed)
                                {
                                    gel = gelPool[pos];
                                    break;
                                }
                            }

                            gel.Reset();
                            gel.gridPiece = piece;
                            gel.gameObject.SetActive(true);
                            gel.transform.localPosition = piece.position;

                            pos++;
                            if (pos >= gelPool.Count)
                                break;
                        }
                    }
                }
            }

            if (RemainingGelsText != null)
                RemainingGelsText(gelAmount.ToString());
        }

        public void GameStart()
        {
            if(ShowGels != null)
                ShowGels();

            StartCoroutine(StartGame(gameStartDelay));
        }

        /// <summary>
        /// Calcula movimento entre as peças,
        /// caso nada foi clicado, inicia contagem da dica e/ ou a mostra.
        /// </summary>
        void Update()
        {
            if (autoRunning || gameEnded || !gameStarted) return;

            if (candyStart != null)
            {
                if (candyStart != null)
                {
                    if (_grid.gridLayout[(int)candyStart.gridPiece.coodinates.y].row[(int)candyStart.gridPiece.coodinates.x] == GridPieceType.Blank)
                        return;
                }

                ClearHintPool();

                Vector2 mousePos = touch.Position;

                float distanceX = mouseStartPos.x - mousePos.x;
                float distanceY = mouseStartPos.y - mousePos.y;

                float distance = Mathf.Abs(distanceX) > Mathf.Abs(distanceY) ? _grid.pieceSize.x : _grid.pieceSize.y;

                if (Vector3.Distance(mousePos, mouseStartPos) >= distance / 2)
                {
                    CandyPiece tPiece = GetMouseOverPiece(mouseStartPos, mousePos);

                    if (tPiece != null)
                    {
                        isSelecting = true;

                        if (candyEnd != tPiece)
                        {
                            KillTween();

                            candyEnd = tPiece;

                            candyStart.transform.localScale = Vector3.one;

                            candyStart.transform.DOLocalMove(candyEnd.gridPiece.position, moveDuration, false).Play();
                            candyStart.transform.DOScale(pulseSelected, moveDuration).SetLoops(-1, LoopType.Yoyo).Play();

                            candyEnd.transform.DOLocalMove(candyStart.gridPiece.position, moveDuration, false).Play();
                            candyEnd.transform.DOScale(pulseSelected, moveDuration).SetLoops(-1, LoopType.Yoyo).Play();

                            ClearMegaPool();

                            if (candyStart.candyType == CandyType.Mega && candyEnd.candyType != CandyType.Mega)
                            {
                                megaPool = FillMegaPool(candyEnd);
                            }
                            else if (candyStart.candyType != CandyType.Mega && candyEnd.candyType == CandyType.Mega)
                            {
                                megaPool = FillMegaPool(candyStart);
                            }
                            else if (candyStart.candyType == CandyType.Mega && candyEnd.candyType == CandyType.Mega)
                            {
                                megaPool = FillMegaPool(candyEnd);
                            }
                        }
                    }
                }
            }
            else
            {
                hintTime += Time.deltaTime;

                if (hintTime >= hintDelay)
                {
                    hintTime = 0f;
                    ShowHint();
                }
            }
        }

        /// <summary>
        /// Mostra dica,
        /// caso não possui dica, destrói todas as peças e reseta o grid.
        /// </summary>
        void ShowHint()
        {
            if (!FindHint())
            {
                //DebugLog("No movements. Candy Shuffle!");

                comboCounter = 0;

                candyStart = null;
                candyEnd = null;

                List<CandyPiece> allPieces = GetAllPieces();

                if (NoMovementsPopUp != null)
                    NoMovementsPopUp(1f);

                StartCoroutine(DestroyPieces(allPieces, 1f, true, false));
            }
            else
            {
                for(int i = 0; i < hintPool.Count; i++)
                {
                    hintPool[i].transform.DOScale(pulseSelected, moveDuration).SetLoops(1000, LoopType.Yoyo).Play();
                }
            }
        }

        /// <summary>
        /// Procura a primeira dica, e adiciona à lista "hintPool".
        /// </summary>
        bool FindHint()
        {
            ClearHintPool();

            Vector2[] directions = new Vector2[4]
            {
                new Vector2(0,1),
                new Vector2(1,0),
                new Vector2(0,-1),
                new Vector2(-1,0)
            };

            for (int i = 0; i < _grid.pieces.Count; i++)
            {
                for (int j = 0; j < _grid.pieces[i].Count; j++)
                {
                    GridPiece piece = _grid.pieces[i][j];

                    if (_grid.gridLayout[(int)piece.coodinates.x].row[(int)piece.coodinates.y] == GridPieceType.Blank)
                        continue;

                    for(int k = 0; k < directions.Length; k++)
                    {
                        hintPool.Clear();

                        CandyPiece candyTo = GetPiece(piece.coodinates, directions[k]);

                        if (candyTo == null)
                            continue;

                        Dictionary<string, List<GridPiece>> dictionary = _grid.GetPiecesRow(candyTo.gridPiece.coodinates);

                        List<GridPiece> hList = new List<GridPiece>();
                        List<GridPiece> vList = new List<GridPiece>();

                        hList = GetMatchPieces((CandyPiece)piece.objScript, dictionary["right"], hList);
                        hList = GetMatchPieces((CandyPiece)piece.objScript, dictionary["left"], hList);
                        vList = GetMatchPieces((CandyPiece)piece.objScript, dictionary["top"], vList);
                        vList = GetMatchPieces((CandyPiece)piece.objScript, dictionary["bottom"], vList);


                        if (hList.Count >= piecesBreakCount || vList.Count >= piecesBreakCount)
                        {
                            hintPool.Add((CandyPiece)piece.objScript);
                            hintPool.Add(candyTo);
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Chamado quando o botão esquerdo do mouse é solto,
        /// verifica os tipos de peças e suas combinações,
        /// troca suas posições e destrói peças combinadas.
        /// </summary>
        public void CheckMatch()
        {
            KillTween();
            comboCounter = 1;

            float waitToDestroy = 0f;

            if (candyStart != null && candyEnd != null)
            {
                List<CandyPiece> toDestroy = new List<CandyPiece>();
                List<CandyPiece> toDestroy1 = new List<CandyPiece>();
                List<CandyPiece> toDestroy2 = new List<CandyPiece>();

                if (candyStart.candyType == CandyType.Mega && candyEnd.candyType == CandyType.Mega)
                {
                    List<CandyPiece> allPieces = GetAllPieces();

                    toDestroy1.AddRange(allPieces);
                }
                else if ((candyStart.candyType == CandyType.Mega && candyEnd.candyType == CandyType.Normal) || (candyEnd.candyType == CandyType.Mega && candyStart.candyType == CandyType.Normal))
                {
                    toDestroy1.AddRange(megaPool);

                    if (candyStart.candyType == CandyType.Mega)
                        toDestroy1.Add(candyStart);
                    else if (candyEnd.candyType == CandyType.Mega)
                        toDestroy1.Add(candyEnd);

                    ChangePiecesPosition(candyStart, candyEnd);
                }
                else if ((candyStart.candyType == CandyType.Mega && (candyEnd.candyType == CandyType.Vertical || candyEnd.candyType == CandyType.Horizontal)) ||
                    (candyEnd.candyType == CandyType.Mega && (candyStart.candyType == CandyType.Vertical || candyStart.candyType == CandyType.Horizontal)))
                {
                    waitToDestroy = 0.5f;

                    CandyType toType = CandyType.Vertical;
                    CandyPiece toKill = null;

                    if (candyStart.candyType == CandyType.Mega)
                    {
                        toKill = candyStart;
                        toType = candyEnd.candyType;
                    }
                    else if (candyEnd.candyType == CandyType.Mega)
                    {
                        toKill = candyEnd;
                        toType = candyStart.candyType;
                    }

                    for(int i = 0; i < megaPool.Count; i++)
                    {
                        ChangePieceToSpecial(megaPool[i], toType);
                    }

                    toDestroy1.AddRange(megaPool);

                    if (toKill != null)
                        toDestroy1.Add(toKill);

                    ChangePiecesPosition(candyStart, candyEnd);
                }
                else if ((candyStart.candyType == CandyType.Horizontal || candyStart.candyType == CandyType.Vertical) && (candyEnd.candyType == CandyType.Horizontal || candyEnd.candyType == CandyType.Vertical))
                {
                    toDestroy1.Add(candyStart);
                    toDestroy1.Add(candyEnd);

                    ChangePiecesPosition(candyStart, candyEnd);
                }
                else if (candyStart.candyType == CandyType.Normal || candyEnd.candyType == CandyType.Normal)
                {
                    bool checkStart = CheckPiece(candyStart, candyEnd, out toDestroy1, false);
                    bool checkEnd = CheckPiece(candyEnd, candyStart, out toDestroy2, false);

                    if (checkStart || checkEnd)
                        ChangePiecesPosition(candyStart, candyEnd);
                }

                if (toDestroy1.Count > 0)
                    toDestroy.AddRange(toDestroy1);
                if (toDestroy2.Count > 0)
                    toDestroy.AddRange(toDestroy2);

                if (toDestroy.Count > 0)
                {
                    StartCoroutine(PlayAudio(pieceMatchSfx, 0));
                    StartCoroutine(DestroyPieces(toDestroy, waitToDestroy, true, true));
                }
                else
                    StartCoroutine(PlayAudio(pieceMismatchSfx, 0));
                
            }

            hintTime = 0f;
            isSelecting = false;
            candyStart = null;
            candyEnd = null;
            ClearMegaPool();
        }

        /// <summary>
        /// Verificação automática do grid.
        /// </summary>
        IEnumerator AutoCheckGrid()
        {
            autoRunning = true;

            if (!gameStarted)
                gameStarted = true;

            List<CandyPiece> toDestroy = new List<CandyPiece>();

            for(int i = 0; i < _grid.pieces.Count; i++)
            {
                for (int j = 0; j < _grid.pieces[i].Count; j++)
                {
                    GridPiece piece = _grid.pieces[i][j];

                    toDestroy = new List<CandyPiece>();

                    if (AutoCheckPiece(piece.objScript as CandyPiece, out toDestroy))
                    {
                        if (destroyPieces != null)
                            StopCoroutine(destroyPieces);

                        destroyPieces = DestroyPieces(toDestroy, 0f, false, true);
                        yield return StartCoroutine(destroyPieces);
                    }
                }
            }
        }

        /// <summary>
        /// Verificação automática da peça.
        /// </summary>
        /// <param name="candy">Peça a ser verificada.</param>
        /// <param name="toDestroy">Referência de lista de peças a serem destruídas.</param>
        /// <returns>Retorna um bool indicando se houve combinação.</returns>
        bool AutoCheckPiece(CandyPiece candy, out List<CandyPiece> toDestroy)
        {
            bool check = false;

            check = CheckPiece(candy, candy, out toDestroy, true);

            return check;
        }

        /// <summary>
        /// Verifica se há combinações para a peça,
        /// se sim, verifica seu tipo e chama método apropriado.
        /// </summary>
        /// <param name="candy">Peça a ser verificada.</param>
        /// <param name="candyTo">Peça a ser movida, utilizada para calcular.</param>
        /// <param name="toDestroy">Referência de lista a ser destruída.</param>
        /// <param name="isAuto">Parâmetro passado caso vindo da função de checagem automática, caso true, verifica qual direção do match tem prioridade.</param>
        /// <returns>Retorna um bool indicando se houve combinação.</returns>
        bool CheckPiece(CandyPiece candy, CandyPiece candyTo, out List<CandyPiece> toDestroy, bool isAuto)
        {
            toDestroy = new List<CandyPiece>();

            bool matched = false;

            Dictionary<string, List<GridPiece>> dictionary = new Dictionary<string, List<GridPiece>>();

            if (candyTo != null && candyTo.gridPiece != null)
                dictionary = _grid.GetPiecesRow(candyTo.gridPiece.coodinates);
            else
                return false;

            Vector2 direction = candy.gridPiece.coodinates - candyTo.gridPiece.coodinates;

            List<GridPiece> hList = new List<GridPiece>();
            List<GridPiece> vList = new List<GridPiece>();

            List<GridPiece> tList1 = new List<GridPiece>();
            List<GridPiece> tList2 = new List<GridPiece>();

            hList = GetMatchPieces(candy, dictionary["right"], hList);
            hList = GetMatchPieces(candy, dictionary["left"], hList);
            vList = GetMatchPieces(candy, dictionary["top"], vList);
            vList = GetMatchPieces(candy, dictionary["bottom"], vList);

            if (isAuto)
            {
                if (hList.Count > vList.Count)
                {
                    tList1 = hList;
                    tList2 = vList;
                }
                else
                {
                    tList1 = vList;
                    tList2 = hList;
                }
            }
            else
            {
                tList1 = hList;
                tList2 = vList;

                if ((direction == Vector2.right || direction == Vector2.left) || vList.Count > hList.Count)
                {
                    tList1 = vList;
                    tList2 = hList;
                }
            }

            switch (candy.candyType)
            {
                case CandyType.Normal:

                    matched = CheckPieceNormal(candy, tList1, direction, out toDestroy);

                    if (!matched)
                        matched = CheckPieceNormal(candy, tList2, direction, out toDestroy);
                    break;

                case CandyType.Horizontal:
                    hList = dictionary["right"];
                    hList.AddRange(dictionary["left"]);

                    matched = CheckPieceSpecial(candy, tList1, hList, out toDestroy);

                    if (!matched)
                        matched = CheckPieceSpecial(candy, tList2, hList, out toDestroy);

                    break;

                case CandyType.Vertical:
                    vList = dictionary["top"];
                    vList.AddRange(dictionary["bottom"]);

                    matched = CheckPieceSpecial(candy, tList1, vList, out toDestroy);

                    if (!matched)
                        matched = CheckPieceSpecial(candy, tList2, vList, out toDestroy);

                    break;

                default:
                    break;
            }

            if(isAuto && matched)
                StartCoroutine(PlayAudio(pieceMatchSfx, 0));

            return matched;
        }

        /// <summary>
        /// Destrói peças,
        /// caso a peça seja horizontal/ vertical, adiciona as peças a serem destruídas por ela à lista de peças a serem destruídas,
        /// verifica e destrói o gel contido naquele gridPiece,
        /// caso todos os gels foram destruídos, ele encerra o jogo com vitória,
        /// por fim chama função de preencher o grid.
        /// </summary>
        /// <param name="toDestroy">Lista de peças a serem destruídas.</param>
        /// <param name="wait">Delay antes de iniciar a função.</param>
        /// <param name="reset">Caso true, passa para a função DropPieces que o input do player deve ser liberado.</param>
        /// <param name="checkSpecial">Caso true, verifica peças horizontais/ verticais e adiciona as peças que elas destruíriam.</param>
        /// <returns></returns>
        IEnumerator DestroyPieces(List<CandyPiece> toDestroy, float wait, bool reset, bool checkSpecial)
        {
            if (wait != 0f)
                yield return new WaitForSeconds(wait);
            else
                yield return new WaitForSeconds(moveDuration);

            if (checkSpecial)
            {
                toDestroy.AddRange(GetKillRow(toDestroy));
                toDestroy.AddRange(GetKillRow(toDestroy));
                toDestroy.AddRange(GetKillRow(toDestroy));
                toDestroy.AddRange(GetKillRow(toDestroy));
                toDestroy.AddRange(GetKillRow(toDestroy));
            }

            pointPos = Vector3.zero;

            for(int i = 0; i < toDestroy.Count; i++)
            {
                CandyPiece candy = toDestroy[i];

                if (candy != null && candy.gridPiece != null)
                {
                    if (checkSpecial)
                    {
                        List<CandyEffect> pool = destroyEffectPool;
                        CandyEffect effect = destroyEffectPool.Last();

                        if (candy.candyType == CandyType.Horizontal || candy.candyType == CandyType.Vertical)
                        {
                                if (_sfxController != null)
                                    _sfxController.PlaySound(pieceSpecialSfx);

                                pool = candy.candyType == CandyType.Horizontal ? horizontalPool : verticalPool;
                        }
                        else if (candy.candyType == CandyType.Normal)
                            pool = destroyEffectPool;
                        else if (candy.candyType == CandyType.Mega)
                        {
                            if (_sfxController != null)
                                _sfxController.PlaySound(pieceMegaSfx);

                            if (CreateSpecial != null)
                                CreateSpecial(candy.transform.localPosition);
                        }

                        for(int j = 0; j < pool.Count; j++)
                        {
                            CandyEffect _effect = pool[j];

                            if (!_effect.gameObject.activeSelf)
                            {
                                effect = _effect;
                                break;
                            }
                        }

                        if (effect != null)
                            effect.Show(candy.transform.localPosition);
                    }

                    pointPos += candy.transform.localPosition;

                    GridPiece piece = candy.gridPiece;

                    if (candy.gridPiece.type == GridPieceType.CandyGel && checkSpecial)
                    {
                        for (int k = 0; k < gelPool.Count; k++)
                        {
                            if (gelPool[k].gridPiece == piece)
                            {
                                if (!gelPool[k].isDestroyed)
                                {
                                    gelPool[k].ToDestroy();
                                    gelsDestroyed++;
                                }

                                break;
                            }
                        }
                    }

                    candy.gridPiece = null;
                    candy.transform.DOScale(0f, breakDuration).Play();
                    StartCoroutine(KillPiece(candy));

                    piece.objScript = null;
                }
            }

            tempScore += toDestroy.Count * piecePoints * comboCounter;

            if (comboCounter > 0)
            {
                pointPos /= toDestroy.Count;

                if (comboText != null)
                    comboText.ShowText(comboCounter.ToString(), pointPos);

                if (RemainingGelsText != null)
                    RemainingGelsText((gelAmount - gelsDestroyed).ToString());
            }

            yield return new WaitForSeconds(breakDuration + 0.1f);

            if (dropPieces != null)
                StopCoroutine(dropPieces);

            dropPieces = DropPieces(reset);
            yield return StartCoroutine(dropPieces);

            if (gelsDestroyed >= gelAmount)
            {
                Victory();
            }
        }

        /// <summary>
        /// Adiciona a peça ao deadPool,
        /// espera a animação de destruição e reseta peça a um estado inerte.
        /// </summary>
        /// <param name="candy"></param>
        IEnumerator KillPiece(CandyPiece candy)
        {
            deadPool.Add(candy);

            yield return new WaitForSeconds(0.1f);

            Vector3 pos = candy.transform.localPosition;
            pos.y = launchFrom;

            candy.gameObject.SetActive(false);
            candy.transform.localPosition = pos;
        }

        /// <summary>
        /// Função que inicia o jogo.
        /// </summary>
        /// <param name="wait">Delay a ser esperado antes de iniciar.</param>
        /// <returns></returns>
        IEnumerator StartGame(float wait)
        {
            yield return new WaitForSeconds(wait);

            StartCoroutine(DropPieces(true));
        }

        /// <summary>
        /// Função que preenche o grid com peças acima ou, caso a coluna esteja vazia, com peças do deadPool.
        /// </summary>
        /// <param name="reset">Caso true, no fim libera o input do player.</param>
        /// <returns></returns>
        IEnumerator DropPieces(bool reset)
        {
            count = 0;
            for(int i = 0; i < _grid.pieces.Count; i++)
            {
                for(int j = 0; j < _grid.pieces[i].Count; j++)
                {
                    GridPiece piece = _grid.pieces[i][j];

                    if (piece.objScript == null)
                    {
                        if (_grid.gridLayout[(int)piece.coodinates.x].row[(int)piece.coodinates.y] == GridPieceType.Blank)
                            continue;

                        bool found = false;

                        for (int k = (int)piece.coodinates.x; k < _grid.size.y; k++)
                        {
                            if (_grid.pieces[k][(int)piece.coodinates.y].objScript != null)
                            {
                                if (_grid.gridLayout[k].row[(int)piece.coodinates.y] == GridPieceType.Blank)
                                    continue;

                                found = true;
                                CandyPiece candyFound = _grid.pieces[k][(int)piece.coodinates.y].objScript as CandyPiece;

                                candyFound.gridPiece.objScript = null;
                                SetPiece(candyFound, piece, false);
                                break;
                            }
                        }

                        if (!found)
                        {
                            int nCount = 0;
                            CandyPiece candy;

                            while (true)
                            {
                                if (deadPool[nCount] != null)
                                {
                                    candy = deadPool[nCount];
                                    deadPool.RemoveAt(nCount);
                                    break;
                                }

                                nCount++;
                            }
                            SetPiece(candy, piece, true);
                        }
                    }
                }
            }

            comboCounter++;

            StartCoroutine(PlayAudio(pieceFallSfx, fallDuration));

            if (gameStarted)
                yield return new WaitForSeconds(fallDuration);
            else
                yield return new WaitForSeconds(2f + 0.5f);

            if (autoCheck != null)
                StopCoroutine(autoCheck);

            autoCheck = AutoCheckGrid();
            yield return StartCoroutine(autoCheck);

            autoRunning = false;
        }

        /// <summary>
        /// Verificação se houve match de peça de tipo normal,
        /// define se a peça será destruída ou alterada para algum tipo especial,
        /// e preenche a lista de peças a serem destruídas.
        /// </summary>
        /// <param name="candy">Peça a ser verificada.</param>
        /// <param name="list">Lista contendo as peças que deram match.</param>
        /// <param name="direction">Direção do match.</param>
        /// <param name="toDestroy">Referência da lista de peças a serem destruídas.</param>
        /// <returns>Retorna booleana caso houve match.</returns>
        bool CheckPieceNormal(CandyPiece candy, List<GridPiece> list, Vector2 direction, out List<CandyPiece> toDestroy)
        {
            bool matched = false;
            toDestroy = new List<CandyPiece>();

            if (list.Count >= piecesBreakCount)
            {
                matched = true;

                if (list.Count >= piecesMegaCount)
                {
                    candy.id = 1000;
                    candy.candyType = CandyType.Mega;
                    candy.image.sprite = candySpecial;

                    if (CreateSpecial != null)
                        CreateSpecial(candy.transform.localPosition);
                }
                else if (list.Count >= piecesSpecialCount)
                {
                    Vector2 tPos = candy.gridPiece.coodinates - direction;
                    Vector2 newDir = tPos - list[0].coodinates;
                    newDir = newDir.normalized;

                    if (newDir == Vector2.up || newDir == Vector2.down)
                    {
                        ChangePieceToSpecial(candy, CandyType.Horizontal);
                    }
                    else if (newDir == Vector2.right || newDir == Vector2.left)
                    {
                        ChangePieceToSpecial(candy, CandyType.Vertical);
                    }
                }
                else if (list.Count >= piecesBreakCount)
                {
                    toDestroy.Add(candy);
                }

                for(int i = 0; i < list.Count; i++)
                {
                    toDestroy.Add(list[i].objScript as CandyPiece);
                }
            }

            return matched;
        }

        /// <summary>
        /// Verifica se houve match de peça de tipo horizontal/ vertical,
        /// caso houve, retorna lista de peças a serem destruídas.
        /// </summary>
        /// <param name="candy">Peça a ser verificada.</param>
        /// <param name="list">Lista de peças que deram match.</param>
        /// <param name="killPieces">Possível lista de peças a serem destruídas.</param>
        /// <param name="toDestroy">Referência da lista de peças a serem destruídas.</param>
        /// <returns>Retorna booleana caso houve match.</returns>
        bool CheckPieceSpecial(CandyPiece candy, List<GridPiece> list, List<GridPiece> killPieces, out List<CandyPiece> toDestroy)
        {
            bool matched = false;
            toDestroy = new List<CandyPiece>();

            if (list.Count >= piecesBreakCount)
            {
                matched = true;
                
                for(int i = 0; i < list.Count; i++)
                {
                    toDestroy.Add(list[i].objScript as CandyPiece);
                }

                toDestroy.Add(candy);
            }

            return matched;
        }

        /// <summary>
        /// Configura peça para ser colocada no grid.
        /// </summary>
        /// <param name="candy">Peça a ser configurada.</param>
        /// <param name="gridPiece">GridPiece para onde a peça será movida.</param>
        /// <param name="isNew">Caso true, reseta peça para configurações padrão, acima do grid.</param>
        void SetPiece(CandyPiece candy, GridPiece gridPiece, bool isNew)
        {
            if (isNew)
            {
                int index = Random.Range(0, qntPieces);

                candy.id = index;
                candy.image.sprite = candyImgs[index][0];
                candy.candyType = CandyType.Normal;

                candy.transform.localPosition = new Vector3(gridPiece.position.x, launchFrom, gridPiece.position.z);
            }

            candy.transform.localScale = Vector3.one;
            candy.transform.localRotation = Quaternion.identity;

            candy.gridPiece = gridPiece;
            gridPiece.objScript = candy;

            Vector3 tPos = Vector3.zero;

            if (Container != null)
                tPos = Container.localPosition + gridPiece.position;
            else
                tPos = gridPiece.position;

            candy.transform.DOKill();

            if (gameStarted)
                candy.transform.DOLocalMoveY(tPos.y, fallDuration + (count * 0.01f)).Play();
            else
                candy.transform.DOLocalMoveY(tPos.y, 2f + (count * 0.01f)).SetEase(Ease.OutBounce).Play();

            candy.gameObject.SetActive(true);

            count++;
        }

        /// <summary>
        /// Faz a troca de posição entre peças.
        /// </summary>
        /// <param name="fromCandy">Peça 1.</param>
        /// <param name="toCandy">Peça 2.</param>
        void ChangePiecesPosition(CandyPiece fromCandy, CandyPiece toCandy)
        {
            fromCandy.gridPiece.objScript = toCandy;
            toCandy.gridPiece.objScript = fromCandy;

            GridPiece toGrid = toCandy.gridPiece;

            toCandy.gridPiece = fromCandy.gridPiece;
            fromCandy.gridPiece = toGrid;

            KillTween();
        }

        /// <summary>
        /// Altera peça para tipo Vertical/ Horizontal.
        /// </summary>
        /// <param name="candy">Peça a ser alterada.</param>
        /// <param name="toType">Tipo a ser alterado para.</param>
        void ChangePieceToSpecial(CandyPiece candy, CandyType toType)
        {
            candy.candyType = toType;

            if (toType == CandyType.Horizontal)
                candy.image.sprite = candyImgs[candy.id][2];
            else if (toType == CandyType.Vertical)
                candy.image.sprite = candyImgs[candy.id][1];
        }

        /// <summary>
        /// Verifica peças contínuas que fazem match com a peça.
        /// </summary>
        /// <param name="piece">Peça a ser verificada.</param>
        /// <param name="list">Lista de peças a serem verificadas.</param>
        /// <param name="_listOut">Lista de peças pré-preenchidas.</param>
        /// <returns>Retorna lista com todos matchs.</returns>
        List<GridPiece> GetMatchPieces(CandyPiece piece, List<GridPiece> list, List<GridPiece> _listOut)
        {
            List<GridPiece> listOut = _listOut;

            for(int i = 0; i < list.Count; i++)
            {
                GridPiece _piece = list[i];

                if (_piece.objScript != null)
                {
                    if (!piece.Equals(_piece.objScript) && ((CandyPiece)_piece.objScript).id == piece.id && ((CandyPiece)_piece.objScript).candyType != CandyType.Mega)
                    {
                        listOut.Add(_piece);
                    }
                    else
                        break;
                }
            }

            return listOut;
        }

        /// <summary>
        /// Retorna todas peças do grid.
        /// </summary>
        /// <returns></returns>
        List<CandyPiece> GetAllPieces()
        {
            List<CandyPiece> candyPieces = new List<CandyPiece>();
            List<GridPiece> pieces = _grid.GetAllPieces();

            for(int i = 0; i < pieces.Count; i++)
            {
                candyPieces.Add(pieces[i].objScript as CandyPiece);
            }

            return candyPieces;
        }

        /// <summary>
        /// Retorna peça do grid.
        /// </summary>
        /// <param name="startPos">Posição inicial.</param>
        /// <param name="direction">De qual direção deve ser pego.</param>
        /// <returns></returns>
        CandyPiece GetPiece(Vector2 startPos, Vector2 direction)
        {
            int indeX = (int)startPos.x + (int)direction.y;
            int indeY = (int)startPos.y + (int)direction.x;

            GridPiece piece = _grid.GetPiece(new Vector2(indeX, indeY));
            CandyPiece candy = null;

            if(piece == null)
                candy = null;
            else if (piece.objScript != null)
                candy = piece.objScript as CandyPiece;

            return candy;
        }

        /// <summary>
        /// Verifica direção do mouse/ touch em relação ao toque inicial,
        /// e retorna peça.
        /// </summary>
        /// <param name="startPos">Press inicial.</param>
        /// <param name="curPos">Posição atual do mouse/ touch.</param>
        /// <returns>Retorna peça.</returns>
        CandyPiece GetMouseOverPiece(Vector2 startPos, Vector2 curPos)
        {
            if (candyStart == null)
                return null;

            float distanceX = startPos.x - curPos.x;
            float distanceY = startPos.y - curPos.y;

            int indeX = (int)candyStart.gridPiece.coodinates.y;
            int indeY = (int)candyStart.gridPiece.coodinates.x;

            if (Mathf.Abs(distanceX) >= Mathf.Abs(distanceY))
                indeX += curPos.x >= startPos.x ? 1 : -1;
            else
                indeY += curPos.y >= startPos.y ? 1 : -1;

            if (_grid.pieces.ElementAtOrDefault(indeY) != null && _grid.pieces[indeY].ElementAtOrDefault(indeX) != null &&
                _grid.gridLayout[indeX].row[indeY] != GridPieceType.Blank)
                return _grid.pieces[indeY][indeX].objScript as CandyPiece;
            else
                return null;
        }

        /// <summary>
        /// Retorna todas as peças contidos na coluna/ linha.
        /// </summary>
        /// <param name="toDestroy">Lista de peças a serem destruídas.</param>
        /// <returns>Retorna lista.</returns>
        List<CandyPiece> GetKillRow(List<CandyPiece> toDestroy)
        {
            List<CandyPiece> tempDestroy = new List<CandyPiece>();

            for(int i = 0; i < toDestroy.Count; i++)
            {
                CandyPiece candy = toDestroy[i];

                if (candy == null) continue;

                if (candy.candyType != CandyType.Normal && candy.candyType != CandyType.Mega)
                {
                    Dictionary<string, List<GridPiece>> dictionary = new Dictionary<string, List<GridPiece>>();
                    List<GridPiece> list = new List<GridPiece>();

                    if (candy.gridPiece != null)
                        dictionary = _grid.GetPiecesRow(candy.gridPiece.coodinates);

                    if (dictionary.Count != 0)
                    {
                        if (candy.candyType == CandyType.Horizontal)
                        {
                            list = dictionary["right"];
                            list.AddRange(dictionary["left"]);
                        }
                        else if (candy.candyType == CandyType.Vertical)
                        {
                            list = dictionary["top"];
                            list.AddRange(dictionary["bottom"]);
                        }

                        for(int j = 0; j < list.Count; j++)
                        {
                            GridPiece piece = list[j];

                            if (piece.objScript != null)
                                if (((CandyPiece)piece.objScript).candyType == CandyType.Mega) continue;

                            if (!tempDestroy.Contains(piece.objScript as CandyPiece) && !toDestroy.Contains(piece.objScript as CandyPiece))
                                tempDestroy.Add(piece.objScript as CandyPiece);
                        }
                    }
                }
            }

            return tempDestroy;
        }

        /// <summary>
        /// Preenche megaPool com todas as peças que fazem match.
        /// </summary>
        /// <param name="candyTarget">Tipo de peça a dar match.</param>
        /// <returns>Retorna lista de peças.</returns>
        List<CandyPiece> FillMegaPool(CandyPiece candyTarget)
        {
            List<CandyPiece> tempPool = new List<CandyPiece>();

            for(int i = 0; i < _grid.pieces.Count; i++)
            {
                for (int j = 0; j < _grid.pieces[i].Count; j++)
                {
                    GridPiece piece = _grid.pieces[i][j];

                    if (_grid.gridLayout[(int)piece.coodinates.y].row[(int)piece.coodinates.x] != GridPieceType.Blank)
                    {
                        CandyPiece candy = piece.objScript as CandyPiece;

                        if (candyTarget.candyType == CandyType.Mega)
                        {
                            tempPool.Add(candy);
                            candy.image.DOColor(Color.red, 0.5f).SetLoops(1000, LoopType.Yoyo).Play();
                        }
                        else if (candy.id == candyTarget.id && candy.candyType != CandyType.Mega)
                        {
                            tempPool.Add(candy);
                            candy.image.DOColor(Color.red, 0.5f).SetLoops(1000, LoopType.Yoyo).Play();
                        }
                    }
                }
            }

            return tempPool;
        }

        /// <summary>
        /// Limpa megaPool e remove todos tweens relacionados.
        /// </summary>
        void ClearMegaPool()
        {
            for(int i = 0; i < megaPool.Count; i++)
            {
                CandyPiece candy = megaPool[i];

                candy.image.DOKill(false);
                candy.image.color = Color.white;
            }

            megaPool.Clear();
        }

        /// <summary>
        /// Limpa hintPool e remove todos tweens relacionados.
        /// </summary>
        void ClearHintPool()
        {
            for(int i = 0; i < hintPool.Count; i++)
            {
                CandyPiece candy = hintPool[i];

                candy.transform.DOKill(false);
                candy.transform.DOScale(Vector3.one, moveDuration);
            }

            hintPool.Clear();
        }

        /// <summary>
        /// Termina tween das peças e reseta para posições originais.
        /// </summary>
        void KillTween()
        {
            if (candyStart != null && candyStart.gridPiece != null)
            {
                candyStart.transform.DOKill(false);
                candyStart.transform.DOLocalMove(candyStart.gridPiece.position, moveDuration, false).Play();
                candyStart.transform.DOScale(1f, moveDuration).Play();
            }

            if (candyEnd != null && candyEnd.gridPiece != null)
            {
                candyEnd.transform.DOKill(false);
                candyEnd.transform.DOLocalMove(candyEnd.gridPiece.position, moveDuration, false).Play();
                candyEnd.transform.DOScale(1f, moveDuration).Play();
            }
        }

        /// <summary>
        /// Função a ser chamado ao término do jogo.
        /// </summary>
        void GameEnded()
        {
            if (KillGels != null)
                KillGels();

            if (_registerController != null && saveData)
            {
                _registerController.AddRegisterValue("CandyWon", gameWon.ToString(), false);
                _registerController.AddRegisterValue("CandyScore", tempScore.ToString(), false);
            }

            gameEnded = true;
            gameStarted = false;

            StopAllCoroutines();
        }

        /// <summary>
        /// Função que reseta o controller.
        /// </summary>
        protected override void CallReset()
        {
            StopAllCoroutines();

            hintTime = 0f;
            tempScore = 0;
            comboCounter = 0;
            gelsDestroyed = 0;
            gameWon = false;
            gameEnded = false;
            gameStarted = false;

            _timeController.GameTimeLimit = gameDuration;
        }

        /// <summary>
        /// Função chamada em caso de vitória.
        /// </summary>
        void Victory()
        {
            gameWon = true;

            if (_scoreController != null)
                _scoreController.AddScore(tempScore);

                _registerController.AddRegisterValue("CandyWon", true.ToString(), false);

            if (_sfxController != null)
                _sfxController.PlaySound(victorySfx);

            DebugLog("GameWin");
            CallAction(victoryPage);
            EndGame();
        }

        /// <summary>
        /// Função a ser chamado caso o tempo termine.
        /// </summary>
        void GameTimeOut()
        {
            gameWon = false;
            
            if (_sfxController != null)
                _sfxController.PlaySound(gameOverSfx);

                _registerController.AddRegisterValue("CandyWon", false.ToString(), false);
            gameEnded = true;
            gameStarted = false;

            StopAllCoroutines();

            CallAction(timeOutPage);

            Invoke("EndGame", 1f);
        }*/

        /// <summary>
        /// Função para tocar audio.
        /// </summary>
        /// <param name="audio">Nome do áudio.</param>
        /// <param name="delay">Delay para iniciar o áudio.</param>
        /// <returns></returns>
        public IEnumerator PlayAudio(string audio, float delay)
        {
            if (IsGameStarted && _sfxController != null)
            {
                yield return new WaitForSeconds(delay);

                _sfxController.PlaySound(audio);
            }
        }
    }
}