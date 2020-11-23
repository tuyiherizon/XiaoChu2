using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tables;

public class UIGemPackItem : UIItemBase
{

    public Text _Name;
    public Image _Icon;
    public List<UIImgText> _AttrText;
    public GameObject _SkillPanel;
    public Image _SkillIcon;
    public Text _SkillDesc;

    public Button _BtnEquip;
    public Button _BtnEquiped;
    
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
            _SkillPanel.gameObject.SetActive(true);
            ResourceManager.Instance.SetImage(_SkillIcon, gemItem.GemExAttrRecord.Icon);
            _SkillDesc.text = StrDictionary.GetFormatStr(gemItem.GemExAttrRecord.NameDict);
        }
        else
        {
            _SkillPanel.gameObject.SetActive(false);
        }

        for (int i = 0; i < _AttrText.Count; ++i)
        {
            _AttrText[i].text = GameDataValue.ConfigIntToPersent(gemItem.GemRecord.Attrs[i]) + "%";
        }

        if (gemItem == GemDataPack.Instance.SelectedGemItem)
        {
            _BtnEquip.gameObject.SetActive(false);
            _BtnEquiped.gameObject.SetActive(true);
        }
        else
        {
            _BtnEquip.gameObject.SetActive(true);
            _BtnEquiped.gameObject.SetActive(false);
        }
    }

    public void OnBtnEquip()
    {
        UIGemPack.SetSelectGem(_ShowGem);
    }
}
