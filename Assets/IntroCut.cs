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

        public void Play(float letterSpawnDelay = .05f)
        {
            StartCoroutine(Launch(letterSpawnDelay));
        }
        public void Stop()
        {
            foreach (RectTransform letter in letters)
            {
                letter.gameObject.SetActive(false);
            }
        }

        private IEnumerator Launch(float letterSpawnDelay)
        {
            yield return new WaitForSeconds(1f);

            foreach (RectTransform letter in letters)
            {
                letter.gameObject.SetActive(true);

                yield return new WaitForSeconds(letterSpawnDelay);
            }

            Color color = initialColor;
            color.a = 0f;
            background.DOColor(color, 1f);
            onComplete?.Invoke();
        }
    }
}
