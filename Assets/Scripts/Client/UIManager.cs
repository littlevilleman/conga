using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;
        public static UIManager Instance => instance;

        [SerializeField] private List<View> views;
        [SerializeField] private Canvas viewLayer;
        [SerializeField] private ViewTransitionEffect transitionEffect;

        private IView currentView;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
        }

        private void Start()
        {
            HideAllViews();
            currentView = views[0];
            currentView?.Display();
        }

        public IView GetView<T>() where T : IView
        {
            foreach (IView view in views)
            {
                if (view.GetType() == typeof(T))
                    return view;
            }

            Debug.LogWarning($"View {typeof(T)} not found");
            return null;
        }

        public void DisplayView<T>(bool displayTransition = false, bool reverse = true, bool waitForAnimation = false) where T : IView
        {
            currentView?.Hide();
            currentView = GetView<T>();

            if (displayTransition)
            {
                if (waitForAnimation)
                {
                    transitionEffect.Launch(reverse, () => currentView?.Display());
                    return;
                }

                transitionEffect.Launch(reverse);
            }

            currentView?.Display();
        }


        public void HideAllViews()
        {
            foreach (View view in views)
            {
                view.gameObject.SetActive(false);
            }
        }
    }
}