using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFightMonsterHPItem : UIItemBase
{

    #region 

    public RectTransform _RectTransform;
    public UIImgText _AtkCD;
    public UIImgText _MagicCD;
    public Image _MagicImg;
    public Slider _HPBar;
    
    private float _MaxHP;
    private MotionBase _Monster;

    public void InitMonsterInfo(MotionBase monster)
    {
        _Monster = monster;

        ResourceManager.Instance.SetImage(_MagicImg, CommonDefine.GetElementIcon(monster._MonsterRecord.ElementType));
        RefreshCD();

        _MaxHP = monster._MaxHP;
        RefreshHP((float)monster._HP);

    }

    public void RefreshHP(float curHP)
    {
        _HPBar.value = (float)curHP / _MaxHP;
    }

    public void RefreshCD()
    {
        if (_Monster == null || _Monster.IsDied)
            return;

        if (_Monster._Skills.Count > 1)
        {
            if (_Monster._Skills[0].LastCDTime > _Monster._Skills[1].LastCDTime)
            {
                _AtkCD.text = _Monster._Skills[1].LastCDTime.ToString();
                _MagicCD.text = "";
            }
            else
            {
                _AtkCD.text = "";
                _MagicCD.text = _Monster._Skills[0].LastCDTime.ToString();
            }
        }
        else if (_Monster._Skills.Count > 0)
        {
            _AtkCD.text = _Monster._Skills[0].LastCDTime.ToString();
            _MagicCD.text = "";
        }
        else
        {
            _AtkCD.text = "";
            _MagicCD.text = "";
        }
    }

    #endregion
}
