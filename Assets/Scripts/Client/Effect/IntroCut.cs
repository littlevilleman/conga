using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class IntroCut : MonoBehaviour
    {
        [SerializeField] private List<RectTransform> letters;
        [SerializeField] private Image background;
        private float letterSpawnDelay = .05f;
        private Color initialColor;


        public Action onComplete;

        private void Awake()
        {
            initialColor = background.color;
        }

        private void OnEnable()
        {
            background.color = initialColor;
        }

        public void Play()
        {
            StartCoroutine(PlayAnimation());
        }
        public void Stop()
        {
            foreach (RectTransform letter in letters)
            {
                letter.gameObject.SetActive(false);
            }
        }

        private IEnumerator PlayAnimation()
        {
            yield return PlayLettersAnimation();
            yield return PlayBackgroundAnimation();
        }

        private IEnumerator PlayLettersAnimation()
        {
            yield return new WaitForSeconds(1f);

            foreach (RectTransform letter in letters)
            {
                letter.gameObject.SetActive(true);

                yield return new WaitForSeconds(letterSpawnDelay);
            }
        }

        private IEnumerator PlayBackgroundAnimation()
        {
            Color color = initialColor;
            color.a = 0f;
            background.DOColor(color, 1f);

            yield return new WaitForSeconds(1f);
            onComplete?.Invoke();
        }
    }
}
