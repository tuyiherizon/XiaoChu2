using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptImpactBase
{
    public int _DamageToTrap = 1;

    public virtual void Init()
    {
        _DamageToTrap = 1;
    }

    public virtual void SetElimitExtra(string spType, BallInfo optBall)
    {
        optBall.SetBallInitType(spType);
        optBall.BornRound = BallBox.Instance._Round;
    }

    public virtual string GetExtraSpType(int rawCnt, int clumnCnt, BallInfo optBall)
    {
        return "";
    }
}
