using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInfoSPLineRowTrap : BallInfoSPLineBase
{

    protected override List<BallInfo> GetBombBalls()
    {
        List<BallInfo> bombBalls = new List<BallInfo>();

        //for (int i = 0; i <= BallBox.Instance.BoxWidth; ++i)
        //{
        //    var bombBall = BallBox.Instance.GetBallInfo((int)i, (int)_BallInfo.Pos.y);
        //    if (bombBall != null && bombBall.IsCanBeSPElimit(_BallInfo))
        //    {
        //        bombBalls.Add(bombBall);
        //    }
        //}

        for (int i = (int)_BallInfo.Pos.x; i >= 0; --i)
        {
            var bombBall = BallBox.Instance.GetBallInfo((int)i, (int)_BallInfo.Pos.y);
            if (bombBall == null)
                continue;
            //if (bombBall.BallSPType == BallType.Clod
            //    || bombBall.BallSPType == BallType.Ice
            //    || bombBall.BallSPType == BallType.Stone)
            //    break;
            if (bombBall != null && bombBall.IsCanBeSPElimit(_BallInfo))
            {
                bombBalls.Add(bombBall);
            }
            else
            {
                int tet = 1 + 1;
            }
        }

        for (int i = (int)_BallInfo.Pos.x + 1; i < BallBox.Instance.BoxWidth; ++i)
        {
            var bombBall = BallBox.Instance.GetBallInfo((int)i, (int)_BallInfo.Pos.y);
            if (bombBall == null)
                continue;
            //if (bombBall.BallSPType == BallType.Clod
            //    || bombBall.BallSPType == BallType.Ice
            //    || bombBall.BallSPType == BallType.Stone)
            //    break;
            if (bombBall != null && bombBall.IsCanBeSPElimit(_BallInfo))
            {
                bombBalls.Add(bombBall);
            }
            else
            {
                int tet = 1 + 1;
            }
        }
        //bombBalls.Add(_BallInfo);

        return bombBalls;
    }
}
