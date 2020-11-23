using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInfoSPTrapIce : BallInfoSPTrapBase
{

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
        return true;
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

    public override bool IsCanContentSP()
    {
        return true;
    }

    public override void OnExplore()
    {
        ElimitNum -= 1;
        if (ElimitNum <= 0)
        {
            _BallInfo.SpRemove();
        }
    }

    public override void OnSPElimit()
    {
        ElimitNum -= BallBox.Instance._OptImpact._DamageToTrap;
        if (ElimitNum <= 0)
        {
            _BallInfo.SpRemove();
        }
    }

    public override void SetParam(string[] param)
    {
        if (param.Length > 1)
        {
            ElimitNum = int.Parse(param[1]);
        }
        if (param.Length > 2)
        {
            BallInfo.SetInterSP(param[2]);
        }
    }
}
