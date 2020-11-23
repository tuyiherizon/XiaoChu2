using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallType
{
    None = 0,

    NormalBallStart = 1,
    Color1,
    Color2,
    Color3,
    Color4,
    Color5,
    ColorEmpty,
    NormalBallEnd,

    SPTrapStart = 10,
    Ice,
    Clod,
    Stone,
    Posion,
    Iron,
    SPTrapEnd,

    SPBombStart = 100,
    BombSmall1,
    BombBig1,
    BombSmallEnlarge1,
    BombBigEnlarge,
    BombSmallHitTrap,
    BombBigHitTrap,
    BombSmallReact,
    BombBigReact,
    BombSmallLighting,
    BombBigLighting,
    BombSmallAuto,
    BombBigAuto,
    SPBombEnd,

    SPLineStart = 200,
    LineRow,
    LineClumn,
    LineCross,
    LineRowEnlarge,
    LineClumnEnlarge,
    LineCrossEnlarge,
    LineRowHitTrap,
    LineClumnHitTrap,
    LineCrossHitTrap,
    LineRowReact,
    LineClumnReact,
    LineCrossReact,
    LineRowLighting,
    LineClumnLighting,
    LineCrossLighting,
    LineRowAuto,
    LineClumnAuto,
    LineCrossAuto,
    SPLineEnd,

    SPRPGStart = 300,
    RPGHP = 301,
    SPRPGEnd,

}

public class BallInfo
{
    private BallType _BallType;
    public BallType BallType
    {
        get
        {
            return _BallType;
        }
        set
        {
            _BallType = value;
        }
    }

    private BallType _BallSPType;
    public BallType BallSPType
    {
        get
        {
            return _BallSPType;
        }
        set
        {
            _BallSPType = value;
        }
    }
    public BallInfoSPBase _BallInfoSP = null;

    private BallType _IncludeBallSPType;
    public BallType IncludeBallSPType
    {
        get
        {
            return _IncludeBallSPType;
        }
        set
        {
            _IncludeBallSPType = value;
        }
    }
    public BallInfoSPBase _IncludeBallInfoSP = null;

    private Vector2 _Pos;
    public Vector2 Pos
    {
        get
        {
            return _Pos;
        }
    }

    public BallInfo(int posX, int posY)
    {
        _Pos = new Vector2(posX, posY);
        _BallType = BallType.None;
        _BallSPType = BallType.None;
        _BallInfoSP = null;
    }

    public void Clear()
    {
        _BallType = BallType.None;
        _ElimitFlag = 0;
        ClearSP();
    }

    public void ClearSP()
    {
        if (IsBombBall())
        {
            ++BallBox.Instance._ElimitBombCnt;
        }

        if (_BallInfoSP is BallInfoSPLineReact)
        {
            BallInfoSPLineReact.RemoveReactBall(_BallInfoSP as BallInfoSPLineReact);
        }

        if (_BallInfoSP is BallInfoSPBombAuto)
        {
            BallInfoSPBombAuto.AutoBombList.Remove(_BallInfoSP as BallInfoSPBombAuto);
        }

        

        _BallSPType = BallType.None;
        _BallInfoSP = null;
    }

    #region normal

    public bool IsContainNormal()
    {
        return _BallType > BallType.NormalBallStart && _BallType < BallType.NormalBallEnd;
    }

    public bool IsEmpty()
    {
        return _BallInfoSP == null && _BallType == BallType.None;
    }

    public bool IsNormalBall()
    {
        return _BallType > BallType.NormalBallStart && _BallType < BallType.NormalBallEnd && _BallInfoSP == null;
    }

    public bool IsTrapBall()
    {
        return _BallSPType > BallType.SPTrapStart && _BallSPType < BallType.SPTrapEnd && _BallInfoSP != null;
    }

    public bool IsBombBall()
    {
        return _BallSPType > BallType.SPBombStart && _BallSPType < BallType.SPLineEnd && _BallInfoSP != null;
    }

    public bool IsSPBomb()
    {
        return _BallSPType > BallType.SPBombStart && _BallSPType < BallType.SPBombEnd && _BallInfoSP != null;
    }

    public bool IsSPLine()
    {
        return _BallSPType > BallType.SPLineStart && _BallSPType < BallType.SPLineEnd && _BallInfoSP != null;
    }

    public bool IsRPGBall()
    {
        return _BallSPType > BallType.SPRPGStart && _BallSPType < BallType.SPRPGEnd && _BallInfoSP != null;
    }

