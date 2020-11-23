using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInfoSPTrapPosion : BallInfoSPTrapBase
{
    public static List<BallInfoSPTrapPosion> _TrapPosionList = new List<BallInfoSPTrapPosion>();
    static bool _IsRoundHitPosion = false;
    

    public override bool IsCanExchange(BallInfo other)
    {
        return false;
    }

    public override bool IsExchangeSpInfo(BallInfo other)
    {
        return true;
    }

    public override bool IsCanNormalElimit()
    {
        return false;
    }

    public override bool IsContentNormal()
    {
        return false;
    }

    public override bool IsCanFall()
    {
        return false;
    }

    public override bool IsCanMove()
    {
        return false;
    }

    public override bool IsCanPass()
    {
        return false;
    }

    public override bool IsExplore()
    {
        
        return true;
    }
    
    public override void OnExplore()
    {
        ElimitNum -= 1;
        if (ElimitNum <= 0)
        {
            _BallInfo.SpRemove();
            _TrapPosionList.Remove(this);
        }
        _IsRoundHitPosion = true;
    }

    public override void OnSPElimit()
    {
        ElimitNum -= BallBox.Instance._OptImpact._DamageToTrap;
        if (ElimitNum <= 0)
        {
            _BallInfo.SpRemove();
            _TrapPosionList.Remove(this);
        }
        _IsRoundHitPosion = true;
    }

    public override void SetParam(string[] param)
    {
        if (param.Length > 1)
        {
            ElimitNum = int.Parse(param[1]);
        }

        if (!_TrapPosionList.Contains(this))
        {
            _TrapPosionList.Add(this);
        }
    }

    public static void InitPosion()
    {
        _TrapPosionList.Clear();
    }

    public static List<BallInfo> OnPosionRoundEnd()
    {
        if (_IsRoundHitPosion)
        {
            _IsRoundHitPosion = false;
            return null;
        }

        if (_TrapPosionList.Count == 0)
            return null;

        BallInfoSPTrapPosion enhanceTrap = null;
        foreach (var posionTrap in _TrapPosionList)
        {
            if (posionTrap.ElimitNum < 2)
            {
                enhanceTrap = posionTrap;
            }
        }

        if (enhanceTrap != null)
        {
            ++enhanceTrap.ElimitNum;
            return new List<BallInfo>() { enhanceTrap.BallInfo };
        }
        else
        {
            var canPosBall = GetCanPosionPos();
            int random = Random.Range(0, canPosBall.Count);

            string ballType = (int)BallType.Posion + "," + 1;
            canPosBall[random].SetBallInitType(ballType);
            _IsRoundHitPosion = false;

            return new List<BallInfo>() { canPosBall[random] };
        }
    }

    public static List<BallInfo> GetCanPosionPos()
    {
        List<BallInfo> canPos = new List<BallInfo>();
        foreach (var trap in _TrapPosionList)
        {
            int x = (int)trap._BallInfo.Pos.x;
            int y = (int)trap._BallInfo.Pos.y;
            var posBall = BallBox.Instance.GetBallInfo(x + 1, y);
            if (posBall != null && (posBall.IsNormalBall() || posBall.IsEmpty()))
            {
                canPos.Add(posBall);
            }

            posBall = BallBox.Instance.GetBallInfo(x - 1, y);
            if (posBall != null && (posBall.IsNormalBall() || posBall.IsEmpty()))
            {
                canPos.Add(posBall);
            }

            posBall = BallBox.Instance.GetBallInfo(x, y+1);
            if (posBall != null && (posBall.IsNormalBall() || posBall.IsEmpty()))
            {
                canPos.Add(posBall);
            }

            posBall = BallBox.Instance.GetBallInfo(x, y-1);
            if (posBall != null && (posBall.IsNormalBall() || posBall.IsEmpty()))
            {
                canPos.Add(posBall);
            }
        }
        return canPos;
    }
}
