using DG.Tweening;
using UnityEngine;

namespace Client
{
    public class SpawnEffect : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Play(bool reverse = false)
        {
            spriteRenderer.transform.localScale = reverse ? Vector3.one : Vector3.zero;
            spriteRenderer.material.SetFloat("_IsAwaiting", reverse ? 0f : 1f);

            spriteRenderer.material.DOFloat(reverse ? 0f : 1f, "_IsAwaiting", .25f);
            spriteRenderer.transform.DOScale(reverse ? Vector3.zero : Vector3.one, .25f);
        }
    }
}