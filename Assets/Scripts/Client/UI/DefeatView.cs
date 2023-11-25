using UnityEngine;

namespace Client
{
    public class DefeatView : View
    {
        [SerializeField] private MenuSelector menuSelector;

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