using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInfoSPRPGHP : BallInfoSPRPGBase
{

    public override bool IsCanExchange(BallInfo other)
    {
        return true;
    }

    public override bool IsExchangeSpInfo(BallInfo other)
    {
        return true;
    }

    public override bool IsCanNormalElimit()
    {
        return false;
    }

    public override bool IsCanBeSPElimit(BallInfo other)
    {
        return true;
    }

    public override bool IsContentNormal()
    {
        return false;
    }

    public override bool IsCanFall()
    {
        return true;
    }

    public override bool IsCanMove()
    {
        return true;
    }

    public override bool IsCanPass()
    {
        return true;
    }

    public override bool IsExplore()
    {
        return false;
    }

    public override bool IsCanBeContentSP()
    {
        return true;
    }

    public override void OnSPElimit()
    {
        _BallInfo.OnNormalElimit();

        float addHPRate = GemDataPack.Instance.SelectedGemItem.GetHPBallRate();
        var hpValue = BattleField.Instance._RoleMotion.AddHPPresent(addHPRate);
        DamageResult dmgResult = new DamageResult();
        dmgResult.DamageValue = hpValue;
        dmgResult._AfterHP = BattleField.Instance._RoleMotion._HP;

        Hashtable hash = new Hashtable();
        hash.Add("SkillResult", dmgResult);

        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_ADD_ROLE_HP, this, hash);
    }

    public override List<BallInfo> CheckSPElimit()
    {
        return GetBombBalls();
    }

    public override List<BallInfo> CheckSPMove(List<BallInfo> checkBalls)
    {
        return GetBombBalls();
    }

    public override void SetParam(string[] param)
    {
        if (param.Length > 1)
        {
            _BallInfo.SetBallType((BallType)int.Parse(param[1]));
        }
    }
    
    protected override List<BallInfo> GetBombBalls()
    {
        return new List<BallInfo>() { BallInfo};
    }
}
