using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class UIGrayImage
{
    private static Material _GrayMat;

    public static void InitGrayMat()
    {
        if (_GrayMat != null)
            return;

        _GrayMat = Resources.Load<Material>("Effect/UI/ImgGray");
    }

    public static void SetImageGray(Image image, bool isGray)
    {
        if (isGray)
        {
            InitGrayMat();
            image.material = _GrayMat;
        }
        else
        {
            image.material = null;
        }
    }
}

