using Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{

    public class MatchManager : MonoBehaviour
    {
        [SerializeField] private ConfigManager configManager;
        [SerializeField] private InputManager inputManager;
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private ParticipantPool pool;

        private IMatch match;
        private List<ParticipantBehaviour> participants = new();

        void OnEnable()
        {
            EventBus.Register<EventStartGame>(StartGame);
            EventBus.Register<EventExitGame>(ExitGame);
            EventBus.Register<EventRestartGame>(RestartGame);
            EventBus.Register<EventBackToMenu>(BackToMenu);
            EventBus.Register<EventChangeDifficulty>(ChangeDifficulty);
        }

        private void ChangeDifficulty(EventChangeDifficulty context)
        {

        }

        private void Update()
        {
            if (match == null)
                return;

            match.Update(Time.deltaTime, inputManager.GetDirectionInput());
        }

        private void StartGame(EventStartGame context)
        {
            UIManager.Instance.DisplayView<MatchView>(true, true);

            match = new Match(configManager.Participants, configManager.Rythm);
            match.OnAddParticipant += AddParticipant;
            match.OnStart += OnStartGame;
            match.OnDefeat += Defeat;

            audioManager.Setup(match.Rythm);
            audioManager.PlaySound(ESoundCode.MATCH_START);

            StartCoroutine(match.Launch());

        }

        private void OnStartGame()
        {

        }

        private void AddParticipant(IParticipant newParticipant, bool isAwaiting = true)
        {
            ParticipantBehaviour participant = pool.PullElement();
            participant.Setup(newParticipant, pool, isAwaiting);
            participants.Add(participant);

            if (isAwaiting)
                audioManager.PlaySound(ESoundCode.PARTICIPANT_JOIN_CONGA);
        }

        private void RecycleParticipants()
        {
            foreach (ParticipantBehaviour participant in participants)
                participant.Recycle(pool);

            participants.Clear();
        }

        private void Defeat()
        {
            match = null;
            UIManager.Instance.DisplayView<DefeatView>(true, false, true);

            audioManager.PlaySound(ESoundCode.MATCH_DEFEAT);
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

        void OnDisable()
        {
            EventBus.Unregister<EventStartGame>(StartGame);
            EventBus.Unregister<EventExitGame>(ExitGame);
            EventBus.Unregister<EventRestartGame>(RestartGame);
            EventBus.Unregister<EventBackToMenu>(BackToMenu);
        }
    }
}
