using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBallInfo : MonoBehaviour
{
    #region show

    protected BallType _BallSPType;
    public BallType BallSPType
    {
        get
        {
            return _BallSPType;
        }
    }

    protected BallInfo _BallInfo;

    public virtual void SetBallSPType(BallInfo ballInfo, bool isInner = false)
    {
        if (ballInfo == null)
        {
            _BallSPType = BallType.None;
            return;
        }
        _BallInfo = ballInfo;
        _BallSPType = ballInfo.BallSPType;
        if (isInner)
        {
            _BallSPType = ballInfo.IncludeBallSPType;
        }
    }

    public virtual void SetBallIncludeSPType(BallInfo ballInfo)
    {
        if (ballInfo == null)
        {
            _BallSPType = BallType.None;
            return;
        }
        _BallInfo = ballInfo;
        _BallSPType = ballInfo.IncludeBallSPType;
    }

    public virtual void SetBallNormalType(BallInfo ballInfo)
    {
        if (ballInfo == null)
        {
            _BallSPType = BallType.None;
            return;
        }
        _BallSPType = ballInfo.BallType;
    }

    public virtual void ShowBallInfo(BallInfo ballInfo, bool isInner = false)
    { }

    public virtual void OnElimit()
    {
        //PlayBomb();
    }

    public virtual void OnClearInfo()
    {
        _BallSPType = BallType.None;
        _BallInfo = null;
    }

    protected void ShowLineEffect(List<BallInfo> ballInfos, bool isSmallLine)
    {
        var lineEffectGO = ResourcePool.Instance.GetIdleEffect(ResourcePool.LineEffectName);

        lineEffectGO.gameObject.SetActive(true);
        lineEffectGO.transform.localScale = Vector3.one;
        lineEffectGO.transform.position = transform.position;
        lineEffectGO.transform.rotation = Quaternion.Euler(Vector3.zero);

        var lineEffect = lineEffectGO as UIEffectLine;
        lineEffect.StartEffect(_BallSPType, _BallInfo, ballInfos, isSmallLine);
    }

    protected void ShowBombEffect(List<BallInfo> ballInfos, bool isSmallLine)
    {
        var effectGO = ResourcePool.Instance.GetIdleEffect(ResourcePool.BombEffectName);

        effectGO.transform.SetParent(UIManager.Instance.GetLayerTrans(UILayer.TopUI));
        effectGO.gameObject.SetActive(true);
        effectGO.transform.localScale = Vector3.one;
        effectGO.transform.position = transform.position;
        effectGO.transform.rotation = Quaternion.Euler(Vector3.zero);

        var effect = effectGO as UIEffectBomb;
        effect.StartEffect(_BallSPType);
    }

    protected void ShowLightingEffect(List<BallInfo> ballInfos, bool isSmallLine)
    {
        var effectGO = ResourcePool.Instance.GetIdleEffect(ResourcePool.LightEffectName);

        effectGO.transform.SetParent(UIManager.Instance.GetLayerTrans(UILayer.TopUI));
        effectGO.gameObject.SetActive(true);
        effectGO.transform.localScale = Vector3.one;
        effectGO.transform.position = transform.position;
        effectGO.transform.rotation = Quaternion.Euler(Vector3.zero);

        var effect = effectGO as UIEffectLighting;
        effect.StartEffect(_BallSPType, _BallInfo, ballInfos);
    }

    protected void ShowHPBombEffect()
    {
        var effectGO = ResourcePool.Instance.GetIdleEffect(ResourcePool.HPBombName);

        effectGO.transform.SetParent(UIManager.Instance.GetLayerTrans(UILayer.TopUI));
        effectGO.gameObject.SetActive(true);
        effectGO.transform.localScale = Vector3.one;
        effectGO.transform.position = transform.position;
        effectGO.transform.rotation = Quaternion.Euler(Vector3.zero);

        effectGO.PlayEffect();
    }
    #endregion

    #region sound

    public AudioSource _AudioSource;

    public AudioClip _MoveAudio;
    public AudioClip _BombAudio;

    private void FindAudioSource()
    {
        if (_AudioSource == null)
        {
            _AudioSource = GetComponentInParent<AudioSource>();
        }
    }

    public void PlayMove()
    {
        if (_MoveAudio == null)
            return;

        FindAudioSource();
        _AudioSource.clip = (_MoveAudio);
        _AudioSource.volume = 0.5f;
        _AudioSource.loop = false;
        _AudioSource.Play();
    }

    public void PlayBomb()
    {
        if (_BombAudio == null)
            return;

        FindAudioSource();
        _AudioSource.clip = (_BombAudio);
        _AudioSource.volume = 0.5f;
        _AudioSource.loop = false;
        _AudioSource.Play();
    }


    #endregion
}
