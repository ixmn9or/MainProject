using System;
using System.Collections;
using System.Collections.Generic;
using ChessEngine.Game;
using DG.Tweening;
using UnityEngine;

public class DoScale : MonoBehaviour
{
    private AudioSource moveAudio;

    private void OnEnable()
    {
        moveAudio = GetComponent<AudioSource>();
    }

    public void DoOut()
    {
        moveAudio.Play();
        gameObject.transform.DOScale(1.3f, 0.8f)
            .SetEase(Ease.OutElastic)
            .OnComplete(SetBack);
    }

    private void SetBack()
    {
        gameObject.transform.DOScale(1f, 2f)
            .SetEase(Ease.OutElastic);
    }
}
