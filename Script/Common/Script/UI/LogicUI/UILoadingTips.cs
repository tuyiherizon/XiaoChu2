using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UILoadingTips : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UILoadingTips, UILayer.TopUI, hash);
    }

    public static void ShowAsyn(float delayTime)
    {
        Hashtable hash = new Hashtable();
        hash.Add("DelayTime", delayTime);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UILoadingTips, UILayer.TopUI, hash);
    }

    public static void HideAsyn()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UILoadingTips>(UIConfig.UILoadingTips);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.Hide();
    }

    public static bool IsShowing()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UILoadingTips>(UIConfig.UILoadingTips);
        if (instance == null)
            return false;

        return instance.isActiveAndEnabled;
            
    }

    #endregion

    private float _DelayTime = 0;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        if (hash.ContainsKey("DelayTime"))
        {
            _DelayTime = (float)hash["DelayTime"];
            ShowLast(_DelayTime);
        }
    }

    

}

