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
        public virtual void Display()
        {
            gameObject.SetActive(true);
        }
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}