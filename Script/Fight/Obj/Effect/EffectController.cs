using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectController : MonoBehaviour
{
    
    #region Binded Effect

    protected float _LastPlaySpeed = 1;
    public float LastPlaySpeed
    {
        get
        {
            return _LastPlaySpeed;
        }
    }
    private ParticleSystem[] _Particles;

    public ParticleSystem[] Particles
    {
        get
        {
            if (_Particles == null || _Particles.Length == 0)
                _Particles = gameObject.GetComponentsInChildren<ParticleSystem>(true);

            return _Particles;
        }
    }

    public virtual void PlayEffect()
    {
        _LastPlaySpeed = 1;
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        gameObject.SetActive(true);
    }

    public virtual void PlayEffect(float speed)
    {
        if (speed != _LastPlaySpeed)
        {
            _LastPlaySpeed = speed;

            foreach (var particle in Particles)
            {
                var particleMain = particle.main;
                particleMain.simulationSpeed = speed;
            }
        }
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        gameObject.SetActive(true);
    }

    public virtual void SetEffectSpeed(float speed)
    {
        if (speed != _LastPlaySpeed)
        {
            _LastPlaySpeed = speed;

            foreach (var particle in Particles)
            {
                var particleMain = particle.main;
                particleMain.simulationSpeed = speed;
            }
        }
    }

    public virtual void PlayEffect(Hashtable hash)
    {
        PlayEffect();
    }

    public virtual void HideEffect()
    {
        gameObject.SetActive(false);
    }

    public virtual void PauseEffect()
    {
        if (!gameObject.activeSelf)
            return;

        foreach (var particle in Particles)
        {
            particle.Pause();
        }
        Debug.Log("PauseEffect");
    }

    public virtual void ResumeEffect()
    {
        if (!gameObject.activeSelf)
            return;

        foreach (var particle in Particles)
        {
            particle.Play();
        }
        Debug.Log("ResumeEffect");
    }

    #endregion

    #region no Instance Effect

    public string _BindPos;
    public float _EffectLastTime;

    #endregion

    #region modify

    public float _EffectSizeRate = 1;

    public void SetEffectSize(float sizeRate)
    {
        foreach (var particle in Particles)
        {
            particle.startSize *= sizeRate;
        }
    }

    #endregion
    
}
