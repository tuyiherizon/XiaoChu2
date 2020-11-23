using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptImpactBombReact : OptImpactBase
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
            return (int)BallType.BombSmallReact + "," + (int)optBall.BallType;
        }
        else if(rawCnt + clumnCnt > 4)
        {
            return (int)BallType.BombBigReact + "," + (int)optBall.BallType;
        }
       
        return "";
    }
}