    public bool IsEquipNormal(BallInfo other)
    {
        if (other == null)
            return false;
        if (_BallType == BallType.None)
            return false;
        return _BallType == other.BallType;
    }

    public bool IsCanFillNormal()
    {
        if (_BallInfoSP != null && !_BallInfoSP.IsContentNormal())
        {
            return false;
        }

        if (_IncludeBallInfoSP != null && !_IncludeBallInfoSP.IsContentNormal())
        {
            return false;
        }

        return _BallType == BallType.None;
    }

    public void SetBallType(BallType ballType)
    {
        _BallType = ballType;
    }

    public void SetBallInitType(string ballSPType)
    {
        var spArgs = ballSPType.Split(',');
        if (spArgs.Length < 1)
            return;

        int spType = int.Parse(spArgs[0]);
        if (!BallBox.IsTypeSP(spType))
        {
            _BallType = (BallType)spType;
            SetBallType(_BallType);
        }
        else
        {

            BallType initType = (BallType)spType;
            BallInfoSPBase initBall = GetBallInfoSP(initType, spArgs);

            if (_BallInfoSP != null)
            {
                if (_BallInfoSP.IsCanBeContentSP() && initBall.IsCanContentSP())
                {
                    _IncludeBallSPType = _BallSPType;
                    _IncludeBallInfoSP = _BallInfoSP;
                }
            }

            _BallInfoSP = initBall;
            _BallSPType = initType;
            if (!_BallInfoSP.IsContentNormal())
            {
                SetBallType(BallType.None);
            }
        }
    }

    public void SetInterSP(string interInfo)
    {
        var spArgs = interInfo.Trim('(', ')').Split(';');

        int spType = int.Parse(spArgs[0]);
        if (!BallBox.IsTypeSP(spType))
        {
            _BallType = (BallType)spType;
            SetBallType(_BallType);
        }
        else
        {

            BallType initType = (BallType)spType;
            BallInfoSPBase initBall = GetBallInfoSP(initType, spArgs);

            _IncludeBallInfoSP = initBall;
            _IncludeBallSPType = initType;
        }
    }

    private BallInfoSPBase GetBallInfoSP(BallType ballType, string[] spArgs)
    {
        BallInfoSPBase initBall = null;
        switch (ballType)
        {
            case BallType.Ice:
                initBall = new BallInfoSPTrapIce();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.Clod:
                initBall = new BallInfoSPTrapClod();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.Stone:
                initBall = new BallInfoSPTrapStone();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.Posion:
                initBall = new BallInfoSPTrapPosion();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.Iron:
                initBall = new BallInfoSPTrapIron();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.BombSmall1:
                initBall = new BallInfoSPBombSmall();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.BombBig1:
                initBall = new BallInfoSPBombBig();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.BombSmallEnlarge1:
                initBall = new BallInfoSPBombSmallEnlarge();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.BombBigEnlarge:
                initBall = new BallInfoSPBombBigEnlarge();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.BombSmallHitTrap:
                initBall = new BallInfoSPBombSmallTrap();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.BombBigHitTrap:
                initBall = new BallInfoSPBombBigTrap();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.BombSmallReact:
                initBall = new BallInfoSPBombSmallReact();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.BombBigReact:
                initBall = new BallInfoSPBombBigReact();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.BombSmallLighting:
                initBall = new BallInfoSPBombSmallLighting();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.BombBigLighting:
                initBall = new BallInfoSPBombBigLighting();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.BombSmallAuto:
                initBall = new BallInfoSPBombSmallAuto();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.BombBigAuto:
                initBall = new BallInfoSPBombBigAuto();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.LineRow:
                initBall = new BallInfoSPLineRow();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.LineClumn:
                initBall = new BallInfoSPLineClumn();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.LineCross:
                initBall = new BallInfoSPLineCross();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.LineRowEnlarge:
                initBall = new BallInfoSPLineRowEnlarge();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.LineClumnEnlarge:
                initBall = new BallInfoSPLineClumnEnlarge();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.LineCrossEnlarge:
                initBall = new BallInfoSPLineCrossEnlarge();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.LineRowHitTrap:
                initBall = new BallInfoSPLineRowTrap();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.LineClumnHitTrap:
                initBall = new BallInfoSPLineClumnTrap();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.LineCrossHitTrap:
                initBall = new BallInfoSPLineCrossTrap();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.LineRowReact:
                initBall = new BallInfoSPLineRowReact();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.LineClumnReact:
                initBall = new BallInfoSPLineClumnReact();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.LineCrossReact:
                initBall = new BallInfoSPLineCrossReact();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.LineRowLighting:
                initBall = new BallInfoSPLineRowLighting();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.LineClumnLighting:
                initBall = new BallInfoSPLineClumnLighting();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.LineCrossLighting:
                initBall = new BallInfoSPLineCrossLighting();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.LineRowAuto:
                initBall = new BallInfoSPLineRowAuto();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.LineClumnAuto:
                initBall = new BallInfoSPLineClumnAuto();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.LineCrossAuto:
                initBall = new BallInfoSPLineCrossAuto();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
            case BallType.RPGHP:
                initBall = new BallInfoSPRPGHP();
                initBall.SetBallInfo(this);
                initBall.SetParam(spArgs);
                break;
        }

        return initBall;
    }

