using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tables;

public class UIStageEnsure : UIBase
{
    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIStageEnsure, UILayer.PopUI, hash);
    }

    public static void ShowStageEnsure(StageDataItem stageInfo)
    {
        Hashtable hash = new Hashtable();
        hash.Add("StageInfo", stageInfo);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIStageEnsure, UILayer.PopUI, hash);
    }

    #endregion

    public List<Image> _StarImgs;
    public List<Text> _StarTexts;
    public List<Image> _EnemyImgs;
    public List<UIImgText> _EnemyTexts;

    private StageDataItem _StageInfo;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);
        _StageInfo = (StageDataItem)hash["StageInfo"];
        Refresh();
    }

    private void Refresh()
    {
        var mapRecord = StageMapRecord.ReadStageMap(_StageInfo.StageRecord.ScenePath);
        for (int i = 0; i < _StarImgs.Count; ++i)
        {
            if (_StageInfo.IsStarOn(i))
            {
                _StarImgs[i].gameObject.SetActive(true);
            }
            else
            {
                _StarImgs[i].gameObject.SetActive(false);
            }

            _StarTexts[i].text = StarInfoBase.GetStarConditionStr(mapRecord._StarInfos[i]);
        }

        Dictionary<ELEMENT_TYPE, int> monsterList = new Dictionary<ELEMENT_TYPE, int>();
        foreach (var wave in mapRecord._MapStageLogic._Waves)
        {
            foreach (var monsterID in wave.NPCs)
            {
                var monRecord = Tables.TableReader.MonsterBase.GetRecord(monsterID);
                if (!monsterList.ContainsKey(monRecord.ElementType))
                {
                    monsterList.Add(monRecord.ElementType, 0);
                }
                ++monsterList[monRecord.ElementType];
            }
        }

        int monImgIdx = 0;
        foreach (var monsterType in monsterList)
        {
            if (monImgIdx == _EnemyImgs.Count)
                break;

            _EnemyImgs[monImgIdx].transform.parent.gameObject.SetActive(true);
            ResourceManager.Instance.SetImage(_EnemyImgs[monImgIdx], CommonDefine.GetElementIcon(monsterType.Key));
            _EnemyTexts[monImgIdx].text = monsterType.Value.ToString();

            ++monImgIdx;
        }

        for (int i = monImgIdx; i < _EnemyImgs.Count; ++i)
        {
            _EnemyImgs[i].transform.parent.gameObject.SetActive(false);
        }
    }

    public void OnOK()
    {
        StageDataPack.Instance._FightingStage = _StageInfo;
        LogicManager.Instance.EnterFight(_StageInfo);
    }

    public void OnTestPass1()
    {
        StageDataPack.Instance.TestPass(_StageInfo.StageID, 3);
        Hide();
        UIStageSelect.RefreshStage();

        //GameCore.Instance.TestStage(_StageInfo);

    }
}
