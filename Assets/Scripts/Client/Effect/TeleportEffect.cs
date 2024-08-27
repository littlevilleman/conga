using UnityEngine;

namespace Client
{
    public class TeleportEffect : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        public void Play(Vector3 fromPosition, bool flipX)
        {
            spriteRenderer.flipX = flipX;
            spriteRenderer.transform.position = fromPosition;
            gameObject.SetActive(true);
        }

        public void UpdateFrame(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
        }

        public void Stop()
        {
            gameObject.SetActive(false);
        }
    }
}