using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Tables;

public class UIAwardItem : UIItemBase
{

    #region 

    public Image _AwardQuality;
    public Image _AwardIcon;
    public Text _AwardName;
    public Text _AwardValue;

    #endregion

    public void SetAwardInfo(AwardItem awardItem)
    {
        SetAwardInfo(awardItem.AwardType, awardItem.AwardValue);
    }

    public void SetAwardInfo(string awardID, int value)
    {
        var itemRecord = TableReader.CommonItem.GetRecord(awardID);
        if (_AwardQuality != null)
        {
            ResourceManager.Instance.SetImage(_AwardQuality, CommonDefine.GetQualityIcon(itemRecord.Quality));
        }
        if (_AwardIcon != null)
        {
            ResourceManager.Instance.SetImage(_AwardIcon, itemRecord.Icon);
        }
        if (_AwardName != null)
        {
            _AwardName.text = StrDictionary.GetFormatStr(itemRecord.NameStrDict);
        }
        if (_AwardValue != null)
        {
            _AwardValue.text = value.ToString();
        }
    }

}

