using System;
using UnityEngine;

public interface IView
{
    void Display();
    void Hide();
}

public class MenuView : MonoBehaviour, IView
{
    [SerializeField] private MenuButton startGameButton;
    [SerializeField] private MenuButton optionsButton;
    [SerializeField] private MenuButton exitGameButton;

    public Action startGame;
    public Action exitGame;

    private EMenuOption selected = EMenuOption.START_GAME;

    private enum EMenuOption
    {
        START_GAME, OPTIONS, QUIT
    }

    private void OnEnable()
    {
        startGameButton.Button.onClick.AddListener(OnClickStartGame);
        startGameButton.Button.onClick.AddListener(OnClickOptions);
        startGameButton.Button.onClick.AddListener(OnClickExitGame);
    }

    public void Display()
    {
        gameObject.SetActive(true);
        SelectOption(selected);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            SelectOption(selected - 1);

        if (Input.GetKeyDown(KeyCode.S))
            SelectOption(selected + 1);

        if (Input.GetKeyDown(KeyCode.Return))
            GetButton(selected).Button.onClick?.Invoke();
    }

    private void SelectOption(EMenuOption toOption)
    {
        toOption = ClampSelection(toOption);
        GetButton(selected)?.Select(false);
        selected = toOption;
        GetButton(selected)?.Select(true);
    }

    private EMenuOption ClampSelection(EMenuOption toOption)
    {
        int options = Enum.GetNames(typeof(EMenuOption)).Length;

        if (toOption < 0)
            toOption += options;

        if ((int) toOption >= options)
            toOption -= options;

        return toOption;
    }

    private MenuButton GetButton(EMenuOption selected)
    {
        switch (selected)
        {
            case EMenuOption.START_GAME:
                return startGameButton;

            case EMenuOption.OPTIONS:
                return optionsButton;

            case EMenuOption.QUIT:
                return exitGameButton;
        }

        return startGameButton;
    }

    private void OnClickStartGame()
    {
        Hide();
        startGame?.Invoke();
    }

    private void OnClickOptions()
    {

    }

    private void OnClickExitGame()
    {
        exitGame?.Invoke();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        startGameButton.Button.onClick.RemoveListener(OnClickStartGame);
        startGameButton.Button.onClick.RemoveListener(OnClickOptions);
        startGameButton.Button.onClick.RemoveListener(OnClickExitGame);
    }
}
