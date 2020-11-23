using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIBase : MonoBehaviour
{
    public string UIPath;
    public UILayer UILayer;
    public Text UINameText;
    public int NameID;

    #region fiex fun

    public void Awake()
    {
        Init();
    }
    public virtual void Init()
    {
        if (UINameText != null)
        {
            UINameText.text = Tables.StrDictionary.GetFormatStr(NameID);
        }
        InitButtonSound();
    }

    #endregion

    #region show

    public virtual void PreLoad()
    {
        gameObject.SetActive(false);
    }

    public virtual void Show()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
    }

    public virtual void Show(Hashtable hash)
    {
        Show();
    }

    public virtual void Hide()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    public virtual void ShowDelay(float time)
    {
        Hide();
        Invoke("Show", time);
    }

    public virtual void ShowLast(float time)
    {
        Show();
        Invoke("Hide", time);
    }

    public virtual void Destory()
    {
        UIManager.Instance.DestoryUI(UIPath);
    }

    #endregion

    #region sound

    public void InitButtonSound()
    {
        Button[] btns = GetComponentsInChildren<Button>();
        for (int i = 0; i < btns.Length; i++)
        {
            Button btnChild = btns[i];
            btnChild.onClick.RemoveListener(OnBtnClickSound);
            btnChild.onClick.AddListener(OnBtnClickSound);
        }
    }

    public virtual void OnBtnClickSound()
    {
        PlayerUISound(GameCore.Instance._SoundManager._BtnClickAudio);
    }

    public virtual void PlayerUISound(AudioClip logicAudio, float volumn = 0.5f)
    {
        UIManager.Instance.AndioSource.clip = (logicAudio);
        UIManager.Instance.AndioSource.volume = volumn;
        UIManager.Instance.AndioSource.loop = false;
        UIManager.Instance.AndioSource.Play();
    }


    #endregion

    #region static

    public static void SetGOActive(Component actGO, bool isActive)
    {
        if (actGO == null)
            return;
 
        actGO.gameObject.SetActive(isActive);
    }

    public static void SetGOActive(GameObject actGO, bool isActive)
    {
        if (actGO == null)
            return;

        actGO.SetActive(isActive);
    }

    #endregion
}

