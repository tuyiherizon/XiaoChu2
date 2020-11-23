using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tables;

public class UIGemCombineSelectPanel : UIBase
{
    #region static funs

    public static void ShowAsyn(int level, List<GemDataItem> selectedItems)
    {
        Hashtable hash = new Hashtable();
        hash.Add("Level", level);
        hash.Add("SelectedItems", selectedItems);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIGemCombineSelectPanel, UILayer.Sub2PopUI, hash);
    }

    #endregion

    public UIContainerSelect _GemContainer;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        List<GemDataItem> selectedItems = (List<GemDataItem>)hash["SelectedItems"];
        int level = (int)hash["Level"];
        RefreshItems(level, selectedItems);
    }

    private void RefreshItems(int level,List<GemDataItem> selectedItems)
    {
        List<GemDataItem> showGems = new List<GemDataItem>();
        foreach (var gemItem in GemDataPack.Instance._GemItems._PackItems)
        {
            if (gemItem.GemRecord.Level == level && gemItem != GemDataPack.Instance.SelectedGemItem)
            {
                showGems.Add(gemItem);
            }
        }
        _GemContainer.InitSelectContent(showGems, selectedItems);
    }

    public void OnBtnOk()
    {
        List<GemDataItem> selectedItems = _GemContainer.GetSelecteds<GemDataItem>();
        UIGemCombinePanel.SetSelects(selectedItems);
    }

}
