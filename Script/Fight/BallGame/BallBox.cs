using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;
using System;

public class ExchangeBalls
{
    public BallInfo FromBall;
    public BallInfo ToBall;

    public List<BallInfo> PathBalls;

    public ExchangeBalls(BallInfo from, BallInfo to)
    {
        FromBall = from;
        ToBall = to;
    }

    public ExchangeBalls(BallInfo from, BallInfo to, List<BallInfo> pathBalls)
    {
        FromBall = from;
        ToBall = to;
        PathBalls = pathBalls;
    }
}

public class EliminateInfo
{
    public BallInfo KeyBall;
    public BallInfo MoveBall;

    public int EliminateCnt;
}

public class OptExtra
{
    public string _ExtraType;
    public BallInfo _OptBall;
    public List<BallInfo> _ElimitBalls;
}

public class BallBox
{
    #region static

    private static BallBox _Instance;

    public static BallBox Instance
    {
        get
        {
            if (_Instance == null)
                _Instance = new BallBox();

            return _Instance;
        }
    }

    public static bool IsTypeNormal(int type)
    {
        return (BallType)type > BallType.NormalBallStart && (BallType)type < BallType.NormalBallEnd;
    }

    public static bool IsTypeSP(int type)
    {
        return (BallType)type > BallType.SPTrapStart && (BallType)type < BallType.SPRPGEnd;
    }
    #endregion


    #region base

    private int _BoxWidth = 6;
    public int BoxWidth
    {
        get
        {
            return _BoxWidth;
        }
    }

    public int _BoxHeight = 7;
    public int BoxHeight
    {
        get
        {
            return _BoxHeight;
        }
    }

    public const int _DefaultPassWidth = 2;
    public const int _DefaultDisapearCnt = 3;
    public BallInfo[][] _BallBoxInfo;
    public bool _IsContainEmptyNormal = false;

    public int _ElimitBombCnt = 0;
    public int _ElimitTrapCnt = 0;

    private StageMapRecord _MapRecord;

    public void Init(StageMapRecord mapRecord)
    {
        _MapRecord = mapRecord;
        _BoxWidth = _MapRecord._Width;
        _BoxHeight = _MapRecord._Height;
        _BallBoxInfo = new BallInfo[_BoxWidth][];
        for (int i = 0; i < _BoxWidth; ++i)
        {
            _BallBoxInfo[i] = new BallInfo[_BoxHeight];
            for (int j = 0; j < _BoxHeight; ++j)
            {
                _BallBoxInfo[i][j] = new BallInfo(i,j);
            }
        }

        var weaponType = Type.GetType(WeaponDataPack.Instance.SelectedWeaponItem.WeaponRecord.Script);
        Debug.Log("OptType:" + WeaponDataPack.Instance.SelectedWeaponItem.WeaponRecord.Script);
        _OptImpact = Activator.CreateInstance(weaponType) as OptImpactBase;
        _OptImpact.Init();
        _Round = 1;

        _ElimitBombCnt = 0;
        _ElimitTrapCnt = 0;

        _OptRound = 0;
        _Round = 0;
    }

    public void Refresh()
    {
        for (int j = 0; j < _BallBoxInfo[0].Length; ++j)
        {
            for (int i = 0; i < _BallBoxInfo.Length; ++i)
            {
                _BallBoxInfo[i][j].Clear();
            }
        }
        InitBallInfo();

        RandomMonster._MapRecord.Clear();
        RandomMonster.RecordMap(_BallBoxInfo);
    }

