using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptImpactLineReact : OptImpactBase
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
            return (int)BallType.LineCrossReact + "," + (int)optBall.BallType;
        }
        else if (rawCnt == 4)
        {
            return (int)BallType.LineRowReact + "," + (int)optBall.BallType;
        }
        else if (clumnCnt == 4)
        {
            return (int)BallType.LineClumnReact + "," + (int)optBall.BallType;
        }

        return "";
    }
}
