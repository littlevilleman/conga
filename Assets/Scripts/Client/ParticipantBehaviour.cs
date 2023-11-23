using Core;
using DG.Tweening;
using UnityEngine;

namespace Client
{
    public class ParticipantBehaviour : MonoBehaviour, IPoolable<ParticipantBehaviour>
    {
        private IParticipant participant;
        private IPool<ParticipantBehaviour> pool;

        [SerializeField] private SpriteRenderer spriteRenderer;
        private int currentFrame;

        private float timeCount = 0f;
        private float animationTime = .15f;

        public void Setup(IParticipant participantSetup, IPool<ParticipantBehaviour> poolSetup)
        {
            pool = poolSetup;
            participant = participantSetup;
            participant.OnMove += Move;

            transform.position = new Vector3(participant.Location.x, participant.Location.y, 0f) * .12f - new Vector3(4f, 4.5f, 0f) * .12f;
        }

        private void Update()
        {
            timeCount -= Time.deltaTime;

            if (timeCount <= 0f)
            {
                spriteRenderer.sprite = participant.Config.Sprites[currentFrame];
                currentFrame = GetNextFrame();
                timeCount += animationTime;
            }

        }

        private int GetNextFrame()
        {
            if (currentFrame == participant.Config.Sprites.Length - 1)
                return 0;

            return currentFrame + 1;
        }

        private void Move(Vector2Int location, Vector2Int direction)
        {
            spriteRenderer.flipX = direction.x == 0 ? spriteRenderer.flipX : direction.x > 0f;

            var position = new Vector3(location.x, location.y, 0f) * .12f - new Vector3(4f, 4.5f, 0f) * .12f;
            transform.DOMoveX(position.x, .25f);
            transform.DOMoveY(position.y, .25f);
        }

        public void Recycle()
        {
            pool.Recycle(this);
        }
    }
}