using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInfoSPRPGBase : BallInfoSPBase
{

    public override bool IsCanExchange(BallInfo other)
    {
        return true;
    }

    public override bool IsExchangeSpInfo(BallInfo other)
    {
        return true;
    }

    public override bool IsCanNormalElimit()
    {
        return true;
    }

    public override bool IsCanBeSPElimit(BallInfo other)
    {
        return true;
    }

    public override bool IsContentNormal()
    {
        return true;
    }

    public override bool IsCanFall()
    {
        return true;
    }

    public override bool IsCanMove()
    {
        return true;
    }

    public override bool IsCanPass()
    {
        return true;
    }

    public override bool IsExplore()
    {
        return false;
    }

    public override bool IsCanBeContentSP()
    {
        return true;
    }

    public override void OnSPElimit()
    {
        _BallInfo.OnNormalElimit();
    }

    public override List<BallInfo> CheckSPElimit()
    {
        return GetBombBalls();
    }

    public override List<BallInfo> CheckSPMove(List<BallInfo> checkBalls)
    {
        //return GetBombBalls();
        return null;
    }

    public override void SetParam(string[] param)
    {
        if (param.Length > 1)
        {
            _BallInfo.SetBallType((BallType)int.Parse(param[1]));
        }
    }

    protected bool IsPosBlock(BallInfo ballInfo)
    {
        if (ballInfo == null)
            return false;

        if (ballInfo.BallSPType == BallType.Clod
            || ballInfo.BallSPType == BallType.Ice
            || ballInfo.BallSPType == BallType.Iron
            || ballInfo.BallSPType == BallType.Stone)
            return true;

        return false;
    }

    protected virtual List<BallInfo> GetBombBalls()
    {
        return null;
    }
}
