using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBallInfoStone : UIBallInfo
{
    public GameObject[] _SPBallShowGO;
    public Text _EarthNum;

    #region show
    public override void ShowBallInfo(BallInfo ballInfo, bool isInner)
    {
        int spNum = 0;
        if (isInner)
        {
            spNum = ((BallInfoSPTrapStone)ballInfo._IncludeBallInfoSP).ElimitNum;
        }
        else
        {
            spNum = ((BallInfoSPTrapStone)ballInfo._BallInfoSP).ElimitNum;
        }
        int showIdx = 0;
        if (spNum > 2)
        {
            showIdx = 1;
        }
        else
        {
            showIdx = 0;
        }
        for (int i = 0; i < _SPBallShowGO.Length; ++i)
        {
            if (i == showIdx)
            {
                _SPBallShowGO[i].SetActive(true);
            }
            else
            {
                _SPBallShowGO[i].SetActive(false);
            }
        }
        _EarthNum.text = spNum.ToString();
    }
    #endregion
}
