using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInfoSPLineRowAuto : BallInfoSPLineBase
{
    public override bool IsCanNormalElimit()
    {
        return false;
    }

    public override bool IsContentNormal()
    {
        return false;
    }

    public override List<BallInfo> CheckSPMove(List<BallInfo> checkBalls)
    {
        return GetBombBalls();
    }

    protected override List<BallInfo> GetBombBalls()
    {
        List<BallInfo> bombBalls = new List<BallInfo>();

        for (int i = (int)_BallInfo.Pos.x; i >= 0; --i)
        {
            var bombBall = BallBox.Instance.GetBallInfo((int)i, (int)_BallInfo.Pos.y);
            if (bombBall == null)
                continue;
            if (IsPosBlock(bombBall))
                break;
            if (bombBall != null && bombBall.IsCanBeSPElimit(_BallInfo))
            {
                bombBalls.Add(bombBall);
            }
        }

        for (int i = (int)_BallInfo.Pos.x + 1; i < BallBox.Instance.BoxWidth; ++i)
        {
            var bombBall = BallBox.Instance.GetBallInfo((int)i, (int)_BallInfo.Pos.y);
            if (bombBall == null)
                continue;
            if (IsPosBlock(bombBall))
                break;
            if (bombBall != null && bombBall.IsCanBeSPElimit(_BallInfo))
            {
                bombBalls.Add(bombBall);
            }
        }
        return bombBalls;
    }
}
