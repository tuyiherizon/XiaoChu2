using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInfoSPBombSmallReact : BallInfoSPBase
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
        if (!_IsReactBall)
        {
            int rate = Random.Range(0, 10000);
            if (rate < 5000)
            {
                _IsReactBall = true;
                return;
            }
        }
        
        _IsReactBall = false;
        _BallInfo.Clear();
    }

    public override List<BallInfo> CheckSPElimit()
    {
        return GetBombBalls();
    }

    public override List<BallInfo> CheckSPMove(List<BallInfo> checkBalls)
    {
        return GetBombBalls();
    }

    public override void SetParam(string[] param)
    {
        _IsReactBall = false;
    }


    private List<BallInfo> GetBombBalls()
    {
        List<BallInfo> bombBalls = new List<BallInfo>();
        
        int n = 1;
        for (int i = -n; i <= n; ++i)
        {
            int ny = n - Mathf.Abs(i);
            for (int j = -ny; j <= ny; ++j)
            {
                var bombBall = BallBox.Instance.GetBallInfo((int)_BallInfo.Pos.x + i, (int)_BallInfo.Pos.y + j);
                if (bombBall != null && bombBall.IsCanBeSPElimit(_BallInfo))
                {
                    bombBalls.Add(bombBall);
                }
            }
        }
        

        return bombBalls;
    }

    public bool _IsReactBall = false;
}
