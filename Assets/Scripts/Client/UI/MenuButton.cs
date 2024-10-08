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

        text.color = initialTextColor;
        text.DOColor(defaultTextColor, animationTime * .95f);
    }

    public void Hide()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
}
