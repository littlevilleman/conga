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
}
