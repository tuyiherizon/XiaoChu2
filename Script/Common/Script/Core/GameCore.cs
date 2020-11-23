using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

/// <summary>
/// 游戏核心
/// </summary>
public class GameCore : MonoBehaviour
{
    #region 固有

    public void Awake()
    {
        DontDestroyOnLoad(this);
        Application.runInBackground = false;
        Application.targetFrameRate = 60;
        _Instance = this;
    }

    public void Start()
    {
        ResourceManager.InitResourceManager();
        StartCoroutine(UpdateInit());

        DataRecordManager.Instance.InitDataRecord();
    }

    public void Update()
    {
        UpdateQuit();
#if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.C))
        {
            LogicManager.Instance.CleanUpSave();
        }

        TestStageUpdate();
#endif
    }

    void UpdateQuit()
    {
        if ((Application.platform == RuntimePlatform.Android
        || Application.platform == RuntimePlatform.WindowsPlayer
        || Application.platform == RuntimePlatform.WindowsEditor) && (Input.GetKeyDown(KeyCode.Escape)))
        {
            {
                UIMessageBox.Show(1000006, () =>
                {
                    LogicManager.Instance.QuitGame();
                    Debug.Log("save data");
                }, null);
            }

        }
    }

    

    void OnApplicationQuit()
    {
        LogicManager.Instance.QuitGame();
    }
    #endregion

    #region start logic

    IEnumerator UpdateInit()
    {
        while (!_HasInitLogic)
        {
            if (ResourceManager.Instance != null && ResourcePool.Instance != null)
            {
                StartGameLogic();
                break;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    bool _HasInitLogic = false;
    void StartGameLogic()
    {
        InitLanguage();
        Tables.TableReader.ReadTables();
        UILogin.ShowAsyn();
        DataRecordManager.Instance.InitDataRecord();
        AdManager.InitAdManager();
    }

    #endregion

    #region 唯一

    private static GameCore _Instance = null;
    public static GameCore Instance
    {
        get
        {
            return _Instance;
        }
    }

    #endregion

    #region 管理者

    /// <summary>
    /// 主UI画布
    /// </summary>
    [SerializeField]
    private UIManager _UIManager;
    public UIManager UIManager { get { return _UIManager; } }

    [SerializeField]
    private EventController _EventController;
    public EventController EventController { get { return _EventController; } }

    public SoundManager _SoundManager;

    #endregion

    #region 

    public int _StrVersion = 0;
    public bool _IsTestMode = false;

    public void InitLanguage()
    {
//#if UNITY_EDITOR
//        _StrVersion = 0;
//        return;
//#else
        if (Application.systemLanguage == SystemLanguage.Chinese
            || Application.systemLanguage == SystemLanguage.ChineseSimplified)
        {
            _StrVersion = 1;
        }
        else if (Application.systemLanguage == SystemLanguage.ChineseTraditional)
        {
            _StrVersion = 2;
        }
        else
        {
            _StrVersion = 0;
        }
//#endif
    }

    #endregion

    #region test stage

    public class TestStageInfo
    {
        public bool _IsWin;
        public int _ElimitBall;
        public int _ElimitTrap;
        public int _ElimitBomb;
        public int _RemainHP;
        public int _Round;
    }

    private int _DefaultTestTimes = 10;
    private int _TestTimes = 0;
    private StageDataItem _TestingStage;
    private int _CurIdx = 0;
    private int _TargetIdx = 1;

    private Dictionary<StageDataItem, List<TestStageInfo>> _TestInfos = new Dictionary<StageDataItem, List<TestStageInfo>>();

    public void TestStage(StageDataItem stageItem)
    {
        _TestingStage = stageItem;
        _TestInfos.Clear();
        _TestInfos.Add(_TestingStage, new List<TestStageInfo>());
        StageDataPack.Instance._FightingStage = _TestingStage;
        LogicManager.Instance.EnterFight(_TestingStage);
    }

    public void TestStageUpdate()
    {
        if (_TestingStage == null)
            return;

        if (!UIFightBox.IsTestMode())
        {
            UIFightBox.SetTest();
        }

        if (UIStageSucess.IsShow() || UIStageFail.IsShow())
        {
            TestStageInfo testInfo = new TestStageInfo();
            if (UIStageSucess.IsShow())
            {
                testInfo._IsWin = true;
            }
            else
            {
                testInfo._IsWin = false;
            }
            testInfo._Round = BattleField.Instance._BattleRound - 1;
            testInfo._RemainHP = (int)(((float)BattleField.Instance._RoleMotion._HP / BattleField.Instance._RoleMotion._MaxHP) * 10);
            testInfo._ElimitTrap = BallBox.Instance._ElimitTrapCnt;
            testInfo._ElimitBomb = BallBox.Instance._ElimitBombCnt;

            ++_TestTimes;
            _TestInfos[_TestingStage].Add(testInfo);
            LogicManager.Instance.ExitFight();

            if (_TestTimes >= _DefaultTestTimes)
            {
                _TestTimes = 0;
                //++_CurIdx;
                //if (_CurIdx >= _TargetIdx)
                {
                    _TestingStage = null;
                    WriteRecords();
                }
                //else
                //{
                //    _TestingStage = StageDataPack.Instance._StageItems[_CurIdx];
                //    _TestInfos.Add(_TestingStage, new List<TestStageInfo>());
                //    StageDataPack.Instance._FightingStage = _TestingStage;
                //    LogicManager.Instance.EnterFight(_TestingStage);
                //}
            }
            else
            {
                StageDataPack.Instance._FightingStage = _TestingStage;
                LogicManager.Instance.EnterFight(_TestingStage);
            }
        }
    }

    public void WriteRecords()
    {
        string recordPath = Application.dataPath + "/../build/Records/StageTestAll.txt";
        StreamWriter writerAll = File.AppendText(recordPath);

        foreach (var testInfo in _TestInfos)
        {
            string testStagePath = Application.dataPath + "/../build/Records/StageTest" + "_" + testInfo.Key.StageID + ".txt";
            StreamWriter writerSingle = new StreamWriter(testStagePath);

            string winTag = testInfo.Key.StageID + "\t";
            foreach (var testSingle in testInfo.Value)
            {
                int winFlag = testSingle._IsWin ? 1 : 0;
                string testSingleInfo = winFlag + "\t" + testSingle._RemainHP + "\t" + testSingle._Round + "\t" + testSingle._ElimitBomb + "\t" + testSingle._ElimitTrap;
                writerSingle.WriteLine(testSingleInfo);


                winTag += winFlag.ToString() + "\t";

            }
            writerSingle.Close();
            writerAll.WriteLine(winTag);
        }

        writerAll.Close();
    }

    #endregion

}

