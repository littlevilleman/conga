using Core;
using DG.Tweening;
using System;
using UnityEngine;

namespace Client
{
    public class ParticipantBehaviour : MonoBehaviour, IPoolable<ParticipantBehaviour>
    {
        private IParticipant participant;

        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private SpriteRenderer dummy;
        private int currentFrame;

        private float timeCount = 0f;
        private float animationTime = .1f;


        public void Setup(IParticipant participantSetup, IPool<ParticipantBehaviour> poolSetup)
        {
            participant = participantSetup;
            participant.OnMove += Move;

            transform.position = GetPosition(participant.Location);
        }

        private void Update()
        {
            timeCount -= Time.deltaTime;

            if (timeCount <= 0f)
            {
                spriteRenderer.sprite = dummy.sprite = participant.Config.Sprites[currentFrame];
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

        private void Move(Vector2Int location, Vector2Int direction, bool teleport)
        {
            spriteRenderer.flipX = direction.x == 0 ? spriteRenderer.flipX : direction.x > 0f;

            Vector3 position = GetPosition(location);
            transform.DOMove(position, .2f);

            if(location != participant.Location)
            {
                dummy.flipX = spriteRenderer.flipX;
                dummy.gameObject.SetActive(true);
                dummy.transform.position = GetPosition(participant.Location - direction);
                transform.DOMove(position, .2f).OnComplete(OnComplete);
            }
        }

        private void OnComplete()
        {
            dummy.gameObject.SetActive(false);
            transform.position = GetPosition(participant.Location);
        }

        private Vector3 GetPosition(Vector2Int location)
        {
            return new Vector3(location.x, location.y, 0f) * .12f - new Vector3(4f, 4.5f, 0f) * .12f;
        }

        public void Recycle(IPool<ParticipantBehaviour> pool)
        {
            pool.Recycle(this);
        }
    }
}