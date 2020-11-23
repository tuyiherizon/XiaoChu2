using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tables;

public class UIFuncTips : UIBase
{
 
    public static void ShowAsyn(int posIdx, string strTip)
    {
        Hashtable hash = new Hashtable();
        hash.Add("Message", strTip);
        hash.Add("PosIdx", posIdx);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIFuncTips, UILayer.MessageUI, hash);
    }

    #region 

    public Vector3[] _TipPoses;
    public RectTransform _AnchorTrans;
    public Text _Text;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        string message = hash["Message"] as string;
        int posIdx = (int)hash["PosIdx"];


        _Text.text = StrDictionary.GetFormatStr(message);
        _AnchorTrans.anchoredPosition = _TipPoses[posIdx];
    }



    #endregion
}
