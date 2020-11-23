using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class MonsterModel : MonoBehaviour
{

    public UnityArmatureComponent _DragonArmature;
    public string _AtkAnim;
    public string _IdleAnim;
    public string _MoveAnim;

    public virtual float PlayAttack()
    {
        _DragonArmature.animation.timeScale = 2;
        var animState = _DragonArmature.animation.Play(_AtkAnim, 1);
        StartCoroutine(PlayAttackAfter(animState));
        return animState.totalTime;
    }

    private IEnumerator PlayAttackAfter(DragonBones.AnimationState animState)
    {
        yield return new WaitForSeconds(animState.totalTime);

        _DragonArmature.animation.timeScale = 1;
        PlayIdle();
    }

    public virtual void PlayIdle()
    {
        _DragonArmature.animation.Play(_IdleAnim, 0);
    }
}