    public void RefreshNormalForElimit(bool refreshElimitPos)
    {
        List<BallInfo> normalBalls = new List<BallInfo>();
        
        for (int j = 0; j < _BallBoxInfo[0].Length; ++j)
        {
            for (int i = 0; i < _BallBoxInfo.Length; ++i)
            {
                if (_BallBoxInfo[i][j].IsNormalBall())
                {
                    normalBalls.Add(_BallBoxInfo[i][j]);
                }
            }
        }

        List<BallInfo> elimitPos = new List<BallInfo>();
        if (refreshElimitPos)
        {
            foreach (var normalBall in normalBalls)
            {
                int i = (int)normalBall.Pos.x;
                int j = (int)normalBall.Pos.y;

                var ballnext1 = GetBallInfo(i + 1, j);
                var ballnext2 = GetBallInfo(i + 2, j);
                if (ballnext1!=null && ballnext2 != null && ballnext1.IsNormalBall() && ballnext2.IsNormalBall())
                {
                    var ballmove = GetBallInfo(i+3,j);
                    if (ballmove != null && ballmove.IsNormalBall())
                    {
                        elimitPos.Add(normalBall);
                        elimitPos.Add(ballnext2);
                        elimitPos.Add(ballmove);
                        break;
                    }

                    ballmove = GetBallInfo(i, j + 1);
                    if (ballmove != null && ballmove.IsNormalBall())
                    {
                        elimitPos.Add(ballnext1);
                        elimitPos.Add(ballnext2);
                        elimitPos.Add(ballmove);
                        break;
                    }

                    ballmove = GetBallInfo(i + 1, j + 1);
                    if (ballmove != null && ballmove.IsNormalBall())
                    {
                        elimitPos.Add(normalBall);
                        elimitPos.Add(ballnext2);
                        elimitPos.Add(ballmove);
                        break;
                    }

                    ballmove = GetBallInfo(i + 2, j + 1);
                    if (ballmove != null && ballmove.IsNormalBall())
                    {
                        elimitPos.Add(ballnext1);
                        elimitPos.Add(normalBall);
                        elimitPos.Add(ballmove);
                        break;
                    }
                }

                ballnext1 = GetBallInfo(i, j + 1);
                ballnext2 = GetBallInfo(i, j + 2);
                if (ballnext1 != null && ballnext2 != null && ballnext1.IsNormalBall() && ballnext2.IsNormalBall())
                {
                    var ballmove = GetBallInfo(i, j + 3);
                    if (ballmove != null && ballmove.IsNormalBall())
                    {
                        elimitPos.Add(ballnext1);
                        elimitPos.Add(normalBall);
                        elimitPos.Add(ballmove);
                        break;
                    }

                    ballmove = GetBallInfo(i + 1, j);
                    if (ballmove != null && ballmove.IsNormalBall())
                    {
                        elimitPos.Add(ballnext1);
                        elimitPos.Add(ballnext2);
                        elimitPos.Add(ballmove);
                        break;
                    }

                    ballmove = GetBallInfo(i + 1, j + 1);
                    if (ballmove != null && ballmove.IsNormalBall())
                    {
                        elimitPos.Add(normalBall);
                        elimitPos.Add(ballnext2);
                        elimitPos.Add(ballmove);
                        break;
                    }

                    ballmove = GetBallInfo(i + 1, j + 2);
                    if (ballmove != null && ballmove.IsNormalBall())
                    {
                        elimitPos.Add(ballnext1);
                        elimitPos.Add(normalBall);
                        elimitPos.Add(ballmove);
                        break;
                    }
                }
            }

            List<BallType> exportBall = new List<BallType>();
            foreach (var elimit in elimitPos)
            {
                int i = (int)elimit.Pos.x;
                int j = (int)elimit.Pos.y;
                var ballmove = GetBallInfo(i + 1, j);
                if (ballmove != null && ballmove._BallInfoSP != null && ballmove.IsCanNormalElimit())
                {
                    exportBall.Add(ballmove.BallType);
                }

                ballmove = GetBallInfo(i - 1, j);
                if (ballmove != null && ballmove._BallInfoSP != null && ballmove.IsCanNormalElimit())
                {
                    exportBall.Add(ballmove.BallType);
                }

                ballmove = GetBallInfo(i, j + 1);
                if (ballmove != null && ballmove._BallInfoSP != null && ballmove.IsCanNormalElimit())
                {
                    exportBall.Add(ballmove.BallType);
                }

                ballmove = GetBallInfo(i, j - 1);
                if (ballmove != null && ballmove._BallInfoSP != null && ballmove.IsCanNormalElimit())
                {
                    exportBall.Add(ballmove.BallType);
                }
            }
            
            var randomElimitType = BallInfo.GetRandomBallType(exportBall);
            foreach (var elimit in elimitPos)
            {
                elimit.SetBallType(randomElimitType);
            }
        }

        

        foreach (var normalBall in normalBalls)
        {
            //for (int i = 0; i < _BallBoxInfo.Length; ++i)
            {
                if (elimitPos.Contains(normalBall))
                    continue;

                int i = (int)normalBall.Pos.x;
                int j = (int)normalBall.Pos.y;
                List<BallType> exportBall = new List<BallType>();
                if (i > _DefaultDisapearCnt - 2)
                {
                    if (_BallBoxInfo[i - 1][j].IsCanNormalElimit(_BallBoxInfo[i - 2][j]))
                    {
                        exportBall.Add(_BallBoxInfo[i - 1][j].BallType);
                    }
                }

                if (i < _BallBoxInfo.Length - _DefaultDisapearCnt + 1)
                {
                    if (_BallBoxInfo[i + 1][j].IsCanNormalElimit(_BallBoxInfo[i + 2][j]))
                    {
                        exportBall.Add(_BallBoxInfo[i + 1][j].BallType);
                    }
                }

                if (j > _DefaultDisapearCnt - 2)
                {
                    if (_BallBoxInfo[i][j - 1].IsCanNormalElimit(_BallBoxInfo[i][j - 2]))
                    {
                        exportBall.Add(_BallBoxInfo[i][j - 1].BallType);
                    }
                }

                if (j < _BallBoxInfo[0].Length - _DefaultDisapearCnt + 1)
                {
                    if (_BallBoxInfo[i][j + 1].IsCanNormalElimit(_BallBoxInfo[i][j + 2]))
                    {
                        exportBall.Add(_BallBoxInfo[i][j + 1].BallType);
                    }
                }

                if (j < _BallBoxInfo[0].Length - 1 && j > 0)
                {
                    if (_BallBoxInfo[i][j + 1].IsCanNormalElimit(_BallBoxInfo[i][j - 1]))
                    {
                        exportBall.Add(_BallBoxInfo[i][j + 1].BallType);
                    }
                }

                if (i < _BallBoxInfo.Length - 1 && i > 0)
                {
                    if (_BallBoxInfo[i + 1][j].IsCanNormalElimit(_BallBoxInfo[i - 1][j]))
                    {
                        exportBall.Add(_BallBoxInfo[i + 1][j].BallType);
                    }
                }

                if (exportBall.Count > 0)
                {
                    string exBall = "";
                    foreach (var exBallitem in exportBall)
                    {
                        exBall += ((int)exBallitem).ToString();
                    }
                }

                _BallBoxInfo[i][j].SetRandomBall(exportBall);
            }

        }
    }


    private void InitDefault()
    {
        foreach (var mapPos in _MapRecord._MapDefaults)
        {
            string[] mapPosSplit = mapPos.Key.Split(',');
            var ballInfo = GetBallInfo(int.Parse(mapPosSplit[0]),int.Parse(mapPosSplit[1]));
            if (ballInfo != null)
            {
                string spType = mapPos.Value;
                if (!string.IsNullOrEmpty(spType))
                {
                    ballInfo.SetBallInitType(spType);
                }
            }
        }
    }

    public void InitBallInfo()
    {
        BallInfoSPTrapPosion.InitPosion();
        BallInfoSPLineReact.InitReactBalls();

        InitDefault();
        for (int j = 0; j < _BallBoxInfo[0].Length; ++j)
        {
            for (int i = 0; i < _BallBoxInfo.Length; ++i)
            {
                if (_BallBoxInfo[i][j].BallType != BallType.None)
                    continue;

                _BallBoxInfo[i][j].SetRandomBall(null);
                _BallBoxInfo[i][j].BornRound = _Round;
            }

        }
    }

    public BallInfo GetBallInfo(int x, int y)
    {
        if (x < 0 || x >= _BallBoxInfo.Length)
            return null;

        if (y < 0 || y >= _BallBoxInfo[0].Length)
            return null;

        return _BallBoxInfo[x][y];
    }


    #endregion

    #region opt

    public int _Round = 0;
    public int _OptRound = 0;

    public OptImpactBase _OptImpact;

    public void MoveBall(BallInfo ballA, BallInfo ballB)
    {
        ++_Round;
        ++_OptRound;
        ballA.ExChangeBall(ballB);
    }

    public void MoveBack(BallInfo ballA, BallInfo ballB)
    {
        --_Round;
        --_OptRound;
        ballA.ExChangeBall(ballB);
    }

    public void ExchangeBalls(BallInfo ballA, BallInfo ballB)
    {
        if (!ballA.IsCanExChange(ballB))
            return;

        ballA.ExChangeBall(ballB);
    }

    public List<BallInfo> RoundEnd()
    {
        BattleField.Instance.BallDamage(_RoundTotalEliminate);
        _RoundTotalEliminate.Clear();

        var reshowBalls = BallInfoSPTrapPosion.OnPosionRoundEnd();

        RandomMonster.RecordMap(_BallBoxInfo);
        return reshowBalls;
    }

