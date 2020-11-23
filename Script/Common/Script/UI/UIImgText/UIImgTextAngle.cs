using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UIImgTextAngle : UIImgText
{
    [SerializeField]
    public string textAngle
    {
        get
        {
            return _text;
        }
        set
        {
            _text = value;
            ShowImage(_text);
        }
    }

    protected override void InitCharRoot()
    {
        if (_CharRoot != null)
            return;

        _CharRoot = transform.Find("CharRoot");
        if (_CharRoot != null)
            return;

        var charRoot = new GameObject("CharRoot");
        
        _CharRoot = charRoot.transform;
        _CharRoot.SetParent(transform);
        _CharRoot.localPosition = Vector3.zero;
        _CharRoot.localRotation = Quaternion.Euler(Vector3.zero);
        _CharRoot.localScale = Vector3.one;
    }

    #region

    public float _PosXDelta = 0;
    public float _PosYDelta = 0;

    protected override void ShowImage(string text)
    {
        ClearImage();
        _ImgFont.InitChars();
        
        for (int i = 0; i < text.Length; ++i)
        {
           

            var image = PopIdleImage();
            if (!_ImgFont._DictImgChars.ContainsKey(text[i]))
            {
                Debug.LogError("No Img Char:" + text[i]);
                continue;
            }
            var charImg = _ImgFont._DictImgChars[text[i]];
            if (_PosXDelta == 0)
            {
                _PosXDelta = charImg._CharWidth;
            }
            image.sprite = charImg._Image;
            image.rectTransform.sizeDelta = new Vector2(charImg._CharWidth, charImg._CharHeight);
            image.rectTransform.SetAsLastSibling();
            image.rectTransform.anchoredPosition = new Vector2(i * _PosXDelta, i * _PosYDelta);

            _CharImages.Add(image);
        }

        _CharRoot.GetComponent<RectTransform>().anchoredPosition = new Vector2(-(text.Length - 1) * 0.5f * _PosXDelta, -(text.Length - 1) * 0.5f * _PosYDelta);
    }
    #endregion
}
