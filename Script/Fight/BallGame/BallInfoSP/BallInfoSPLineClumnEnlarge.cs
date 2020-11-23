using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInfoSPLineClumnEnlarge : BallInfoSPLineBase
{


    protected override List<BallInfo> GetBombBalls()
    {
        List<BallInfo> bombBalls = new List<BallInfo>();

        //for (int i = 0; i <= BallBox.Instance.BoxHeight; ++i)
        //{
        //    var bombBall = BallBox.Instance.GetBallInfo((int)_BallInfo.Pos.x, i);
        //    if (bombBall != null && bombBall.IsCanBeSPElimit(_BallInfo))
        //    {
        //        bombBalls.Add(bombBall);
        //    }
        //}

        for (int i = (int)_BallInfo.Pos.y; i >= 0; --i)
        {
            var bombBall = BallBox.Instance.GetBallInfo((int)_BallInfo.Pos.x, i);
            if (bombBall == null)
                continue;
            if (IsPosBlock(bombBall))
                break;
            if (bombBall != null && bombBall.IsCanBeSPElimit(_BallInfo))
            {
                bombBalls.Add(bombBall);
            }
        }

        for (int i = (int)_BallInfo.Pos.y + 1; i < BallBox.Instance.BoxHeight; ++i)
        {
            var bombBall = BallBox.Instance.GetBallInfo((int)_BallInfo.Pos.x, i);
            if (bombBall == null)
                continue;
            if (IsPosBlock(bombBall))
                break;
            if (bombBall != null && bombBall.IsCanBeSPElimit(_BallInfo))
            {
                bombBalls.Add(bombBall);
            }
        }

        var exbombBall = BallBox.Instance.GetBallInfo((int)_BallInfo.Pos.x + 1, (int)_BallInfo.Pos.y + 1);
        if (!IsPosBlock(exbombBall))
        {
            if (exbombBall != null && exbombBall.IsCanBeSPElimit(_BallInfo))
            {
                bombBalls.Add(exbombBall);
            }
        }

        exbombBall = BallBox.Instance.GetBallInfo((int)_BallInfo.Pos.x - 1, (int)_BallInfo.Pos.y + 1);
        if (!IsPosBlock(exbombBall))
        {
            if (exbombBall != null && exbombBall.IsCanBeSPElimit(_BallInfo))
            {
                bombBalls.Add(exbombBall);
            }
        }
        //bombBalls.Add(_BallInfo);

        return bombBalls;
    }
}
