using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class UISystemSetting : UIBase
{

    #region static funs

    public static void ShowAsyn(bool isInFight = false)
    {
        Hashtable hash = new Hashtable();
        hash.Add("IsInFight", isInFight);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UISystemSetting, UILayer.SubPopUI, hash);
    }

    #endregion


    #region setting

    public Slider _Volumn;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        bool isInFight = (bool)hash["IsInFight"];
        InitSetting(isInFight);
    }

    public void InitSetting(bool isInFight)
    {
        if (_Volumn != null)
        {
            _Volumn.value = GlobalValPack.Instance.Volume;
        }
    }
    
    public void OnSlider()
    {
        GlobalValPack.Instance.Volume = _Volumn.value;
    }

    public void OnExit()
    {
        LogicManager.Instance.ExitFight();
    }
    #endregion
}