    public void SetRandomBall()
    {
        int ballTypeEnd = (int)BallType.NormalBallEnd;
        if (!BallBox.Instance._IsContainEmptyNormal)
        {
            ballTypeEnd = (int)BallType.ColorEmpty;
        }
        int randomBall = Random.Range((int)(BallType.NormalBallStart) + 1, ballTypeEnd);
        _BallType = (BallType)randomBall;
    }

    public static BallType GetRandomBallType(List<BallType> exBalls)
    {
        List<int> randomTypes = new List<int>();

        int ballTypeEnd = (int)BallType.NormalBallEnd;
        if (!BallBox.Instance._IsContainEmptyNormal)
        {
            ballTypeEnd = (int)BallType.ColorEmpty;
        }
        for (int i = (int)(BallType.NormalBallStart) + 1; i < ballTypeEnd; ++i)
        {
            if (exBalls.Count == 0)
            {
                randomTypes.Add(i);
            }
            else
            {
                for (int j = 0; j < exBalls.Count; ++j)
                {
                    if ((int)exBalls[j] == i)
                    {
                        break;
                    }

                    if (j == exBalls.Count - 1)
                    {
                        randomTypes.Add(i);
                    }
                }
            }
        }

        int randomIdx = Random.Range(0, randomTypes.Count);
        var ballType = (BallType)randomTypes[randomIdx];
        return ballType;
    }

    public void SetRandomBall(List<BallType> exBalls)
    {
        SetBallInitType("101,5");
        return;

        if (_BallInfoSP != null && !_BallInfoSP.IsContentNormal())
        {
            _BallType = BallType.None;
            return;
        }

        if (exBalls == null || exBalls.Count == 0)
        {
            SetRandomBall();
            return;
        }

        List<int> randomTypes = new List<int>();

        int ballTypeEnd = (int)BallType.NormalBallEnd;
        if (!BallBox.Instance._IsContainEmptyNormal)
        {
            ballTypeEnd = (int)BallType.ColorEmpty;
        }
        for (int i = (int)(BallType.NormalBallStart) + 1; i < ballTypeEnd; ++i)
        {
            for (int j = 0; j < exBalls.Count; ++j)
            {
                if ((int)exBalls[j] == i)
                {
                    break;
                }

                if (j == exBalls.Count - 1)
                {
                    randomTypes.Add(i);
                }
            }
        }

        int randomIdx = Random.Range(0, randomTypes.Count);
        _BallType = (BallType)randomTypes[randomIdx];
    }

    #endregion

    #region opt

    private int _BornRound = 0;
    public int BornRound
    {
        get
        {
            return _BornRound;
        }
        set
        {
            _BornRound = value;
        }
    }

    public bool IsCanExChange(BallInfo other)
    {
        if (other._BallInfoSP != null && _BallInfoSP != null)
        {
            return _BallInfoSP.IsCanExchange(other) & other._BallInfoSP.IsCanExchange(this); ;
        }
        else if (_BallInfoSP != null)
        {
            return _BallInfoSP.IsCanExchange(other);
        }
        else if (other._BallInfoSP != null)
        {
            return other._BallInfoSP.IsCanExchange(this);
        }
        else if (_BallType == BallType.None && other.BallType == BallType.None)
        {
            return false;
        }
        return true;
    }