    public bool IsCanMoveTo(BallInfo ballSource, BallInfo ballDest)
    {
        if (ballDest.Pos.x < 0 || ballDest.Pos.x >= _BallBoxInfo.Length)
            return false;

        if (ballDest.Pos.y < 0 || ballDest.Pos.y >= _BallBoxInfo[0].Length)
            return false;

        int xdelta = (int)Mathf.Abs(ballDest.Pos.x - ballSource.Pos.x);
        int ydelta = (int)Mathf.Abs(ballDest.Pos.y - ballSource.Pos.y);
        if (xdelta + ydelta == 1)
            return true;

        return false;
    }
    #endregion

    #region elimit

    public Dictionary<BallType, int> _RoundTotalEliminate = new Dictionary<BallType, int>();

    private List<BallInfo> _CurrentNormalEliminateBalls = new List<BallInfo>();
    private List<BallInfo> _CurrentSPEliminateBalls = new List<BallInfo>();
    public List<OptExtra> _CurrentOptExtra = new List<OptExtra>();

    private bool AddEliminateBall(BallInfo ballInfo)
    {
        if (!_CurrentNormalEliminateBalls.Contains(ballInfo))
        {
            _CurrentNormalEliminateBalls.Add(ballInfo);
            return true;
        }

        return false;
    }

    private void AddSPEliminateBall(BallInfo ballInfo)
    {
        if (_CurrentNormalEliminateBalls.Contains(ballInfo))
            return;

        if (!_CurrentSPEliminateBalls.Contains(ballInfo))
        {
            _CurrentSPEliminateBalls.Add(ballInfo);
        }

    }

    private void AddRoundTotalEliminate(BallInfo ballInfo)
    {
        if (ballInfo._BallInfoSP != null)
        {
            if (!_RoundTotalEliminate.ContainsKey(ballInfo.BallSPType))
            {
                _RoundTotalEliminate.Add(ballInfo.BallSPType, 0);
            }
            ++_RoundTotalEliminate[ballInfo.BallSPType];
        }
        if (!_RoundTotalEliminate.ContainsKey(ballInfo.BallType))
        {
            _RoundTotalEliminate.Add(ballInfo.BallType, 0);
        }
        ++_RoundTotalEliminate[ballInfo.BallType];
    }

    private void AddOptExtra(OptExtra optExtra)
    {
        if (!_CurrentOptExtra.Contains(optExtra))
        {
            _CurrentOptExtra.Add(optExtra);
        }
    }

    public OptExtra GetOptExtra(List<BallInfo> checkBalls)
    {
        foreach (var optExtra in _CurrentOptExtra)
        {
            if (checkBalls.Contains(optExtra._OptBall))
                return optExtra;
        }

        return null;
    }

    public List<BallInfo> CurrentElimitnate()
    {
        ++_Round;
        List<BallInfo> checkExplore = new List<BallInfo>();
        foreach (var ballInfo in _CurrentNormalEliminateBalls)
        {
            AddRoundTotalEliminate(ballInfo);
            ballInfo.OnNormalElimit();

            checkExplore.Add(ballInfo);
        }

        foreach (var ballInfo in _CurrentSPEliminateBalls)
        {
            if (ballInfo.IsRemain())
                continue;

            bool isContainNormal = ballInfo.IsContainNormal();

            AddRoundTotalEliminate(ballInfo);
            ballInfo.OnSPElimit();
            
            if (isContainNormal)
            {
                if (!ballInfo.IsContainNormal())
                {
                    checkExplore.Add(ballInfo);
                }
            }
        }

        var exploreBalls = GetExploreBalls(checkExplore);

        foreach (var optEx in _CurrentOptExtra)
        {
            foreach (var absorbBall in optEx._ElimitBalls)
            {
                if (absorbBall._BallInfoSP is BallInfoSPLineReact)
                {
                    absorbBall.Clear();
                }
            }

            _OptImpact.SetElimitExtra(optEx._ExtraType, optEx._OptBall);
            
        }

        

        return exploreBalls;
    }

    public void ClearElimitInfo()
    {
        _CurrentNormalEliminateBalls.Clear();
        _CurrentSPEliminateBalls.Clear();
        _CurrentOptExtra.Clear();

        for (int j = 0; j < _BallBoxInfo[0].Length; ++j)
        {
            for (int i = 0; i < _BallBoxInfo.Length; ++i)
            {
                _BallBoxInfo[i][j].ClearElimitInfo();
            }
        }
    }

    public List<BallInfo> AfterElimitnate()
    {
        if (BallInfoSPBombAuto.AutoBombList.Count == 0)
            return null;

        List<BallInfo> ballList = new List<BallInfo>();
        foreach (var autoBomb in BallInfoSPBombAuto.AutoBombList)
        {
            if (autoBomb.BallInfo._BallInfoSP == autoBomb)
            {
                autoBomb.BallInfo.BornRound = autoBomb.BallInfo.BornRound - 1;
                ballList.Add(autoBomb.BallInfo);
            }
        }

        var bombBalls = CheckSpElimit(ballList);
        var exploreBalls = CurrentElimitnate();

        BallBox.AddBallInfos(ballList, bombBalls);
        BallBox.AddBallInfos(ballList, exploreBalls);

        return ballList;
    }

