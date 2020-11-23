using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tables;

public class UIGemCombinePanel : UIBase
{
    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIGemCombinePanel, UILayer.SubPopUI, hash);
    }

    public static void SetSelects(List<GemDataItem> selectedGems)
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIGemCombinePanel>(UIConfig.UIGemCombinePanel);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.SetSelectedItems(selectedGems);
    }

    #endregion

    public UIContainerSelect _CombineLevelTags;
    public List<UIGemSlot> _MatGemSlots;
    public UICurrencyItem _CombineCost;

    private List<GemDataItem> _SelectedItems = new List<GemDataItem>();
    private int _SelectedLevel = 0;
    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        RefreshTags();
    }

    public override void Hide()
    {
        base.Hide();

        UIGemPack.Refresh();
    }

    private void RefreshTags()
    {
        List<int> tags = new List<int>() { 1,2,3,4,5};
        _CombineLevelTags.InitSelectContent(tags, new List<int>() { 1 }, OnSelectLevel);
    }

    private void OnSelectLevel(object select)
    {
        _SelectedLevel = (int)select;
        AutoSelectGems(_SelectedLevel);

        _CombineCost.ShowCurrency(PlayerDataPack.MoneyGemFrag, GemDataPack.GetCombineCost(_SelectedLevel + 1));
    }

    private void AutoSelectGems(int level)
    {
        _SelectedItems.Clear();
        int checkIdx = 0;
        for (int i = 0; i < _MatGemSlots.Count; ++i)
        {
            GemDataItem gemItem = null;
            for (int j = checkIdx; j < GemDataPack.Instance._GemItems._PackItems.Count; ++j)
            {
                if (GemDataPack.Instance._GemItems._PackItems[j] == GemDataPack.Instance.SelectedGemItem)
                    continue;

                if (GemDataPack.Instance._GemItems._PackItems[j].GemRecord.Level == level)
                {
                    gemItem = GemDataPack.Instance._GemItems._PackItems[j];
                    checkIdx = j + 1;
                    break;
                }
            }
            _SelectedItems.Add(gemItem);
            _MatGemSlots[i].ShowGem(gemItem);
        }
    }

    private void SetSelectedItems(List<GemDataItem> selectedItems)
    {
        _SelectedItems = selectedItems;
        for (int i = 0; i < _MatGemSlots.Count; ++i)
        {
            if (selectedItems.Count > i)
            {
                _MatGemSlots[i].ShowGem(selectedItems[i]);
            }
            else
            {
                _MatGemSlots[i].ShowGem(null);
            }
            
        }
    }

    public void OnBtnCombine()
    {
        var resultItem = GemDataPack.Instance.CombineGemItem(_SelectedItems);
        if (resultItem != null)
        {
            _SelectedItems.Clear();
            foreach (var gemSlot in _MatGemSlots)
            {
                gemSlot.ShowGem(null);
            }

            UIGemGetEffect.ShowAsyn(new List<GemDataItem>() { resultItem });
        }
    }

    public void OnBtnAutoSelect()
    {
        int selectedLevel = _CombineLevelTags.GetSelected<int>();
        AutoSelectGems(selectedLevel);
    }

    public void OnBtnSelect()
    {
        UIGemCombineSelectPanel.ShowAsyn(_SelectedLevel, _SelectedItems);
    }
}
