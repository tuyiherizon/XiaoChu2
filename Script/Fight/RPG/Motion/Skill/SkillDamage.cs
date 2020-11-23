using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;
using System;

public class SkillDamage : SkillBase
{
    

    public override  void UseSkill(MotionBase targetMotion, ref DamageResult damageResult)
    {
        base.UseSkill(targetMotion, ref damageResult);

        int damage = _MotionBase._Attack;
        targetMotion.CastDamage(_MotionBase);
        damageResult.DamageValue = damage;
    }

}
