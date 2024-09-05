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
    [SerializeField] private Color initialColor;
    [SerializeField] private Color initialTextColor;
    [SerializeField] private Color defaultTextColor;

    public Button Button => button;

    public void Select(bool select)
    {
        //button.image.color = initialColor;
        //text.color =  initialTextColor;
    }

    public void Display(float animationTime, bool select = false)
    {
        gameObject.SetActive(true);
        StartCoroutine(DisplayAnimation(animationTime, select));
    }

    private IEnumerator DisplayAnimation(float animationTime, bool select)
    {
        Color buttonColor = defaultcolor;
        Color textColor = defaultTextColor;

        button.image.color = new Color(0f, 0f, 0f, 0f);
        text.color = new Color(0f, 0f, 0f, 0f);

        yield return text.DOColor(defaultTextColor, animationTime * .95f).WaitForCompletion();
        //button.image.DOColor(defaultcolor, animationTime);

        Select(select);
    }

    public void Hide()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
}
