using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInfoSPBombBigLighting : BallInfoSPBase
{
    public override bool IsCanExchange(BallInfo other)
    {
        return true;
    }

    public override bool IsExchangeSpInfo(BallInfo other)
    {
        return true;
    }

    public override bool IsCanNormalElimit()
    {
        return false;
    }

    public override bool IsCanBeSPElimit(BallInfo other)
    {
        return true;
    }

    public override bool IsContentNormal()
    {
        return false;
    }

    public override bool IsCanFall()
    {
        return true;
    }

    public override bool IsCanMove()
    {
        return true;
    }

    public override bool IsCanPass()
    {
        return true;
    }

    public override bool IsExplore()
    {
        return false;
    }

    public override void OnSPElimit()
    {
        _BallInfo.Clear();
    }

    public override List<BallInfo> CheckSPElimit()
    {
        //return GetBombBalls();
        return null;
    }

    public override List<BallInfo> CheckSPMove(List<BallInfo> checkBalls)
    {
        return GetBombBalls(checkBalls);
    }

    public override void SetParam(string[] param)
    {

    }

    private List<BallInfo> GetBombBalls(List<BallInfo> moveBalls)
    {
        var moveBall = moveBalls[0];
        if (moveBalls[1] == _BallInfo)
            return null;

        List<BallInfo> bombBalls = new List<BallInfo>();

        if (moveBall._BallInfoSP is BallInfoSPBombSmallLighting)
        {
            List<BallInfo> bombPreRandom = new List<BallInfo>();
            int n = 4;
            for (int i = -n; i <= n; ++i)
            {
                for (int j = -n; j <= n; ++j)
                {
                    var bombBall = BallBox.Instance.GetBallInfo((int)_BallInfo.Pos.x + i, (int)_BallInfo.Pos.y + j);
                    if (bombBall != null && bombBall.IsNormalBall() && bombBall.IsCanBeSPElimit(_BallInfo))
                    {
                        bombPreRandom.Add(bombBall);
                    }
                }
            }

            if (bombPreRandom.Count <= 12)
            {
                bombPreRandom.Add(_BallInfo);
                return bombPreRandom;
            }
            else
            {
                var randomIdxs = GameRandom.GetIndependentRandoms(0, bombPreRandom.Count, 12);
                foreach (var idx in randomIdxs)
                {
                    bombBalls.Add(bombPreRandom[idx]);
                }
            }

            bombBalls.Add(moveBalls[1]);
            bombBalls.Add(_BallInfo);
            return bombBalls;
        }
        else if (moveBall._BallInfoSP is BallInfoSPBombBigLighting)
        {
            List<BallInfo> bombPreRandom = new List<BallInfo>();
            int n = 4;
            for (int i = -n; i <= n; ++i)
            {
                for (int j = -n; j <= n; ++j)
                {
                    var bombBall = BallBox.Instance.GetBallInfo((int)_BallInfo.Pos.x + i, (int)_BallInfo.Pos.y + j);
                    if (bombBall != null && bombBall.IsNormalBall() && bombBall.IsCanBeSPElimit(_BallInfo))
                    {
                        bombPreRandom.Add(bombBall);
                    }
                }
            }

            if (bombPreRandom.Count <= 15)
            {
                bombPreRandom.Add(_BallInfo);
                return bombPreRandom;
            }
            else
            {
                var randomIdxs = GameRandom.GetIndependentRandoms(0, bombPreRandom.Count, 15);
                foreach (var idx in randomIdxs)
                {
                    bombBalls.Add(bombPreRandom[idx]);
                }
            }

            bombBalls.Add(moveBalls[1]);
            bombBalls.Add(_BallInfo);
            return bombBalls;
        }
        else
        {
            List<BallInfo> bombPreRandom = new List<BallInfo>();
            int n = 3;
            for (int i = -n; i <= n; ++i)
            {
                for (int j = -n; j <= n; ++j)
                {
                    var bombBall = BallBox.Instance.GetBallInfo((int)_BallInfo.Pos.x + i, (int)_BallInfo.Pos.y + j);
                    if (bombBall != null && bombBall.IsNormalBall() && bombBall.IsCanBeSPElimit(_BallInfo))
                    {
                        bombPreRandom.Add(bombBall);
                    }
                }
            }

            if (bombPreRandom.Count <= 10)
            {
                bombPreRandom.Add(_BallInfo);
                return bombPreRandom;
            }
            else
            {
                var randomIdxs = GameRandom.GetIndependentRandoms(0, bombPreRandom.Count, 10);
                foreach (var idx in randomIdxs)
                {
                    bombBalls.Add(bombPreRandom[idx]);
                }
            }

            bombBalls.Add(_BallInfo);
            return bombBalls;
        }
    }
}
