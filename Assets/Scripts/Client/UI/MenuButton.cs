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

    public Button Button => button;

    public void Select(bool select)
    {
        button.image.color = select ? selectedColor : defaultcolor;
        text.color = select ? defaultcolor : selectedColor;
    }

    public void Display(float animationTime)
    {
        gameObject.SetActive(true);
        StartCoroutine(DisplayAnimation(animationTime));
    }

    private IEnumerator DisplayAnimation(float animationTime)
    {   
        TMP_Text text = button.GetComponentInChildren<TMP_Text>();
        Color buttonColor = button.image.color;
        Color textColor = text.color;
        button.image.color = new Color(0f, 0f, 0f, 0f);
        text.color = new Color(0f, 0f, 0f, 0f);

        text.DOColor(textColor, animationTime * .95f);
        yield return button.image.DOColor(buttonColor, animationTime).WaitForCompletion();
        
        button.image.color = buttonColor;
        text.color = textColor;
    }

    public void Hide()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
}
