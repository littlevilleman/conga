using DG.Tweening;
using System;
using UnityEngine;

namespace Client
{
    public class CrashEffect : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        public void Play(Vector3 position, float time, Action onComplete = null)
        {
            Color toColor = spriteRenderer.material.GetColor("_ColorA");

            spriteRenderer.sortingOrder += 10;
            spriteRenderer.material.SetFloat("_IsAwaiting", 1f);
            spriteRenderer.transform.DOScale(Vector3.one * 1.25f, time).SetLoops(2, LoopType.Yoyo);
            spriteRenderer.material.DOColor(toColor, "_AwaitingColor", time).OnComplete(() => onComplete?.Invoke()).SetLoops(2, LoopType.Yoyo);
            transform.DOMove(position, time);
        }
    }
}