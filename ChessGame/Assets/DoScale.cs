using System;
using System.Collections;
using System.Collections.Generic;
using ChessEngine.Game;
using DG.Tweening;
using UnityEngine;

public class DoScale : MonoBehaviour
{
    public void DoOut()
    {
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
