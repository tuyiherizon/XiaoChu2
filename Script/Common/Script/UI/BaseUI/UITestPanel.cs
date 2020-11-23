using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITestPanel : MonoBehaviour {

    void Awake()
    {
#if UNITY_EDITOR
        gameObject.SetActive(true);
#else
        gameObject.SetActive(false);
#endif
    }
}
