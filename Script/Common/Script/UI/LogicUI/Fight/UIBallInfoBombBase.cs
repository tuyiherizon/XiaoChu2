using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBallInfoBombBase : UIBallInfo
{

    #region show
    public override void ShowBallInfo(BallInfo ballInfo, bool isInner)
    {

    }

    public override void OnElimit()
    {
        base.OnElimit();

        ShowBombEffect(_BallInfo._BombElimitBalls, false);

        var uiFightBall = transform.GetComponentInParent<UIFightBall>();
        uiFightBall._FightBallAnchor.gameObject.SetActive(false);
    }
    #endregion
}