    public ExchangeBalls FindAnyEliminate()
    {
        EliminateInfo exangeBall = new EliminateInfo();
        for (int j = 0; j < _BallBoxInfo[0].Length; ++j)
        {
            for (int i = 0; i < _BallBoxInfo.Length; ++i)
            {
                var curBall = GetBallInfo(i, j);
                var nextBall = GetBallInfo(i + 1, j);
                if (nextBall == null)
                    continue;

                if (!curBall.IsCanMove() || !nextBall.IsCanMove())
                    continue;

                ExchangeBalls(curBall, nextBall);

                var elimitBallInfos = TestNormalEliminate(new List<BallInfo>() { curBall, nextBall });
                var spElimitsMove = TestSpMove(new List<BallInfo>() { curBall, nextBall });
                var spElimitsElimit = TestSpElimit(elimitBallInfos);
                int checkCnt = elimitBallInfos.Count;
                foreach (var spElimit in spElimitsMove)
                {
                    if (!elimitBallInfos.Contains(spElimit))
                    {
                        ++checkCnt;
                    }
                }
                foreach (var spElimit in spElimitsElimit)
                {
                    if (!elimitBallInfos.Contains(spElimit)
                        && !spElimitsMove.Contains(spElimit))
                    {
                        ++checkCnt;
                    }
                }

                ExchangeBalls(curBall, nextBall);
                if (checkCnt > 0)
                {
                    exangeBall.KeyBall = curBall;
                    exangeBall.MoveBall = nextBall;
                    exangeBall.EliminateCnt = checkCnt;
                    break;
                }

                

                nextBall = GetBallInfo(i, j + 1);
                if (nextBall == null)
                    continue;

                if (!curBall.IsCanMove() || !nextBall.IsCanMove())
                    continue;

                ExchangeBalls(curBall, nextBall);

                elimitBallInfos = TestNormalEliminate(new List<BallInfo>() { curBall, nextBall });
                spElimitsMove = TestSpMove(new List<BallInfo>() { curBall, nextBall });
                spElimitsElimit = TestSpElimit(elimitBallInfos);
                checkCnt = elimitBallInfos.Count;
                foreach (var spElimit in spElimitsMove)
                {
                    if (!elimitBallInfos.Contains(spElimit))
                    {
                        ++checkCnt;
                    }
                }
                foreach (var spElimit in spElimitsElimit)
                {
                    if (!elimitBallInfos.Contains(spElimit)
                        && !spElimitsMove.Contains(spElimit))
                    {
                        ++checkCnt;
                    }
                }

                ExchangeBalls(curBall, nextBall);
                if (checkCnt > 0)
                {
                    exangeBall.KeyBall = curBall;
                    exangeBall.MoveBall = nextBall;
                    exangeBall.EliminateCnt = checkCnt;
                    break;
                }
            }
        }
        if (exangeBall.KeyBall == null)
            return null;

        return new ExchangeBalls(exangeBall.KeyBall, exangeBall.MoveBall);
    }

   

    public ExchangeBalls FindPowerEliminate()
    {
        EliminateInfo exangeBall = new EliminateInfo();
        for (int j = 0; j < _BallBoxInfo[0].Length; ++j)
        {
            for (int i = 0; i < _BallBoxInfo.Length; ++i)
            {
                var curBall = GetBallInfo(i,j);
                var nextBall = GetBallInfo(i+1,j);
                if (nextBall == null)
                    continue;

                if (!curBall.IsCanMove() || !nextBall.IsCanMove())
                    continue;

                ExchangeBalls(curBall, nextBall);

                var elimitBallInfos = TestNormalEliminate(new List<BallInfo>() { curBall, nextBall });
                var spElimitsMove = TestSpMove(new List<BallInfo>() { curBall, nextBall });
                var spElimitsElimit = TestSpElimit(elimitBallInfos);
                int checkCnt = elimitBallInfos.Count;
                foreach (var spElimit in spElimitsMove)
                {
                    if (!elimitBallInfos.Contains(spElimit))
                    {
                        ++checkCnt;
                    }
                }
                foreach (var spElimit in spElimitsElimit)
                {
                    if (!elimitBallInfos.Contains(spElimit)
                        && !spElimitsMove.Contains(spElimit))
                    {
                        ++checkCnt;
                    }
                }

                if (checkCnt > exangeBall.EliminateCnt)
                {
                    exangeBall.KeyBall = curBall;
                    exangeBall.MoveBall = nextBall;
                    exangeBall.EliminateCnt = checkCnt;
                }

                ExchangeBalls(curBall, nextBall);

                nextBall = GetBallInfo(i, j + 1);
                if (nextBall == null)
                    continue;

                if (!curBall.IsCanMove() || !nextBall.IsCanMove())
                    continue;

                ExchangeBalls(curBall, nextBall);

                elimitBallInfos = TestNormalEliminate(new List<BallInfo>() { curBall, nextBall });
                spElimitsMove = TestSpMove(new List<BallInfo>() { curBall, nextBall });
                spElimitsElimit = TestSpElimit(elimitBallInfos);
                checkCnt = elimitBallInfos.Count;
                foreach (var spElimit in spElimitsMove)
                {
                    if (!elimitBallInfos.Contains(spElimit))
                    {
                        ++checkCnt;
                    }
                }
                foreach (var spElimit in spElimitsElimit)
                {
                    if (!elimitBallInfos.Contains(spElimit)
                        && !spElimitsMove.Contains(spElimit))
                    {
                        ++checkCnt;
                    }
                }
                if (checkCnt > exangeBall.EliminateCnt)
                {
                    exangeBall.KeyBall = curBall;
                    exangeBall.MoveBall = nextBall;
                    exangeBall.EliminateCnt = checkCnt;
                }
                ExchangeBalls(curBall, nextBall);
            }
        }
        if (exangeBall.KeyBall == null)
            return null;

        return new ExchangeBalls(exangeBall.KeyBall, exangeBall.MoveBall);
    }

    public BallInfo.ElimitMap FindPowerEliminate2()
    {
        List<BallInfo.ElimitMap> elimitMaps = new List<BallInfo.ElimitMap>();
        for (int j = 0; j < _BallBoxInfo[0].Length; ++j)
        {
            for (int i = 0; i < _BallBoxInfo.Length; ++i)
            {
                var curBall = GetBallInfo(i, j);
                if (!curBall.IsCanMove())
                    continue;

                if (curBall._ElimitMap != null)
                    continue;

                var mapInfo = FindElimitInfoMap(i, j, new Stack<BallInfo>());
                if (mapInfo == null)
                    continue;

                if (mapInfo.SlotCount > _DefaultDisapearCnt)
                {
                    if (elimitMaps.Count == 0)
                    {
                        elimitMaps.Add(mapInfo);
                    }
                    else
                    {
                        for (int k = 0; k < elimitMaps.Count; ++k)
                        {
                            if (mapInfo.SlotCount > elimitMaps[k].SlotCount)
                            {
                                elimitMaps.Insert(k, mapInfo);
                                break;
                            }
                            if (k == elimitMaps.Count - 1)
                            {
                                elimitMaps.Add(mapInfo);
                                break;
                            }
                        }
                    }
                }
            }
        }

        return elimitMaps[0];
    }

    //public void List<BallInfo> FindPathFromMap(BallInfo.ElimitMap elimitMap)
    //{

    //}

