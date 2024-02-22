using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Effect : MonoBehaviour
{
    public Sequence colorSeq;
    void Start()
    {
        DOTween.Init();
    }

    public void PickEffect(GameObject target)
    {
        target.transform.DOScale(Vector3.one * 1.2f, 0.2f).OnComplete(()=>target.transform.DOScale(Vector3.one, 0.2f));      
    }
    public void InstantiateEffect(GameObject target, Vector3 scale)
    {
        target.transform.DOScale(scale, 0.2f);
    }
    public void FadeEffect(Image portrait)
    {
        colorSeq = DOTween.Sequence().SetAutoKill(false).Append(portrait.DOFade(0.1f, 2f)).Append(portrait.DOFade(1f, 2f)).SetLoops(-1);       
    }
    public void StopFadeEffect(Image portrait)
    {
        colorSeq.Kill(false);
        portrait.color = new Color(1f, 1f, 1f, 1f);
    }
}
