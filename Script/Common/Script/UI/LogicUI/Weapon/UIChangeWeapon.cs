using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tables;

public class UIChangeWeapon : UIBase
{
    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIChangeWeapon, UILayer.PopUI, hash);
    }

    public static int GetShowTag()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIChangeWeapon>(UIConfig.UIChangeWeapon);
        if (instance == null)
            return 0;

        if (!instance.isActiveAndEnabled)
            return 0;

        return instance._ShowTag;
    }

    public static void SetSelectWeapon(WeaponDataItem chooseWeapon)
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIChangeWeapon>(UIConfig.UIChangeWeapon);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.OnChooseWeapon(chooseWeapon);
    }

    #endregion

    public UIContainerBase _WeaponContainer;
    public Button _BtnEquip;
    public Button _BtnLvUp;
    private int _ShowTag = 0;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _ShowTag = 0;
        RefreshWeapons();
    }

    private void RefreshWeapons()
    {
        List<WeaponDataItem> weaponList = null;
        if (_ShowTag == 1)
        {
            weaponList = new List<WeaponDataItem>();
            foreach (var weaponItem in WeaponDataPack.Instance._UnLockWeapons)
            {
                //if (weaponItem.IsGetWeapon())
                {
                    weaponList.Add(weaponItem);
                }
            }
        }
        else
        {
            weaponList = new List<WeaponDataItem>(WeaponDataPack.Instance._UnLockWeapons);
        }
        weaponList.Reverse();
        _WeaponContainer.InitContentItem(weaponList);
        RefreshBtn();
    }

    private void RefreshBtn()
    {
        if (_ShowTag == 0)
        {
            _BtnEquip.interactable = false;
            _BtnLvUp.interactable = true;
        }
        else
        {
            _BtnEquip.interactable = true;
            _BtnLvUp.interactable = false;
        }
    }

    private void OnChooseWeapon(WeaponDataItem chooseWeapon)
    {
        WeaponDataPack.Instance.SetSelectWeapon(chooseWeapon.ItemDataID);
        _WeaponContainer.RefreshItems();
    }

    public void ShowLevelUp()
    {
        _ShowTag = 1;
        RefreshWeapons();

        RefreshBtn();
    }

    public void ShowEquip()
    {
        _ShowTag = 0;
        RefreshWeapons();

        RefreshBtn();
    }
}
