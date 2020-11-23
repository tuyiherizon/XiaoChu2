using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBallInfoRPGBase : UIBallInfo
{

    #region show
    public override void ShowBallInfo(BallInfo ballInfo, bool isInner)
    {

    }

    public override void OnElimit()
    {
        base.OnElimit();
        switch (_BallSPType)
        {
            case BallType.RPGHP:
                base.ShowHPBombEffect();
                break;
            default:
                break;
        }
    }
    #endregion
}
