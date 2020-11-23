using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIMainFun : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIMainFun, UILayer.BaseUI, hash);
    }

    public static void ShowAsynInFight()
    {
        Hashtable hash = new Hashtable();
        hash.Add("IsInFight", true);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIMainFun, UILayer.BaseUI, hash);
    }

    public static void UpdateMoney()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIMainFun>(UIConfig.UIMainFun);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.UpdateMoneyInner();
    }
    
    public static bool IsUIShow()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIMainFun>(UIConfig.UIMainFun);
        if (instance == null)
            return false;

        if (!instance.isActiveAndEnabled)
            return false;

        return true;
    }

    public static void ShowRedTip()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIMainFun>(UIConfig.UIMainFun);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.RedTipEnable();
    }

    #endregion

    #region 

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        UpdateMoneyInner();


        bool isInFight = false;
        if (hash.ContainsKey("IsInFight"))
        {
            isInFight = (bool)hash["IsInFight"];
        }

        RedTipEnable();
    }

    #endregion

    #region info

    public UICurrencyItem _GoldItem;
    public UICurrencyItem _DiamondItem;

    private void UpdateMoneyInner()
    {
        //_GoldItem.ShowOwnCurrency(MONEYTYPE.GOLD);
        //_DiamondItem.ShowOwnCurrency(MONEYTYPE.DIAMOND);
    }

    #endregion

    #region 

    public void OnBtnFight()
    {
        UIFightBox.ShowStage(Tables.TableReader.StageInfo.GetRecord("1"));
        Hide();
    }

    public void OnBtnWeapon()
    {
        UIChangeWeapon.ShowAsyn();

        RefreshWeaponTip();
    }

    public void OnBtnGem()
    {
        UIGemPack.ShowAsyn();

        RefreshGemTip();
    }

    public void OnBtnTest()
    {
        foreach (var stageRecord in Tables.TableReader.StageInfo.Records.Values)
        {
            var mapRecord = Tables.StageMapRecord.ReadStageMap(stageRecord.ScenePath);
            mapRecord = RandomMonster.RandomStage(stageRecord.Id, mapRecord);
            //RandomMonster.RefreshMonsterHP(mapRecord);
            //RandomMonster.RefreshMonsterAtk(mapRecord);
            //mapRecord = RandomMonster.RefreshStageHPBall(mapRecord);
            //mapRecord = RandomMonster.RefreshStageStar(mapRecord, stageRecord.Id);
            mapRecord.WriteStageMap();
            //RandomMonster.WriteSkills();

            //break;
        }

        RandomMonster.WriteMonsters();
        RandomMonster.WriteSkills();
    }

    public void OnBtnTestStagePass(int starIdx)
    {
        //GameCore.Instance.TestStage();

    }

    #endregion

    #region red tip

    public GameObject _WeaponTip;
    public GameObject _GemTip;

    private static int _LastWeaponCnt = 0;
    private static int _LastGemCnt = 0;

    public void RedTipEnable()
    {
        if (_LastWeaponCnt == 0)
        {
            _LastWeaponCnt = WeaponDataPack.Instance._UnLockWeapons.Count;
            _WeaponTip.gameObject.SetActive(false);
        }
        else
        {
            if (WeaponDataPack.Instance._UnLockWeapons.Count > _LastWeaponCnt)
            {
                _WeaponTip.gameObject.SetActive(true);

                UIFuncTips.ShowAsyn(0, "2026");
            }
            else
            {
                _WeaponTip.gameObject.SetActive(false);
            }
        }

        if (_LastGemCnt == 0)
        {
            _LastGemCnt = GemDataPack.Instance._GemItems._PackItems.Count;
            _GemTip.gameObject.SetActive(false);
        }
        else
        {
            if (GemDataPack.Instance._GemItems._PackItems.Count > _LastGemCnt)
            {
                _GemTip.gameObject.SetActive(true);

                if (GemDataPack.Instance.IsSelectedGemLvLow())
                {
                    UIFuncTips.ShowAsyn(1, "2026");
                }
            }
            else
            {
                _GemTip.gameObject.SetActive(false);
            }
        }
    }

    public void RefreshWeaponTip()
    {
        _LastWeaponCnt = WeaponDataPack.Instance._UnLockWeapons.Count;

        RedTipEnable();
    }

    public void RefreshGemTip()
    {
        _LastGemCnt = GemDataPack.Instance._GemItems._PackItems.Count;

        RedTipEnable();
    }

    public void RedTipDisable()
    { }

    #endregion
}

