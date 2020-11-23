using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInfoSPBase
{
    protected BallInfo _BallInfo;

    public BallInfo BallInfo
    {
        get
        {
            return _BallInfo;
        }
    }

    public virtual void SetBallInfo(BallInfo ballInfo)
    {
        _BallInfo = ballInfo;
    }

    public virtual bool IsCanExchange(BallInfo other)
    {
        return true;
    }

    public virtual bool IsExchangeSpInfo(BallInfo other)
    {
        return true;
    }

    public virtual bool IsCanNormalElimit()
    {
        return true;
    }

    public virtual bool IsCanBeSPElimit(BallInfo other)
    {
        return true;
    }

    public virtual bool IsContentNormal()
    {
        return true;
    }

    public virtual bool IsCanFall()
    {
        return true;
    }

    public virtual bool IsCanPass()
    {
        return true;
    }

    public virtual bool IsCanMove()
    {
        return true;
    }

    public virtual bool IsExplore()
    {
        return false;
    }

    public virtual bool IsRemain()
    {
        return false;
    }

    public virtual bool IsCanContentSP()
    {
        return false;
    }

    public virtual bool IsCanBeContentSP()
    {
        return true;
    }

    public virtual void OnExplore()
    {
        
    }

    public virtual void SetParam(string[] param)
    {

    }

    public virtual void OnExchange(BallInfo other)
    {

    }

    public virtual void OnNormalElimit()
    {
        _BallInfo.Clear();
    }

    public virtual void OnSPElimit()
    {

    }

    public virtual void OnRoundEnd()
    {

    }

    public virtual List<BallInfo> CheckSPElimit()
    {
        return null;
    }

    public virtual List<BallInfo> CheckSPMove(List<BallInfo> checkBalls)
    {
        return null;
    }
}
