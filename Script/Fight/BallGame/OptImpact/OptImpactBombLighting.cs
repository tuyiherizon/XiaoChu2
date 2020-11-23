using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptImpactBombLighting : OptImpactBase
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
            return (int)BallType.BombSmallLighting + "," + (int)optBall.BallType;
        }
        else 
        {
            return (int)BallType.BombBigLighting + "," + (int)optBall.BallType;
        }
       
        return "";
    }
}
