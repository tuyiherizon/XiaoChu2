using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tables;

public class UIChangeWeaponItem : UIItemBase
{

    public Text _Name;
    public Text _Level;
    public Image _Icon;
    public UIImgText _Atk;
    public UIImgText _HP;
    public Text _SkillName;

    public Button _BtnEquip;
    public Button _BtnEquiped;
    public Button _BtnBuy;
    public UICurrencyItem _BuyPrice;
    public Button _BtnLvUp;
    public UICurrencyItem _LvUpPrice;
    public GameObject _MaxLevelTip;
    public GameObject _NextAttrs;
    public UIImgText _NextLvAtk;
    public UIImgText _NextLvHP;

    private WeaponDataItem _ShowWeapon;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        var weaponItem = (WeaponDataItem)hash["InitObj"];

        _ShowWeapon = weaponItem;
        RefreshWeaponItem(weaponItem);
    }

    public override void Refresh()
    {
        base.Refresh();

        RefreshWeaponItem(_ShowWeapon);
    }

    public void RefreshWeaponItem(WeaponDataItem weaponItem)
    {
        //_Name.text = StrDictionary.GetFormatStr(weaponItem.WeaponRecord.NameDict);
        ResourceManager.Instance.SetImage(_Icon, weaponItem.WeaponRecord.Icon);
        _SkillName.text = StrDictionary.GetFormatStr(weaponItem.WeaponRecord.SkillName);
        var curAttr = weaponItem.GetCurLevelAttrs();
        _Atk.text = ((int)curAttr.x).ToString();
        _HP.text = ((int)curAttr.y).ToString();
        _NextAttrs.SetActive(false);

        if (UIChangeWeapon.GetShowTag() == 0)
        {
            _BtnLvUp.gameObject.SetActive(false);
            _MaxLevelTip.SetActive(false);

            if (weaponItem.Level > 0)
            {
                _BtnBuy.gameObject.SetActive(false);

                if (weaponItem.ItemDataID == WeaponDataPack.Instance._SelectedWeapon)
                {
                    _BtnEquip.gameObject.SetActive(false);
                    _BtnEquiped.gameObject.SetActive(true);
                }
                else
                {
                    _BtnEquip.gameObject.SetActive(true);
                    _BtnEquiped.gameObject.SetActive(false);
                }

                _Level.text = "Lv." + (weaponItem.Level - 1).ToString();
            }
            else
            {
                _BtnEquip.gameObject.SetActive(false);
                _BtnBuy.gameObject.SetActive(true);
                _BuyPrice.ShowCurrency(PlayerDataPack.MoneyGold, weaponItem.WeaponRecord.Price);

                _Level.text = "";
            }
        }
        else
        {
            _BtnEquip.gameObject.SetActive(false);
            _BtnBuy.gameObject.SetActive(false);
            _BtnEquiped.gameObject.SetActive(false);

            _Level.text = "Lv." + (weaponItem.Level - 1) + "/" + weaponItem.WeaponRecord.MaxLevel;

            if (weaponItem.Level == 0)
            {
                _BtnEquip.gameObject.SetActive(false);
                _BtnBuy.gameObject.SetActive(true);
                _Level.text = "Lv.0";
                _BuyPrice.ShowCurrency(PlayerDataPack.MoneyGold, weaponItem.WeaponRecord.Price);
            }
            else if (!weaponItem.IsWeaponMaxLevel())
            {
                _BtnLvUp.gameObject.SetActive(true);
                _MaxLevelTip.SetActive(false);

                _NextAttrs.SetActive(true);
                var nextAttr = weaponItem.GetNextLevelAttrs();
                _NextLvAtk.text = "+" + ((int)nextAttr.x - curAttr.x).ToString();
                _NextLvHP.text = "+" + ((int)nextAttr.y - curAttr.y).ToString();

                if (weaponItem.IsGetWeapon())
                {
                    _LvUpPrice.gameObject.SetActive(true);
                    _LvUpPrice.ShowCurrency(PlayerDataPack.MoneyGold, weaponItem.GetLvUPCost());
                }
                else
                {
                    _LvUpPrice.gameObject.SetActive(false);
                }
            }
            else
            {
                _BtnLvUp.gameObject.SetActive(false);
                _MaxLevelTip.SetActive(true);
            }

            
        }
    }

    public void OnBtnEquip()
    {
        UIChangeWeapon.SetSelectWeapon(_ShowWeapon);
    }

    public void OnBtnBuy()
    {
        WeaponDataPack.Instance.BuyWeapon(_ShowWeapon);
        Refresh();
    }

    public void OnBtnLvUp()
    {
        WeaponDataPack.Instance.LvUpWeapon(_ShowWeapon);
        Refresh();
    }

    public void OnSkillTip()
    {
        UIWeaponSkillTip.ShowAsyn(_ShowWeapon);
    }
}
