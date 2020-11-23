using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tables;

public class UIStageFail : UIBase
{
    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIStageFail, UILayer.SubPopUI, hash);
    }

    public static bool IsShow()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIStageFail>(UIConfig.UIStageFail);
        if (instance == null)
            return false;

        if (!instance.isActiveAndEnabled)
            return false;

        return true;
    }

    #endregion

    public Button _BtnReviveAD;
    public Button _BtnReviveGold;
    public UICurrencyItem _GoldReviveCost;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _GoldReviveCost.ShowCurrency(PlayerDataPack.MoneyGold, BattleField.Instance.GetReviveCost());

        if (BattleField.Instance._AlreadyReviveTypes.Contains(1))
        {
            _BtnReviveAD.interactable = false;
        }
        else
        {
            _BtnReviveAD.interactable = true;
        }

        if (BattleField.Instance._AlreadyReviveTypes.Contains(2))
        {
            _BtnReviveGold.interactable = false;
        }
        else
        {
            _BtnReviveGold.interactable = true;
        }
    }

    public void OnBtnOK()
    {
        LogicManager.Instance.ExitFight();

        Hashtable hash = new Hashtable();
        hash.Add("StageRecord", LogicManager.Instance.EnterStageInfo);

        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_FAIL_STAGE, this, hash);
    }

    public void OnAdRevive()
    {
        AdManager.Instance.WatchAdVideo(OnADFinish);
    }

    public void OnADFinish()
    {
        BattleField.Instance.RoleRelive(1);
        UIFightBattleField.RefreshReviveStatic();
        Hide();

        Hashtable hash = new Hashtable();
        
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_REVIVE_AD, this, hash);
    }

    public void OnGoldRevive()
    {
        BattleField.Instance.RoleRelive(2);
        UIFightBattleField.RefreshReviveStatic();
        Hide();

        Hashtable hash = new Hashtable();

        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_REVIVE_GOLD, this, hash);
    }
}