    public bool ExChangeBall(BallInfo other)
    {
        if (!IsCanExChange(other))
            return false;

        var temp = other.BallType;
        other.BallType = _BallType;
        _BallType = temp;

        var tempRound = other.BornRound;
        other.BornRound = BornRound;
        BornRound = tempRound;

        if (_BallInfoSP != null && _BallInfoSP.IsExchangeSpInfo(other))
        {
            var tempSP = other._BallInfoSP;
            other._BallInfoSP = _BallInfoSP;
            _BallInfoSP = tempSP;
            

            var tempSpType = other.BallSPType;
            other.BallSPType = BallSPType;
            BallSPType = tempSpType;

            if (_BallInfoSP != null)
            {
                _BallInfoSP.SetBallInfo(this);
            }
            other._BallInfoSP.SetBallInfo(other);
        }
        else if (other._BallInfoSP != null && other._BallInfoSP.IsExchangeSpInfo(this))
        {
            var tempSP = other._BallInfoSP;
            other._BallInfoSP = _BallInfoSP;
            _BallInfoSP = tempSP;
            _BallInfoSP.SetBallInfo(other);
            
            var tempSpType = other.BallSPType;
            other.BallSPType = BallSPType;
            BallSPType = tempSpType;

            _BallInfoSP.SetBallInfo(this);
            if (other._BallInfoSP != null)
            {
                other._BallInfoSP.SetBallInfo(other);
            }
        }

        return true;
    }

    public void OnRoundEnd()
    {
        if (_BallInfoSP != null)
        {
            _BallInfoSP.OnRoundEnd();
        }
    }
    #endregion

    #region elimit

    public int _ElimitFlag = 0;


    public bool IsCanNormalElimit(BallInfo other)
    {
        if (other == null)
            return false;

        if (_BallInfoSP != null && other._BallInfoSP != null)
        {
            if (!_BallInfoSP.IsCanNormalElimit() || !other._BallInfoSP.IsCanNormalElimit())
            {
                return false;
            }
        }
        else if (_BallInfoSP != null)
        {
            if (!_BallInfoSP.IsCanNormalElimit())
                return false;
        }
        else if (other._BallInfoSP != null)
        {
            if (!other._BallInfoSP.IsCanNormalElimit())
                return false;
        }

        return IsEquipNormal(other);
    }

    public bool IsCanNormalElimit()
    {
        if (_BallInfoSP != null)
        {
            return _BallInfoSP.IsCanNormalElimit();
        }

        return true;
    }

    public bool IsCanBeSPElimit(BallInfo other)
    {
        if (_BallInfoSP != null)
        {
            return _BallInfoSP.IsCanBeSPElimit(other);
        }

        return true;
    }

    public void OnNormalElimit()
    {
        if (_BallInfoSP != null)
        {
            _BallInfoSP.OnNormalElimit();
            return;
        }
        Clear();
    }

    public void OnSPElimit()
    {
        if (_BallInfoSP != null)
        {
            _BallInfoSP.OnSPElimit();

            return;
        }

        OnNormalElimit();
    }

    public bool IsCanMove()
    {
        if (_BallInfoSP != null)
        {
            return _BallInfoSP.IsCanMove();
        }

        return true;
    }

    public bool IsCanPass()
    {
        if (_BallInfoSP != null)
        {
            return _BallInfoSP.IsCanPass();
        }

        return true;
    }

    public bool IsCanFall( )
    {
        if (_BallInfoSP != null)
        {
            return _BallInfoSP.IsCanFall();
        }

        return  IsNormalBall();
    }

    public bool IsExplore()
    {
        if (_BallInfoSP != null)
        {
            return _BallInfoSP.IsExplore();
        }

        return false;
    }

    public bool IsRemain()
    {
        if (_BallInfoSP != null)
        {
            return _BallInfoSP.IsRemain();
        }

        return false;
    }

    public void OnExplore()
    {
        if (_BallInfoSP != null)
        {
            _BallInfoSP.OnExplore();
        }
        
    }

    public List<BallInfo> CheckSPElimit()
    {
        if (_BallInfoSP != null)
        {
            return _BallInfoSP.CheckSPElimit();
        }

        return null;
    }

    public List<BallInfo> CheckSPMove(List<BallInfo> checkBalls)
    {
        if (_BallInfoSP != null)
        {
            return _BallInfoSP.CheckSPMove(checkBalls);
        }

        return null;
    }

