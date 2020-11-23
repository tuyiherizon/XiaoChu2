using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISortMaterials : MonoBehaviour
{
    public RectTransform _AnchorTransform;
    public List<RectTransform> _MatTrans;
    public float length;

    void Start()
    {
        SetMatPos();
    }

    public void SetMatPos()
    {
        float singleAngle = (float)360 / _MatTrans.Count;

        for (int i = 0; i < _MatTrans.Count; ++i)
        {
            Vector2 gemPos = Vector2.zero;
            gemPos.x = Mathf.Sin((singleAngle * i) * Mathf.Deg2Rad) * length;
            gemPos.y = Mathf.Cos((singleAngle * i) * Mathf.Deg2Rad) * length;
            _MatTrans[i].anchoredPosition = gemPos;
        }
    }
}
