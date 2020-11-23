using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBallInfoLineBase : UIBallInfo
{
    public GameObject _SPShowGO;

    public GameObject _LineUp;
    public GameObject _LineDown;
    public GameObject _LineLeft;
    public GameObject _LineRight;

    public GameObject _Color1Effect;
    public GameObject _Color2Effect;
    public GameObject _Color3Effect;
    public GameObject _Color4Effect;
    public GameObject _Color5Effect;
    public GameObject _ColorEmptyEffect;

    #region show
    public override void ShowBallInfo(BallInfo ballInfo, bool isInner)
    {
        _SPShowGO.SetActive(true);

        _Color1Effect.SetActive(false);
        _Color2Effect.SetActive(false);
        _Color3Effect.SetActive(false);
        _Color4Effect.SetActive(false);
        _Color5Effect.SetActive(false);
        _ColorEmptyEffect.SetActive(false);
        

        switch (ballInfo.BallType)
        {
            case BallType.Color1:
                _Color1Effect.SetActive(true);
                break;
            case BallType.Color2:
                _Color2Effect.SetActive(true);
                break;
            case BallType.Color3:
                _Color3Effect.SetActive(true);
                break;
            case BallType.Color4:
                _Color4Effect.SetActive(true);
                break;
            case BallType.Color5:
                _Color5Effect.SetActive(true);
                break;
            case BallType.ColorEmpty:
                _ColorEmptyEffect.SetActive(true);
                break;
            case BallType.None:
                _ColorEmptyEffect.SetActive(true);
                break;
        }

        _LineUp.SetActive(false);
        _LineDown.SetActive(false);
        _LineLeft.SetActive(false);
        _LineRight.SetActive(false);

        BallType ballType = ballInfo.BallSPType;
        if (isInner)
        {
            ballType = ballInfo.IncludeBallSPType;
        }
        switch (ballType)
        {
            case BallType.LineClumn:
            case BallType.LineClumnEnlarge:
            case BallType.LineClumnReact:
            case BallType.LineClumnHitTrap:
            case BallType.LineClumnLighting:
            case BallType.LineClumnAuto:
                _LineUp.SetActive(true);
                _LineDown.SetActive(true);
                break;
            case BallType.LineRow:
            case BallType.LineRowEnlarge:
            case BallType.LineRowReact:
            case BallType.LineRowHitTrap:
            case BallType.LineRowLighting:
            case BallType.LineRowAuto:
                _LineLeft.SetActive(true);
                _LineRight.SetActive(true);
                break;
            case BallType.LineCross:
            case BallType.LineCrossEnlarge:
            case BallType.LineCrossReact:
            case BallType.LineCrossHitTrap:
            case BallType.LineCrossLighting:
            case BallType.LineCrossAuto:
                _LineUp.SetActive(true);
                _LineDown.SetActive(true);
                _LineLeft.SetActive(true);
                _LineRight.SetActive(true);
                break;
        }
    }

    public override void OnElimit()
    {
        base.OnElimit();
        ShowLineEffect(_BallInfo._BombElimitBalls, false);

        var uiFightBall = transform.GetComponentInParent<UIFightBall>();
        uiFightBall._FightBallAnchor.gameObject.SetActive(false);
    }
    #endregion
}
