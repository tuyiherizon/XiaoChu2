using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInfoSPLineReact : BallInfoSPLineBase
{

    protected static List<BallInfoSPLineReact> _ReactLineBalls = new List<BallInfoSPLineReact>();
    public static List<BallInfo> GetReactBalls()
    {
        if (_ReactLineBalls.Count > 0)
        {
            List<BallInfo> ballList = new List<BallInfo>();
            foreach (var reactBall in _ReactLineBalls)
            {
                if(reactBall.BallInfo._BallInfoSP == reactBall)
                    ballList.Add(reactBall.BallInfo);
            }
            return ballList;
        }
        return null;
    }

    public static void RemoveReactBall(BallInfoSPLineReact reactGO)
    {
        if (_ReactLineBalls.Contains(reactGO))
        {
            _ReactLineBalls.Remove(reactGO);
        }
    }

    public static void InitReactBalls()
    {
        _ReactLineBalls.Clear();
    }

    public bool _IsReactBall = false;

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
        if (_IsReactBall)
            return false;
        return true;
    }

    public override bool IsCanBeSPElimit(BallInfo other)
    {
        return true;
    }

    public override bool IsContentNormal()
    {
        return true;
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

    public override void OnNormalElimit()
    {
        if (!_IsReactBall)
        {
            _IsReactBall = true;
            _ReactLineBalls.Add(this);
            return;
        }

        _IsReactBall = false;
        _ReactLineBalls.Remove(this);
        base.OnNormalElimit();
    }

    public override void OnSPElimit()
    {
        _BallInfo.OnNormalElimit();
    }

    public override List<BallInfo> CheckSPElimit()
    {
        return GetBombBalls();
    }

    public override List<BallInfo> CheckSPMove(List<BallInfo> checkBalls)
    {
        //return GetBombBalls();
        return null;
    }

    public override void SetParam(string[] param)
    {
        if (param.Length > 1)
        {
            _BallInfo.SetBallType((BallType)int.Parse(param[1]));
        }

        _IsReactBall = false;

        if (_ReactLineBalls.Contains(this))
        {
            _ReactLineBalls.Remove(this);
        }
    }
    

}
