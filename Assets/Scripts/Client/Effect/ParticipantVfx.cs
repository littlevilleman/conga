using DG.Tweening;
using System;
using UnityEngine;

namespace Client
{
    public class ParticipantVfx : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer dummy;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Color colorA;
        [SerializeField] private Color colorB;
        [SerializeField] private Color colorC;
        [SerializeField] private Color colorD;


        private void Update()
        {
            dummy.sprite = spriteRenderer.sprite;
        }

        public void PlayTeleportEffect(Vector3 fromPosition, bool flipX)
        {
            dummy.gameObject.SetActive(true);
            dummy.gameObject.SetActive(false);
            dummy.flipX = flipX;
            dummy.transform.position = fromPosition;
            dummy.enabled = false;
            dummy.enabled = true;
            dummy.gameObject.SetActive(true);
            //dummy.sprite = spriteRenderer.sprite;
        }

        public void StopTeleportEffect()
        {
            dummy.gameObject.SetActive(false);
        }

        public void PlayJoinEffect()
        {
            Vector4 color = colorD;
            Texture2D t = spriteRenderer.sprite.texture;

            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            mpb.SetFloat("_IsAwaiting", 1f);
            spriteRenderer.transform.localScale = Vector3.one;
            spriteRenderer.transform.DOScale(Vector3.one * 1.25f, .1f).SetLoops(2, LoopType.Yoyo);

            DOVirtual.Color(colorD, colorA, .125f, (Color value) => {
                mpb.SetColor("_AwaitingColor", value);
                mpb.SetTexture("_MainTex", t);
                spriteRenderer.SetPropertyBlock(mpb);
            }).SetLoops(2, LoopType.Yoyo);

            DOVirtual.Float(1f, 0f, .125f, (float value) => {
                mpb.SetFloat("_IsAwaiting", value);
            }).SetDelay(.125f);
        }

        public void PlayCrashEffect(Vector3 position, float time, Action onComplete = null)
        { 
            float c = 0f;
            MaterialPropertyBlock mpb =new MaterialPropertyBlock();
            mpb.SetColor("_AwaitingColor", colorA);
            spriteRenderer.SetPropertyBlock(mpb);

            void OnUpdate()
            {
                mpb.SetFloat("_IsAwaiting", c);
                spriteRenderer.SetPropertyBlock(mpb);
            }

            spriteRenderer.transform.DOScale(Vector3.zero, time).SetEase(Ease.InBack);
            DOTween.To(() => c, x => c = x, 1f, time * .5f).SetLoops(2, LoopType.Yoyo).OnUpdate(OnUpdate);
        }

        public void PlaySpawnEffect()
        {
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            mpb.SetFloat("_IsAwaiting", 1f);
            spriteRenderer.SetPropertyBlock(mpb);
            spriteRenderer.transform.localScale = Vector3.zero;
            spriteRenderer.transform.DOScale(Vector3.one, .25f);
        }
    }

}