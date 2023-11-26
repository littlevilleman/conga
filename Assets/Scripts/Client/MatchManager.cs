using Client;
using Config;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class MatchManager : MonoBehaviour
    {
        [SerializeField] private List<ParticipantConfig> config;
        [SerializeField] private ParticipantPool pool;

        private IMatch match;
        private List<ParticipantBehaviour> participants = new();

        void OnEnable()
        {
            EventBus.Register<EventStartGame>(StartGame);
            EventBus.Register<EventExitGame>(ExitGame);
            EventBus.Register<EventRestartGame>(RestartGame);
            EventBus.Register<EventBackToMenu>(BackToMenu);
        }

        private void StartGame(EventStartGame context)
        {
            UIManager.Instance.HideAllViews();

            match = new Match(config);
            match.OnAddParticipant += AddParticipant;
            match.OnDefeat += Defeat;

            StartCoroutine(match.Launch());
        }

        private void AddParticipant(IParticipant newParticipant)
        {
            ParticipantBehaviour participant = pool.PullElement();
            participant.Setup(newParticipant, pool);
            participants.Add(participant);
        }

        private void RecycleParticipants()
        {
            foreach (ParticipantBehaviour participant in participants)
                participant.Recycle(pool);

            participants.Clear();
        }

        private void Update()
        {
            if (match == null)
                return;

            match.Update(Time.deltaTime, GetDirectionInput());
        }

        private void Defeat()
        {
            match = null;
            UIManager.Instance.DisplayView<DefeatView>();
        }

        private void RestartGame(EventRestartGame context)
        {
            RecycleParticipants();
            StartGame(new EventStartGame());
        }

        private void BackToMenu(EventBackToMenu context)
        {
            RecycleParticipants();
            UIManager.Instance.DisplayView<MenuView>();
        }

        private void ExitGame(EventExitGame context)
        {
            Application.Quit();
        }

        private Vector2Int GetDirectionInput()
        {
            if (Input.GetKey(KeyCode.W))
                return Vector2Int.up;

            if (Input.GetKey(KeyCode.D))
                return Vector2Int.right;

            if (Input.GetKey(KeyCode.A))
                return Vector2Int.left;

            if (Input.GetKey(KeyCode.S))
                return Vector2Int.down;

            return Vector2Int.zero;
        }

        void OnDisable()
        {
            EventBus.Unregister<EventStartGame>(StartGame);
            EventBus.Unregister<EventExitGame>(ExitGame);
            EventBus.Unregister<EventRestartGame>(RestartGame);
            EventBus.Unregister<EventBackToMenu>(BackToMenu);
        }
    }
}
