using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBallInfoTrapBase : UIBallInfo
{
    public GameObject[] _SPBallFrozen;
    public Text _SPNum;
    public DragonBones.UnityArmatureComponent _Armature;

    #region show

    public override void OnClearInfo()
    {
        base.OnClearInfo();
    }

    public void ClearInfo()
    {
        for (int i = 0; i < _SPBallFrozen.Length; ++i)
        {
            _SPBallFrozen[i].SetActive(false);
        }
        _SPNum.text = "";
    }

    public override void ShowBallInfo(BallInfo ballInfo, bool isInner)
    {
        ClearInfo();
        int spNum = 0;
        int showNum = 0;
        if (isInner)
        {
            spNum = ((BallInfoSPTrapBase)ballInfo._IncludeBallInfoSP).ElimitNum;
            showNum = ((BallInfoSPTrapBase)ballInfo._IncludeBallInfoSP).ShowNum;
        }
        else
        {
            spNum = ((BallInfoSPTrapBase)ballInfo._BallInfoSP).ElimitNum;
            showNum = ((BallInfoSPTrapBase)ballInfo._BallInfoSP).ShowNum;
        }
        int showIdx = 0;
        _SPNum.text = "";
        if (spNum > _SPBallFrozen.Length)
        {
            showIdx = _SPBallFrozen.Length - 1;
            _SPNum.gameObject.SetActive(true);
        }
        else
        {
            showIdx = spNum - 1;
            _SPNum.gameObject.SetActive(false);
        }

        if (showNum < spNum)
        {
            Debug.Log("PlayAnim " + ballInfo.Pos + " showNum:" + showNum);
            StartCoroutine(AppearAnim(showIdx, spNum));

            if (isInner)
            {
                ((BallInfoSPTrapBase)ballInfo._IncludeBallInfoSP).ShowNum = spNum;
            }
            else
            {
                ((BallInfoSPTrapBase)ballInfo._BallInfoSP).ShowNum = spNum;
            }
        }
        else
        {
            SetBallInfo(showIdx, spNum);
        }

    }

    public override void OnElimit()
    {
        base.OnElimit();

        _Armature.gameObject.SetActive(true);
        _Armature.animation.timeScale = 1;
        _Armature.animation.Play("disappear");
    }

    private IEnumerator AppearAnim(int showIdx, int spNum)
    {
        Debug.Log("AppearAnim " + _BallInfo.Pos + " showNum:" + spNum);

        _Armature.gameObject.SetActive(true);
        _Armature.animation.timeScale = 2;
        _Armature.animation.Play("appear");
        yield return new WaitForSeconds(0.25f);

        _Armature.gameObject.SetActive(false);
        SetBallInfo(showIdx, spNum);
    }

    private void SetBallInfo(int showIdx, int spNum)
    {
        for (int i = 0; i < _SPBallFrozen.Length; ++i)
        {
            if (i == showIdx)
            {
                _SPBallFrozen[i].SetActive(true);
            }
            else
            {
                _SPBallFrozen[i].SetActive(false);
            }
        }
        _SPNum.text = spNum.ToString();
    }
    #endregion
}
