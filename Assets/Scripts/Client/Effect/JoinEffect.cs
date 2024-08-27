using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class JoinEffect : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Play()
        {
            Color fromColor = spriteRenderer.material.GetColor("_ColorD");
            Color toColor = spriteRenderer.material.GetColor("_ColorA");
            Color toColorb = spriteRenderer.material.GetColor("_ColorC");

            spriteRenderer.transform.localScale = Vector3.one;
            spriteRenderer.transform.DOScale(Vector3.one * 1.25f, .1f).SetLoops(2, LoopType.Yoyo);
            spriteRenderer.material.DOColor(toColorb, "_BorderColor", .25f).SetLoops(2, LoopType.Yoyo);
            spriteRenderer.material.DOColor(toColor, "_AwaitingColor", .25f).OnStepComplete(OnStepComplete).SetLoops(2, LoopType.Yoyo);

            void OnStepComplete()
            {
                spriteRenderer.material.DOFloat(0f, "_IsAwaiting", .35f);
            }
        }
    }
}