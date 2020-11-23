using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tables;

public class UIWeaponSkillTip : UIBase
{
    #region static funs

    public static void ShowAsyn(WeaponDataItem weaponItem)
    {
        Hashtable hash = new Hashtable();
        hash.Add("WeaponItem", weaponItem);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIWeaponSkillTip, UILayer.SubPopUI, hash);
    }


    #endregion

    public Text _SkillName;
    public Text _SkillDesc;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        var weaponItem = (WeaponDataItem)hash["WeaponItem"];

        _SkillName.text = StrDictionary.GetFormatStr(weaponItem.WeaponRecord.SkillName);
        _SkillDesc.text = StrDictionary.GetFormatStr(weaponItem.WeaponRecord.SkillDesc);
    }

}
