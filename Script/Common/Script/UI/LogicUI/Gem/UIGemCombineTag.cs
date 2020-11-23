using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tables;

public class UIGemCombineTag : UIItemSelect
{
    public Text _TagName;
   
    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        var level = (int)hash["InitObj"];
        _TagName.text = level.ToString();
    }
    
}
