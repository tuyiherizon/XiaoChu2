using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;
using System;

public class SkillBase
{
    #region static

    public static SkillBase GetSkillInstance(SkillBaseRecord script)
    {
        var impactType = Type.GetType(script.Script);
        if (impactType == null)
            return null;

        var impactBase = Activator.CreateInstance(impactType) as SkillBase;
        if (impactBase == null)
            return null;

        return impactBase;
    }

    #endregion

    protected MotionBase _MotionBase;
    protected SkillBaseRecord _SkillRecord;
    public int _SkillCD;
    protected int _LastSkillRound;

    public int LastCDTime
    {
        get
        {
            int lastCD = _SkillCD - (BattleField.Instance._BattleRound - _LastSkillRound) + 1;
            lastCD = Mathf.Clamp(lastCD, 0, _SkillCD);
            return lastCD;
        }
    }

    public virtual void InitSkill(MotionBase motionBase, SkillBaseRecord skillBase)
    {
        _MotionBase = motionBase;
        _SkillRecord = skillBase;
        _LastSkillRound = BattleField.Instance._BattleRound - 1;
        _SkillCD = _SkillRecord.PreCD;
    }

    public virtual bool IsCanUseSkill()
    {
        if (BattleField.Instance._BattleRound - _LastSkillRound <= _SkillCD)
            return false;

        return true;
    }

    

    public virtual void UseSkill(MotionBase targetMotion, ref DamageResult damageResult)
    {
        _LastSkillRound = BattleField.Instance._BattleRound;
        _SkillCD = _SkillRecord.CD;
    }

}
