using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class ViewTransitionEffect : MonoBehaviour
    {
        [SerializeField] private List<Image> boardTiles;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public IEnumerator Launch(bool reverse = false)
        {
            spriteRenderer.material.SetFloat("_Range", reverse ? 1.8f : 0f);
            yield return spriteRenderer.material.DOFloat(reverse ? -.5f : 1.8f, "_Range", .5f).SetEase(Ease.InSine).WaitForCompletion();
        }
    }
}