    public BallInfo.ElimitMap FindElimitInfoMap(int posX, int posY, Stack<BallInfo> ballStack)
    {
        var curBall = GetBallInfo(posX, posY);
        

        List<BallInfo> aroundBalls = new List<BallInfo>();

        aroundBalls.Add(GetBallInfo(posX, posY +1));
        aroundBalls.Add(GetBallInfo(posX + 1, posY + 1));
        aroundBalls.Add(GetBallInfo(posX + 1, posY));
        aroundBalls.Add(GetBallInfo(posX + 1, posY - 1));
        aroundBalls.Add(GetBallInfo(posX, posY - 1));
        aroundBalls.Add(GetBallInfo(posX - 1, posY - 1));
        aroundBalls.Add(GetBallInfo(posX - 1, posY));
        aroundBalls.Add(GetBallInfo(posX - 1, posY + 1));

        int elimitCnt = 0;
        bool isKnot = false;

        for (int i = 0; i < aroundBalls.Count; ++i)
        {
            if (curBall.IsCanNormalElimit(aroundBalls[i]))
            {
                if (curBall._ElimitMap == null)
                {
                    curBall.SetMap(null);
                }

                if (aroundBalls[i]._ElimitMap == null)
                {
                    PushElimitStack(ballStack, aroundBalls[i]);
                    aroundBalls[i].SetMap(curBall._ElimitMap);
                }
                ++elimitCnt;
                int checknum = 1;
                if (i % 2 == 0)
                {
                    checknum = 2;
                }

                bool thisKnot = false;
                for (int j = -checknum; j <= checknum; ++j)
                {
                    if (j == 0)
                        continue;

                    int checkIdx = i + j;
                    if (checkIdx < 0)
                    {
                        checkIdx = aroundBalls.Count + checkIdx;
                    }

                    if (checkIdx > aroundBalls.Count - 1)
                    {
                        checkIdx = checkIdx - (aroundBalls.Count);
                    }

                    if (curBall.IsCanNormalElimit(aroundBalls[checkIdx]))
                    {
                        curBall._ElimitMap.AddKnot(curBall, aroundBalls[i], aroundBalls[checkIdx]);
                        if (aroundBalls[checkIdx]._ElimitMap == null)
                        {
                            PushElimitStack(ballStack, aroundBalls[checkIdx]);
                            aroundBalls[checkIdx].SetMap(curBall._ElimitMap);
                        }
                        isKnot = true;
                        thisKnot = true;
                    }
                }

                if (!thisKnot)
                {
                    if (curBall._ElimitKnot == null)
                    {
                        curBall._Passes.Add(aroundBalls[i]);
                    }
                    else
                    {
                        curBall._ElimitKnot._Gates.Add(curBall, aroundBalls[i]);
                    }
                }
            }
        }

        if (!isKnot)
        {
            if (elimitCnt > 2)
            {
                curBall._ElimitMap.AddCrossWay(curBall);
            }
            else if (elimitCnt > 1)
            {
                curBall._ElimitMap.AddPassWay(curBall);
            }
            else if(elimitCnt == 1)
            {
                curBall._ElimitMap.AddGate(curBall);
            }

            
        }

        if (ballStack.Count != 0)
        {
            var nextBall = ballStack.Pop();
            FindElimitInfoMap((int)nextBall.Pos.x, (int)nextBall.Pos.y, ballStack);
        }
        return curBall._ElimitMap;
    }

    public void PushElimitStack(Stack<BallInfo> elimitStack, BallInfo ballInfo)
    {
        if (elimitStack.Contains(ballInfo))
            return;

        if (ballInfo._ElimitMap != null)
            return;

        elimitStack.Push(ballInfo);
    }

    public List<BallInfo> FindLastEliminate()
    {
        List<BallInfo> lastElimts = new List<BallInfo>();
        for (int j = _BallBoxInfo[0].Length - 1; j >= 0; --j)
        {
            for (int i = _BallBoxInfo.Length - 1; i >= 0; --i)
            {
                var curBall = GetBallInfo(i, j);
                var nextBall = GetBallInfo(i - 1, j);
                if (nextBall == null)
                    continue;

                if (!curBall.IsCanMove() || !nextBall.IsCanMove())
                    continue;

                ExchangeBalls(curBall, nextBall);

                var elimitBallInfos = TestNormalEliminate(new List<BallInfo>() { curBall, nextBall });
                var spElimitsMove = TestSpMove(new List<BallInfo>() { curBall, nextBall });
                var spElimitsElimit = TestSpElimit(elimitBallInfos);
                int checkCnt = elimitBallInfos.Count;
                foreach (var spElimit in spElimitsMove)
                {
                    if (!elimitBallInfos.Contains(spElimit))
                    {
                        ++checkCnt;
                    }
                }
                foreach (var spElimit in spElimitsElimit)
                {
                    if (!elimitBallInfos.Contains(spElimit)
                        && !spElimitsMove.Contains(spElimit))
                    {
                        ++checkCnt;
                    }
                }

                ExchangeBalls(curBall, nextBall);

                if (checkCnt > 0)
                {
                    BallBox.AddBallInfos(lastElimts, curBall);
                    BallBox.AddBallInfos(lastElimts, nextBall);
                    BallBox.AddBallInfos(lastElimts, elimitBallInfos);

                    return lastElimts;
                }

                

                nextBall = GetBallInfo(i, j - 1);
                if (nextBall == null)
                    continue;
                ExchangeBalls(curBall, nextBall);

                elimitBallInfos = TestNormalEliminate(new List<BallInfo>() { curBall, nextBall });
                spElimitsMove = TestSpMove(new List<BallInfo>() { curBall, nextBall });
                spElimitsElimit = TestSpElimit(elimitBallInfos);
                checkCnt = elimitBallInfos.Count;
                foreach (var spElimit in spElimitsMove)
                {
                    if (!elimitBallInfos.Contains(spElimit))
                    {
                        ++checkCnt;
                    }
                }
                foreach (var spElimit in spElimitsElimit)
                {
                    if (!elimitBallInfos.Contains(spElimit)
                        && !spElimitsMove.Contains(spElimit))
                    {
                        ++checkCnt;
                    }
                }

                ExchangeBalls(curBall, nextBall);

                if (checkCnt > 0)
                {
                    BallBox.AddBallInfos(lastElimts, curBall);
                    BallBox.AddBallInfos(lastElimts, nextBall);
                    BallBox.AddBallInfos(lastElimts, elimitBallInfos);
                    return lastElimts;
                }
                
            }
        }

        return lastElimts;
    }

    private EliminateInfo GetEliminateInfo(int posX, int posY, int posX2, int posY2)
    {
        var bombBall = GetBallInfo(posX, posY);
        if (bombBall != null && bombBall.IsCanNormalElimit() && bombBall.IsCanMove())
        {
            EliminateInfo elimitInfo = new EliminateInfo();
            elimitInfo.KeyBall = bombBall;
            elimitInfo.EliminateCnt = 1;

            var bombBallEx = GetBallInfo(posX2, posY2);
            if (bombBallEx != null
                && bombBallEx.IsCanNormalElimit(bombBall)
                && bombBallEx.IsCanMove())
            {
                elimitInfo.EliminateCnt = 2;
            }

            return elimitInfo;
        }

        return null;
    }

