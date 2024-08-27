using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public abstract class Pool<T> : MonoBehaviour, IPool<T> where T : MonoBehaviour, IPoolable<T>
    {
        [SerializeField] protected Transform world;
        [SerializeField] protected int initialCount = 10;
        [SerializeField] protected int limitCount = 50;
        [SerializeField] protected T prefab;

        protected List<T> elements = new();

        private void Awake()
        {
            for (int i = 0; i < initialCount; i++)
            {
                CreateElement();
            }
        }

        public abstract T CreateElement();

        public T PullElement()
        {
            foreach (T element in elements)
            {
                if (!element.gameObject.activeInHierarchy)
                {
                    element.transform.SetParent(world);
                    element.gameObject.SetActive(true);
                    return element;
                }
            }

            T newElement = CreateElement();
            newElement?.transform.SetParent(world);
            newElement?.gameObject.SetActive(true);
            return newElement;
        }

        public void Recycle(T element)
        {
            element.gameObject.SetActive(false);
            element.transform.SetParent(transform);
            element.transform.position = Vector3.zero;
        }
    }

    public interface IPool<T> where T : MonoBehaviour, IPoolable<T>
    {
        T PullElement();
        void Recycle(T element);
    }

    public interface IPoolable<T> where T : MonoBehaviour, IPoolable<T>
    {
        public void Recycle(IPool<T> pool);
    }
}