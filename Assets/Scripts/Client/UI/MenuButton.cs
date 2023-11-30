using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Color defaultcolor;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color defaultTextColor;
    [SerializeField] private Color selectedTextColor;

    public Button Button => button;

    public void Select(bool select)
    {
        button.image.color = select ? selectedColor : defaultcolor;
        text.color = select ? selectedTextColor : defaultTextColor;
    }

    public void Display(float animationTime, bool select = false)
    {
        gameObject.SetActive(true);
        StartCoroutine(DisplayAnimation(animationTime, select));
    }

    private IEnumerator DisplayAnimation(float animationTime, bool select)
    {
        Color buttonColor = select ? selectedColor : defaultcolor;
        Color textColor = select ? selectedTextColor : defaultTextColor;

        button.image.color = new Color(0f, 0f, 0f, 0f);
        text.color = new Color(0f, 0f, 0f, 0f);

        text.DOColor(textColor, animationTime * .95f);
        yield return button.image.DOColor(buttonColor, animationTime).WaitForCompletion();
        
        button.image.color = buttonColor;
        text.color = textColor;

        Select(select);
    }

    public void Hide()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
}
