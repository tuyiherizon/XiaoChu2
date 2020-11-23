using DragonBones;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEffectBomb : EffectController
{
    
    public UnityArmatureComponent _BombBigAnim;
    public UnityArmatureComponent _BombSmallAnim;

    public void StartEffect(BallType ballType)
    {
        _BombBigAnim.gameObject.SetActive(false);
        _BombSmallAnim.gameObject.SetActive(false);

        switch (ballType)
        {
            case BallType.BombBig1:
            case BallType.BombBigAuto:
            case BallType.BombBigEnlarge:
            case BallType.BombBigHitTrap:
            case BallType.BombBigLighting:
            case BallType.BombBigReact:
                _BombBigAnim.gameObject.SetActive(true);
                _BombBigAnim.animation.Play("sgzd_3");
                break;
            case BallType.BombSmall1:
            case BallType.BombSmallAuto:
            case BallType.BombSmallEnlarge1:
            case BallType.BombSmallHitTrap:
            case BallType.BombSmallLighting:
            case BallType.BombSmallReact:
                _BombSmallAnim.gameObject.SetActive(true);
                _BombSmallAnim.animation.Play("disappear");
                break;
        }

        StartCoroutine(EffectFinish());
    }

    private IEnumerator EffectFinish()
    {
        yield return new WaitForSeconds(0.25f);

        ResourcePool.Instance.RecvIldeEffect(this);
    }
}
