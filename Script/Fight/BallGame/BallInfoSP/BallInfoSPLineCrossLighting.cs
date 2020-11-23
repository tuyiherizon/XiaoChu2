using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInfoSPLineCrossLighting : BallInfoSPLineBase
{


    protected override List<BallInfo> GetBombBalls()
    {
        List<BallInfo> bombBalls = new List<BallInfo>();

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
        BallInfo boundBall = null;
        if (bombBalls.Count != 0)
        {
            boundBall = bombBalls[bombBalls.Count - 1];
            for (int i = -1; i <= 1; ++i)
            {
                var bombBall = BallBox.Instance.GetBallInfo((int)boundBall.Pos.x + i, (int)boundBall.Pos.y);
                if (bombBall == null)
                    continue;
                if (IsPosBlock(bombBall))
                    continue;
                if (bombBall != null && bombBall.IsCanBeSPElimit(_BallInfo))
                {
                    bombBalls.Add(bombBall);
                }
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
        if (bombBalls.Count != 0)
        {
            boundBall = bombBalls[bombBalls.Count - 1];
            for (int i = -1; i <= 1; ++i)
            {
                var bombBall = BallBox.Instance.GetBallInfo((int)boundBall.Pos.x + i, (int)boundBall.Pos.y);
                if (bombBall == null)
                    continue;
                if (IsPosBlock(bombBall))
                    continue;
                if (bombBall != null && bombBall.IsCanBeSPElimit(_BallInfo))
                {
                    bombBalls.Add(bombBall);
                }
            }
        }

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
        if (bombBalls.Count != 0)
        {
            boundBall = bombBalls[bombBalls.Count - 1];
            for (int i = -1; i <= 1; ++i)
            {
                var bombBall = BallBox.Instance.GetBallInfo((int)boundBall.Pos.x, (int)boundBall.Pos.y + i);
                if (bombBall == null)
                    continue;
                if (IsPosBlock(bombBall))
                    continue;
                if (bombBall != null && bombBall.IsCanBeSPElimit(_BallInfo))
                {
                    bombBalls.Add(bombBall);
                }
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
        if (bombBalls.Count != 0)
        {
            boundBall = bombBalls[bombBalls.Count - 1];
            for (int i = -1; i <= 1; ++i)
            {
                var bombBall = BallBox.Instance.GetBallInfo((int)boundBall.Pos.x, (int)boundBall.Pos.y + i);
                if (bombBall == null)
                    continue;
                if (IsPosBlock(bombBall))
                    continue;
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
