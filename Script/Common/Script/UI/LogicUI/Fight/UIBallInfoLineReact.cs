using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBallInfoLineReact : UIBallInfoLineBase
{
    public GameObject _SPShowReactGO;
    public GameObject _SubLineUp;
    public GameObject _SubLineDown;
    public GameObject _SubLineLeft;
    public GameObject _SubLineRight;

    #region show
    public override void ShowBallInfo(BallInfo ballInfo, bool isInner)
    {
        base.ShowBallInfo(ballInfo,isInner);

        BallInfoSPLineReact reactBall = null;

        if (isInner)
        {
            reactBall = (BallInfoSPLineReact)ballInfo._IncludeBallInfoSP;
        }
        else
        {
            reactBall = (BallInfoSPLineReact)ballInfo._BallInfoSP;
        }
        if (reactBall != null)
        {
            if (reactBall._IsReactBall)
            {
                _SPShowGO.SetActive(false);
                _SPShowReactGO.SetActive(true);

                _SubLineUp.SetActive(false);
                _SubLineDown.SetActive(false);
                _SubLineLeft.SetActive(false);
                _SubLineRight.SetActive(false);
                switch (ballInfo.BallSPType)
                {
                    case BallType.LineClumn:
                    case BallType.LineClumnEnlarge:
                    case BallType.LineClumnReact:
                    case BallType.LineClumnHitTrap:
                    case BallType.LineClumnLighting:
                    case BallType.LineClumnAuto:
                        _SubLineUp.SetActive(true);
                        _SubLineDown.SetActive(true);
                        break;
                    case BallType.LineRow:
                    case BallType.LineRowEnlarge:
                    case BallType.LineRowReact:
                    case BallType.LineRowHitTrap:
                    case BallType.LineRowLighting:
                    case BallType.LineRowAuto:
                        _SubLineLeft.SetActive(true);
                        _SubLineRight.SetActive(true);
                        break;
                    case BallType.LineCross:
                    case BallType.LineCrossEnlarge:
                    case BallType.LineCrossReact:
                    case BallType.LineCrossHitTrap:
                    case BallType.LineCrossLighting:
                    case BallType.LineCrossAuto:
                        _SubLineUp.SetActive(true);
                        _SubLineDown.SetActive(true);
                        _SubLineLeft.SetActive(true);
                        _SubLineRight.SetActive(true);
                        break;
                }
            }
            else
            {
                _SPShowGO.SetActive(true);
                _SPShowReactGO.SetActive(false);
            }
        }
    }

    public override void OnElimit()
    {
        var reactBall = (BallInfoSPLineReact)_BallInfo._BallInfoSP;
        if (reactBall == null)
        {
            ShowLineEffect(_BallInfo._BombElimitBalls, true);
        }
        else
        {
            ShowLineEffect(_BallInfo._BombElimitBalls, false);
        }
    }
    #endregion
}
