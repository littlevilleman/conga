using Config;
using Core.Conga;
using DG.Tweening;
using UnityEngine;

namespace Client
{
    public class ParticipantBehaviour : MonoBehaviour, IPoolable<ParticipantBehaviour>
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private SpriteRenderer shadow;
        [SerializeField] private ParticipantVfx vfx;
        [SerializeField] private ParticipantConfig config;

        private IParticipant participant;
        private FrameAnimator animator;

        private const float LOCATION_WIDTH = .12f;
        private Vector3 Position => GetPosition(participant.Location);

        private void Awake()
        {
            if (config == null)
                return;
            
            participant = config.Build(new Vector2Int (4 + Mathf.CeilToInt(transform.position.x / .16f), 4 + Mathf.CeilToInt((transform.position.y / .16f) - .06f)));
            Setup(participant, false);
        }


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

            //if (participant.Config.Colors.Length > 0)
            //{
            //    spriteRenderer.material.SetColor("_ColorA", participant.Config.Colors[0]);
            //    spriteRenderer.material.SetColor("_ColorB", participant.Config.Colors[1]);
            //    spriteRenderer.material.SetColor("_ColorC", participant.Config.Colors[2]);
            //    spriteRenderer.material.SetColor("_ColorD", participant.Config.Colors[3]);
            //}

            transform.localScale = Vector3.one;
            shadow.transform.localScale = Vector3.one;
            transform.position = Position;

            if (spawnAwaiting)
                vfx.PlaySpawnEffect();

            vfx.StopTeleportEffect();
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

            transform.DOMove(GetPosition(location), time).OnComplete(OnMoveComplete);

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

        private void OnMoveComplete()
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
            shadow.transform.localScale = Vector3.zero;

            transform.position = GetPosition(new Vector2Int(10, 4));
            transform.DOMove(GetPosition(new Vector2Int(-1, 4)), 2f).SetEase(Ease.Linear).OnComplete(OnComplete);

            void OnComplete()
            {
                spriteRenderer.transform.localScale = Vector3.zero;
                spriteRenderer.transform.SetParent(board);
            }
        }

        public void Recycle(IPool<ParticipantBehaviour> pool)
        {
            StopAllCoroutines();
            pool.Recycle(this);
        }
    }
}