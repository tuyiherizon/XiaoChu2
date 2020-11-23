using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptImpactBombEnlarge : OptImpactBase
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
            return (int)BallType.BombSmallEnlarge1 + "," + (int)optBall.BallType;
        }
        else if(rawCnt + clumnCnt > 4)
        {
            return (int)BallType.BombBigEnlarge + "," + (int)optBall.BallType;
        }
       
        return "";
    }
}
