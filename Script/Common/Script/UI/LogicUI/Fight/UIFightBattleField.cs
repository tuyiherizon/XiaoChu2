using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tables;

public class UIFightBattleField : UIBase
{
    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIFightBattleField, UILayer.PopUI, hash);
    }

    public static void InitMonstersStatic(List<MotionBase> monsters)
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIFightBattleField>(UIConfig.UIFightBattleField);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.InitMonsters(monsters);
    }

    public static void ShowElimitInfo()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIFightBattleField>(UIConfig.UIFightBattleField);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.ShowElimit();
    }

    public static void ShowRefreshOptRound()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIFightBattleField>(UIConfig.UIFightBattleField);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.RefreshOptRound();
    }

    public static void RefreshMonster(MotionBase monster)
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIFightBattleField>(UIConfig.UIFightBattleField);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.RefreshMonsterInfo(monster);
    }

    public static void PlayDamageAnim(List<DamageResult> damageResult)
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIFightBattleField>(UIConfig.UIFightBattleField);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.StartShowDamage(damageResult);
    }

    public static void RefreshReviveStatic()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIFightBattleField>(UIConfig.UIFightBattleField);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.RefreshRevive();
    }
    #endregion

    public class MonsterShowInfo
    {
        public UIFightMonsterHPItem _UIHPItem;
        public EffectController _AppearAnim;
        public EffectController _DeadAnim;
        public MonsterModel _MonsterModel;
        public MotionBase _MonsterInfo;
    }

    public Vector2 _MonsterBasePos = new Vector2(220, -110);
    public Vector2 _HPItemPosDelta = new Vector2(0, -30);
    public BattleScene _BattleScene;
    public RawImage _BattleImage;
    public List<UIFightMonsterHPItem> _HPItem;
    public List<EffectController> _MonAppearAnims;
    public List<EffectController> _MonDeadAnims;

    private Dictionary<MotionBase, MonsterShowInfo> _MonsterShowDict = new Dictionary<MotionBase, MonsterShowInfo>();

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        float canvaHeight = ((float)760 / Screen.width) * Screen.height;
        float battleImgY = (canvaHeight * 0.5f - 337) * 0.5f + 377;
        _BattleImage.rectTransform.anchoredPosition = new Vector2(0, battleImgY);
    }

    public void OnEnable()
    {
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_START_WAVE, StartWaveHandle);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_CAST_DAMAGE, CastDamageHandle);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_MONSTER_SKILL, MonsterSkillHandle);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_ADD_ROLE_HP, AddRoleHPHandle);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_ROUND_END, RoundEndHandle);
    }

    public void OnDisable()
    {
        GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_LOGIC_START_WAVE, StartWaveHandle);
        GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_LOGIC_CAST_DAMAGE, CastDamageHandle);
        GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_LOGIC_MONSTER_SKILL, MonsterSkillHandle);
        GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_LOGIC_ADD_ROLE_HP, AddRoleHPHandle);
        GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_LOGIC_ROUND_END, RoundEndHandle);
    }

    public void RefreshRevive()
    {
        RefreshOptRound();

        int waveIdx = BattleField.Instance.StageLogic._Waves.Count - BattleField.Instance._CurWave;
        _WaveText.text = waveIdx.ToString();

        UpdateSelfHPBar(BattleField.Instance._RoleMotion._HP);
    }

    public void StartWaveHandle(object sender, Hashtable hash)
    {
        List<MotionBase> monsters = (List<MotionBase>)hash["Monsters"];
        InitMonsters(monsters);

        RefreshOptRound();
    }

    public void RoundEndHandle(object sender, Hashtable hash)
    {
        RefreshMonsterCD();
        ShowElimit();
    }

    public void CastDamageHandle(object sender, Hashtable hash)
    {
        List<DamageResult> damageResults = (List<DamageResult>)hash["DamageResult"];
        StartShowDamage(damageResults);

        HideOptRound();

        ShowElimit();
    }

    public void MonsterSkillHandle(object sender, Hashtable hash)
    {
        List<DamageResult> damageResults = (List<DamageResult>)hash["SkillResult"];
        StartShowMonsterSkill(damageResults);
    }

    public void AddRoleHPHandle(object sender, Hashtable hash)
    {
        DamageResult damageResults = (DamageResult)hash["SkillResult"];
        RoleAddHP(damageResults);
    }

    public void InitMonsters(List<MotionBase> monsters)
    {
        UIFightBox.ShowOptMask();
        foreach (var monster in _MonsterShowDict.Values)
        {
            ResourcePool.Instance.RecvIldeModelItem(monster._MonsterModel.gameObject);
        }
        _MonsterShowDict.Clear();
        for (int i = 0; i < _HPItem.Count; ++i)
        {
            _HPItem[i].gameObject.SetActive(false);
            if (monsters.Count > i)
            {
                MonsterShowInfo monShowInfo = new MonsterShowInfo();
                monShowInfo._MonsterInfo = monsters[i];
                monShowInfo._UIHPItem = _HPItem[i];
                _MonsterShowDict.Add(monsters[i], monShowInfo);
            }
        }
        for(int i = 0; i< monsters.Count; ++i)
        {
            Hashtable hash = new Hashtable();
            hash.Add("motion", monsters[i]);
            hash.Add("posIdx", i);
            hash.Add("posCnt", monsters.Count);
            ResourcePool.Instance.LoadModel(monsters[i]._MonsterRecord.Model, LoadMonsterFinish, hash);
        }

        int waveIdx = BattleField.Instance.StageLogic._Waves.Count - BattleField.Instance._CurWave;
        _WaveText.text = waveIdx.ToString();
    }

    public void LoadMonsterFinish(string modelName, GameObject modelGO, Hashtable hash)
    {
        MotionBase monster = (MotionBase)hash["motion"];
        int posIdx = (int)hash["posIdx"];
        int posCnt = (int)hash["posCnt"];
        modelGO.transform.SetParent(_BattleImage.transform);
        modelGO.transform.localScale = Vector3.one;
        Vector2 monsterAnchorPos = Vector2.zero;
        if (posCnt == 1)
        {
            monsterAnchorPos = new Vector2(0, _MonsterBasePos.y);
            //modelGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, _MonsterBasePos.y);
        }
        else if (posCnt == 2)
        {
            if (posIdx == 0)
            {
                monsterAnchorPos = new Vector2(-_MonsterBasePos.x * 0.5f, _MonsterBasePos.y);
                //modelGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(-_MonsterBasePos.x*0.5f, _MonsterBasePos.y);
            }
            else
            {
                monsterAnchorPos = new Vector2(_MonsterBasePos.x * 0.5f, _MonsterBasePos.y);
                //modelGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(_MonsterBasePos.x * 0.5f, _MonsterBasePos.y);
            }
        }
        else if (posCnt == 3)
        {
            if (posIdx == 0)
            {
                monsterAnchorPos = new Vector2(-_MonsterBasePos.x, _MonsterBasePos.y);
                //modelGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(-_MonsterBasePos.x, _MonsterBasePos.y);
            }
            else if (posIdx == 1)
            {
                monsterAnchorPos = new Vector2(0, _MonsterBasePos.y);
                //modelGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, _MonsterBasePos.y);
            }
            else
            {
                monsterAnchorPos = new Vector2(_MonsterBasePos.x, _MonsterBasePos.y);
                //modelGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(_MonsterBasePos.x, _MonsterBasePos.y);
            }
        }

        modelGO.GetComponent<RectTransform>().anchoredPosition = monsterAnchorPos;

        Debug.Log("monsterAnchorPos:" + monsterAnchorPos);

        _MonsterShowDict[monster]._UIHPItem.InitMonsterInfo(monster);
        _MonsterShowDict[monster]._UIHPItem._RectTransform.anchoredPosition = monsterAnchorPos + _HPItemPosDelta;
        _MonsterShowDict[monster]._UIHPItem.gameObject.SetActive(false);

        modelGO.SetActive(false);
        _MonsterShowDict[monster]._MonsterModel = modelGO.GetComponent<MonsterModel>();
        _MonsterShowDict[monster]._AppearAnim = _MonAppearAnims[posIdx];
        _MonsterShowDict[monster]._AppearAnim.gameObject.SetActive(false);
        _MonsterShowDict[monster]._DeadAnim = _MonDeadAnims[posIdx];
        _MonsterShowDict[monster]._DeadAnim.gameObject.SetActive(false);

        ModelInitFinish(posIdx, posCnt);
    }

    public void ModelInitFinish(int idx, int num)
    {
        if (idx >= num - 1)
        {
            StartCoroutine(ShowAppearAnim());
        }
    }

    public void RefreshMonsterInfo(MotionBase monster)
    {
        if (_MonsterShowDict.ContainsKey(monster))
        {
            if (monster.IsDied)
            {
                _MonsterShowDict[monster]._MonsterModel.gameObject.SetActive(false);
                _MonsterShowDict[monster]._AppearAnim.gameObject.SetActive(false);
                _MonsterShowDict[monster]._UIHPItem.gameObject.SetActive(false);
            }
            else
            {
                _MonsterShowDict[monster]._UIHPItem.InitMonsterInfo(monster);
            }
        }
    }

    public IEnumerator ShowAppearAnim()
    {
        yield return new WaitForSeconds(0.2f);
        foreach (var monsterInfo in _MonsterShowDict.Values)
        {
            monsterInfo._AppearAnim.transform.position = monsterInfo._MonsterModel.transform.position;
            monsterInfo._AppearAnim.transform.localPosition += new Vector3(0, 50, 0);
            monsterInfo._AppearAnim.gameObject.SetActive(true);
            monsterInfo._AppearAnim.PlayEffect();

                PlayerUISound(_BornSound, 1);

            yield return new WaitForSeconds(0.4f);
            monsterInfo._MonsterModel.gameObject.SetActive(true);
            monsterInfo._MonsterModel.PlayIdle();
        }

        yield return new WaitForSeconds(0.2f);
        foreach (var monsterInfo in _MonsterShowDict.Values)
        {
            monsterInfo._UIHPItem.gameObject.SetActive(true);
        }

        UIFightBox.HideOptMask();
    }

    public IEnumerator ShowDisappearAnim(MonsterShowInfo monsterInfo)
    {
        yield return new WaitForSeconds(0.3f);

        monsterInfo._DeadAnim.transform.position = monsterInfo._MonsterModel.transform.position;
        monsterInfo._DeadAnim.transform.localPosition += new Vector3(0, 50, 0);
        monsterInfo._DeadAnim.gameObject.SetActive(true);
        monsterInfo._DeadAnim.PlayEffect();

        PlayerUISound(_DiedSound, 1);

        yield return new WaitForSeconds(0.4f);
        monsterInfo._MonsterModel.gameObject.SetActive(false);
        monsterInfo._MonsterModel.PlayIdle();
        monsterInfo._UIHPItem.gameObject.SetActive(false);
    }


    #region role

    public Slider _HPBar;
    public UIImgText _WaveText;
    public UIImgText _OptRound;
    public GameObject _OptGO;

    public void UpdateSelfHPBar(float hpValue)
    {
        _HPBar.value = hpValue / BattleField.Instance._RoleMotion._MaxHP;
    }

    public void RoleAddHP(DamageResult damageResult)
    {
        StartCoroutine(RoleAddHPAnim(damageResult));
    }

    public IEnumerator RoleAddHPAnim(DamageResult damageResult)
    {
        ClearDamageShow();

        _AddHPText.transform.position = _HPBar.transform.position;
        _AddHPText.transform.localPosition += new Vector3(0, 10, 0);
        _AddHPText.gameObject.SetActive(true);
        _AddHPText.text = "+" + damageResult.DamageValue;

        UpdateSelfHPBar(damageResult._AfterHP);

        yield return new WaitForSeconds(0.5f);
        ClearDamageShow();
    }

    public void RefreshOptRound()
    {
        int lastRound = (BattleField._DamageOptRound - BattleField.Instance.CurOptRound);

        _OptGO.gameObject.SetActive(true);
        _OptRound.text = lastRound.ToString();
    }

    public void HideOptRound()
    {
        _OptGO.gameObject.SetActive(false);
    }

    #endregion

    #region elimit info

    public List<Text> _ElimitCnt;

    public void ShowElimit()
    {
        int colorName1 = GetColorDamage(BallType.Color1);
        _ElimitCnt[0].text = colorName1.ToString();

        _ElimitCnt[1].text = GetColorDamage(BallType.Color2).ToString();
        _ElimitCnt[2].text = GetColorDamage(BallType.Color3).ToString();
        _ElimitCnt[3].text = GetColorDamage(BallType.Color4).ToString();
        _ElimitCnt[4].text = GetColorDamage(BallType.Color5).ToString();
    }

    private int GetColorDamage(BallType ballType)
    {
        int colorNum = 0;
        if (BattleField.Instance._RoundDamageBalls.ContainsKey(ballType))
        {
            colorNum += BattleField.Instance._RoundDamageBalls[ballType];
        }
        if (BallBox.Instance._RoundTotalEliminate.ContainsKey(ballType))
        {
            colorNum += BallBox.Instance._RoundTotalEliminate[ballType];
        }

        return colorNum;
    }

    #endregion

    #region damage show

    public List<EffectController> _ElementEffects;
    public UIImgText _DmgTextNormal;
    public UIImgText _DmgTextDouble;
    public UIImgText _DmgTextHalf;
    public UIImgText _AddHPText;

    public void StartShowDamage(List<DamageResult> damageResults)
    {
        UIFightBox.ShowOptMask();
        if (UIFightBox.IsTestMode())
        {
            GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_CAST_DAMAGE_FINISH, this, null);
            return;
        }
        StartCoroutine(DamageShow(damageResults));
    }

    public IEnumerator DamageShow(List<DamageResult> damageResults)
    {
        ClearDamageShow();
        foreach (var damageResult in damageResults)
        {
            if (!_MonsterShowDict.ContainsKey(damageResult._TargetMotion))
                continue;

            var showEffect = _ElementEffects[(int)damageResult._AtkType];
            if (showEffect != null)
            {
                var monInfo = _MonsterShowDict[damageResult._TargetMotion];
                showEffect.transform.position = _MonsterShowDict[damageResult._TargetMotion]._MonsterModel.transform.position;
                showEffect.transform.localPosition += new Vector3(0, 30, 0);
                showEffect.PlayEffect();
                switch (damageResult._AtkType)
                {
                    case ELEMENT_TYPE.FIRE:
                        PlayerUISound(_PlayerAttackSounds[0], 1);
                        break;
                    case ELEMENT_TYPE.ICE:
                        PlayerUISound(_PlayerAttackSounds[1], 1);
                        break;
                    case ELEMENT_TYPE.WIND:
                        PlayerUISound(_PlayerAttackSounds[2], 1);
                        break;
                    case ELEMENT_TYPE.LIGHT:
                        PlayerUISound(_PlayerAttackSounds[3], 1);
                        break;
                    case ELEMENT_TYPE.DARK:
                        PlayerUISound(_PlayerAttackSounds[4], 1);
                        break;
                }
                yield return new WaitForSeconds(0.15f);

                monInfo._UIHPItem.RefreshHP(damageResult._AfterHP);
                switch (damageResult._DamageType)
                {
                    case DAMAGE_TYPE.Normal:
                        _DmgTextNormal.transform.position = monInfo._MonsterModel.transform.position;
                        _DmgTextNormal.transform.localPosition += new Vector3(0, 30, 0);
                        _DmgTextNormal.gameObject.SetActive(true);
                        _DmgTextNormal.text = "-" + damageResult.DamageValue;
                        break;
                    case DAMAGE_TYPE.Double:
                        _DmgTextDouble.transform.position = monInfo._MonsterModel.transform.position;
                        _DmgTextDouble.transform.localPosition += new Vector3(0, 30, 0);
                        _DmgTextDouble.gameObject.SetActive(true);
                        _DmgTextDouble.text = "-" + damageResult.DamageValue;
                        break;
                    case DAMAGE_TYPE.Half:
                        _DmgTextHalf.transform.position = monInfo._MonsterModel.transform.position;
                        _DmgTextHalf.transform.localPosition += new Vector3(0, 30, 0);
                        _DmgTextHalf.gameObject.SetActive(true);
                        _DmgTextHalf.text = "-" + damageResult.DamageValue;
                        break;
                }

                yield return new WaitForSeconds(0.3f);
                ClearDamageShow();

                if(damageResult._AfterHP <= 0)
                {
                    StartCoroutine(ShowDisappearAnim(monInfo));
                    yield return new WaitForSeconds(0.5f);
                }
            }
        }
        yield return new WaitForSeconds(1.0f);

        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_CAST_DAMAGE_FINISH, this, null);

    }


    public void ClearDamageShow()
    {
        _DmgTextNormal.gameObject.SetActive(false);
        _DmgTextDouble.gameObject.SetActive(false);
        _DmgTextHalf.gameObject.SetActive(false);
        _AddHPText.gameObject.SetActive(false);
    }

    #endregion

    #region monster skill 

    public void StartShowMonsterSkill(List<DamageResult> damageResult)
    {
        if (UIFightBox.IsTestMode())
        {
            if (BattleField.Instance._RoleMotion._HP <= 0 && BattleField.Instance._RoleMotion.IsDied)
            {
                UIStageFail.ShowAsyn();
            }

            UpdateSelfHPBar(BattleField.Instance._RoleMotion._HP);
            GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_MONSTER_SKILL_FINISH, this, null);
            return;
        }
        StartCoroutine(MonsterSkillShow(damageResult));
    }

    public IEnumerator MonsterSkillShow(List<DamageResult> damageResults)
    {
        ClearDamageShow();
        foreach (var damageResult in damageResults)
        {
            var monInfo = _MonsterShowDict[damageResult._TargetMotion];
            float animTime = monInfo._MonsterModel.PlayAttack();

            yield return new WaitForSeconds(0.2f);
            if (damageResult._UseSkill is SkillDamage)
            {
                PlayerUISound(_ElimitAttackSound, 1);
            }
            else if (damageResult._UseSkill != null)
            {
                PlayerUISound(_ElimitSkillSound, 1);
            }
            yield return new WaitForSeconds(animTime - 0.2f);

            _DmgTextNormal.transform.position = _HPBar.transform.position;
            _DmgTextNormal.transform.localPosition += new Vector3(0, 10, 0);
            _DmgTextNormal.gameObject.SetActive(true);
            _DmgTextNormal.text = "-" + damageResult.DamageValue;

            UpdateSelfHPBar(damageResult._AfterHP);

            if (damageResult._SkillBallsResult != null)
            {
                UIFightBox.ShowMonsterBalls(damageResult._SkillBallsResult);
            }

            yield return new WaitForSeconds(0.3f);
            ClearDamageShow();

            if (damageResult._AfterHP <= 0 && BattleField.Instance._RoleMotion.IsDied)
            {
                UIStageFail.ShowAsyn();
            }

            yield return new WaitForSeconds(0.2f);
        }
        RefreshMonsterCD();

        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_MONSTER_SKILL_FINISH, this, null);
        UIFightBox.HideOptMask();
        RefreshOptRound();
    }

    public void RefreshMonsterCD()
    {
        foreach (var monsterShow in _MonsterShowDict.Values)
        {
            monsterShow._UIHPItem.RefreshCD();
        }
    }

#endregion

#region interface

    public void OnShowSystemSetting()
    {
        UISystemSetting.ShowAsyn();
    }

    #endregion

    #region sound

    public AudioClip _ElimitAttackSound;
    public AudioClip _ElimitSkillSound;
    public AudioClip _DiedSound;
    public AudioClip _BornSound;
    public AudioClip[] _PlayerAttackSounds;

    #endregion
}
