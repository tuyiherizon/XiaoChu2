using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBallInfoBombSmallNormal : UIBallInfoBombBase
{
    public GameObject _SPShowGO;
  

    #region show
    public override void ShowBallInfo(BallInfo ballInfo, bool isInner)
    {
        _SPShowGO.SetActive(true);
    }
    #endregion
}
