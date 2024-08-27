using UnityEngine;

namespace Client.Tetris
{
    public class PiecePool : Pool<PieceBehavior>
    {
        public override PieceBehavior CreateElement()
        {
            PieceBehavior element = null;

            if (elements.Count < limitCount)
            {
                element = Instantiate(prefab, transform);
                element.gameObject.SetActive(false);
                elements.Add(element);
            }
            else
                Debug.LogError("Pieces reached limit");

            return element;
        }
    }

    public class PieceBehavior : MonoBehaviour, IPoolable<PieceBehavior>
    {
        public void Recycle(IPool<PieceBehavior> pool)
        {
            StopAllCoroutines();
            pool.Recycle(this);
        }
    }
}
