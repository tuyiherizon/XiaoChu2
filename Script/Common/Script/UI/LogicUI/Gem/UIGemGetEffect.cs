using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tables;

public class UIGemGetEffect : UIBase
{
    #region static funs

    public static void ShowAsyn(List<GemDataItem> gotGems)
    {
        Hashtable hash = new Hashtable();
        hash.Add("GotGems", gotGems);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIGemGetEffect, UILayer.TopUI, hash);
    }

    #endregion

    public UIContainerBase _GotGemContainer;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        var gemList = (List<GemDataItem>)hash["GotGems"];
        _GotGemContainer.InitContentItem(gemList);
    }
    
}