    public void SpRemove()
    {
        if (_IncludeBallInfoSP != null)
        {
            if (_IncludeBallSPType > BallType.SPTrapStart && _IncludeBallSPType < BallType.SPTrapEnd)
            {
                ++BallBox.Instance._ElimitTrapCnt;
            }

            _BallInfoSP = _IncludeBallInfoSP;
            _BallSPType = _IncludeBallSPType;

            if (_IncludeBallInfoSP is BallInfoSPTrapPosion)
            {
                BallInfoSPTrapPosion._TrapPosionList.Remove(_IncludeBallInfoSP as BallInfoSPTrapPosion);
            }

            _IncludeBallInfoSP = null;
            _IncludeBallSPType = BallType.None;

            

            return;
        }

        if (_BallSPType > BallType.SPTrapStart && _BallSPType < BallType.SPTrapEnd)
        {
            ++BallBox.Instance._ElimitTrapCnt;
        }

        if (_BallInfoSP is BallInfoSPTrapPosion)
        {
            BallInfoSPTrapPosion._TrapPosionList.Remove(_BallInfoSP as BallInfoSPTrapPosion);
        }

        _BallInfoSP = null;
        _BallSPType = BallType.None;

        
    }
    #endregion

    #region elimit info

    public bool _IsBoomSP = false;
    public List<BallInfo> _BombElimitBalls = new List<BallInfo>();
    public int _BombPrivite = 0;

    public void ClearElimitInfo()
    {
        _BombElimitBalls.Clear();
        _BombPrivite = 0;
        _IsBoomSP = false;

        _ElimitMap = null;
        _ElimitKnot = null;
    }

    #endregion

    #region elimitInfo 2

    public class ElimitMap
    {
        private List<BallInfo> _Gate = new List<BallInfo>();
        public List<BallInfo> Gate
        {
            get
            {
                return _Gate;
            }
        }

        private List<BallInfo> _PassWay = new List<BallInfo>();
        public List<BallInfo> PassWay
        {
            get
            {
                return _PassWay;
            }
        }

        private List<BallInfo> _CrossWay = new List<BallInfo>();
        public List<BallInfo> CrossWay
        {
            get
            {
                return _CrossWay;
            }
        }

        private List<Knot> _Konts = new List<Knot>();
        public List<Knot> Konts
        {
            get
            {
                return _Konts;
            }
        }

        private int _SlotCount = -1;
        public int SlotCount
        {
            get
            {
                if (_SlotCount == -1)
                {
                    int count = 0;
                    count += _Gate.Count;
                    count += _PassWay.Count;
                    foreach (var kont in _Konts)
                    {
                        count += kont._KnotBalls.Count;
                    }
                    _SlotCount = count;
                }
                return _SlotCount;
            }
        }

        public void AddGate(BallInfo ballInfo)
        {
            if (_Gate.Contains(ballInfo))
                return;

            _Gate.Add(ballInfo);
            _SlotCount = -1;
        }

        public void AddPassWay(BallInfo ballInfo)
        {
            if (_PassWay.Contains(ballInfo))
                return;

            _PassWay.Add(ballInfo);
            _SlotCount = -1;
        }

        public void AddCrossWay(BallInfo ballInfo)
        {
            if (_CrossWay.Contains(ballInfo))
                return;

            _CrossWay.Add(ballInfo);
            _SlotCount = -1;
        }

        public void AddKnot(BallInfo curBall, BallInfo tarBall1, BallInfo tarBall2)
        {
            if (curBall._ElimitKnot == null)
            {
                curBall._ElimitKnot = new Knot();
                _Konts.Add(curBall._ElimitKnot);
            }

            if (!curBall._ElimitKnot._KnotBalls.Contains(curBall))
            {
                curBall._ElimitKnot._KnotBalls.Add(curBall);
                _SlotCount = -1;
            }

            if (!curBall._ElimitKnot._KnotBalls.Contains(tarBall1))
            {
                tarBall1._ElimitKnot = curBall._ElimitKnot;
                curBall._ElimitKnot._KnotBalls.Add(tarBall1);
                _SlotCount = -1;
            }

            if (!curBall._ElimitKnot._KnotBalls.Contains(tarBall2))
            {
                tarBall2._ElimitKnot = curBall._ElimitKnot;
                curBall._ElimitKnot._KnotBalls.Add(tarBall2);
                _SlotCount = -1;
            }
        }

