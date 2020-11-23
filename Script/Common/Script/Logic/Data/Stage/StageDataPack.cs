using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;

public class StageDataItem
{
    [SaveField(1)]
    public string StageID;
    [SaveField(2)]
    public int Star;

    private Tables.StageInfoRecord _StageRecord;
    public StageInfoRecord StageRecord
    {
        get
        {
            if (_StageRecord == null)
            {
                _StageRecord = TableReader.StageInfo.GetRecord(StageID);
            }
            return _StageRecord;
        }
    }

    public void SetStar(int idx)
    {
        Star |= 1 << idx;
    }

    public bool IsStarOn(int idx)
    {
        return (Star & (1 << idx)) > 0;
    }
}

public class StageDataPack : DataPackBase
{
    #region 单例

    private static StageDataPack _Instance;
    public static StageDataPack Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new StageDataPack();
            }
            return _Instance;
        }
    }

    private StageDataPack()
    {
        _SaveFileName = "StageDataPack";
    }

    #endregion

    #region 

    [SaveField(1)]
    public List<StageDataItem> _StageItems;

    public StageDataItem _FightingStage;

    public void InitStageInfo()
    {
        if (_StageItems == null)
        {
            _StageItems = new List<StageDataItem>();

        }

        if (_StageItems.Count == 0)
        { 
            foreach (var tabRecord in TableReader.StageInfo.Records)
            {
                StageDataItem stageItem = new StageDataItem();
                stageItem.StageID = tabRecord.Key;
                stageItem.Star = 0;

                _StageItems.Add(stageItem);
            }
        }
        
    }

    public void PassStage(StageMapRecord passStageMap)
    {
        List<AwardItem> awardList = new List<AwardItem>();
        _FightingStage = LogicManager.Instance.EnterStageInfo;
        var baseAward = AwardManager.AddAward(_FightingStage.StageRecord.AwardType[0], _FightingStage.StageRecord.AwardValue[0]);
        awardList.Add(baseAward);

        int starCnt = 0;
        for (int i = 0; i < 3; ++i)
        {
            if (_FightingStage.IsStarOn(i))
                continue;

            bool isGetStar = true;
            if (passStageMap._StarInfos.Count > i)
            {
                isGetStar = StarInfoBase.isCanGetStar(passStageMap._StarInfos[i]);
            }

            if (isGetStar)
            {
                StageDataPack.Instance.SetStageStar(_FightingStage.StageRecord.Id, i);

                var starAward = AwardManager.AddAward(_FightingStage.StageRecord.AwardType[i + 1], _FightingStage.StageRecord.AwardValue[i + 1]);
                awardList.Add(starAward);
            }
        }

        for (int i = 0; i < 3; ++i)
        {
            if (_FightingStage.IsStarOn(i))
            {
                ++starCnt;
            }
        }

        StageDataPack.Instance.SaveClass(true);
        UIFightBox.HideOptMask();
        UIStageSucess.ShowAsyn(_FightingStage.StageRecord, starCnt, awardList);

        WeaponDataPack.Instance.RefreshUnLock();
    }

    public void TestPass(string stageID, int starCnt)
    {
        List<AwardItem> awardList = new List<AwardItem>();
        var stage = Tables.TableReader.StageInfo.GetRecord(stageID);
        var baseAward = AwardManager.AddAward(stage.AwardType[0], stage.AwardValue[0]);
        awardList.Add(baseAward);

        for (int i = 0; i < starCnt; ++i)
        {
            StageDataPack.Instance.SetStageStar(stage.Id, i);

            var starAward = AwardManager.AddAward(stage.AwardType[i + 1], stage.AwardValue[i + 1]);
            awardList.Add(starAward);
        }

        StageDataPack.Instance.SaveClass(true);
        //UIStageSucess.ShowAsyn(stage, starCnt, awardList);

        WeaponDataPack.Instance.RefreshUnLock();
    }

    public void SetStageStar(string stageID, int starIdx)
    {
        int stageIdx = int.Parse(stageID) - 1;
        _StageItems[stageIdx].SetStar(starIdx);

        RefreshCurIdx();
    }

    public void GetAward(string id, int value)
    {
        if (PlayerDataPack.IsMoney(id))
        {
            PlayerDataPack.Instance.AddMoney(id, value);
        }

        //gem
        if (id.Equals("20001"))
        {
            GemDataPack.Instance.AddRandomGem(1);
        }
        else if (id.Equals("20002"))
        {
            GemDataPack.Instance.AddRandomGem(2);
        }
        else if (id.Equals("20003"))
        {
            GemDataPack.Instance.AddRandomGem(3);
        }
        else if (id.Equals("20004"))
        {
            GemDataPack.Instance.AddRandomGem(4);
        }
    }

    #endregion

    #region func

    private int _CurIdx = -1;
    public int CurIdx
    {
        get
        {
            if (_CurIdx < 0)
            {
                RefreshCurIdx();
            }
            return _CurIdx;
        }
    }

    private void RefreshCurIdx()
    {
        int noStarIdx = -1;
        for (int i = 0; i < _StageItems.Count; ++i)
        {
            if (_StageItems[i].Star == 0)
            {
                noStarIdx = i;
                break;
            }
        }

        if (noStarIdx < 0)
        {
            noStarIdx = _StageItems.Count;
        }

        if (noStarIdx > _CurIdx && _CurIdx != -1)
        {
            Hashtable hash = new Hashtable();

            GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_PASS_NEW_STAGE, this, hash);
        }

        _CurIdx = noStarIdx;
    }

    #endregion
}