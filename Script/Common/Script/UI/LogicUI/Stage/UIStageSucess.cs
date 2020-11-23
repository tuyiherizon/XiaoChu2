using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tables;

public class UIStageSucess : UIBase
{
    #region static funs

    public static void ShowAsyn(StageInfoRecord stageRecord, int starCnt, List<AwardItem> awardList)
    {
        Hashtable hash = new Hashtable();
        hash.Add("StageRecord", stageRecord);
        hash.Add("StarCnt", starCnt);
        hash.Add("AwardList", awardList);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIStageSucess, UILayer.SubPopUI, hash);
    }

    public static bool IsShow()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIStageSucess>(UIConfig.UIStageSucess);
        if (instance == null)
            return false;

        if (!instance.isActiveAndEnabled)
            return false;

        return true;
    }

    #endregion

    public EffectController _PassEffect;
    public GameObject _Panel;

    public List<EffectController> _StarEffect1;
    public List<EffectController> _StarEffect2;
    public List<EffectController> _StarEffect3;

    public UIImgText _StageName;

    public List<UIAwardItem> _AwardItems;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);
        var stageRecord = (StageInfoRecord)hash["StageRecord"];
        var starCnt = (int)hash["StarCnt"];
        var awardList = (List<AwardItem>)hash["AwardList"];
        //Refresh(stageRecord, starCnt);
        StartCoroutine(ShowPassEffect(stageRecord, starCnt, awardList));

        GameCore.Instance._SoundManager.PlayBGMusic(null);
        PlayerUISound(GameCore.Instance._SoundManager._Sucess, 1);
    }

    private IEnumerator ShowPassEffect(StageInfoRecord stageRecord, int starCnt, List<AwardItem> awardList)
    {
        _PassEffect.gameObject.SetActive(true);
        _Panel.gameObject.SetActive(false);
        _PassEffect.PlayEffect();

        foreach (var awardItem in _AwardItems)
        {
            awardItem.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(1.8f);

        _PassEffect.gameObject.SetActive(false);
        _Panel.gameObject.SetActive(true);
        Refresh(stageRecord, starCnt);

        for (int i = 0; i < awardList.Count; ++i)
        {
            _AwardItems[i].gameObject.SetActive(true);
            _AwardItems[i].SetAwardInfo(awardList[i]);
        }
    }

    private void Refresh(StageInfoRecord stageRecord, int starCnt)
    {
        _StageName.text = stageRecord.Id;
        foreach (var effect in _StarEffect1)
        {
            effect.HideEffect();
        }
        foreach (var effect in _StarEffect2)
        {
            effect.HideEffect();
        }
        foreach (var effect in _StarEffect3)
        {
            effect.HideEffect();
        }
        switch (starCnt)
        {
            case 1:
                foreach (var effect in _StarEffect1)
                {
                    effect.PlayEffect();
                }
                break;
            case 2:
                foreach (var effect in _StarEffect2)
                {
                    effect.PlayEffect();
                }
                break;
            case 3:
                foreach (var effect in _StarEffect3)
                {
                    effect.PlayEffect();
                }
                break;
        }
    }

    public void OnOK()
    {
        LogicManager.Instance.ExitFight();

        Hashtable hash = new Hashtable();
        hash.Add("StageRecord", LogicManager.Instance.EnterStageInfo);

        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_PASS_STAGE, this, hash);
    }
}
