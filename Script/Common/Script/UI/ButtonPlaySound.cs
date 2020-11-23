using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonPlaySound : MonoBehaviour {


    public AudioClip SoundSource;


    void Start () {

        //先添加这两种吧，看后面有没有需要增加的
        SetButtonSound();
        SetToggleSound();
    }
	
    private void SetToggleSound()
    {
        Toggle[] toggles = GetComponentsInChildren<Toggle>();
        for (int i = 0; i < toggles.Length; i++)
        {
            Toggle toggle = toggles[i];
            ButtonPlaySound ChildSound = toggle.gameObject.GetComponent<ButtonPlaySound>();
            if (ChildSound != null)
            {
                if (ChildSound.SoundSource != null)
                    continue;
            }

            for (int j = 0; j < toggle.onValueChanged.GetPersistentEventCount(); j++)
            {
                toggle.onValueChanged.SetPersistentListenerState(j, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
            }

            toggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                if (isOn)
                    PlaySound();
            });
        }
    }

    private void SetButtonSound()
    {
        Button[] btns = GetComponentsInChildren<Button>();
        for (int i = 0; i < btns.Length; i++)
        {
            Button btnChild = btns[i];
            ButtonPlaySound ChildSound = btnChild.gameObject.GetComponent<ButtonPlaySound>();
            if (ChildSound != null)
            {
                if (ChildSound.SoundSource != null)
                    continue;
            }

            for (int j = 0; j < btnChild.onClick.GetPersistentEventCount(); j++)
            {
                btnChild.onClick.SetPersistentListenerState(j, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
            }

            btnChild.onClick.AddListener(PlaySound);
        }
    }

	public void PlaySound()
    {
        
    }
}
