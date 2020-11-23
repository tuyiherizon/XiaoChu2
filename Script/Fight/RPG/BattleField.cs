using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tables;



public class BattleField
{
    public enum ElementRelation
    {
        None,
        Restraint,
        BeRestraint,
        MutualRestraint
    }

    #region static

    private static BattleField _Instance;

    public static BattleField Instance
    {
        get
        {
            if (_Instance == null)
                _Instance = new BattleField();

            return _Instance;
        }
    }

    public static ELEMENT_TYPE BallTypeToElementType(BallType ballType)
    {
        switch (ballType)
        {
            case BallType.Color1:
                return ELEMENT_TYPE.FIRE;
            case BallType.Color2:
                return ELEMENT_TYPE.ICE;
            case BallType.Color3:
                return ELEMENT_TYPE.WIND;
            case BallType.Color4:
                return ELEMENT_TYPE.LIGHT;
            case BallType.Color5:
                return ELEMENT_TYPE.DARK;
        }

        return ELEMENT_TYPE.NONE;
    }

    public static BallType ElementTypeToBattleType(ELEMENT_TYPE elementType)
    {
        switch (elementType)
        {
            case ELEMENT_TYPE.FIRE:
                return BallType.Color1;
            case ELEMENT_TYPE.ICE:
                return BallType.Color2;
            case ELEMENT_TYPE.WIND:
                return BallType.Color3;
            case ELEMENT_TYPE.LIGHT:
                return BallType.Color4;
            case ELEMENT_TYPE.DARK:
                return BallType.Color5;
        }

        return BallType.None;
    }

    public static BallType ElementTypeToTrapType(ELEMENT_TYPE elementType)
    {
        switch (elementType)
        {
            case ELEMENT_TYPE.FIRE:
                return BallType.Clod;
            case ELEMENT_TYPE.ICE:
                return BallType.Ice;
            case ELEMENT_TYPE.WIND:
                return BallType.Posion;
            case ELEMENT_TYPE.LIGHT:
                return BallType.Stone;
            case ELEMENT_TYPE.DARK:
                int random = Random.Range(11, 15);
                return (BallType)random;
        }

        return BallType.None;
    }

    public static ElementRelation GetElementRelation(ELEMENT_TYPE elementAtk, ELEMENT_TYPE elementDef)
    {
        ElementRelation eleRelation = BattleField.ElementRelation.None;
        switch (elementAtk)
        {
            case ELEMENT_TYPE.FIRE:
                if (elementDef == ELEMENT_TYPE.ICE)
                {
                    eleRelation = ElementRelation.BeRestraint;
                }
                if (elementDef == ELEMENT_TYPE.WIND)
                {
                    eleRelation = ElementRelation.Restraint;
                }
                break;
            case ELEMENT_TYPE.ICE:
                if (elementDef == ELEMENT_TYPE.WIND)
                {
                    eleRelation = ElementRelation.BeRestraint;
                }
                if (elementDef == ELEMENT_TYPE.FIRE)
                {
                    eleRelation = ElementRelation.Restraint;
                }
                break;
            case ELEMENT_TYPE.WIND:
                if (elementDef == ELEMENT_TYPE.FIRE)
                {
                    eleRelation = ElementRelation.BeRestraint;
                }
                if (elementDef == ELEMENT_TYPE.ICE)
                {
                    eleRelation = ElementRelation.Restraint;
                }
                break;
            case ELEMENT_TYPE.LIGHT:
                if (elementDef == ELEMENT_TYPE.DARK)
                {
                    eleRelation = ElementRelation.MutualRestraint;
                }
                break;
            case ELEMENT_TYPE.DARK:
                if (elementDef == ELEMENT_TYPE.LIGHT)
                {
                    eleRelation = ElementRelation.MutualRestraint;
                }
                break;
        }

        return eleRelation;
    }

