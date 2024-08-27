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
            spriteRenderer.material.SetFloat("_Range", reverse ? 2f : -.5f);
            yield return new WaitForSeconds(.1f);
            yield return spriteRenderer.material.DOFloat(reverse ? 0f : 2f, "_Range", 1f).WaitForCompletion();
        }
    }
}