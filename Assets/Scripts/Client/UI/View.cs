using UnityEngine;

namespace Client
{
    public interface IView
    {
        void Display();
        void Hide();
    }

    public abstract class View : MonoBehaviour, IView
    {
        public void Display()
        {
            gameObject.SetActive(true);
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}