    public static ElementRelation GetRoleAtkRelation(ELEMENT_TYPE elementAtk, ELEMENT_TYPE elementDef)
    {
        var exAttrScript = "";
        if (GemDataPack.Instance.SelectedGemItem.GemExAttrRecord != null)
        {
            exAttrScript = GemDataPack.Instance.SelectedGemItem.GemExAttrRecord.Script;
        }

        ElementRelation eleRelation = ElementRelation.None;
        if (exAttrScript.Equals("FireEnhance"))
        {
            if (elementAtk == ELEMENT_TYPE.FIRE && elementDef == ELEMENT_TYPE.ICE)
            {
                eleRelation = ElementRelation.None;
                return eleRelation;
            }
        }
        else if (exAttrScript.Equals("IceEnhance"))
        {
            if (elementAtk == ELEMENT_TYPE.ICE && elementDef == ELEMENT_TYPE.WIND)
            {
                eleRelation = ElementRelation.None;
                return eleRelation;
            }
        }
        else if (exAttrScript.Equals("WindEnhance"))
        {
            if (elementAtk == ELEMENT_TYPE.WIND && elementDef == ELEMENT_TYPE.FIRE)
            {
                eleRelation = ElementRelation.None;
                return eleRelation;
            }
        }
        else if (exAttrScript.Equals("FireDouble"))
        {
            if (elementAtk == ELEMENT_TYPE.FIRE)
            {
                if (elementDef == ELEMENT_TYPE.FIRE)
                    eleRelation = ElementRelation.Restraint;
            }
        }
        else if (exAttrScript.Equals("IceDouble"))
        {
            if (elementAtk == ELEMENT_TYPE.ICE)
            {
                if (elementDef == ELEMENT_TYPE.ICE)
                    eleRelation = ElementRelation.Restraint;
            }
        }
        else if (exAttrScript.Equals("WindDouble"))
        {
            if (elementAtk == ELEMENT_TYPE.WIND)
            {
                if (elementDef == ELEMENT_TYPE.WIND)
                    eleRelation = ElementRelation.Restraint;
            }
        }
        else if (exAttrScript.Equals("LightDouble"))
        {
            if (elementAtk == ELEMENT_TYPE.LIGHT)
            {
                if (elementDef == ELEMENT_TYPE.LIGHT)
                    eleRelation = ElementRelation.Restraint;
            }
        }
        else if (exAttrScript.Equals("DarkDouble"))
        {
            if (elementAtk == ELEMENT_TYPE.DARK)
            {
                if (elementDef == ELEMENT_TYPE.DARK)
                    eleRelation = ElementRelation.Restraint;
            }
        }

        if (eleRelation == ElementRelation.None)
        {
            eleRelation = GetElementRelation(elementAtk, elementDef);
        }

        return eleRelation;
    }

    public static ElementRelation GetRoleDefRelation(ELEMENT_TYPE elementAtk)
    {
        var exAttrScript = "";
        if (GemDataPack.Instance.SelectedGemItem.GemExAttrRecord != null)
        {
            exAttrScript = GemDataPack.Instance.SelectedGemItem.GemExAttrRecord.Script;
        }

        ElementRelation eleRelation = ElementRelation.None;
        if (exAttrScript.Equals("FireDefence"))
        {
            if (elementAtk == ELEMENT_TYPE.FIRE)
            {
                eleRelation = ElementRelation.BeRestraint;
                return eleRelation;
            }
        }
        else if (exAttrScript.Equals("IceDefence"))
        {
            if (elementAtk == ELEMENT_TYPE.ICE)
            {
                eleRelation = ElementRelation.BeRestraint;
                return eleRelation;
            }
        }
        else if (exAttrScript.Equals("WindDefence"))
        {
            if (elementAtk == ELEMENT_TYPE.WIND)
            {
                eleRelation = ElementRelation.BeRestraint;
                return eleRelation;
            }
        }
        else if (exAttrScript.Equals("LightDefence"))
        {
            if (elementAtk == ELEMENT_TYPE.LIGHT)
            {
                eleRelation = ElementRelation.BeRestraint;
                return eleRelation;
            }
        }
        else if (exAttrScript.Equals("DarkDefence"))
        {
            if (elementAtk == ELEMENT_TYPE.DARK)
            {
                eleRelation = ElementRelation.BeRestraint;
                return eleRelation;
            }
        }

        return ElementRelation.None;
    }

    public static float GetRelationRate(ElementRelation eleRelation)
    {
        switch (eleRelation)
        {
            case ElementRelation.None:
                return 1;
            case ElementRelation.Restraint:
                return 2;
            case ElementRelation.BeRestraint:
                return 0.5f;
            case ElementRelation.MutualRestraint:
                return 2;
            default:
                return 1;
        }
    }

    public static float GetRoleAtkRelationRate(ElementRelation eleRelation)
    {
        var exAttrScript = "";
        if (GemDataPack.Instance.SelectedGemItem.GemExAttrRecord != null)
        {
            exAttrScript = GemDataPack.Instance.SelectedGemItem.GemExAttrRecord.Script;
        }

        if (exAttrScript.Equals("DoubleEnhance"))
        {
            if (eleRelation == ElementRelation.Restraint || eleRelation == ElementRelation.MutualRestraint)
                return 2.5f;
        }
        else if (exAttrScript.Equals("HalfEnhance"))
        {
            if (eleRelation == ElementRelation.BeRestraint)
                return 0.75f;
        }

        return GetRelationRate(eleRelation);
    }

