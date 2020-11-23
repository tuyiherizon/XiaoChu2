﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInfoSPBombBigReact : BallInfoSPBase
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
        return GetBombBalls();
    }

    public override List<BallInfo> CheckSPMove(List<BallInfo> checkBalls)
    {
        return GetBombBalls();
    }

    public override void SetParam(string[] param)
    {

    }

    private bool _IsRemain = false;
    public override bool IsRemain()
    {
        if (_IsRemain)
            return true;
        return false;
    }

    private List<BallInfo> GetBombBalls()
    {
        List<BallInfo> bombBalls = new List<BallInfo>();

        int n = 2;
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
        _IsRemain = false;

        if (!_IsReactBall)
        {
            int rate = Random.Range(0, 10000);
            if (rate < 1500)
            {
                bombBalls.Remove(_BallInfo);
                _IsReactBall = true;
                _IsRemain = true;
            }
        }

        return bombBalls;
    }

    public bool _IsReactBall = false;
}
