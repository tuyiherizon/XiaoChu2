using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum UILayer
{
    ControlUI,
    BaseUI,
    MainFunUI,
    PopUI,
    SubPopUI,
    Sub2PopUI,
    MessageUI,
    TopUI
}

public class AssetInfo
{
    public string AssetPath;

    public AssetInfo(string assetPath)
    {
        AssetPath = assetPath;
    }
}

public class UIConfig
{
    public static AssetInfo UILogin = new AssetInfo("SystemUI/UILogin");
    public static AssetInfo UILoadingScene = new AssetInfo("SystemUI/UILoadingScene");
    public static AssetInfo UIMessageBox = new AssetInfo("SystemUI/UIMessageBox");
    public static AssetInfo UISystemSetting = new AssetInfo("SystemUI/UISystemSetting");
    public static AssetInfo UIMessageTip = new AssetInfo("SystemUI/Message/UIMessageTip");
    public static AssetInfo UIFuncTips = new AssetInfo("SystemUI/Message/UIFuncTips");
    public static AssetInfo UIDiamondEnsureMsgBox = new AssetInfo("SystemUI/UIDiamondEnsureMsgBox");
    public static AssetInfo UITextTip = new AssetInfo("SystemUI/Message/UITextTip");

    public static AssetInfo UIMainFun = new AssetInfo("LogicUI/UIMainFun");
    public static AssetInfo UILoadingTips = new AssetInfo("LogicUI/UILoadingTips");

    public static AssetInfo UIFightBox = new AssetInfo("LogicUI/Fight/BallGame/UIFightBox");
    public static AssetInfo UIFightBattleField = new AssetInfo("LogicUI/Fight/BattleField/UIFightBattleField");

    public static AssetInfo UIChangeWeapon = new AssetInfo("LogicUI/Weapon/UIChangeWeapon");
    public static AssetInfo UIWeaponSkillTip = new AssetInfo("LogicUI/Weapon/UIWeaponSkillTip");

    public static AssetInfo UIGemPack = new AssetInfo("LogicUI/Gem/UIGemPack");
    public static AssetInfo UIGemBuyPanel = new AssetInfo("LogicUI/Gem/UIGemBuyPanel");
    public static AssetInfo UIGemGetEffect = new AssetInfo("LogicUI/Gem/UIGemGetEffect");
    public static AssetInfo UIGemCombinePanel = new AssetInfo("LogicUI/Gem/UIGemCombinePanel");
    public static AssetInfo UIGemCombineSelectPanel = new AssetInfo("LogicUI/Gem/UIGemCombineSelectPanel");

    public static AssetInfo UIStageSelect = new AssetInfo("LogicUI/Stage/UIStageSelect");
    public static AssetInfo UIStageEnsure = new AssetInfo("LogicUI/Stage/UIStageEnsure");
    public static AssetInfo UIStageSucess = new AssetInfo("LogicUI/Stage/UIStageSucess");
    public static AssetInfo UIStageFail = new AssetInfo("LogicUI/Stage/UIStageFail");

    public static AssetInfo UIGuide = new AssetInfo("LogicUI/Guide/UIGuide");

    public static AssetInfo UIRechargePack = new AssetInfo("LogicUI/Shop/UIRechargePack");
    public static AssetInfo UIMoneyLackTip = new AssetInfo("LogicUI/Shop/UIMoneyLackTip");


}
