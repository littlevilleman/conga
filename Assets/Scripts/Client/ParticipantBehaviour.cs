using Core;
using DG.Tweening;
using System;
using UnityEngine;

namespace Client
{
    public class ParticipantBehaviour : MonoBehaviour, IPoolable<ParticipantBehaviour>
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private TeleportEffect teleportEffect;
        private Material material;

        private IParticipant participant;
        private FrameAnimator animator;

        private const float LOCATION_WIDTH = .12f;

        private Vector3 Position => GetPosition(participant.Location);


        public void Setup(IParticipant participantSetup, IPool<ParticipantBehaviour> poolSetup, bool isAwaiting = true)
        {
            participant = participantSetup;
            participant.OnJoinConga += Join;
            participant.OnMove += Move;
            animator = new FrameAnimator(participant.Config.Sprites.Length);

            transform.position = Position;

            PlaySpawnEffect(isAwaiting);
        }

        private void PlaySpawnEffect(bool isAwaiting)
        {

            if (isAwaiting)
            {
                spriteRenderer.transform.localScale = Vector3.zero;
                spriteRenderer.material.SetFloat("_IsAwaiting", 1f);
                spriteRenderer.material.DOFloat(1f, "_IsAwaiting", .25f);
                spriteRenderer.transform.DOScale(Vector3.one, .25f);
                //spriteRenderer.transform.DOScale(Vector3.one, .25f);
            }
            else
            {
                spriteRenderer.material.SetFloat("_IsAwaiting", 0f);
            }
        }

        private void Update()
        {
            animator?.Update(Time.deltaTime);
            spriteRenderer.sprite = participant.Config.Sprites[animator.CurrentFrame];
            teleportEffect.UpdateFrame(spriteRenderer.sprite);
        }

        private void Join()
        {
            Color fromColor = spriteRenderer.material.GetColor("_ColorD");
            Color toColor = spriteRenderer.material.GetColor("_ColorA");
            Color toColorb = spriteRenderer.material.GetColor("_ColorC");


            spriteRenderer.transform.localScale = Vector3.one;
            spriteRenderer.transform.DOScale(Vector3.one * 1.25f, .1f).SetLoops(2, LoopType.Yoyo);
            spriteRenderer.material.DOColor(toColorb, "_BorderColor",.25f).SetLoops(2, LoopType.Yoyo);
            spriteRenderer.material.DOColor(toColor,"_AwaitingColor",.25f).OnStepComplete(OnStepComplete).SetLoops(2, LoopType.Yoyo);

            void OnStepComplete()
            {
                spriteRenderer.material.DOFloat(0f, "_IsAwaiting", .35f);
            }
        }

        private void Move(Vector2Int location, Vector2Int direction, float time)
        {
            spriteRenderer.flipX = direction.x == 0 ? spriteRenderer.flipX : direction.x > 0f;
            transform.DOMove(GetPosition(location), time).OnComplete(OnMoveComplete);

            if (location != participant.Location)
            {
                var fromPosition = GetPosition(participant.Location - direction);
                teleportEffect.Play(fromPosition, spriteRenderer.flipX);
            }

            spriteRenderer.sortingOrder = 10 - participant.Location.y;
        }

        private void OnMoveComplete()
        {
            teleportEffect.Stop();
            transform.position = Position;
        }

        private Vector3 GetPosition(Vector2Int location)
        {
            return (new Vector3(location.x, location.y, 0f) - new Vector3(4f, 4.5f, 0f)) * LOCATION_WIDTH;
        }

        public void Recycle(IPool<ParticipantBehaviour> pool)
        {
            pool.Recycle(this);
        }
    }
}