using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBallInfoLineCrossNormal : UIBallInfo
{
    public GameObject _SPShowGO;

    #region show
    public override void ShowBallInfo(BallInfo ballInfo, bool isInner)
    {
        _SPShowGO.SetActive(true);
    }

    public override void OnElimit()
    {
        ShowLineEffect(_BallInfo._BombElimitBalls, false);
    }
    #endregion
}