    public List<BallInfo> TestNormalEliminate(List<BallInfo> checkBalls)
    {
        List<BallInfo> elimitBalls = new List<BallInfo>();
        {
            foreach (var checkBall in checkBalls)
            {
                BallInfo curBall = checkBall;

                //if (curBall._BornRound == _Round)
                //    continue;

                var clumnElimit = CheckClumnElimit((int)checkBall.Pos.x, (int)checkBall.Pos.y);
                var rawElimit = CheckRawElimit((int)checkBall.Pos.x, (int)checkBall.Pos.y);
                int elimitNum = 0;
                if (clumnElimit != null)
                {
                    elimitNum += clumnElimit.Count;
                    elimitBalls.AddRange(clumnElimit);
                }
                if (rawElimit != null)
                {
                    elimitNum += rawElimit.Count;
                    elimitBalls.AddRange(rawElimit);
                }
            }
        }

        return elimitBalls;
    }

    public List<BallInfo> CheckNormalEliminate()
    {
        List<BallInfo> allBalls = new List<BallInfo>();
        foreach (var ballLine in _BallBoxInfo)
        {
            foreach (var ballInfo in ballLine)
            {
                allBalls.Add(ballInfo);
            }
        }
        return CheckNormalEliminate(allBalls);
    }

    public List<BallInfo> CheckNormalEliminate(List<BallInfo> checkBalls)
    {
        List<BallInfo> elimitBalls = new List<BallInfo>();

        foreach (var checkBall in checkBalls)
        {
            BallInfo curBall = checkBall;

            if (curBall.BornRound == _Round)
                continue;

            var clumnElimit = CheckClumnElimit((int)checkBall.Pos.x, (int)checkBall.Pos.y);
            var rawElimit = CheckRawElimit((int)checkBall.Pos.x, (int)checkBall.Pos.y);
            int elimitNum = 0;
            int clumnNum = 0;
            int rowNum = 0;

            bool isAddNewBall = false;
            if (clumnElimit != null)
            {
                clumnNum = clumnElimit.Count;
                elimitNum += clumnNum;
                elimitBalls.AddRange(clumnElimit);

                foreach (var elimitBall in clumnElimit)
                {
                    //elimitBall.OnNormalElimit();
                    if (AddEliminateBall(elimitBall))
                    {
                        isAddNewBall = true;
                    }

                    elimitBall._BombPrivite = 1;
                }
            }
            if (rawElimit != null)
            {
                rowNum = rawElimit.Count;
                elimitNum += rowNum;
                elimitBalls.AddRange(rawElimit);

                foreach (var elimitBall in rawElimit)
                {
                    //elimitBall.OnNormalElimit();
                    if (AddEliminateBall(elimitBall))
                    {
                        isAddNewBall = true;
                    }

                    elimitBall._BombPrivite = 1;
                }
            }

            if (elimitNum > _DefaultDisapearCnt)
            {
                var checkBallExtras = AddBallInfos(clumnElimit, rawElimit);
                var exitOptExtra = GetOptExtra(checkBallExtras);
                if (exitOptExtra == null || exitOptExtra._ElimitBalls.Count < elimitNum)
                {
                    if (exitOptExtra != null)
                    {
                        _CurrentOptExtra.Remove(exitOptExtra);
                    }
                    OptExtra optExtra = new OptExtra();
                    optExtra._ExtraType = _OptImpact.GetExtraSpType(clumnNum, rowNum, curBall);
                    optExtra._OptBall = curBall;
                    optExtra._ElimitBalls = AddBallInfos(clumnElimit, rawElimit);
                    //_OptImpact.ElimitExtra(elimitNum, curBall);
                    AddOptExtra(optExtra);
                }
                
            }

        }

        return elimitBalls;
    }

    public List<BallInfo> CheckClumnElimit(int i, int j)
    {
        BallInfo curBall = GetBallInfo(i, j);
        BallInfo nextClumnBall = GetBallInfo(i + 1, j);
        int nextCntRight = 1;
        while (nextClumnBall != null)
        {
            if (curBall.IsCanNormalElimit(nextClumnBall))
            {
                ++nextCntRight;
                nextClumnBall = GetBallInfo(i + nextCntRight, j);
            }
            else
            {
                break;
            }
        }
        nextClumnBall = GetBallInfo(i - 1, j);
        int nextCntLeft = 1;
        while (nextClumnBall != null)
        {
            if (curBall.IsCanNormalElimit(nextClumnBall))
            {
                ++nextCntLeft;
                nextClumnBall = GetBallInfo(i - nextCntLeft, j);
            }
            else
            {
                break;
            }
        }
        if (nextCntRight + nextCntLeft - 1 >= _DefaultDisapearCnt)
        {
            List<BallInfo> elimitClumn = new List<BallInfo>();
            for (int k = 0; k < nextCntRight; ++k)
            {
                var elimitBall = GetBallInfo(i + k, j);
                if (!elimitClumn.Contains(elimitBall))
                {
                    elimitClumn.Add(elimitBall);
                }
            }
            for (int k = 0; k < nextCntLeft; ++k)
            {
                var elimitBall = GetBallInfo(i - k, j);
                if (!elimitClumn.Contains(elimitBall))
                {
                    elimitClumn.Add(elimitBall);
                }
            }
            return elimitClumn;
        }

        return null;
    }

    public List<BallInfo> CheckRawElimit(int i, int j)
    {
        BallInfo curBall = GetBallInfo(i, j);
        BallInfo nextRawBall = GetBallInfo(i, j + 1);
        var nextCntUp = 1;
        while (nextRawBall != null)
        {
            if (curBall.IsCanNormalElimit(nextRawBall))
            {
                ++nextCntUp;
                nextRawBall = GetBallInfo(i, j + nextCntUp);
            }
            else
            {
                break;
            }
        }
        nextRawBall = GetBallInfo(i, j - 1);
        var nextCntDown = 1;
        while (nextRawBall != null)
        {
            if (curBall.IsCanNormalElimit(nextRawBall))
            {
                ++nextCntDown;
                nextRawBall = GetBallInfo(i, j - nextCntDown);
            }
            else
            {
                break;
            }
        }
        if (nextCntUp  + nextCntDown - 1 >= _DefaultDisapearCnt)
        {
            List<BallInfo> elimitRaw = new List<BallInfo>();
            for (int k = 0; k < nextCntUp; ++k)
            {
                var elimitBall = GetBallInfo(i, j + k);
                if (!elimitRaw.Contains(elimitBall))
                {
                    elimitRaw.Add(elimitBall);
                }
            }
            for (int k = 0; k < nextCntDown; ++k)
            {
                var elimitBall = GetBallInfo(i, j - k);
                if (!elimitRaw.Contains(elimitBall))
                {
                    elimitRaw.Add(elimitBall);
                }
            }
            return elimitRaw;
        }

        return null;
    }

