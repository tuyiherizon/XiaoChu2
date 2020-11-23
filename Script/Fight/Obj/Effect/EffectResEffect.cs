using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectResEffect : EffectController
{
    public ResEffect _ResEffect;

    public override void PlayEffect()
    {
        base.PlayEffect();
        _ResEffect.PlayAnim();
        StartCoroutine(AutoStop());
    }

    public IEnumerator AutoStop()
    {
        yield return new WaitForSeconds(_EffectLastTime);

        HideEffect();
    }    

}
