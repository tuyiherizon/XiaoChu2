using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBallInfoBombBigReact : UIBallInfoBombBase
{
    public GameObject _SPShowGO;
    public GameObject _SPShowReactGO;

    #region show
    public override void ShowBallInfo(BallInfo ballInfo, bool isInner)
    {
        BallInfoSPBombBigReact reactBall = null;

        if (isInner)
        {
            reactBall = (BallInfoSPBombBigReact)ballInfo._IncludeBallInfoSP;
        }
        else
        {
            reactBall = (BallInfoSPBombBigReact)ballInfo._BallInfoSP;
        }
        if (reactBall != null)
        {
            if (reactBall._IsReactBall)
            {
                _SPShowGO.SetActive(false);
                _SPShowReactGO.SetActive(true);
            }
            else
            {
                _SPShowGO.SetActive(true);
                _SPShowReactGO.SetActive(false);
            }
        }
    }
    #endregion
}
