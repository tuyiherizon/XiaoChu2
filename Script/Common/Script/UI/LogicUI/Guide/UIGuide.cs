using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tables;

public class UIGuide : UIBase
{
    #region static funs

    public static void ShowPoint(Transform targetTran)
    {
        Hashtable hash = new Hashtable();
        hash.Add("Target", new List<Transform>() { targetTran });
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIGuide, UILayer.TopUI, hash);
    }

    public static void ShowPoint(List<Transform> targetTrans)
    {
        Hashtable hash = new Hashtable();
        hash.Add("Target", targetTrans);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIGuide, UILayer.TopUI, hash);
    }

    public static void HideGuide()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIGuide>(UIConfig.UIGuide);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.Hide();
    }

    public static bool IsShowingGuide()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIGuide>(UIConfig.UIGuide);
        if (instance == null)
            return false;

        if (!instance.isActiveAndEnabled)
            return false;

        return true;
    }

    #endregion

    #region 

    void OnDisable()
    {
        StopAllCoroutines();
    }

    void Update()
    {
        if (_TargetsList.Count == 1)
        {
            _Finger.transform.position = _TargetsList[0].position;
        }
    }

    #endregion

    public GameObject _Finger;
    public float _FingerMoveDelta = 1.5f;

    private List<Transform> _TargetsList;
    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _TargetsList = (List<Transform>)hash["Target"];
        _Finger.transform.position = _TargetsList[0].position;

        if (_TargetsList.Count > 1)
        {
            StartCoroutine(MoveFingerUpdate());
        }
    }

    private IEnumerator MoveFingerUpdate()
    {
        while (true)
        {
            _Finger.transform.position = _TargetsList[0].position;
            yield return new WaitForSeconds(0.4f);
            iTween.MoveTo(_Finger, _TargetsList[1].position, 1.5f);
            yield return new WaitForSeconds(1.6f);
            _Finger.transform.position = _TargetsList[1].position;
            yield return new WaitForSeconds(0.1f);

        }
    }
}