    public static float GetRoleAtkElementRate(ELEMENT_TYPE eleType)
    {
        GemRecord gemRecord = null;
        if (GemDataPack.Instance.SelectedGemItem.GemRecord != null)
        {
            gemRecord = GemDataPack.Instance.SelectedGemItem.GemRecord;
        }

        if (gemRecord == null)
            return 1;

        int baseValue = 10000;
        switch (eleType)
        {
            case ELEMENT_TYPE.FIRE:
                baseValue = gemRecord.Attrs[0];
                break;
            case ELEMENT_TYPE.ICE:
                baseValue = gemRecord.Attrs[1];
                break;
            case ELEMENT_TYPE.WIND:
                baseValue = gemRecord.Attrs[2];
                break;
            case ELEMENT_TYPE.LIGHT:
                baseValue = gemRecord.Attrs[3];
                break;
            case ELEMENT_TYPE.DARK:
                baseValue = gemRecord.Attrs[4];
                break;
        }



        return GameDataValue.ConfigIntToFloat(baseValue);
    }

    public static float GetBallNumRate(int num)
    {
        float baseValue = (float)num / 3;
        float remainder = num % 3;

        return baseValue + remainder * 0.3f;
    }

    private BattleField()
    {
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_CAST_DAMAGE_FINISH, CastDamageFinishHandler);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_MONSTER_SKILL_FINISH, MonsterSkillFinishHandler);
    }


    #endregion

    #region 

    private StageInfoRecord _StageRecord;
    private StageMapRecord _StageMapRecord;

    public MotionBase _RoleMotion;

    private StageLogic _StageLogic;

    public StageLogic StageLogic
    {
        get
        {
            return _StageLogic;
        }
    }

    public bool _IsWinBattle = false;

    public void InitBattle(StageInfoRecord stageRecord, StageMapRecord stageMapRecord)
    {
        _StageRecord = stageRecord;
        _StageMapRecord = stageMapRecord;
        _BattleRound = 1;

        _StageLogic = stageMapRecord._MapStageLogic;
        InitRole();
        _CurWave = -1;
        _CurOptRound = 0;
        StartNextWave();

        _IsWinBattle = false;

        LogicManager.Instance.EnterFightFinish();
        _AlreadyReviveTypes = new List<int>();
    }
    
    private void InitRole()
    {
        _RoleMotion = new MotionBase();

        var attr = WeaponDataPack.Instance.SelectedWeaponItem.GetCurLevelAttrs();
        //_RoleMotion._Attack = (int)attr[0];
        //_RoleMotion._MaxHP = (int)attr[1];

        int stageLevel = int.Parse(_StageRecord.Id);
        int testLevel = stageLevel;
        int attrValue = Mathf.Max(testLevel, 1);
        _RoleMotion._Attack = GameDataValue.GetLevelAtk(attrValue);
        _RoleMotion._MaxHP = GameDataValue.GetLevelHP(attrValue);

        _RoleMotion._HP = _RoleMotion._MaxHP;
        _RoleMotion._ElementType = ELEMENT_TYPE.NONE;


    }

    private void BattleSucess()
    {
        _IsWinBattle = true;
        StageDataPack.Instance.PassStage(_StageMapRecord);
        Debug.Log("BattleSucess");
    }

    private void BattleFail()
    {
        UIStageFail.ShowAsyn();
        Debug.Log("BattleFail");
    }

    public void CastDamageFinishHandler(object sender, Hashtable hash)
    {
        var skillResults = RoundMonsterAct();
        Hashtable skillHash = new Hashtable();
        skillHash.Add("SkillResult", skillResults);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_MONSTER_SKILL, this, skillHash);
    }

    public void MonsterSkillFinishHandler(object sender, Hashtable hash)
    {
        RoundEnd();
    }


    #endregion

    #region revive

    public List<int> _AlreadyReviveTypes = new List<int>();

    public void RoleRelive(int type)
    {
        if (_AlreadyReviveTypes.Contains(type))
            return;

        if (type == 1)
        {
            AdManager.Instance.WatchAdVideo(() => 
            {
                _RoleMotion.SetHPPresent(0.5f);
                _AlreadyReviveTypes.Add(type);
            });
            
        }
        else if (type == 2)
        {
            int costGold = GetReviveCost();
            if (PlayerDataPack.Instance.DecMoney(PlayerDataPack.MoneyGold, costGold))
            {
                _RoleMotion.SetHPPresent(1.0f);
                _AlreadyReviveTypes.Add(type);
            }
        }
    }

    public int GetReviveCost()
    {
        int stageLevel = _StageRecord.Level;
        int costValue = Tables.GameDataValue.GetLevelDataValue(stageLevel, VALUE_IDX.STAGE_GOLD) * 4;
        return costValue;
    }

    #endregion

    #region enemy

    public int _CurWave = -1;

    public List<MotionBase> _Monster;
    public List<MotionBase> _DiedMonster = new List<MotionBase>();

    private void StartNextWave()
    {
        ++_CurWave;

        if (_StageLogic._Waves.Count <= _CurWave)
        {
            BattleSucess();
            return;
        }

        _Monster = new List<MotionBase>();
        foreach (var monsterID in _StageLogic._Waves[_CurWave].NPCs)
        {
            var monsterMotion = GetMonsterMotion(monsterID);
            if (monsterMotion == null)
            {
                Debug.LogError("Create monster motion error:" + monsterID);
            }
            _Monster.Add(monsterMotion);
        }

        Hashtable hash = new Hashtable();
        hash.Add("Monsters", _Monster);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_START_WAVE, this, hash);
        //UIFightBattleField.InitMonstersStatic(_Monster);

        RoundStart();
    }

    public MotionBase GetMonsterMotion(string monsterID)
    {
        Debug.Log("GetMonsterMotion:" + monsterID);
        MotionBase motionBase = new MotionBase();
        motionBase.InitMonster(TableReader.MonsterBase.GetRecord(monsterID), _StageRecord.Level);
        return motionBase;
    }

    #endregion

    #region calculate

    public int _BattleRound = 0;

    public Dictionary<BallType, int> _RoundDamageBalls = new Dictionary<BallType, int>();
    public const int _DamageOptRound = 2;
    private int _CurOptRound = 0;
    public int CurOptRound
    {
        get
        {
            return _CurOptRound;
        }
    }

    public const int _DamageMinBallCnt = 3;

    public void RoundStart()
    {

    }

    public List<DamageResult> RoundAct()
    {
        int totalDamage = 0;
        List<DamageResult> damageResult = new List<DamageResult>();
        for (int i = (int)BallType.Color1; i <= (int)BallType.Color5; ++i)
        {
            if (_Monster.Count == 0)
            {
                continue;
            }

            BallType actType = (BallType)i;
            if (!_RoundDamageBalls.ContainsKey(actType))
                continue;

            var elimitBall = _RoundDamageBalls[actType];

            if (elimitBall >= _DamageMinBallCnt)
            {
                totalDamage += elimitBall;
                var result = _Monster[0].CastDamage(BallTypeToElementType(actType), elimitBall);

                result._AtkType = BallTypeToElementType(actType);

                if (_Monster[0].IsDied)
                {

                    MotionDie(_Monster[0]);
                }
                damageResult.Add(result);
            }


        }

        //RecordBallDamage.RecordDamage(totalDamage);

        return damageResult;
    }

    public List<DamageResult> RoundMonsterAct()
    {
        List<DamageResult> damageResults = new List<DamageResult>();
        if (_Monster.Count > 0)
        {
            foreach (var monster in _Monster)
            {
                var result = monster.MonsterRoundAct();
                if (result != null)
                {
                    damageResults.Add(result);
                }
            }
        }

        return damageResults;
    }

    public void RoundEnd()
    {   
        ++_BattleRound;
        _RoundDamageBalls.Clear();

        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_ROUND_END, this, null);

        if (_Monster.Count == 0)
        {
            StartNextWave();
        }
    }

    public void BallDamage(Dictionary<BallType, int> elimitBalls)
    {
        ++_CurOptRound;

        foreach (var elimitBall in elimitBalls)
        {
            if (!_RoundDamageBalls.ContainsKey(elimitBall.Key))
            {
                _RoundDamageBalls.Add(elimitBall.Key, 0);
            }

            _RoundDamageBalls[elimitBall.Key] += elimitBall.Value;
        }

        if (_CurOptRound == _DamageOptRound)
        {
            var damageResult = RoundAct();
            //UIFightBattleField.PlayDamageAnim(damageResult);
            Hashtable hash = new Hashtable();
            hash.Add("DamageResult", damageResult);
            GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_CAST_DAMAGE, this, hash);
            _CurOptRound = 0;
        }

        UIFightBattleField.ShowRefreshOptRound();
    }

    public void MotionDie(MotionBase motion)
    {
        _DiedMonster.Add(motion);
        _Monster.Remove(motion);
    }

    #endregion
}
