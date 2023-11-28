using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class ViewTransitionEffect : MonoBehaviour
    {
        [SerializeField] private List<Image> boardTiles;

        public void Launch(bool reverse, Action callback = null)
        {
            gameObject.SetActive(true);
            StartCoroutine(DisplayAnimation(reverse, callback));
        }

        private IEnumerator DisplayAnimation(bool reverse, Action callback = null)
        {
            foreach (var tile in boardTiles)
            {
                tile.rectTransform.localScale = reverse ? Vector3.one : Vector3.zero;
            }

            List<Image> _tiles = new();
            _tiles.AddRange(boardTiles);

            if (reverse)
                _tiles.Reverse();

            _tiles[0].rectTransform.localScale = reverse ? Vector3.zero : Vector3.one;
            
            yield return new WaitForSeconds(.25f * (reverse ? 1f : 0f));

            for (int i = 1; i < _tiles.Count; i++)
            {
                Image tile = _tiles[i];
                tile.rectTransform.DOScale(reverse ? Vector3.zero : Vector3.one, i * .5f / _tiles.Count );

                yield return new WaitForSeconds(.5f / 80f);
            }

            yield return new WaitForSeconds(.75f);

            callback?.Invoke();
            gameObject.SetActive(false);
        }
    }
}