using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class DefeatView : View
    {
        [SerializeField] private Transform resultsBoard;
        [SerializeField] private Transform resultsMenu;
        [SerializeField] private TMP_Text pointsText;
        [SerializeField] private TMP_Text pointsTextb;
        [SerializeField] private MenuSelector menuSelector;

        private List<ParticipantBehaviour> participants;
        private Vector2 deltaSize;
        private Vector2 textPosition;


        private MenuButton restartButton => menuSelector.GetButton((int) EMenuOption.RESTART_GAME);
        private MenuButton backButton => menuSelector.GetButton((int) EMenuOption.BACK);

        private enum EMenuOption
        {
            RESTART_GAME, BACK
        }

        private void Awake()
        {
            deltaSize = resultsMenu.GetComponent<RectTransform>().sizeDelta;
        }

        private void OnEnable()
        {
            restartButton.Button.onClick.AddListener(OnClickRestartGameButton);
            backButton.Button.onClick.AddListener(OnClickBackButton);
        }

        public override void Display(params object[] parameters)
        {
            participants = parameters[0] as List<ParticipantBehaviour>;
            pointsText.text = 0.ToString("00");
            base.Display(parameters);

            menuSelector.Hide();
            resultsMenu.gameObject.SetActive(true);
            StartCoroutine(DisplayResultsAnimation());
        }

        private IEnumerator DisplayResultsAnimation()
        {
            resultsMenu.GetComponent<RectTransform>().sizeDelta = new Vector2(deltaSize.x, 0);

            pointsText.transform.localScale = Vector2.zero;
            yield return resultsMenu.GetComponent<RectTransform>().DOSizeDelta(deltaSize, .25f).WaitForCompletion();
            yield return pointsText.transform.DOScale(1f, .125f).WaitForCompletion();
            yield return new WaitForSeconds(.5f);
            
            int points = 0;
            foreach (var participant in participants)
            {
                participant.MoveResult(resultsBoard);

                points += 1;
                yield return new WaitForSeconds(.25f);
                StartCoroutine(UpdateCounterDelay(points));
            }
            yield return new WaitForSeconds(2f);

            yield return pointsText.transform.DOScale(Vector2.zero, .125f).WaitForCompletion();

            yield return resultsMenu.GetComponent<RectTransform>().DOSizeDelta(new Vector2(deltaSize.x, 0), .25f).WaitForCompletion();

            resultsMenu.gameObject.SetActive(false);
            menuSelector.Display();
            pointsTextb.text = pointsText.text;
            EventBus.Send(new EventBackToMenu());
        }

        private IEnumerator UpdateCounterDelay(int points)
        {
            yield return new WaitForSeconds(.25f);
            pointsText.text = points.ToString("00");
        }

        private void OnClickRestartGameButton()
        {
            EventBus.Send(new EventRestartGame());
        }

        private void OnClickBackButton()
        {
            EventBus.Send(new EventBackToMenu());
        }

        private void OnDisable()
        {
            restartButton.Button.onClick.RemoveListener(OnClickRestartGameButton);
            backButton.Button.onClick.RemoveListener(OnClickBackButton);
        }
    }
}