using DragonBones;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEffectLighting : EffectController
{

    public GameObject _LightingGO;

    private List<GameObject> _LightingGOList = new List<GameObject>();
    private List<GameObject> _IdleLighting = new List<GameObject>();

    private GameObject GetIdleLightingGO()
    {
        if (_IdleLighting.Count > 0)
        {
            var idleGO = _IdleLighting[0];
            _IdleLighting.Remove(idleGO);
            idleGO.SetActive(true);
            return idleGO;
        }

        var effectGO = GameObject.Instantiate(_LightingGO);
        _LightingGOList.Add(effectGO);
        effectGO.SetActive(true);
        return effectGO;
    }

    public void StartEffect(BallType ballType, BallInfo baseBall, List<BallInfo> elimitBalls)
    {
        var uiBaseBall = UIFightBox.GetFightBall(baseBall);
        _IdleLighting = new List<GameObject>(_LightingGOList);

        foreach (var lightingGO in _LightingGOList)
        {
            lightingGO.gameObject.SetActive(false);
        }

        foreach (var elimitBall in elimitBalls)
        {
            if (elimitBall == baseBall)
                continue;

            var effectGO = GetIdleLightingGO();

            effectGO.transform.SetParent(transform);
            var uiElimitBall = UIFightBox.GetFightBall(elimitBall);
            Vector3 direct = (uiElimitBall.transform.position - uiBaseBall.transform.position) * 0.5f;
            float ballDis = Vector3.Distance(uiBaseBall.transform.position, uiElimitBall.transform.position);
            float ballAngle = Vector3.Angle(direct, new Vector3(1,0,0));
            if (direct.y < 0)
            {
                ballAngle = 180 - ballAngle;
            }
            float length = ballDis / 0.77f * 12;
            effectGO.transform.localScale = new Vector3(length, 50, 0);
            effectGO.transform.rotation = Quaternion.Euler(new Vector3(0, 0, ballAngle));
            effectGO.transform.position = uiBaseBall.transform.position + direct;
        }

        StartCoroutine(EffectFinish());
    }

    private IEnumerator EffectFinish()
    {
        yield return new WaitForSeconds(0.25f);

        ResourcePool.Instance.RecvIldeEffect(this);
    }
}
