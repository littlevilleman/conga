using DG.Tweening;
using System;
using UnityEngine;

namespace Client
{
    public class ParticipantVfx : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer dummy;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }


        private void Update()
        {
            dummy.sprite = spriteRenderer.sprite;
        }

        public void PlayJoinEffect()
        {
            Color fromColor = spriteRenderer.material.GetColor("_ColorD");
            Color toColor = spriteRenderer.material.GetColor("_ColorA");
            Color toColorb = spriteRenderer.material.GetColor("_ColorC");

            spriteRenderer.transform.localScale = Vector3.one;
            spriteRenderer.transform.DOScale(Vector3.one * 1.25f, .1f).SetLoops(2, LoopType.Yoyo);
            spriteRenderer.material.DOColor(toColorb, "_BorderColor", .25f).SetLoops(2, LoopType.Yoyo);
            spriteRenderer.material.DOColor(toColor, "_AwaitingColor", .25f).OnStepComplete(OnJoinStepComplete).SetLoops(2, LoopType.Yoyo);

            void OnJoinStepComplete()
            {
                spriteRenderer.material.DOFloat(0f, "_IsAwaiting", .35f);
            }
        }

        public void PlayCrashEffect(Vector3 position, float time, Action onComplete = null)
        {
            Color toColor = spriteRenderer.material.GetColor("_ColorA");

            spriteRenderer.sortingOrder += 10;
            spriteRenderer.material.SetFloat("_IsAwaiting", 1f);
            spriteRenderer.transform.DOScale(Vector3.one * 1.25f, time).SetLoops(2, LoopType.Yoyo);
            spriteRenderer.material.DOColor(toColor, "_AwaitingColor", time).OnComplete(() => onComplete?.Invoke()).SetLoops(2, LoopType.Yoyo);
            transform.DOMove(position, time);
        }

        public void PlaySpawnEffect(bool reverse = false)
        {
            spriteRenderer.transform.localScale = reverse ? Vector3.one : Vector3.zero;
            spriteRenderer.material.SetFloat("_IsAwaiting", reverse ? 0f : 1f);

            spriteRenderer.material.DOFloat(reverse ? 0f : 1f, "_IsAwaiting", .25f);
            spriteRenderer.transform.DOScale(reverse ? Vector3.zero : Vector3.one, .25f);
        }

        public void PlayTeleportEffect(Vector3 fromPosition, bool flipX)
        {
            dummy.flipX = flipX;
            dummy.transform.position = fromPosition;
            dummy.gameObject.SetActive(true);
        }

        public void StopTeleportEffect()
        {
            dummy.gameObject.SetActive(false);
        }
    }

}