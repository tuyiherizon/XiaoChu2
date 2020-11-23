using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInfoSPLineRowReact : BallInfoSPLineReact
{

    protected override List<BallInfo> GetBombBalls()
    {
        List<BallInfo> bombBalls = new List<BallInfo>();

        if (_IsReactBall)
        {
            for (int i = -1; i < 2; ++i)
            {
                var bombBall = BallBox.Instance.GetBallInfo((int)_BallInfo.Pos.x + i, (int)_BallInfo.Pos.y);
                if (bombBall == null)
                    continue;
                if (IsPosBlock(bombBall))
                    break;
                if (bombBall != null && bombBall.IsCanBeSPElimit(_BallInfo))
                {
                    bombBalls.Add(bombBall);
                }
            }
        }
        else
        {
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
        }
        //bombBalls.Add(_BallInfo);

        return bombBalls;
    }
}
