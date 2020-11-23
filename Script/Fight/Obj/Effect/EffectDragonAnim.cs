using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectDragonAnim : EffectController
{
    public DragonBones.UnityArmatureComponent _EffectArmature;
    public string _EffectAnimName;

    public override void PlayEffect()
    {
        base.PlayEffect();
        _EffectArmature.animation.Play(_EffectAnimName, _EffectArmature.PlayTimes);

    }

}
