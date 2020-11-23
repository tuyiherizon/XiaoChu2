using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptImpactBombTrap : OptImpactBase
{

    public override void Init()
    {
        _DamageToTrap = 2;
    }

    public override void SetElimitExtra(string spType, BallInfo optBall)
    {
        optBall.SetBallInitType(spType);
        optBall.BornRound = BallBox.Instance._Round;
    }

    public override string GetExtraSpType(int rawCnt, int clumnCnt, BallInfo optBall)
    {
        if (rawCnt + clumnCnt == 4)
        {
            return (int)BallType.BombSmallHitTrap + "," + (int)optBall.BallType;
        }
        else 
        {
            return (int)BallType.BombBigHitTrap + "," + (int)optBall.BallType;
        }
       
        return "";
    }
}
