using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;

public class DataRecordManager
{
    #region 唯一

    private static DataRecordManager _Instance = null;
    public static DataRecordManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new DataRecordManager();
            }
            return _Instance;
        }
    }

    private DataRecordManager() { }

    #endregion

    TDGAAccount account;

    public void InitDataRecord()
    {
        TalkingDataGA.OnStart("18CA14E84A59417B9266D490D14C50D7", "ggp");
        account = TDGAAccount.SetAccount(TalkingDataGA.GetDeviceId());

        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_ENTER_STAGE, EnterStageHandle);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_PASS_STAGE, PassStageHandle);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_FAIL_STAGE, FailStageHandle);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_PASS_NEW_STAGE, PassNewStageHandle);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_REVIVE_AD, ReviveADHandle);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_REVIVE_GOLD, ReviveGOLDHandle);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_BUY_WEAPON, BuyWeaponHandle);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_UPGRADE_WEAPON, UpGradeWeaponHandle);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_BUY_GEM, BuyGemHandle);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_COMBINE_GEM, CombineGemHandle);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_IAP_REQ, IAPReqHandle);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_IAP_SUCESS, IAPSucessHandle);
    }

    public void EnterStageHandle(object e, Hashtable hash)
    {
        Debug.Log("EnterStageHandle");
        TDGAMission.OnCompleted(LogicManager.Instance.EnterStageInfo.StageID);
    }

    public void PassStageHandle(object e, Hashtable hash)
    {
        Debug.Log("PassStageHandle");

        TDGAMission.OnCompleted(LogicManager.Instance.EnterStageInfo.StageID);
        
        account.SetLevel(StageDataPack.Instance.CurIdx);

    }

    public void FailStageHandle(object e, Hashtable hash)
    {
        Debug.Log("FailStageHandle");

        TDGAMission.OnFailed(LogicManager.Instance.EnterStageInfo.StageID, "1");

        account.SetLevel(StageDataPack.Instance.CurIdx);

    }

    public void PassNewStageHandle(object e, Hashtable hash)
    {
        Debug.Log("PassNewStageHandle");
        
        account.SetLevel(StageDataPack.Instance.CurIdx);

    }

    public void ReviveADHandle(object e, Hashtable hash)
    {
        Debug.Log("ReviveADHandle");
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("Stage", LogicManager.Instance.EnterStageInfo.StageID);
        TalkingDataGA.OnEvent("ReviveAD", dic);
    }

    public void ReviveGOLDHandle(object e, Hashtable hash)
    {
        Debug.Log("ReviveGOLDHandle");
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("Stage", LogicManager.Instance.EnterStageInfo.StageID);
        TalkingDataGA.OnEvent("ReviveGOLD", dic);
    }

    public void BuyWeaponHandle(object e, Hashtable hash)
    {
        Debug.Log("BuyWeaponHandle");
        WeaponDataItem weaponItem = (WeaponDataItem)hash["WeaponItem"];
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("WeaponID", weaponItem.ItemDataID);
        TalkingDataGA.OnEvent("BuyWeapon", dic);
    }

    public void UpGradeWeaponHandle(object e, Hashtable hash)
    {
        Debug.Log("UpGradeWeaponHandle");
        WeaponDataItem weaponItem = (WeaponDataItem)hash["WeaponItem"];
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("WeaponID", weaponItem.ItemDataID);
        TalkingDataGA.OnEvent("UpGradeWeapon", dic);
    }

    public void BuyGemHandle(object e, Hashtable hash)
    {
        Debug.Log("BuyGemHandle");
        int num = (int)hash["Num"];
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("Num", num);
        TalkingDataGA.OnEvent("BuyGem", dic);
    }

    public void CombineGemHandle(object e, Hashtable hash)
    {
        Debug.Log("CombineGemHandle");
        int level = (int)hash["Level"];
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("Level", level);
        TalkingDataGA.OnEvent("CombineGem", dic);
    }

    public void IAPReqHandle(object e, Hashtable hash)
    {
        Debug.Log("IAPReqHandle");
        string punchID = (string)hash["PurchID"];
        string orderID = (string)hash["OrderID"];
        var chargeRecord = Tables.TableReader.Recharge.GetRecord(punchID);

        TDGAVirtualCurrency.OnChargeRequest(orderID, chargeRecord.Id, chargeRecord.Price, "CH", chargeRecord.Num, "PT");
    }

    public void IAPSucessHandle(object e, Hashtable hash)
    {
        Debug.Log("IAPSucessHandle");

        string punchID = (string)hash["PurchID"];
        string orderID = (string)hash["OrderID"];
        var chargeRecord = Tables.TableReader.Recharge.GetRecord(punchID);

        TDGAVirtualCurrency.OnChargeSuccess(orderID);
    }
}
