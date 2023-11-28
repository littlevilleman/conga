using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Client 
{
    public class MenuSelector : MonoBehaviour
    {
        [SerializeField] private List<MenuButton> buttons;

        private int selected;

        public void Display(float optionAnimationTime= 1f)
        {
            gameObject.SetActive(true);

            StartCoroutine(DisplayAnimation(optionAnimationTime));
        }

        public void Hide()
        {
            StopAllCoroutines();

            foreach (var option in buttons)
            {
                option.Hide();
            }

            gameObject.SetActive(false);
        }

        private IEnumerator DisplayAnimation(float animationTime)
        {
            foreach (var option in buttons)
            {
                option.Display(.5f);

                yield return new WaitForSeconds(.25f);
            }
        }


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