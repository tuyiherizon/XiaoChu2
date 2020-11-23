using UnityEngine;
using System.Collections;
using System;

using Tables;
using UnityEngine.SceneManagement;

public class LogicManager
{
    #region 唯一

    private static LogicManager _Instance = null;
    public static LogicManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new LogicManager();
            }
            return _Instance;
        }
    }

    private LogicManager() { }

    #endregion

    #region start logic

    public void StartLoadLogic()
    {
        SceneManager.LoadScene(GameDefine.GAMELOGIC_SCENE_NAME);

        PlayerDataPack.Instance.LoadClass(true);
        PlayerDataPack.Instance.InitPlayerData();

        StageDataPack.Instance.LoadClass(true);
        StageDataPack.Instance.InitStageInfo();

        WeaponDataPack.Instance.LoadClass(true);
        WeaponDataPack.Instance.InitWeaponInfo();

        GemDataPack.Instance.LoadClass(true);
        GemDataPack.Instance.InitGemInfo();
    }

    #endregion

    #region

    public void StartLogic()
    {
        UIStageSelect.ShowAsyn();
        UIMainFun.ShowAsyn();

        PurchManager.Instance.InitIAPInfo();

        GameCore.Instance._SoundManager.PlayBGMusic(GameCore.Instance._SoundManager._LogicAudio);
    }

    public void SaveGame()
    {
        PlayerDataPack.Instance.SaveClass(false);
        
    }

    public void QuitGame()
    {
        try
        {
            SaveGame();
            DataLog.StopLog();
            Application.Quit();
        }
        catch (Exception e)
        {
            Application.Quit();
        }
    }

    public void CleanUpSave()
    {

    }
    #endregion

    #region Fight

    public StageDataItem EnterStageInfo;

    public void EnterFight(StageDataItem enterStage)
    {
        EnterStageInfo = enterStage;

        GameCore.Instance.UIManager.DestoryAllUI();

        Hashtable hash = new Hashtable();
        hash.Add("StageRecord", enterStage);

        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_ENTER_STAGE, this, hash);

        UIFightBattleField.ShowAsyn();

        var mapRecord = StageMapRecord.ReadStageMap(enterStage.StageRecord.ScenePath);
        BallBox.Instance.Init(mapRecord);
        BallBox.Instance.InitBallInfo();

        BattleField.Instance.InitBattle(enterStage.StageRecord, mapRecord);
    }

    public void EnterFightFinish()
    {
        UIFightBox.ShowStage(EnterStageInfo.StageRecord);
        GameCore.Instance._SoundManager.PlayBGMusic(GameCore.Instance._SoundManager._FightAudio);
    }

    public void ExitFight()
    {
        GameCore.Instance.UIManager.DestoryAllUI();

        UIStageSelect.ShowAsyn();
        UIMainFun.ShowAsyn();

        GameCore.Instance._SoundManager.PlayBGMusic(GameCore.Instance._SoundManager._LogicAudio);

        
    }

    public void ExitFightScene()
    {
        GameCore.Instance.UIManager.DestoryAllUI();
        DestoryFightLogic();
        UILoadingScene.ShowAsyn(GameDefine.GAMELOGIC_SCENE_NAME);

        //UIMainFun.ShowAsynInFight();
    }

    public void DestoryFightLogic()
    {

    }

    public void InitFightScene()
    {
        DestoryFightLogic();

    }


    #endregion
}

