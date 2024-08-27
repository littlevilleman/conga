using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

        private MenuButton restartButton => menuSelector.GetButton((int) EMenuOption.RESTART_GAME);
        private MenuButton backButton => menuSelector.GetButton((int) EMenuOption.BACK);

        private enum EMenuOption
        {
            RESTART_GAME, BACK
        }

        private void OnEnable()
        {
            restartButton.Button.onClick.AddListener(OnClickRestartGameButton);
            backButton.Button.onClick.AddListener(OnClickBackButton);
        }

        public override void Display(params object[] parameters)
        {
            participants = parameters[0] as List<ParticipantBehaviour>;
            base.Display(parameters);

            menuSelector.Hide();
            resultsMenu.gameObject.SetActive(true);
            StartCoroutine(DisplayResultsAnimation());
        }

        private IEnumerator DisplayResultsAnimation()
        {
            int points = 0;
            foreach (var participant in participants)
            {
                participant.MoveResult(resultsBoard);
                yield return new WaitForSeconds(.25f);

                points += 1;
                pointsText.text = points.ToString("00");
            }

            yield return new WaitForSeconds(1f);

            resultsMenu.gameObject.SetActive(false);
            menuSelector.Display();
            pointsTextb.text = pointsText.text;
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