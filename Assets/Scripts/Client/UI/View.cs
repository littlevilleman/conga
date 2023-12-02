using UnityEngine;

namespace Client
{
    public interface IView
    {
        void Display(params object[] parameters);
        void Hide();
    }

    public abstract class View : MonoBehaviour, IView
    {
        public virtual void Display(params object[] parameters)
        {
            gameObject.SetActive(true);
        }
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}