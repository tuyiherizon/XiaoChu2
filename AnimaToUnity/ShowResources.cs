using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowResources : MonoBehaviour
{

    void Start()
    {
        string filePath = "E:\\resource\\usableRes\\Total\\傲世之剑-45Anim卡通\\img\\actor\\fashi_cg01.plist";
        StartCoroutine(ShowResource(filePath));
    }

    IEnumerator ShowResource(string fileName)
    {
        var filePath = "file://" + fileName;

        WWW wwwTexture = new WWW(filePath);

        yield return wwwTexture;

        Debug.Log(wwwTexture.text);
    }
}
