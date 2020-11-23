using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class ResEffect : MonoBehaviour
{
    #region 

    public enum ANGLE_TYPE
    {
        ANGLE_2D,
        ANGLE_45A
    }
    public ANGLE_TYPE _AngleType = ANGLE_TYPE.ANGLE_45A;

    public enum ACT_TYPE
    {
        Bomb,
        Buff,
        Hit,
        Bullet,
        SelfBuff,
        UI,
        Scene,
        Skill,
        Gem,
        Other,
    }
    public ACT_TYPE _ActType;

    public enum ELEMENT_TYPE
    {
        Fire,
        Ice,
        Lighting,
        Posion,
        Light,
        Dark,
        Cloud,
        Earth,
        Wind,
        Other,
    }
    public ELEMENT_TYPE _ElementType;

    #endregion
    // Start is called before the first frame update

    void Start()
    {
        PlayAnim();
    }

    
    void OnEnable()
    {
        PlayAnim();

#if UNITY_EDITOR
        EditorApplication.update += AnimUpdate;
#endif
    }

    void OnDisable()
    {
#if UNITY_EDITOR
        EditorApplication.update -= AnimUpdate;
#endif
    }

    // Update is called once per frame
    void Update()
    {
        AnimUpdate();
    }

    #region anim effect

    [Serializable]
    public class EffectSpriteInfo
    {
        public Sprite _Sprites;
        public bool _IsRote;
    }

    [SerializeField]
    public List<EffectSpriteInfo> _Sprites;
    public SpriteRenderer _SpriteRender;
    public float _Interval = 0.05f;
    public int _PlayTimes = 1;

    private int _AlreadyPlayTimes = 0;
    private float _StartPlayTime;
    private int _CurFrameIdx = -1;
    private bool _IsPlayAnim = false;

    public int GetCurFrameIdx()
    {
        return _CurFrameIdx;
    }

    public void PlayAnim()
    {
        if (_Sprites.Count == 0)
            return;

        _StartPlayTime = Time.realtimeSinceStartup;
        _AlreadyPlayTimes = 0;
        _IsPlayAnim = true;
    }

    public void StopAnim()
    {
        _IsPlayAnim = false;
    }

    public void FinishOnece()
    {
        ++_AlreadyPlayTimes;
        if (_AlreadyPlayTimes == _PlayTimes)
        {
            StopAnim();
        }
    }

    public void AnimUpdate()
    {
        if (!_IsPlayAnim)
            return;

        float deltaFrame = Time.realtimeSinceStartup - _StartPlayTime;
        var frameIdx = (int)(deltaFrame / _Interval) % _Sprites.Count;
        if (_CurFrameIdx != frameIdx)
        {
            if (frameIdx == _Sprites.Count - 1)
            {
                FinishOnece();
            }
            ShowCurFrame(frameIdx);
            
        }
    }

    public void ShowCurFrame(int frameIdx)
    {
        _CurFrameIdx = frameIdx;
        _SpriteRender.sprite = _Sprites[_CurFrameIdx]._Sprites;
        if (_Sprites[_CurFrameIdx]._IsRote)
        {
            _SpriteRender.transform.localRotation = Quaternion.Euler(new Vector3(0,0,90));
        }
        else
        {
            _SpriteRender.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }

    #endregion
}
