using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;

public class WeaponDataItem:ItemBase
{
    
    public int Level
    {
        get
        {
            if (_DynamicDataInt.Count > 0)
                return _DynamicDataInt[0];
            return 0;
        }
        set
        {
            if (_DynamicDataInt.Count == 0)
            {
                _DynamicDataInt.Add(value);
            }
            else
            {
                _DynamicDataInt[0] = value;
            }
        }
    }

    private Tables.WeaponRecord _WeaponRecord;
    public WeaponRecord WeaponRecord
    {
        get
        {
            if (_WeaponRecord == null)
            {
                _WeaponRecord = TableReader.Weapon.GetRecord(ItemDataID);
            }
            return _WeaponRecord;
        }
    }

    public bool IsGetWeapon()
    {
        return Level > 0;
    }

    public bool IsWeaponMaxLevel()
    {
        return IsLevelMaxLevel(Level);
    }

    public bool IsLevelMaxLevel(int level)
    {
        if (level >= WeaponRecord.MaxLevel + 1)
            return true;
        return false;
    }

    public int GetUpgradeDelta()
    {
        int attrLevel = WeaponRecord.UnLockLevel;
        if (attrLevel == 1)
        {
            attrLevel = 0;
        }

        int upgradeDelta = 5;
        if (attrLevel == 0)
        {
            upgradeDelta = 1;
        }
        else if (attrLevel == 5)
        {
            upgradeDelta = 2;
        }
        else if (attrLevel == 10)
        {
            upgradeDelta = 3;
        }
        else if (attrLevel == 15)
        {
            upgradeDelta = 4;
        }

        return upgradeDelta;
    }

    public int GetUpGrageLevelToAttrLevel(int upgradeLv)
    {
        int attrLevel = WeaponRecord.UnLockLevel;
        if (attrLevel == 1)
        {
            attrLevel = 0;
        }

        int upgradeDelta = GetUpgradeDelta();


        int upgradeLevel = (upgradeLv - 1) * upgradeDelta;
        upgradeLevel = Mathf.Max(upgradeLevel, 0);

        int totalLevel = attrLevel + upgradeLevel;

        return totalLevel;
    }


    public Vector2 GetCurLevelAttrs()
    {

        int attrLevel = GetUpGrageLevelToAttrLevel(Level);

        return new Vector2(GameDataValue.GetLevelAtk(attrLevel), GameDataValue.GetLevelHP(attrLevel));
    }

    public Vector2 GetNextLevelAttrs()
    {
        int tarLevel = Level + 1;
        if (IsLevelMaxLevel(Level))
        {
            tarLevel = Level;
        }
        if (tarLevel == 1)
        {
            tarLevel = 2;
        }

        int attrLevel = GetUpGrageLevelToAttrLevel(tarLevel);

        return new Vector2(GameDataValue.GetLevelAtk(attrLevel), GameDataValue.GetLevelHP(attrLevel));
    }

    public int GetLvUPCost()
    {
        int attrLevel = WeaponRecord.UnLockLevel;
        if (attrLevel == 1)
        {
            attrLevel = 0;
        }

        int upgradeDelta = GetUpgradeDelta();

        int costGold = 0;
        for (int i = 0; i < upgradeDelta; ++i)
        {
            var costLevel = attrLevel + upgradeDelta;
            if (costLevel < 100)
            {
                costGold += Tables.GameDataValue.GetLevelDataValue(costLevel, VALUE_IDX.STAGE_GOLD) * 2;
            }
            else
            {
                costGold += Tables.GameDataValue.GetLevelDataValue(costLevel, VALUE_IDX.STAGE_GOLD) * 3;
            }
        }

        return costGold;
    }
}

public class WeaponDataPack : DataPackBase
{
    #region 单例

    private static WeaponDataPack _Instance;
    public static WeaponDataPack Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new WeaponDataPack();
            }
            return _Instance;
        }
    }

    private WeaponDataPack()
    {
        _SaveFileName = "WeaponDataPack";
    }

    #endregion

    #region 

    [SaveField(1)]
    public string _SelectedWeapon;

    private WeaponDataItem _SelectedWeaponItem;

    public WeaponDataItem SelectedWeaponItem
    {
        get
        {
            if (_SelectedWeaponItem == null)
            {
                foreach (var unLockItem in _UnLockWeapons)
                {
                    if (unLockItem.ItemDataID == _SelectedWeapon)
                    {
                        _SelectedWeaponItem = unLockItem;
                        break;
                    }
                }
            }
            return _SelectedWeaponItem;
        }
    }

    [SaveField(2)]
    public List<WeaponDataItem> _UnLockWeapons = new List<WeaponDataItem>();


    public void InitWeaponInfo()
    {
        RefreshUnLock();
    }

    public WeaponDataItem GetWeaponItem(string id)
    {
        var findItem = _UnLockWeapons.Find((findWeapon) =>
        {
            if (findWeapon.ItemDataID == id)
                return true;
            return false;
        });

        return findItem;
    }

    public void RefreshUnLock()
    {
        foreach (var tabWeaponRecord in TableReader.Weapon.Records.Values)
        {

            if (StageDataPack.Instance.CurIdx < tabWeaponRecord.UnLockLevel)
            //if (200 < tabWeaponRecord.UnLockLevel)
            {
                continue;
            }

            if (string.IsNullOrEmpty(_SelectedWeapon))
            {
                _SelectedWeapon = tabWeaponRecord.Id;
            }

            var findItem = GetWeaponItem(tabWeaponRecord.Id);
            if (findItem != null)
                continue;

            WeaponDataItem weaponItem = new WeaponDataItem();
            weaponItem.ItemDataID = tabWeaponRecord.Id;
            weaponItem.Level = 0;
            if (weaponItem.WeaponRecord.Price == 0)
            {
                weaponItem.Level = 1;
            }
            _UnLockWeapons.Add(weaponItem);
        }

        UIMainFun.ShowRedTip();
    }

    public void SetSelectWeapon(string weaponID)
    {
        _SelectedWeapon = weaponID;
        _SelectedWeaponItem = null;
        SaveClass(true);
    }

    public void BuyWeapon(WeaponDataItem weaponItem)
    {
        if (weaponItem.Level > 0)
            return;

        if (PlayerDataPack.Instance.DecMoney(PlayerDataPack.MoneyGold, weaponItem.WeaponRecord.Price))
        {
            weaponItem.Level = 1;
            SaveClass(true);
        }

        Hashtable hash = new Hashtable();
        hash.Add("WeaponItem", weaponItem);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_BUY_WEAPON, this, hash);
    }

    public void LvUpWeapon(WeaponDataItem weaponItem)
    {
        if (weaponItem.IsWeaponMaxLevel())
            return;

        if (weaponItem.Level == 0)
            return;

        if (PlayerDataPack.Instance.DecMoney(PlayerDataPack.MoneyGold, weaponItem.GetLvUPCost()))
        {
            ++weaponItem.Level;
            SaveClass(true);
        }

        Hashtable hash = new Hashtable();
        hash.Add("WeaponItem", weaponItem);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_UPGRADE_WEAPON, this, hash);
    }
    #endregion


}