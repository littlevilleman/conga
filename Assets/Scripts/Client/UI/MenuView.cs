using UnityEngine;

namespace Client
{
    public class MenuView : View
    {
        [SerializeField] private MenuSelector menuSelector;
        [SerializeField] private IntroCut intro;

        private MenuButton startGameButton => menuSelector.GetButton((int) EMenuOption.START_GAME);
        private MenuButton optionsButton => menuSelector.GetButton((int) EMenuOption.OPTIONS);
        private MenuButton exitGameButton => menuSelector.GetButton((int) EMenuOption.QUIT);

        private enum EMenuOption
        {
            START_GAME, OPTIONS, QUIT
        }

        private void OnEnable()
        {
            startGameButton.Button.onClick.AddListener(OnClickStartGame);
            //optionsButton.Button.onClick.AddListener(OnClickOptions);
            //exitGameButton.Button.onClick.AddListener(OnClickExitGame);

            intro.onComplete += OnCompleteIntro;
            intro.Play();
        }

        private void OnCompleteIntro()
        {
            menuSelector.Display();
        }

        private void OnClickStartGame()
        {
            EventBus.Send(new EventStartGame());
        }

        private void OnClickOptions()
        {
            UIManager.Instance.DisplayView<OptionsView>();
        }

        private void OnClickExitGame()
        {
            EventBus.Send(new EventExitGame());
        }

        private void OnDisable()
        {
            startGameButton.Button.onClick.RemoveListener(OnClickStartGame);
            //optionsButton.Button.onClick.RemoveListener(OnClickOptions);
            //exitGameButton.Button.onClick.RemoveListener(OnClickExitGame);

            intro.onComplete -= OnCompleteIntro;
            intro.Stop();
            menuSelector.Hide();
        }
    }
}