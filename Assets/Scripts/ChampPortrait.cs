using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChampPortrait : MonoBehaviour
{
    public bool isAssigned = false;
    Button button;
    Image portrait;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnChampPortraitClick);
    }

    public void OnChampPortraitClick()
    {
        if (isAssigned)
        {
            portrait = GetComponent<Image>();
            RandomPicker.Instance.selectedChamp.sprite = portrait.sprite;
            RandomPicker.Instance.selectedPortrait = portrait;
            RandomPicker.Instance.effector.StopFadeEffect(RandomPicker.Instance.selectedChamp);
            RandomPicker.Instance.selectedChamp.color = new Color(1f, 1f, 1f, 1f);
            RandomPicker.Instance.effector.FadeEffect(RandomPicker.Instance.selectedChamp);
        }
    }
}
