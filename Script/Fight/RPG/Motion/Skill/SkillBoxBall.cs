using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;
using System;

public class SkillBoxBall : SkillBase
{
    public override void InitSkill(MotionBase motionBase, SkillBaseRecord skillBase)
    {
        base.InitSkill(motionBase, skillBase);

        _SameRate = GameDataValue.ConfigIntToFloat(skillBase.Param[0]);
        _BombRate = GameDataValue.ConfigIntToFloat(skillBase.Param[1]);
        _TrapType = (BallType)skillBase.Param[2];
        _FindPosNum.Clear();
        _FindPosNum.Add(skillBase.Param[3]);
        _FindPosNum.Add(skillBase.Param[4]);
        _FindPosNum.Add(skillBase.Param[5]);
        _FindPosNum.Add(skillBase.Param[6]);
    }

    public override  void UseSkill(MotionBase targetMotion, ref DamageResult damageResult)
    {
        base.UseSkill(targetMotion, ref damageResult);

        int damage = _MotionBase._Attack;
        //targetMotion.CastDamage(_MotionBase);
        //damageResult.DamageValue = damage;
        //RandomTrapInfo();
        damageResult._SkillBallsResult = SetPosTrap();
    }

    #region 

    public enum FIND_POS_TYPE
    {
        ALL,
        TOP,
        BUTTOM,
    }

    public enum FIND_POS_DIFF
    {
        NORMAL = 0,
        SAME = 50,
        BOMB = 100,
    }

    public static int _SkillMinNormalCnt = 25;

    protected float _SameRate;
    protected float _BombRate;
    protected List<int> _FindPosNum = new List<int>();
    protected BallType _TrapType;
    

    protected List<BallInfo> SetPosTrap()
    {
        List<BallInfo> posBalls = new List<BallInfo>();

        List<BallInfo> normalBalls = new List<BallInfo>();
        List<BallInfo> sameTraps = new List<BallInfo>();
        List<BallInfo> bombs = new List<BallInfo>();
        List<BallInfo> sameNormal = new List<BallInfo>();

        List<BallInfo> lastElimit = BallBox.Instance.FindLastEliminate();

        int heightStart = 0;
        int heightEnd = BallBox.Instance.BoxHeight;
        
        for (int i = 0; i < BallBox.Instance.BoxWidth; ++i)
        {
            for (int j = heightStart; j < heightEnd; ++j)
            {
                var posBall = BallBox.Instance.GetBallInfo(i, j);

                if (posBall.IsTrapBall())
                {
                    if (posBall.BallSPType == _TrapType && !lastElimit.Contains(posBall))
                    {
                        sameTraps.Add(posBall);
                    }
                }
                else if ((posBall.IsBombBall() || posBall.IsRPGBall()) && !lastElimit.Contains(posBall))
                {
                    bombs.Add(posBall);
                }
                else
                {
                    var preBall1 = BallBox.Instance.GetBallInfo(i - 1, j);
                    var preBall2 = BallBox.Instance.GetBallInfo(i, j - 1);
                    if (preBall1 != null && preBall1.BallType == posBall.BallType && !lastElimit.Contains(posBall))
                    {
                        sameNormal.Add(posBall);
                    }
                    else if (preBall2 != null && preBall2.BallType == posBall.BallType && !lastElimit.Contains(posBall))
                    {
                        sameNormal.Add(posBall);
                    }
                    else if(!lastElimit.Contains(posBall))
                    {
                        normalBalls.Add(posBall);
                    }
                }
            }
        }

        if (normalBalls.Count + sameNormal.Count < _SkillMinNormalCnt)
            return null;

        float bombRate = _BombRate;
        float sameRate = _SameRate;

        foreach(var trapParam in _FindPosNum)
        {
            if (trapParam <= 0)
                continue;

            BallInfo posBall = null;
            if (bombRate > 0)
            {
                float ballRate = UnityEngine.Random.Range(0, 1.0f);
                if (ballRate < bombRate && bombs.Count > 0) 
                {
                    int randomIdx = UnityEngine.Random.Range(0, bombs.Count);
                    posBall = (bombs[randomIdx]);
                    bombs.RemoveAt(randomIdx);
                }
                else if (ballRate < bombRate && sameNormal.Count > 0)
                {
                    int randomIdx = UnityEngine.Random.Range(0, sameNormal.Count);
                    posBall = (sameNormal[randomIdx]);
                    sameNormal.RemoveAt(randomIdx);
                }
                else
                {
                    int randomIdx = UnityEngine.Random.Range(0, normalBalls.Count);
                    posBall = (normalBalls[randomIdx]);
                    normalBalls.RemoveAt(randomIdx);
                }
            }
            else if (sameRate > 0)
            {
                float ballRate = UnityEngine.Random.Range(0, 1.0f);
                if (ballRate < sameRate && sameNormal.Count > 0)
                {
                    int randomIdx = UnityEngine.Random.Range(0, sameNormal.Count);
                    posBall = (sameNormal[randomIdx]);
                    sameNormal.RemoveAt(randomIdx);
                }
                else
                {
                    int randomIdx = UnityEngine.Random.Range(0, normalBalls.Count);
                    posBall = (normalBalls[randomIdx]);
                    normalBalls.RemoveAt(randomIdx);
                }
            }
            else
            {
                int randomIdx = UnityEngine.Random.Range(0, normalBalls.Count);
                posBall = (normalBalls[randomIdx]);
                normalBalls.RemoveAt(randomIdx);
            }

            string trapType = (int)_TrapType + "," + trapParam;
            posBall.SetBallInitType(trapType);
            posBalls.Add(posBall);
        }

        //UIFightBox.ShowMonsterBalls(posBalls);
        return posBalls;
    }

    #endregion

}
