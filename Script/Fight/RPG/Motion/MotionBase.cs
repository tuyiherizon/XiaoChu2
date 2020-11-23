using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tables;

public enum DAMAGE_TYPE
{
    Normal,
    Double,
    Half
}

public class DamageResult
{
    public ELEMENT_TYPE _AtkType;
    public int DamageValue = 0;
    public MotionBase _TargetMotion;
    public int _BeforeHP;
    public int _AfterHP;
    public DAMAGE_TYPE _DamageType;
    public SkillBase _UseSkill;
    public List<BallInfo> _SkillBallsResult;
}

public class MotionBase
{
    #region base attr

    public int _Attack;

    public int _Defence;

    public int _HP;

    public ELEMENT_TYPE _ElementType;

    public int _Diff;

    public int _MaxHP;

    public bool IsDied
    {
        get
        {
            return _HP <= 0;
        }
    }

    public int AddHP(int value)
    {
        int orgValue = _HP;
        _HP += value;
        _HP = Mathf.Clamp(_HP, 0, _MaxHP);
        return _HP - orgValue;
    }

    public int AddHPPresent(float value)
    {
        int addValue = (int)(value * _MaxHP);
        return AddHP(addValue);
    }

    public void SetHPPresent(float hpPresent)
    {
        _HP = (int)(_MaxHP * hpPresent);
    }

    #endregion

    public MonsterBaseRecord _MonsterRecord;
    public List<SkillBase> _Skills;

    public void InitMonster(MonsterBaseRecord monster, int level)
    {
        _MonsterRecord = monster;

        _Attack = (GameDataValue.GetLevelHP(level)) * monster.Attack / 10;
        _Attack = Mathf.Max(_Attack, 1);

        float hprate = GameDataValue.GetMonsterHPRate(monster.HP);
        _HP = (int)(GameDataValue.GetLevelAtk(level) * BattleField.GetBallNumRate(12) * hprate * GameDataValue.GetMonsterHPGemFix(level));
        _MaxHP = _HP;
        _ElementType = monster.ElementType;
        //_Diff = LogicManager.Instance.EnterStageInfo.Level;
        _Diff = 100;
        _Skills = new List<SkillBase>();
        foreach (var skillrecord in monster.Skills)
        {
            if (skillrecord == null)
                continue;

            var skillBase = SkillBase.GetSkillInstance(skillrecord);
            if (skillBase != null)
            {
                skillBase.InitSkill(this, skillrecord);
                _Skills.Add(skillBase);
            }
        }
    }

    public DamageResult MonsterRoundAct()
    {
        DamageResult dmgResult = new DamageResult();
        dmgResult._TargetMotion = this;
        dmgResult._BeforeHP = BattleField.Instance._RoleMotion._HP;

        foreach (var skill in _Skills)
        {
            if (skill.IsCanUseSkill())
            {
                skill.UseSkill(BattleField.Instance._RoleMotion, ref dmgResult);
                dmgResult._UseSkill = skill;
                dmgResult._AfterHP = BattleField.Instance._RoleMotion._HP;
                break;
            }
        }

        if (dmgResult._UseSkill == null)
            return null;

        return dmgResult;
    }

    public DamageResult CastDamage(ELEMENT_TYPE elimitElement, int elimitCount)
    {
        DamageResult result = new DamageResult();
        result._TargetMotion = this;
        result._BeforeHP = _HP;

        int damage = 0;
        if (_MonsterRecord != null)
        {
            var elementRelation = BattleField.GetRoleAtkRelation(elimitElement, _MonsterRecord.ElementType);
            float relation = BattleField.GetRoleAtkRelationRate(elementRelation);
            float dmgRate = BattleField.GetRoleAtkElementRate(elimitElement);
            float numRate = BattleField.GetBallNumRate(elimitCount);

            damage = (int)(BattleField.Instance._RoleMotion._Attack * numRate * dmgRate * relation);

            result._DamageType = DAMAGE_TYPE.Normal;
            if (relation > 1)
            {
                result._DamageType = DAMAGE_TYPE.Double;
            }
            else if (relation < 1)
            {
                result._DamageType = DAMAGE_TYPE.Half;
            }
        }

        result.DamageValue = damage;
        _HP -= damage;

        result._AfterHP = _HP;

        return result;
    }

    public DamageResult CastDamage(MotionBase monster)
    {
        DamageResult result = new DamageResult();
        result._TargetMotion = this;
        result._BeforeHP = _HP;

        int damage = 0;
        if (monster._MonsterRecord != null)
        {
            var elementRelation = BattleField.GetRoleDefRelation(monster._MonsterRecord.ElementType);
            float relation = BattleField.GetRelationRate(elementRelation);

            damage = (int)(monster._Attack * relation);

            result._DamageType = DAMAGE_TYPE.Normal;
            if (relation >  1)
            {
                result._DamageType = DAMAGE_TYPE.Double;
            }
            else if (relation < 1)
            {
                result._DamageType = DAMAGE_TYPE.Half;
            }
        }

        result.DamageValue = damage;
        _HP -= damage;

        result._AfterHP = _HP;

        return result;
    }
}
