using System;
using UnityEngine;

public class DefeatView : MonoBehaviour, IView
{
    [SerializeField] private MenuButton restartButton;
    [SerializeField] private MenuButton backButton;

    private EDefeatMenuOption selected;
    public Action restart;
    public Action back;

    private enum EDefeatMenuOption
    {
        RESTART_GAME, BACK
    }

    public void Display()
    {
        gameObject.SetActive(true);
        SelectOption(selected);
    }

    private void OnEnable()
    {
        restartButton.Button.onClick.AddListener(OnClickRestartGameButton);
        backButton.Button.onClick.AddListener(OnClickBackButton);
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

    private void SelectOption(EDefeatMenuOption toOption)
    {
        toOption = ClampSelection(toOption);
        GetButton(selected)?.Select(false);
        selected = toOption;
        GetButton(selected)?.Select(true);
    }

    private EDefeatMenuOption ClampSelection(EDefeatMenuOption toOption)
    {
        int options = Enum.GetNames(typeof(EDefeatMenuOption)).Length;

        if (toOption < 0)
            toOption += options;

        if ((int)toOption >= options)
            toOption -= options;

        return toOption;
    }

    private MenuButton GetButton(EDefeatMenuOption selected)
    {
        switch (selected)
        {
            case EDefeatMenuOption.RESTART_GAME:
                return restartButton;

            case EDefeatMenuOption.BACK:
                return backButton;
        }

        return restartButton;
    }

    private void OnClickRestartGameButton()
    {
        restart?.Invoke();
        Hide();
    }

    private void OnClickBackButton()
    {
        back?.Invoke();
        Hide();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        restartButton.Button.onClick.RemoveListener(OnClickRestartGameButton);
        backButton.Button.onClick.RemoveListener(OnClickBackButton);
    }

}
