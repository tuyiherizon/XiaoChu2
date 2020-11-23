using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tables;

public class UIStageSelect : UIBase
{
    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIStageSelect, UILayer.BaseUI, hash);
    }

    public static void RefreshStage()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIStageSelect>(UIConfig.UIStageSelect);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.InitStageDoorDict();
    }

    #endregion

    public RectTransform _AnchorRoot;
    public RectTransform _UpBGBase;
    public float _UpBGHeight;
    public UIStageDoor _StageInfoBase;
    public float _StageInfoHeight;

    public List<RectTransform> _ForwardImgPre;

    public class UIStageSelectDoorItem
    {
        public int _Index;
        public GameObject _LockGO;
        public GameObject _OpenGO;
        public GameObject _BattleGO;
        public GameObject _PassedGO;
        public List<GameObject> _StarGO = new List<GameObject>();
        public UIImgTextAngle _ImgTexts;
        public Button _Button;
    }

    private List<UIStageSelectDoorItem> _StageDoorItems;
    private List<UIStageDoor> _StageDoors;

    private void InitItems()
    {
        if (_StageDoorItems != null && _StageDoorItems.Count > 0)
            return;

        _StageDoorItems = new List<UIStageSelectDoorItem>();
        _StageDoors = new List<UIStageDoor>();

        int stageCnt = StageDataPack.Instance._StageItems.Count;
        int stageRemainder = stageCnt % 3;
        int stageExtend = stageRemainder > 0 ? 1 : 0;
        int needCopyBGCnt = stageCnt / 3 + stageExtend;
        for (int i = 2; i < needCopyBGCnt; ++i)
        {
            var upBG = GameObject.Instantiate<GameObject>(_UpBGBase.gameObject);
            upBG.transform.SetParent(_UpBGBase.parent);
            upBG.transform.localScale = Vector3.one;
            var upTransform = upBG.GetComponent<RectTransform>();
            upTransform.anchoredPosition = _UpBGBase.anchoredPosition + new Vector2(0, (i - 1) * _UpBGHeight);

            
        }

        for (int j = 0; j < 3; ++j)
        {
            UIStageSelectDoorItem stageItem = new UIStageSelectDoorItem();
            stageItem._LockGO = _StageInfoBase._LockGO[j];
            stageItem._OpenGO = _StageInfoBase._OpenGO[j];
            stageItem._BattleGO = _StageInfoBase._BattleGO[j];
            stageItem._PassedGO = _StageInfoBase._PassedGO[j];
            for (int k = 0; k < 3; ++k)
            {
                stageItem._StarGO.Add(_StageInfoBase._StarGO[j*3+k]);
            }
            stageItem._ImgTexts = _StageInfoBase._ImgTexts[j];
            stageItem._Button = _StageInfoBase._Button[j];
            _StageDoorItems.Add(stageItem);

            _StageDoors.Add(_StageInfoBase);
        }
        for (int i = 1; i < needCopyBGCnt; ++i)
        {
            var stageDoorGO = GameObject.Instantiate<GameObject>(_StageInfoBase.gameObject);
            stageDoorGO.transform.SetParent(_StageInfoBase.transform.parent);
            stageDoorGO.transform.localScale = Vector3.one;
            var stageDoor = stageDoorGO.GetComponent<UIStageDoor>();
            _StageDoors.Add(stageDoor);
            stageDoor._RectTransform.anchoredPosition = _StageInfoBase._RectTransform.anchoredPosition + new Vector2(0, i * _StageInfoHeight);

            for (int j = 0; j < 3; ++j)
            {
                UIStageSelectDoorItem stageItem = new UIStageSelectDoorItem();
                stageItem._LockGO = stageDoor._LockGO[j];
                stageItem._OpenGO = stageDoor._OpenGO[j];
                stageItem._BattleGO = stageDoor._BattleGO[j];
                stageItem._PassedGO = stageDoor._PassedGO[j];
                for (int k = 0; k < 3; ++k)
                {
                    stageItem._StarGO.Add(stageDoor._StarGO[j * 3 + k]);
                }
                stageItem._ImgTexts = stageDoor._ImgTexts[j];
                stageItem._Button = stageDoor._Button[j];
                _StageDoorItems.Add(stageItem);
            }
        }

        _AnchorRoot.sizeDelta += new Vector2(0, (needCopyBGCnt - 2) * _UpBGHeight);
        int heightIdx = StageDataPack.Instance.CurIdx / 3;
        if (heightIdx == 0)
        {
            _AnchorRoot.anchoredPosition = new Vector2(0, 0);
        }
        else
        {
            _AnchorRoot.anchoredPosition = new Vector2(0, -heightIdx * _UpBGHeight* _AnchorRoot.localScale.x);
        }

        InitForwardImg(needCopyBGCnt);

        InitStageDoorDict();
    }

    private void InitForwardImg(int copyCnt)
    {
        for (int i = 1; i < copyCnt; ++i)
        {
            if (i % 3 == 0)
            {
                CopyGO(_ForwardImgPre[0], i);
            }
            else if (i % 3 == 1)
            {
                CopyGO(_ForwardImgPre[1], i);
            }
            else if (i % 3 == 2)
            {
                CopyGO(_ForwardImgPre[2], i);
            }

            if (i % 4 == 0)
            {
                CopyGO(_ForwardImgPre[3], i);
            }
            else if (i % 4 == 2)
            {
                CopyGO(_ForwardImgPre[4], i);
            }
            else if (i % 4 == 3)
            {
                CopyGO(_ForwardImgPre[5], i);
            }

            if (i % 5 == 0)
            {
                CopyGO(_ForwardImgPre[6], i);
            }
            else if (i % 5 == 1)
            {
                CopyGO(_ForwardImgPre[7], i);
            }
            else if (i % 5 == 3)
            {
                CopyGO(_ForwardImgPre[8], i);
            }
            else if (i % 5 == 4)
            {
                CopyGO(_ForwardImgPre[9], i);
            }
        }
    }

    private void CopyGO(RectTransform goPrefab, int posIdx)
    {
        var copyGO = GameObject.Instantiate<GameObject>(goPrefab.gameObject);
        copyGO.transform.SetParent(goPrefab.parent);
        copyGO.transform.localScale = Vector3.one;
        copyGO.SetActive(true);
        var copyTrans = copyGO.GetComponent<RectTransform>();
        copyTrans.anchoredPosition = goPrefab.anchoredPosition + new Vector2(0, posIdx * _UpBGHeight);
    }

    private void InitStageDoorDict()
    {
        var stageRecords = StageDataPack.Instance._StageItems;
        foreach (var stageRecord in stageRecords)
        {
            RefreshStageInfo(stageRecord.StageID);
        }
    }

    private void RefreshStageInfo(string stageID)
    {
        int stageIdx = int.Parse(stageID) - 1;
        var stageInfoItem = _StageDoorItems[stageIdx];
        var stageDataItem = StageDataPack.Instance._StageItems[stageIdx];

        stageInfoItem._ImgTexts.text = stageID;
        stageInfoItem._Button.onClick.AddListener(delegate()
        {
            OnChooseStage(stageDataItem);
        });
        if (stageIdx > StageDataPack.Instance.CurIdx)
        {
            stageInfoItem._LockGO.SetActive(true);
            stageInfoItem._OpenGO.SetActive(false);
        }
        else if (stageIdx == StageDataPack.Instance.CurIdx)
        {
            stageInfoItem._ImgTexts.text = "";
            stageInfoItem._LockGO.SetActive(false);
            stageInfoItem._OpenGO.SetActive(true);
            stageInfoItem._BattleGO.SetActive(true);
            stageInfoItem._PassedGO.SetActive(false);
        }
        if (stageDataItem.Star > 0)
        {
            stageInfoItem._LockGO.SetActive(false);
            stageInfoItem._OpenGO.SetActive(true);
            stageInfoItem._BattleGO.SetActive(false);
            stageInfoItem._PassedGO.SetActive(true);

            int starCnt = 0;
            for (int i = 1; i <= stageInfoItem._StarGO.Count; ++i)
            {
                if (stageDataItem.IsStarOn(i - 1))
                {
                    ++starCnt;
                }
            }

            for (int i = 0; i < stageInfoItem._StarGO.Count; ++i)
            {
                if (i == starCnt - 1)
                {
                    stageInfoItem._StarGO[i].SetActive(true);
                }
                else
                {
                    stageInfoItem._StarGO[i].SetActive(false);
                }

            }
        }
        
    }

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        InitItems();

        Refresh();

        SetGuide();
    }

    private void Refresh()
    {
        
    }

    private void OnChooseStage(object selectItem)
    {
        StageDataItem selectStage = (StageDataItem)selectItem;
        Debug.Log("OnChooseStage:" + selectStage.StageID);
        int stageIdx = int.Parse(selectStage.StageID) - 1;
        if (stageIdx <= StageDataPack.Instance.CurIdx)
        {
            UIStageEnsure.ShowStageEnsure(selectStage);
        }

        UIGuide.HideGuide();
    }

    public void SetGuide()
    {
        if (StageDataPack.Instance.CurIdx == 0)
        {
            UIGuide.ShowPoint(_StageDoorItems[0]._OpenGO.transform);
        }
    }
}