        public List<BallInfo> FindPowerPath()
        {
            List<BallInfo> maxPathBalls = new List<BallInfo>();
            if (CrossWay.Count > 0)
            {
                var crossPathes = FindCrossWayPathes(CrossWay[0], null);
                var maxPath = FindMaxPath(crossPathes);
                crossPathes.Remove(maxPath);
                maxPath.Reverse();
                maxPathBalls.AddRange(maxPath);

                while(crossPathes.Count > 0)
                {
                    var nextMaxPath = FindMaxPath(crossPathes);
                    if (!IsListConflict(maxPathBalls, nextMaxPath))
                    {
                        maxPathBalls.AddRange(nextMaxPath);
                        break;
                    }
                    else
                    {
                        crossPathes.Remove(nextMaxPath);
                    }
                }
                
            }
            else if (Konts.Count > 0)
            {

            }
            else if (Gate.Count > 0)
            {
                var nextBall = FindNextWay(Gate[0], null);
                maxPathBalls.Add(Gate[0]);
                while (nextBall != null)
                {
                    nextBall = FindNextPath(nextBall, Gate[0], maxPathBalls);
                }
            }
            else if (PassWay.Count > 0)
            {
                var nextBall = FindNextWay(PassWay[0], null);
                maxPathBalls.Add(PassWay[0]);
                while (nextBall != null)
                {
                    nextBall = FindNextPath(nextBall, PassWay[0], maxPathBalls);
                }
            }


            return maxPathBalls;
        }

        public bool IsListConflict(List<BallInfo> ballsA, List<BallInfo> ballsB)
        {
            foreach (var ball in ballsA)
            {
                if (ballsB.Contains(ball))
                    return true;
            }

            return false;
        }

        private List<List<BallInfo>> FindCrossWayPathes(BallInfo crossWay, BallInfo fromWay)
        {
            List<List<BallInfo>> passes = new List<List<BallInfo>>();
            if (crossWay._ElimitKnot == null)
            {
                foreach (var passWay in crossWay._Passes)
                {
                    if (passWay == fromWay)
                        continue;

                    List<BallInfo> pass = new List<BallInfo>();
                    pass.Add(crossWay);
                    FindNextPath(passWay, crossWay, pass);
                    passes.Add(pass);
                }
            }
            else
            {
                foreach (var passWay in crossWay._ElimitKnot._Gates.Values)
                {
                    if (passWay == fromWay)
                        continue;

                    List<BallInfo> pass = new List<BallInfo>();
                    pass.Add(crossWay);
                    FindNextPath(passWay, crossWay, pass);
                    passes.Add(pass);
                }
            }

            return passes;
        }

        private BallInfo FindNextPath(BallInfo curBall, BallInfo fromBall, List<BallInfo> curWay)
        {
            if (curWay.Contains(curBall))
                return null;
            if (curBall._Passes.Count > 2)
            {
                curWay.Add(curBall);
                var nextPathes = FindCrossWayPathes(curBall, fromBall);
                var maxNextPath = FindMaxPath(nextPathes);
                curWay.AddRange(maxNextPath);

                return null;
            }
            else if (curBall._Passes.Count > 1)
            {
                var nextBall = FindNextWay(curBall, fromBall);
                curWay.Add(curBall);
                while (nextBall != null)
                {
                    nextBall = FindNextPath(nextBall, curBall, curWay);
                }
                return nextBall;
            }
            else
            {
                curWay.Add(curBall);
                return null;
            }
        }

        private List<BallInfo> FindMaxPath(List<List<BallInfo>> pathes)
        {
            int maxPathIdx = 0;
            int maxPathCnt = 0;
            for (int i = 0; i < pathes.Count; ++i)
            {
                if (pathes[i].Count > maxPathCnt)
                {
                    maxPathCnt = pathes[i].Count;
                    maxPathIdx = i;
                }
                
            }

            return pathes[maxPathIdx];
        }

        private BallInfo FindNextWay(BallInfo pathWay, BallInfo fromWay)
        {
            foreach (var pathBall in pathWay._Passes)
            {
                if (pathBall != fromWay)
                    return pathBall;
            }

            return null;
        }
    }

    public class Knot
    {
        public List<BallInfo> _KnotBalls = new List<BallInfo>();
        public Dictionary<BallInfo, BallInfo> _Gates = new Dictionary<BallInfo, BallInfo>();
    }

    public ElimitMap _ElimitMap;
    public Knot _ElimitKnot;
    public List<BallInfo> _Passes = new List<BallInfo>();

    public void SetMap(ElimitMap elimitMap)
    {
        if (elimitMap == null)
        {
            _ElimitMap = new ElimitMap();
        }
        else
        {
            _ElimitMap = elimitMap;
        }
    }

    #endregion
}
