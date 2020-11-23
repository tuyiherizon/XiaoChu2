using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class MonsterModelChange : MonsterModel
{

    public UnityArmatureComponent _DragonArmatureAtk;
    public UnityArmatureComponent _DragonArmatureIdle;
    public UnityArmatureComponent _DragonArmatureMove;

    public override float PlayAttack()
    {
        _DragonArmatureAtk.animation.timeScale = 2;

        _DragonArmatureIdle.gameObject.SetActive(false);
        _DragonArmatureMove.gameObject.SetActive(false);
        _DragonArmatureAtk.gameObject.SetActive(true);
        var animState = _DragonArmatureAtk.animation.Play(_AtkAnim, 1);
        StartCoroutine(PlayAttackAfter(animState));
        return animState.totalTime;
    }

    private IEnumerator PlayAttackAfter(DragonBones.AnimationState animState)
    {
        yield return new WaitForSeconds(animState.totalTime);

        _DragonArmatureAtk.animation.timeScale = 1;
        PlayIdle();
    }

    public override void PlayIdle()
    {
        _DragonArmatureAtk.gameObject.SetActive(false);
        _DragonArmatureMove.gameObject.SetActive(false);
        _DragonArmatureIdle.gameObject.SetActive(true);
        _DragonArmatureIdle.animation.Play(_IdleAnim, 0);
    }
}
