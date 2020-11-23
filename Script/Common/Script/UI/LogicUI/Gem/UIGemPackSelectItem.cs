using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tables;

public class UIGemPackSelectItem : UIItemSelect
{

    public Text _Name;
    public Image _Icon;
    public List<UIImgText> _AttrText;
    public Image _SkillIcon;
    public Text _SkillDesc;
    
    private GemDataItem _ShowGem;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        var gemItem = (GemDataItem)hash["InitObj"];

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
        _Name.text = StrDictionary.GetFormatStr(gemItem.GemRecord.NameDict);
        ResourceManager.Instance.SetImage(_Icon, gemItem.GemRecord.Icon);
        if (gemItem.GemExAttrRecord != null)
        {
            _SkillIcon.gameObject.SetActive(true);
            ResourceManager.Instance.SetImage(_SkillIcon, gemItem.GemExAttrRecord.Icon);
            _SkillDesc.text = StrDictionary.GetFormatStr(gemItem.GemExAttrRecord.NameDict);
        }
        else
        {
            _SkillIcon.gameObject.SetActive(false);
        }

        for (int i = 0; i < _AttrText.Count; ++i)
        {
            _AttrText[i].text = GameDataValue.ConfigIntToPersent(gemItem.GemRecord.Attrs[i]) + "%";
        }
        
    }

    public GameObject _SelectedTag;

    public override void Selected()
    {
        base.Selected();

        _SelectedTag.SetActive(true);
    }

    public override void UnSelected()
    {
        base.UnSelected();

        _SelectedTag.SetActive(false);
    }
}
