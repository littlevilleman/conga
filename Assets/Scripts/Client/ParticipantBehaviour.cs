using Core;
using DG.Tweening;
using UnityEngine;

namespace Client
{
    public class ParticipantBehaviour : MonoBehaviour, IPoolable<ParticipantBehaviour>
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private SpriteRenderer shadow;
        [SerializeField] private ParticipantVfx vfx;

        private IParticipant participant;
        private FrameAnimator animator;

        private const float LOCATION_WIDTH = .12f;
        private Vector3 Position => GetPosition(participant.Location);

        public void Setup(IParticipant participantSetup, bool spawnAwaiting = true)
        {
            participant = participantSetup;
            participant.OnJoinConga += Join;
            participant.OnMove += Move;
            participant.OnCrash += Crash;
            animator = new FrameAnimator(participant.Config.Sprites.Length);

            spriteRenderer.sortingOrder = 10 - participant.Location.y;
            spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
            spriteRenderer.material.SetFloat("_IsAwaiting", spawnAwaiting ? 1f : 0f);

            transform.localScale = Vector3.one;
            transform.position = Position;

            if (spawnAwaiting)
                vfx.PlaySpawnEffect();
        }

        private void Join()
        {
            vfx.PlayJoinEffect();
        }

        private void Update()
        {
            animator?.Update(Time.deltaTime);
            spriteRenderer.sprite = participant.Config.Sprites[animator.CurrentFrame];
        }

        private void Move(Vector2Int location, Vector2Int direction, float time)
        {
            spriteRenderer.flipX = direction.x == 0 ? spriteRenderer.flipX : direction.x > 0f;
            spriteRenderer.sortingOrder = 10 - participant.Location.y;

            transform.DOMove(GetPosition(location), time).OnComplete(() => OnMoveComplete(direction, time));

            if (location != participant.Location)
            {
                var fromPosition = GetPosition(participant.Location - direction);
                vfx.PlayTeleportEffect(fromPosition, spriteRenderer.flipX);
            }
        }

        private void Crash(Vector2Int location, Vector2Int direction, float time)
        {
            vfx.PlayCrashEffect(GetPosition(participant.Location - direction), time * 1.25f, OnCrashEffectComplete);
        }

        private void OnMoveComplete(Vector2Int direction, float time)
        {
            vfx.StopTeleportEffect();
            transform.position = Position;
        }

        private void OnCrashEffectComplete()
        {
            spriteRenderer.material.SetFloat("_IsAwaiting", 0f);
            spriteRenderer.sortingOrder -= 10 - participant.Location.y;
            vfx.PlaySpawnEffect(true);
        }

        private Vector3 GetPosition(Vector2Int location)
        {
            return (new Vector3(location.x, location.y, 0f) - new Vector3(4f, 4.5f, 0f)) * LOCATION_WIDTH;
        }

        public void MoveResult(Transform resultBoard)
        {
            Transform board = transform.parent;
            spriteRenderer.flipX = false;
            spriteRenderer.material.SetFloat("_IsAwaiting", 0f);
            spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            spriteRenderer.transform.SetParent(resultBoard);
            spriteRenderer.transform.localScale= Vector3.one;
            shadow.gameObject.SetActive(false);

            transform.position = GetPosition(new Vector2Int(10, 4));
            transform.DOMove(GetPosition(new Vector2Int(-1, 4)), 2f).SetEase(Ease.Linear).OnComplete(OnComplete);

            void OnComplete()
            {
                shadow.gameObject.SetActive(true);
                spriteRenderer.transform.localScale = Vector3.zero;
                spriteRenderer.transform.SetParent(board);
            }
        }

        public void Recycle(IPool<ParticipantBehaviour> pool)
        {
            pool.Recycle(this);
        }
    }
}