    public bool CheckElimitSimple()
    {
        List<BallInfo> allBalls = new List<BallInfo>();
        foreach (var ballLine in _BallBoxInfo)
        {
            foreach (var ballInfo in ballLine)
            {
                int i = (int)ballInfo.Pos.x;
                int j = (int)ballInfo.Pos.y;

                int nextCntUp = 1;
                BallInfo nextRawBall = GetBallInfo(i, j + nextCntUp);
                while (nextRawBall != null)
                {
                    if (ballInfo.IsCanNormalElimit(nextRawBall))
                    {
                        ++nextCntUp;
                        nextRawBall = GetBallInfo(i, j + nextCntUp);
                    }
                    else
                    {
                        break;
                    }
                }
                if (nextCntUp >= _DefaultDisapearCnt)
                {
                    return true;
                }

                nextCntUp = 1;
                nextRawBall = GetBallInfo(i + nextCntUp, j);
                while (nextRawBall != null)
                {
                    if (ballInfo.IsCanNormalElimit(nextRawBall))
                    {
                        ++nextCntUp;
                        nextRawBall = GetBallInfo(i + nextCntUp, j);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        return false;
    }

    public List<BallInfo> CheckSpElimit(List<BallInfo> checkBalls)
    {
        List<BallInfo> spElimit = new List<BallInfo>();

        CheckSpElimitCircle(checkBalls, ref spElimit);

        var reactBalls = BallInfoSPLineReact.GetReactBalls();
        if (reactBalls != null)
        {
            CheckSpElimitCircle(reactBalls, ref spElimit);

            foreach (var subElimit in reactBalls)
            {
                //subElimit.OnSPElimit();
                AddSPEliminateBall(subElimit);
            }
        }

        foreach (var subElimit in spElimit)
        {
            //subElimit.OnSPElimit();
            AddSPEliminateBall(subElimit);
        }

        return spElimit;
    }

    public List<BallInfo> CheckSpMove(List<BallInfo> checkBalls)
    {
        List<BallInfo> spElimit = new List<BallInfo>();

        CheckSpElimitCircle(checkBalls, ref spElimit, true);

        foreach (var subElimit in spElimit)
        {
            //subElimit.OnSPElimit();
            AddSPEliminateBall(subElimit);
        }

        return spElimit;
    }

    public List<BallInfo> TestSpMove(List<BallInfo> checkBalls)
    {
        List<BallInfo> spElimit = new List<BallInfo>();

        CheckSpElimitCircle(checkBalls, ref spElimit, true, true);

        return spElimit;
    }

    public List<BallInfo> TestSpElimit(List<BallInfo> checkBalls)
    {
        List<BallInfo> spElimit = new List<BallInfo>();

        CheckSpElimitCircle(checkBalls, ref spElimit, false, true);

        return spElimit;
    }

    public void CheckSpElimitCircle(List<BallInfo> checkBalls, ref List<BallInfo> spElimitBalls, bool isCheckMove = false, bool testMode = false, int deepth = 0)
    {
        foreach (var elimitBall in checkBalls)
        {

            List<BallInfo> subElimitList = new List<BallInfo>();
            List<BallInfo> spElimits = null;
            if (!isCheckMove)
            {
                spElimits = elimitBall.CheckSPElimit();
            }
            else
            {
                spElimits = elimitBall.CheckSPMove(checkBalls);
            }
            if (spElimits != null && spElimits.Count > 0 && !elimitBall._IsBoomSP)
            {
                if (!testMode)
                {
                    elimitBall._IsBoomSP = true;

                    if (elimitBall._BombPrivite == 0)
                    {
                        elimitBall._BombPrivite = deepth + 1;
                    }
                }
                
                foreach (var subElimit in spElimits)
                {
                    if (!testMode)
                    {
                        if (subElimit.BornRound == _Round)
                            continue;
                    }

                    if (!spElimitBalls.Contains(subElimit))
                    {
                        spElimitBalls.Add(subElimit);
                        subElimitList.Add(subElimit);
                    }

                    if (!testMode)
                    {
                        elimitBall._BombElimitBalls.Add(subElimit);
                        if (subElimit._BombPrivite <= 0 || subElimit._BombPrivite > elimitBall._BombPrivite)
                        {
                            subElimit._BombPrivite = elimitBall._BombPrivite;
                        }
                    }
                }


                CheckSpElimitCircle(subElimitList, ref spElimitBalls, false, testMode, deepth + 1);
            }
        }
    }

    public List<BallInfo> GetExploreBalls(List<BallInfo> elimitBalls)
    {
        List<BallInfo> exploreBalls = new List<BallInfo>();
        foreach (var elimitBall in elimitBalls)
        {
            BallInfo exploreBall = GetBallInfo((int)elimitBall.Pos.x, (int)elimitBall.Pos.y);
            if (!_CurrentNormalEliminateBalls.Contains(exploreBall) && !_CurrentSPEliminateBalls.Contains(exploreBall))
            {
                if (exploreBall != null && exploreBall.IsExplore())
                {
                    exploreBalls.Add(exploreBall);
                }
            }

            exploreBall = GetBallInfo((int)elimitBall.Pos.x + 1, (int)elimitBall.Pos.y);
            if (!_CurrentNormalEliminateBalls.Contains(exploreBall) && !_CurrentSPEliminateBalls.Contains(exploreBall))
            {
                if (exploreBall != null && exploreBall.IsExplore())
                {
                    exploreBalls.Add(exploreBall);
                }
            }

            exploreBall = GetBallInfo((int)elimitBall.Pos.x - 1, (int)elimitBall.Pos.y);
            if (!_CurrentNormalEliminateBalls.Contains(exploreBall) && !_CurrentSPEliminateBalls.Contains(exploreBall))
            {
                if (exploreBall != null && exploreBall.IsExplore())
                {
                    exploreBalls.Add(exploreBall);
                }
            }

            exploreBall = GetBallInfo((int)elimitBall.Pos.x, (int)elimitBall.Pos.y + 1);
            if (!_CurrentNormalEliminateBalls.Contains(exploreBall) && !_CurrentSPEliminateBalls.Contains(exploreBall))
            {
                if (exploreBall != null && exploreBall.IsExplore())
                {
                    exploreBalls.Add(exploreBall);
                }
            }

            exploreBall = GetBallInfo((int)elimitBall.Pos.x, (int)elimitBall.Pos.y - 1);
            if (!_CurrentNormalEliminateBalls.Contains(exploreBall) && !_CurrentSPEliminateBalls.Contains(exploreBall))
            {
                if (exploreBall != null && exploreBall.IsExplore())
                {
                    exploreBalls.Add(exploreBall);
                }
            }
        }

        foreach(var exploreBall in exploreBalls)
        {
            exploreBall.OnExplore();
        }

        return exploreBalls;
    }

    #endregion

    #region 

    private int _BonrBallCnt = 0;
    private List<int> _LastBornHPIdx = new List<int>();
    //public BallInfo FindFallBall(int x, int y)
    //{
    //    for (int j = y; j < _BallBoxInfo[0].Length; ++j)
    //    {
    //        BallInfo nextBall = GetBallInfo(x, y + 1);
    //        if (nextBall != null && nextBall.IsNormalBall())
    //        {
    //            return nextBall;
    //        }
    //    }
    //    return null;
    //}

    private Dictionary<int, int> _FillPath = new Dictionary<int, int>();
    private List<BallInfo> _NoPassBall = new List<BallInfo>();

    public void ClearFillPath()
    {
        _FillPath.Clear();
        _NoPassBall.Clear();
    }

    public bool IsBallCanPath(BallInfo ballInfo)
    {
        if (ballInfo == null)
            return false;

        if (!ballInfo.IsCanPass())
        {
            return false;
        }

        return true;
    }

    public BallInfo FindFallBall(int x, int y, ref List<BallInfo> pathBalls)
    {
        var curBall = GetBallInfo(x, y);
        if (y >= _BoxHeight-1)
        {
            var fillBall = FindFillPath(x, y);
            pathBalls.Add(fillBall);
            return fillBall;
        }

        BallInfo pathBall = null;
        for (int i = 0; i < _DefaultPassWidth; ++i)
        {
            var nextBall = GetBallInfo(x + i, y + 1);
            if (nextBall!= null && nextBall.IsCanFall())
            {
                pathBalls.Add(nextBall);
                return nextBall;
            }
            else if (nextBall != null && IsBallCanPath(nextBall))
            {
                pathBall = FindFallBall(x + i, y + 1, ref pathBalls);
                if (pathBall != null)
                {
                    pathBalls.Add(nextBall);
                    return pathBall;
                }
            }

            if (i != 0)
            {
                var nextBall2 = GetBallInfo(x - i, y + 1);
                if (nextBall2 != null &&nextBall2.IsCanFall())
                {
                    pathBalls.Add(nextBall2);
                    return nextBall2;
                }
                else if (nextBall2 != null && IsBallCanPath(nextBall2))
                {
                    pathBall = FindFallBall(x - i, y + 1, ref pathBalls);
                    if (pathBall != null)
                    {
                        pathBalls.Add(nextBall2);
                        return pathBall;
                    }
                }
            }
        }

        return null;
    }

    public BallInfo FindFillPath(int x, int y)
    {
        if (!_FillPath.ContainsKey(x))
        {
            _FillPath.Add(x, _BoxHeight - 1);
        }
        ++_FillPath[x];

        var randomFall = new BallInfo(x, _FillPath[x]);
        SetBornBall(randomFall);
        return randomFall;
    }

    private void SetBornBall(BallInfo ballInfo)
    {
        ++_BonrBallCnt;

        int HPBallIdx = 25;
        if (_MapRecord._HPBall.Count > 0)
        {
            if (_LastBornHPIdx.Count < _MapRecord._HPBall.Count)
            {
                HPBallIdx = _MapRecord._HPBall[_LastBornHPIdx.Count];
            }
            else
            {
                HPBallIdx = _MapRecord._HPBall[_MapRecord._HPBall.Count - 1];
            }
        }
        int lastIdx = 0;
        if (_LastBornHPIdx.Count > 0)
        {
            lastIdx = _LastBornHPIdx[_LastBornHPIdx.Count - 1];
        }
        if (_BonrBallCnt - lastIdx == HPBallIdx)
        {
            string hpBallInitStr = "301";
            ballInfo.SetBallInitType(hpBallInitStr);
            _LastBornHPIdx.Add(_BonrBallCnt);
        }
        else
        {
            ballInfo.SetRandomBall();
        }
    }

    public List<ExchangeBalls> ElimitnateFall()
    {
        ClearFillPath();
        List<ExchangeBalls> exchangeList = new List<ExchangeBalls>();
        for (int j = 0; j < _BallBoxInfo[0].Length; ++j)
        {
            for (int i = 0; i < _BallBoxInfo.Length; ++i)
            {
                BallInfo curBall = GetBallInfo(i, j);
                if (curBall.IsCanFillNormal())
                {
                    List<BallInfo> pathBalls = new List<BallInfo>();
                    var fallBall = FindFallBall((int)curBall.Pos.x, (int)curBall.Pos.y, ref pathBalls);
                    if (fallBall == null)
                        continue;

                    pathBalls.Add(curBall);
                    ExchangeBalls(curBall, fallBall);
                    exchangeList.Add(new ExchangeBalls(fallBall, curBall, pathBalls));
                    //Debug.Log("curPos:" + curBall.Pos + ", tarPos:" + fallBall.Pos + ",type:" + curBall.BallType + ",path:" + pathBalls.Count);
                }
            }
        }

        return exchangeList;
    }

    public List<ExchangeBalls> FillEmpty()
    {
        ClearFillPath();
        List<ExchangeBalls> exchangeList = new List<ExchangeBalls>();
        for (int i = 0; i < _BallBoxInfo.Length; ++i)
        {
            for (int j = 0; j < _BallBoxInfo[0].Length; ++j)
            {

                BallInfo curBall = GetBallInfo(i, j);
                if (curBall.IsCanFillNormal())
                {
                    var ballInfo = FindFillPath(i, j);
                    ballInfo.SetRandomBall();
                    ExchangeBalls(curBall, ballInfo);
                    exchangeList.Add(new ExchangeBalls(ballInfo, curBall));
                }
            }
        }
        return exchangeList;
    }

    #endregion

    #region static

    public static List<BallInfo> AddBallInfos(List<BallInfo> listA, BallInfo singleBall)
    {
        if (listA == null)
        {
            listA = new List<BallInfo>();
        }

        if (!listA.Contains(singleBall))
        {
            listA.Add(singleBall);
        }
        
        return listA;
    }

    public static List<BallInfo> AddBallInfos(List<BallInfo> listA, List<BallInfo> listB)
    {
        if (listA == null)
        {
            listA = new List<BallInfo>();
        }
        if (listB != null)
        {
            foreach (var ballInfo in listB)
            {
                if (!listA.Contains(ballInfo))
                {
                    listA.Add(ballInfo);
                }
            }
        }

        return listA;
    }

    #endregion
}
