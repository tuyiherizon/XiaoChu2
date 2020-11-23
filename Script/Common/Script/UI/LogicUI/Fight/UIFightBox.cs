using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tables;

public class UIFightBox : UIBase
{
    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIFightBox, UILayer.BaseUI, hash);
    }

    public static void ShowStage(StageInfoRecord stageRecord)
    {
        Hashtable hash = new Hashtable();
        hash.Add("StageInfo", stageRecord);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIFightBox, UILayer.BaseUI, hash);
    }

    public static void ShowMonsterBalls(List<BallInfo> animBalls)
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIFightBox>(UIConfig.UIFightBox);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.ShowMonsterSkillBalls(animBalls);
    }

    public static UIFightBall GetFightBall(BallInfo ballInfo)
    {
        if (ballInfo == null)
            return null;
        return GetFightBall((int)ballInfo.Pos.x, (int)ballInfo.Pos.y);
    }

    public static UIFightBall GetFightBall(int x, int y)
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIFightBox>(UIConfig.UIFightBox);
        if (instance == null)
            return null;

        if (!instance.isActiveAndEnabled)
            return null;

        return instance.GetBallUI(x, y);
    }

    public static void ShowOptMask()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIFightBox>(UIConfig.UIFightBox);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.ShowMask();
    }

    public static void HideOptMask()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIFightBox>(UIConfig.UIFightBox);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.HideMask();
    }

    public static bool IsTestMode()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIFightBox>(UIConfig.UIFightBox);
        if (instance == null)
            return false;

        if (!instance.isActiveAndEnabled)
            return false;

        return instance._TestMode;
    }

    public static void SetTest()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIFightBox>(UIConfig.UIFightBox);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.TestBall();
    }

    #endregion

    void OnEnable()
    {
        _LastMoveTime = Time.time;
    }

    // Start is called before the first frame update
    void Start()
    {
        UIFightBattleField.ShowElimitInfo();
    }

    // Update is called once per frame
    void Update()
    {
        GuideUpdate();

        TestFightUpdate();
    }

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        ResourceManager.Instance.SetImage(_BG, LogicManager.Instance.EnterStageInfo.StageRecord.BG);

        InitBox(BallBox.Instance.BoxWidth, BallBox.Instance.BoxHeight);

        ResourcePool.Instance.InitDefaultRes();
    }

    #region box

    public Image _BG;
    public GameObject _BallPrefab;
    public GridLayoutGroup _FightBox;
    public GameObject _EliminateTipGO1;
    public GameObject _EliminateTipGO2;
    public GameObject _OptMask;

    public GameObject _BallBGPrefab;
    public GameObject _BallBGBox;
    public RectTransform _BoxBGBounderBottom;
    public RectTransform _BoxBGBounderLeft;
    public RectTransform _BoxBGBounderRight;

    private UIFightBall[][] _BallInfos;
    private int _BoxWidth;
    private int _BoxLength;

    public void ResetSceneLogic(StageInfoRecord sceneRecord)
    {
        var mapRecord = StageMapRecord.ReadStageMap(sceneRecord.ScenePath);
        BallBox.Instance.Init(mapRecord);
        BallBox.Instance.InitBallInfo();

        UpdateBalls();

        RecordBallDamage.LoadingStageID = sceneRecord.Id;
    }

    public void OnReset()
    {
        ClearCheckGOs();
        BallBox.Instance.Refresh();
        UpdateBalls();
        //DebugTest();
    }

    public void RefreshNormal()
    {
        ClearCheckGOs();
        BallBox.Instance.RefreshNormalForElimit(false);
        UpdateBalls();
        //DebugTest();
    }

    public void OnRecordMap()
    {
        RandomMonster.WriteMapRecord(LogicManager.Instance.EnterStageInfo.StageID);
    }

    public void OnBtnAutoEliminate()
    {
        var eliminate = BallBox.Instance.FindPowerEliminate2();
        if (eliminate == null)
            return;

        var powerPath = eliminate.FindPowerPath();
        for (int i = 0; i< powerPath.Count; ++i)
        {
            var tipGO = GameObject.Instantiate<GameObject>(_EliminateTipGO1);
            var ballUI = GetBallUI(powerPath[i]);
            tipGO.transform.SetParent(transform);
            tipGO.transform.localScale = Vector3.one;
            tipGO.transform.position = ballUI.transform.position;
            tipGO.gameObject.SetActive(true);

            var name = tipGO.transform.Find("Name").GetComponent<Text>();
            name.text = i.ToString();
        }
        
    }

    public UIFightBall GetBallUI(BallInfo ballInfo, bool isSafe = false)
    {
        return GetBallUI(ballInfo.Pos, isSafe);
    }

    public UIFightBall GetBallUI(Vector2 pos, bool isSafe = false)
    {
        int posX = (int)pos.x;
        int posY = (int)pos.y;

        return GetBallUI(posX, posY, isSafe);
    }

    public UIFightBall GetBallUI(int posX, int posY, bool isSafe = false)
    {
        if (posX < 0 || posX > _BallInfos.Length - 1)
            return null;

        if (posY < 0 || posY > _BoxLength * 2 - 1)
        {
            if (isSafe)
            {
                if (posY > _BoxLength * 2 - 1)
                {
                    return _BallInfos[posX][_BoxLength * 2 - 1];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        return _BallInfos[posX][posY];
    }

    public void InitBox(int x, int y)
    {
        _BoxWidth = x;
        _BoxLength = y;

        _FightBox.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _FightBox.constraintCount = x;

        _BallInfos = new UIFightBall[x][];
        for (int i = 0; i < x; ++i)
        {
            _BallInfos[i] = new UIFightBall[y * 2];
            for (int j = 0; j < y * 2; ++j)
            {
                GameObject ballGO = GameObject.Instantiate(_BallPrefab.gameObject);
                UIFightBall ballInfo = ballGO.GetComponentInChildren<UIFightBall>();

               
                ballGO.transform.SetParent(_FightBox.transform);
                ballGO.transform.localScale = Vector3.one;
                var localPos = GetBallPosByIdx(0,0, i, j);
                ballGO.transform.localPosition = localPos;

                _BallInfos[i][j] = ballInfo;
                _BallInfos[i][j]._Pos = new Vector2(i, j);
                _BallInfos[i][j].SetFightBox(this);

                if (j < y)
                {
                    ballGO.gameObject.SetActive(true);
                    var bgGO = GameObject.Instantiate(_BallBGPrefab);
                    bgGO.gameObject.SetActive(true);
                    bgGO.transform.SetParent(_BallBGBox.transform);
                    bgGO.transform.position = ballGO.transform.position;
                    bgGO.transform.localScale = Vector3.one;

                }
                else
                {
                    ballGO.gameObject.SetActive(false);
                }
            }
        }

        _BoxBGBounderBottom.sizeDelta = new Vector2(_BallWidth * x, _BoxBGBounderBottom.sizeDelta.y);
        _BoxBGBounderBottom.localPosition = _BallInfos[0][0].transform.localPosition - new Vector3(_BallWidth * 0.5f, _BallHeight * 0.5f, 0);

        _BoxBGBounderLeft.sizeDelta = new Vector2(_BallHeight * y, _BoxBGBounderLeft.sizeDelta.y);
        _BoxBGBounderLeft.localPosition = _BallInfos[0][(int)_BoxLength - 1].transform.localPosition + new Vector3(-_BallWidth * 0.5f, _BallHeight * 0.5f, 0);

        _BoxBGBounderRight.sizeDelta = new Vector2(_BallHeight * y, _BoxBGBounderRight.sizeDelta.y);
        _BoxBGBounderRight.localPosition = _BallInfos[(int)_BoxWidth - 1][0].transform.localPosition + new Vector3(_BallWidth * 0.5f, -_BallHeight * 0.5f, 0);

        for (int j = 0; j < _BoxLength; ++j)
        {
            for (int i = 0; i < _BoxWidth; ++i)
            {
                _BallInfos[i][j].SetBallInfo(BallBox.Instance._BallBoxInfo[i][j]);
                _BallInfos[i][j].ShowBall();
            }
        }

    }

    public void UpdateBalls()
    {
        for (int j = 0; j < _BoxLength; ++j)
        {
            for (int i = 0; i < _BoxWidth; ++i)
            {
                _BallInfos[i][j].SetBallInfo(BallBox.Instance._BallBoxInfo[i][j]);
                _BallInfos[i][j].ShowBall();
            }
        }
    }

    public float _BallWidth = 165;
    public float _BallHeight = 165;

    public Vector2 GetBallPosByIdx(float curX, float curY, float targetX, float targetY)
    {
       var idxPos = new Vector2((targetX - curX) * _BallWidth, (targetY - curY) * _BallHeight);
       return idxPos;
    }

    public List<Vector3> FindPath(Vector2 fromPos, Vector2 toPos)
    {
        int fromPosX = (int)fromPos.x;
        int fromPosY = (int)fromPos.y;
        int toPosX = (int)toPos.x;
        int toPosY = (int)toPos.y;

        List<Vector3> pathList = new List<Vector3>();
        while (true)
        {
            if (fromPosY > _BoxLength - 1)
            {
                fromPosY = _BoxLength - 1;
            }
            else if (fromPosX > toPosX && fromPosY > toPosY)
            {
                --fromPosX;
                --fromPosY;
            }
            else if (fromPosX < toPosX && fromPosY > toPosY)
            {
                ++fromPosX;
                --fromPosY;
            }
            else if (fromPosX > toPosX)
            {
                --fromPosX;
            }
            else if (fromPosX < toPosX)
            {
                ++fromPosX;
            }
            else if (fromPosY > toPosY)
            {
                --fromPosY;
                
            }
            else
            {

            }
            var uiBall = GetBallUI(fromPosX, fromPosY);
            pathList.Add(uiBall.transform.position);

            if (fromPosX == toPosX && fromPosY == toPosY)
            {
                break;
            }
        }

        return pathList;
    }

    public List<Vector3> GetMovePathPos(List<BallInfo> pathBalls)
    {
        List<Vector3> pathList = new List<Vector3>();
        foreach (var pathBall in pathBalls)
        {
            if (pathBall.Pos.y > _BoxLength - 1)
            {
                int start = (int)pathBall.Pos.y;
                int end = _BoxLength - 1;
                for (int i = start; i >= _BoxLength; --i)
                {
                    var uiBall = GetBallUI((int)pathBall.Pos.x, i, true);
                    pathList.Add(uiBall.transform.position);
                }
            }
            else
            {
                var uiBall = GetBallUI(pathBall.Pos, true);
                pathList.Add(uiBall.transform.position);
            }
        }
        return pathList;
    }

    private float GetFallPathTime(List<BallInfo> pathBalls)
    {
        float pathTime = 0;
        foreach (var pathBall in pathBalls)
        {
            if (pathBall.Pos.y > _BoxLength - 1)
            {
                pathTime += (pathBall.Pos.y - _BoxLength + 1) * 0.1f;
            }
            else
            {
                pathTime += 0.1f;
            }
        }

        return pathTime;
    }

    #endregion

    #region anim

    public GameObject _ElimitCheckPrefab;
    public float _ExchangeBallTime = 0.3f;
    public float _ElimitBallTime = 0.2f;

    private List<GameObject> _ElimitCheckGOs = new List<GameObject>();
    private void CreateCheckGOs(int num)
    {
        if (_ElimitCheckGOs.Count < num)
        {
            int deltaCnt = num - _ElimitCheckGOs.Count;
            for (int i = 0; i < deltaCnt; ++i)
            {
                GameObject checkGO = GameObject.Instantiate(_ElimitCheckPrefab);
                checkGO.transform.SetParent(_ElimitCheckPrefab.transform.parent);
                checkGO.transform.localScale = Vector3.one;
                _ElimitCheckGOs.Add(checkGO);
            }
        }
    }

    public void ClearCheckGOs()
    {
        for (int i = 0; i < _ElimitCheckGOs.Count; ++i)
        {
            _ElimitCheckGOs[i].transform.position = new Vector3(10000, 0, 0);
        }
    }

    public void ExChangeBalls(UIFightBall ballA, UIFightBall ballB)
    {
        _TestMode = false;
        _LastMoveTime = Time.time;
        
        if (!ballA.BallInfo.IsCanMove() || !ballB.BallInfo.IsCanMove())
            return;

        StarAnim();
        iTween.MoveTo(ballA._FightBallAnchor.gameObject, ballB.transform.position, _ExchangeBallTime);
        iTween.MoveTo(ballB._FightBallAnchor.gameObject, ballA.transform.position, _ExchangeBallTime);
        StartCoroutine(AnimEnd(ballA, ballB));
        PlayMoveSound();
        UIGuide.HideGuide();
    }

    public IEnumerator AnimEnd(UIFightBall ballA, UIFightBall ballB)
    {

        yield return new WaitForSeconds(_ExchangeBallTime + 0.1f);
        //iTween.Stop();
        BallBox.Instance.MoveBall(ballA.BallInfo, ballB.BallInfo);
        //ballA.Exchange(ballB);

        ballA.ResetRoot();
        ballB.ResetRoot();

        ballA.ShowBall();
        ballB.ShowBall();

        var moveList = new List<BallInfo>() { ballA.BallInfo, ballB.BallInfo };
        var elimitBalls = BallBox.Instance.CheckNormalEliminate(moveList);
        var spElimitMove = BallBox.Instance.CheckSpMove(moveList);
        var spElimitElimit = BallBox.Instance.CheckSpElimit(elimitBalls);
        var exploreBalls = BallBox.Instance.CurrentElimitnate();
        var afterElimit = BallBox.Instance.AfterElimitnate();

        BallBox.AddBallInfos(elimitBalls, spElimitMove);
        BallBox.AddBallInfos(elimitBalls, spElimitElimit);
        BallBox.AddBallInfos(elimitBalls, exploreBalls);
        BallBox.AddBallInfos(elimitBalls, afterElimit);
        if (elimitBalls.Count == 0)
        {
            iTween.MoveTo(ballA._FightBallAnchor.gameObject, ballB.transform.position, _ExchangeBallTime);
            iTween.MoveTo(ballB._FightBallAnchor.gameObject, ballA.transform.position, _ExchangeBallTime);

            yield return new WaitForSeconds(_ExchangeBallTime + 0.1f);

            BallBox.Instance.MoveBack(ballA.BallInfo, ballB.BallInfo);
            //ballA.Exchange(ballB);
            ballA.ResetRoot();
            ballB.ResetRoot();
            ballA.ShowBall();
            ballB.ShowBall();

            EndAnim();

            yield break;
        }
        else
        {
            
            do
            {
                float elimitAnimTime = 0;
                AudioClip elimitSound = _ElimitAudio;
                FIGHT_SOUND_TYPE soundLevel = FIGHT_SOUND_TYPE.ELIMIT;
                foreach (var elimitBall in elimitBalls)
                {
                    bool isContentBomb = false;
                    if (!elimitBall._IsBoomSP)
                    {
                        foreach (var optBombInfo in BallBox.Instance._CurrentOptExtra)
                        {
                            if (optBombInfo._OptBall == elimitBall)
                            {
                                isContentBomb = true;
                                break;
                            }
                            foreach (var bombElimitBall in optBombInfo._ElimitBalls)
                            {
                                if (bombElimitBall == elimitBall)
                                {
                                    isContentBomb = true;
                                    break;
                                }
                            }
                            if (isContentBomb)
                            {
                                break;
                            }
                        }
                    }

                    var uiBall = GetBallUI(elimitBall);
                    if (uiBall.IsSPBallBomb())
                    {
                        elimitSound = _BombAudio;
                        soundLevel = FIGHT_SOUND_TYPE.BOMB;
                    }
                    else if (soundLevel < FIGHT_SOUND_TYPE.LINE && uiBall.IsSPBallLine())
                    {
                        elimitSound = _LineAudio;
                        soundLevel = FIGHT_SOUND_TYPE.LINE;
                    }
                    else if (soundLevel < FIGHT_SOUND_TYPE.COMBINE && BallBox.Instance._CurrentOptExtra.Count > 0)
                    {
                        elimitSound = _CombineAudio;
                        soundLevel = FIGHT_SOUND_TYPE.COMBINE;
                    }

                    if (isContentBomb)
                    {
                        continue;
                    }
                    
                    var elimitT = uiBall.Elimit();
                    if (elimitT > elimitAnimTime)
                    {
                        elimitAnimTime = elimitT;
                    }

                    
                }
                PlayerUISound(elimitSound, 1);
                StartCoroutine( BombCreateAnim());

                yield return new WaitForSeconds(elimitAnimTime + _ElimitBallTime);
                

                BallBox.Instance.ClearElimitInfo();

                foreach (var elimitBall in elimitBalls)
                {
                    var uiBall = GetBallUI(elimitBall);
                    uiBall.ShowBall();

                }
                yield return new WaitForSeconds(_ElimitBallTime);

                UIFightBattleField.ShowElimitInfo();

                var fillingTime = Filling();
                yield return new WaitForSeconds(fillingTime);
                List<BallInfo> checkBalls = new List<BallInfo>();
                BallBox.AddBallInfos(checkBalls, _FillingBalls);
                BallBox.AddBallInfos(checkBalls, exploreBalls);
                elimitBalls = BallBox.Instance.CheckNormalEliminate();
                //elimitBalls = BallBox.Instance.CheckNormalEliminate();
                var spSubElimitBalls = BallBox.Instance.CheckSpElimit(elimitBalls);
                exploreBalls = BallBox.Instance.CurrentElimitnate();
                var subafterElimit = BallBox.Instance.AfterElimitnate();

                BallBox.AddBallInfos(elimitBalls, spSubElimitBalls);
                BallBox.AddBallInfos(elimitBalls, exploreBalls);
                BallBox.AddBallInfos(elimitBalls, subafterElimit);
            }
            while (elimitBalls.Count > 0);
        }

        var reShowList = BallBox.Instance.RoundEnd();

        if (reShowList != null && reShowList.Count > 0)
        {
            foreach (var elimitBall in reShowList)
            {
                var uiBall = GetBallUI(elimitBall);
                uiBall.ShowBall();
            }
        }

        var eliminate = BallBox.Instance.FindPowerEliminate();
        if(eliminate == null)
        {
            Debug.Log("No eliminate!!!");
            yield return new WaitForSeconds(1);

            int refreshTimes = 0;
            while (eliminate == null)
            {
                if (refreshTimes < 2)
                {
                    RefreshNormal();
                }
                else
                {
                    BallBox.Instance.RefreshNormalForElimit(false);
                    UpdateBalls();
                }
                ++refreshTimes;
                eliminate = BallBox.Instance.FindPowerEliminate();
            }
        }

        //ShowElimit();
        EndAnim();
    }

    public IEnumerator BombCreateAnim()
    {
        
        foreach (var optBombInfo in BallBox.Instance._CurrentOptExtra)
        {
            var uiOptBall = GetBallUI(optBombInfo._OptBall);
            foreach (var elimitBall in optBombInfo._ElimitBalls)
            {
                if (elimitBall != optBombInfo._OptBall)
                {
                    var uiElimitBall = GetBallUI(elimitBall);
                    uiElimitBall.BombMoveTo(uiOptBall);
                }
            }
        }

        yield return new WaitForSeconds(_ElimitBallTime);

        foreach (var optBombInfo in BallBox.Instance._CurrentOptExtra)
        {
            var uiOptBall = GetBallUI(optBombInfo._OptBall);
            uiOptBall.ShowBall();
        }
    }

    public void BombCreateMoveComplate()
    {

    }

    List<BallInfo> _FillingBalls = new List<BallInfo>();

    public float Filling()
    {
        var fallExchanges = BallBox.Instance.ElimitnateFall();
        //var fillBalls = BallBox.Instance.FillEmpty();
        _FillingBalls.Clear();

        float maxMoveTime = 0.0f;
        for (int i = 0; i < fallExchanges.Count; ++i)
        {
            var uiBall1 = GetBallUI(fallExchanges[i].FromBall);
            if (uiBall1 != null)
            {
                uiBall1.ShowBall();
            }

            var uiBall2 = GetBallUI(fallExchanges[i].ToBall);
            uiBall2.ShowBall();

            uiBall2._FightBallAnchor.anchoredPosition = GetBallPosByIdx(fallExchanges[i].ToBall.Pos.x, fallExchanges[i].ToBall.Pos.y, fallExchanges[i].FromBall.Pos.x, fallExchanges[i].FromBall.Pos.y);
            var movePath = GetMovePathPos(fallExchanges[i].PathBalls);
            //movePath.Add(uiBall2.transform.position);
            float moveTime = Mathf.Abs(movePath.Count) * 0.1f;
            //Debug.Log("MovePos:" + fallExchanges[i].ToBall.Pos + ", anchorePos:" + uiBall2._UIBallInfo.RectTransform.anchoredPosition + ", path:" + movePath.Count);

            Hashtable hash = new Hashtable();
            hash.Add("position", uiBall2.transform.position);
            hash.Add("path", movePath.ToArray());
            hash.Add("speed", 6);
            //hash.Add("time", moveTime);
            iTween.MoveTo(uiBall2._FightBallAnchor.gameObject, hash);

            //uiBall2.UIBallFall(fallExchanges[i].PathBalls);

            if (moveTime > maxMoveTime)
                maxMoveTime = moveTime;

            _FillingBalls.Add(uiBall2.BallInfo);
        }

        
        //for (int i = 0; i < fillBalls.Count; ++i)
        //{
        //    var uiBall2 = GetBallUI(fillBalls[i].ToBall);
        //    uiBall2.ShowBall();

        //    uiBall2._UIBallInfo.RectTransform.anchoredPosition = GetBallPosByIdx(fillBalls[i].ToBall.Pos.x, fillBalls[i].ToBall.Pos.y, fillBalls[i].FromBall.Pos.x, fillBalls[i].FromBall.Pos.y);
        //    float moveTime = (fillBalls[i].FromBall.Pos.x - fillBalls[i].ToBall.Pos.x) + (fillBalls[i].FromBall.Pos.y - fillBalls[i].ToBall.Pos.y) * 0.1f;
        //    Hashtable hash = new Hashtable();
        //    hash.Add("position", uiBall2.transform.position);
        //    //hash.Add("speed", 10);
        //    hash.Add("time", moveTime);
        //    iTween.MoveTo(uiBall2._UIBallInfo.gameObject, hash);

        //    if (moveTime > maxMoveTime)
        //        maxMoveTime = moveTime;
        //}

        return maxMoveTime;
    }

    public void StarAnim()
    {
        ShowMask();
    }

    public void EndAnim()
    {
        HideMask();
    }

    public void ShowMask()
    {
        _OptMask.SetActive(true);
    }

    public void HideMask()
    {
        _OptMask.SetActive(false);
        _LastMoveTime = Time.time;
    }

    public void ShowMonsterSkillBalls(List<BallInfo> animBalls)
    {
        foreach (var animBall1 in animBalls)
        {
            var uiBall = GetBallUI(animBall1);
            uiBall.ShowBall();

        }
    }

    #endregion

    #region opt

    private UIFightBall _DragBall;
    public UIFightBall DragBall
    {
        get
        {
            return _DragBall;
        }
        set
        {
            _DragBall = value;
        }
    }

    public float _GuideShowDelay = 5;
    private float _LastMoveTime;
    public void GuideTips()
    {
        var moveBalls = BallBox.Instance.FindAnyEliminate();

        List<Transform> moveTrans = new List<Transform>();
        var ballUIFrom = GetBallUI(moveBalls.FromBall);
        moveTrans.Add(ballUIFrom.transform);
        var ballUITo = GetBallUI(moveBalls.ToBall);
        moveTrans.Add(ballUITo.transform);

        UIGuide.ShowPoint(moveTrans);
    }

    private void GuideUpdate()
    {
        if (UIGuide.IsShowingGuide())
            return;

        if (_LastMoveTime == 0)
            return;

        if (Time.time - _LastMoveTime > _GuideShowDelay)
        {
            GuideTips();
            _LastMoveTime = Time.time;
        }
    }

    #endregion

    #region sound

    public AudioClip _CombineAudio;
    public AudioClip _BombAudio;
    public AudioClip _LineAudio;
    public AudioClip _ElimitAudio;
    public AudioClip _MoveAudio;

    public enum FIGHT_SOUND_TYPE
    {
        ELIMIT = 0,
        CLUB,
        ICE,
        COMBINE,
        LINE,
        BOMB,
    }

    public void PlayCombineSound()
    {
        PlayerUISound(_CombineAudio);
    }

    public void PlayMoveSound()
    {
        PlayerUISound(_MoveAudio);
    }

    public void PlayBombSound()
    {
        PlayerUISound(_BombAudio);
    }

    public void PlayLineSound()
    {
        PlayerUISound(_LineAudio);
    }

    #endregion

    public void DebugTest()
    {
        for (int j = BallBox.Instance._BallBoxInfo[0].Length - 1; j >= 0; --j)
        {
            string ballInfo = "";
            for (int i = 0; i < BallBox.Instance._BallBoxInfo.Length; ++i)
            {
                ballInfo += (int)BallBox.Instance._BallBoxInfo[i][j].BallType + " ";
            }
            Debug.Log(ballInfo);
        }

        string testStr = "";
        testStr += (int)BallBox.Instance._BallBoxInfo[0][0].BallType + " ";
        testStr += (int)BallBox.Instance._BallBoxInfo[1][1].BallType + " ";
        testStr += (int)BallBox.Instance._BallBoxInfo[2][2].BallType + " ";
        testStr += (int)BallBox.Instance._BallBoxInfo[3][3].BallType + " ";
        testStr += (int)BallBox.Instance._BallBoxInfo[4][4].BallType + " ";
        testStr += (int)BallBox.Instance._BallBoxInfo[5][5].BallType + " ";
        Debug.Log("UIFIghtBox test:" + testStr);
    }

    public bool _TestMode = false;
    public void TestBallStep()
    {
        int textTime = 0;
        
        //foreach (var stageRecord in TableReader.StageInfo.Records.Values)
        {
            //ResetSceneLogic(stageRecord);
            //for (int k = 0; k < RecordBallDamage._RecordTimes; ++k)
            {
                //for (int j = 0; j < RecordBallDamage._RecordRound; ++j)
                {
                    for (int i = 0; i < BattleField._DamageOptRound; ++i)
                    {
                        TestBallMove();
                    }
                }
                //OnReset();
            }
        }

        //RecordBallDamage.WriteRecords();
    }

    public void TestBall()
    {
        int textTime = 0;
        _TestMode = true;
        //foreach (var stageRecord in TableReader.StageInfo.Records.Values)
        {
            //ResetSceneLogic(stageRecord);
            //for (int k = 0; k < RecordBallDamage._RecordTimes; ++k)
            {
                //for (int j = 0; j < RecordBallDamage._RecordRound; ++j)
                {
                    //for (int i = 0; i < BattleField._DamageOptRound; ++i)
                    //{
                    //    TestBallMove();
                    //}
                }
                //OnReset();
            }
        }

        //RecordBallDamage.WriteRecords();
    }

    private float _LastOptTime = -1;
    private float _OptWait = 0.1f;
    public void TestFightUpdate()
    {
        //return;
        if (!_TestMode)
            return;

        if (UIStageFail.IsShow())
            return;

        if (UIStageSucess.IsShow())
            return;

        if (Time.time - _LastOptTime > _OptWait)
        {
            TestBallMove();
            _LastOptTime = Time.time;
        }
    }

    public void TestBallMove()
    {
        var eliminate = BallBox.Instance.FindPowerEliminate();
        if (eliminate == null)
        {
            while (eliminate == null)
            {
                RefreshNormal();
                eliminate = BallBox.Instance.FindPowerEliminate();
            }
        }

        var ballA = GetBallUI(eliminate.FromBall);
        var ballB = GetBallUI(eliminate.ToBall);
        BallBox.Instance.MoveBall(ballA.BallInfo, ballB.BallInfo);

        ballA.ShowBall();
        ballB.ShowBall();

        var moveList = new List<BallInfo>() { ballA.BallInfo, ballB.BallInfo };
        var elimitBalls = BallBox.Instance.CheckNormalEliminate(moveList);
        var spElimitMove = BallBox.Instance.CheckSpMove(moveList);
        var spElimitElimit = BallBox.Instance.CheckSpElimit(elimitBalls);
        var exploreBalls = BallBox.Instance.CurrentElimitnate();
        var afterElimit = BallBox.Instance.AfterElimitnate();

        BallBox.AddBallInfos(elimitBalls, spElimitMove);
        BallBox.AddBallInfos(elimitBalls, spElimitElimit);
        BallBox.AddBallInfos(elimitBalls, exploreBalls);
        BallBox.AddBallInfos(elimitBalls, afterElimit);
        {

            do
            {
                BallBox.Instance.ClearElimitInfo();

                foreach (var elimitBall in elimitBalls)
                {
                    var uiBall = GetBallUI(elimitBall);
                    uiBall.ShowBall();

                }
                UIFightBattleField.ShowElimitInfo();

                var fallExchanges = BallBox.Instance.ElimitnateFall();
                for (int i = 0; i < fallExchanges.Count; ++i)
                {
                    var uiBall1 = GetBallUI(fallExchanges[i].FromBall);
                    if (uiBall1 != null)
                    {
                        uiBall1.ShowBall();
                    }

                    var uiBall2 = GetBallUI(fallExchanges[i].ToBall);
                    uiBall2.ShowBall();

                    _FillingBalls.Add(uiBall2.BallInfo);
                }
                elimitBalls = BallBox.Instance.CheckNormalEliminate(_FillingBalls);
                var spSubElimitBalls = BallBox.Instance.CheckSpElimit(elimitBalls);
                var subexploreBalls = BallBox.Instance.CurrentElimitnate();
                var subafterElimit = BallBox.Instance.AfterElimitnate();

                BallBox.AddBallInfos(elimitBalls, spSubElimitBalls);
                BallBox.AddBallInfos(elimitBalls, subexploreBalls);
                BallBox.AddBallInfos(elimitBalls, subafterElimit);
            }
            while (elimitBalls.Count > 0);
        }

        if (BallBox.Instance.CheckElimitSimple())
        {
            _TestMode = false;
            return;
        }

        var reShowList = BallBox.Instance.RoundEnd();

        if (reShowList != null && reShowList.Count > 0)
        {
            foreach (var elimitBall in reShowList)
            {
                var uiBall = GetBallUI(elimitBall);
                uiBall.ShowBall();
            }
        }

        eliminate = BallBox.Instance.FindPowerEliminate();
        if (eliminate == null)
        {
            int refreshTimes = 0;
            while (eliminate == null)
            {
                if (refreshTimes < 2)
                {
                    RefreshNormal();
                }
                else
                {
                    BallBox.Instance.RefreshNormalForElimit(true);
                    UpdateBalls();
                }
                ++refreshTimes;

                eliminate = BallBox.Instance.FindPowerEliminate();
            }
        }
        _LastMoveTime = Time.time;
    }

}
