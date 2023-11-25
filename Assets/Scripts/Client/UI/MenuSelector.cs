using System.Collections.Generic;
using UnityEngine;

namespace Client 
{
    public class MenuSelector : MonoBehaviour
    {
        [SerializeField] private List<MenuButton> buttons;

        private int selected;

        private void OnEnable()
        {
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

        public void SelectOption(int toOption)
        {
            toOption = ClampSelection(toOption);
            GetButton(selected)?.Select(false);
            selected = toOption;
            GetButton(selected)?.Select(true);
        }

        private int ClampSelection(int toOption)
        {
            if (toOption < 0)
                toOption += buttons.Count;

            if ((int)toOption >= buttons.Count)
                toOption -= buttons.Count;

            return toOption;
        }

        public MenuButton GetButton(int selected)
        {
            return buttons[selected];
        }
    }
}