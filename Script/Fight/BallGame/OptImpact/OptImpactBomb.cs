using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptImpactBomb: OptImpactBase
{

    public override void SetElimitExtra(string spType, BallInfo optBall)
    {
        optBall.SetBallInitType(spType);
        optBall.BornRound = BallBox.Instance._Round;
    }

    public override string GetExtraSpType(int rawCnt, int clumnCnt, BallInfo optBall)
    {
        if (rawCnt + clumnCnt == 4)
        {
            return (int)BallType.BombSmall1 + "," + (int)optBall.BallType;
        }
        else 
        {
            return (int)BallType.BombBig1 + "," + (int)optBall.BallType;
        }
       
        return "";
    }
}
