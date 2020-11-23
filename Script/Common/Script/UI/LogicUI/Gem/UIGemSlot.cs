using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tables;

public class UIGemSlot : UIItemBase
{

    public Text _Name;
    public Image _Icon;
    
    private GemDataItem _ShowGem;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        var gemItem = (GemDataItem)hash["InitObj"];

        _ShowGem = gemItem;
        RefreshGemItem(gemItem);
    }

    public void ShowGem(GemDataItem gemItem)
    {
        _ShowGem = gemItem;
        RefreshGemItem(gemItem);
    }

    public override void Refresh()
    {
        base.Refresh();

        RefreshGemItem(_ShowGem);
    }

    public void RefreshGemItem(GemDataItem gemItem)
    {
        if (gemItem == null)
        {
            _Name.text = "";
            _Icon.gameObject.SetActive(false);
        }
        else
        {
            _Name.text = StrDictionary.GetFormatStr(gemItem.GemRecord.NameDict);
            _Icon.gameObject.SetActive(true);
            ResourceManager.Instance.SetImage(_Icon, gemItem.GemRecord.Icon);
        }
        
    }
}
