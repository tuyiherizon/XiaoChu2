using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStageSelectItem : UIItemBase
{
    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        StageDataItem stageItem = (StageDataItem)hash["InitObj"];
        ShowStageInfo(stageItem);
    }

    public Text _UIStageName;

    private void ShowStageInfo(StageDataItem stageItem)
    {
        _UIStageName.text = Tables.StrDictionary.GetFormatStr(stageItem.StageRecord.Name);
    }
}
