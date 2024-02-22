using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgmController : MonoBehaviour
{
    public Sprite soundSprite;
    public Sprite muteSprite;
    Image icon;
    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        icon = GetComponent<Image>();
        audioSource.PlayDelayed(10f);
    }

    public void OnBgmBtnClick()
    {
        if (audioSource.mute)
        {
            icon.sprite = soundSprite;
            audioSource.mute = false;
        }
        else
        {
            icon.sprite = muteSprite;
            audioSource.mute = true;
        }
    }
}
