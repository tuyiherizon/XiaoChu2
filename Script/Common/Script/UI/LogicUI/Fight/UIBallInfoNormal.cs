using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBallInfoNormal : UIBallInfo
{
    #region show

    public GameObject _NormalBall1;
    public GameObject _NormalBall2;
    public GameObject _NormalBall3;
    public GameObject _NormalBall4;
    public GameObject _NormalBall5;
    public GameObject _NormalBallEmpty;

    public DragonBones.UnityArmatureComponent _Color1Armature;
    public DragonBones.UnityArmatureComponent _Color2Armature;
    public DragonBones.UnityArmatureComponent _Color3Armature;
    public DragonBones.UnityArmatureComponent _Color4Armature;
    public DragonBones.UnityArmatureComponent _Color5Armature;
    public DragonBones.UnityArmatureComponent _ColorEmptyArmature;

    public Text _BallText;

    
    private RectTransform _RectTransform;
    public RectTransform RectTransform
    {
        get
        {
            if (_RectTransform == null)
            {
                _RectTransform = gameObject.GetComponent<RectTransform>();
            }

            return _RectTransform;
        }
    }
    
    public void ClearBall()
    {
        _NormalBall1.SetActive(false);
        _NormalBall2.SetActive(false);
        _NormalBall3.SetActive(false);
        _NormalBall4.SetActive(false);
        _NormalBall5.SetActive(false);
        _NormalBallEmpty.SetActive(false);
    }

    public override void ShowBallInfo(BallInfo ballInfo, bool isInner)
    {
        _Color1Armature.gameObject.SetActive(false);
        _Color2Armature.gameObject.SetActive(false);
        _Color3Armature.gameObject.SetActive(false);
        _Color4Armature.gameObject.SetActive(false);
        _Color5Armature.gameObject.SetActive(false);
        _ColorEmptyArmature.gameObject.SetActive(false);

        switch (ballInfo.BallType)
        {
            case BallType.Color1:
                ClearBall();
                _NormalBall1.SetActive(true);
                
                break;
            case BallType.Color2:
                ClearBall();
                _NormalBall2.SetActive(true);
                _Color2Armature.gameObject.SetActive(false);
                break;
            case BallType.Color3:
                ClearBall();
                _NormalBall3.SetActive(true);
                _Color3Armature.gameObject.SetActive(false);
                break;
            case BallType.Color4:
                ClearBall();
                _NormalBall4.SetActive(true);
                _Color4Armature.gameObject.SetActive(false);
                break;
            case BallType.Color5:
                ClearBall();
                _NormalBall5.SetActive(true);
                _Color5Armature.gameObject.SetActive(false);
                break;
            case BallType.ColorEmpty:
                ClearBall();
                _NormalBallEmpty.SetActive(true);
                _ColorEmptyArmature.gameObject.SetActive(false);
                break;
            case BallType.None:
                ClearBall();
                break;
        }

        //_BallText.text = ((int)ballInfo.BallType).ToString();
        _BallText.text = (int)ballInfo.Pos.x + "," + (int)ballInfo.Pos.y;
    }

    public override void OnElimit()
    {
        base.OnElimit();

        switch (_BallSPType)
        {
            case BallType.Color1:
                _NormalBall1.SetActive(false);
                _Color1Armature.gameObject.SetActive(true);
                _Color1Armature.animation.Play("Disappear");
                break;
            case BallType.Color2:
                _NormalBall2.SetActive(false);
                _Color2Armature.gameObject.SetActive(true);
                _Color2Armature.animation.Play("Disappear");
                break;
            case BallType.Color3:
                _NormalBall3.SetActive(false);
                _Color3Armature.gameObject.SetActive(true);
                _Color3Armature.animation.Play("Disappear");
                break;
            case BallType.Color4:
                _NormalBall4.SetActive(false);
                _Color4Armature.gameObject.SetActive(true);
                _Color4Armature.animation.Play("Disappear");
                break;
            case BallType.Color5:
                _NormalBall5.SetActive(false);
                _Color5Armature.gameObject.SetActive(true);
                _Color5Armature.animation.Play("Disappear");
                break;
            case BallType.ColorEmpty:
                _NormalBallEmpty.SetActive(false);
                _ColorEmptyArmature.gameObject.SetActive(true);
                _ColorEmptyArmature.animation.Play("Disappear");
                break;
            case BallType.None:
                ClearBall();
                break;
        }
    }
    #endregion
}
