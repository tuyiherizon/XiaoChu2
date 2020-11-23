using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptImpactLineEnlarge : OptImpactBase
{

    public override void SetElimitExtra(string spType, BallInfo optBall)
    {
        optBall.SetBallInitType(spType);
        optBall.BornRound = BallBox.Instance._Round;
    }

    public override string GetExtraSpType(int rawCnt, int clumnCnt, BallInfo optBall)
    {
        if (rawCnt + clumnCnt > 4)
        {
            return (int)BallType.LineCross + "," + (int)optBall.BallType;
        }
        else if (rawCnt == 4)
        {
            return (int)BallType.LineRowEnlarge + "," + (int)optBall.BallType;
        }
        else if (clumnCnt == 4)
        {
            return (int)BallType.LineClumnEnlarge + "," + (int)optBall.BallType;
        }

        return "";
    }
}
