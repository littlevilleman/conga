using Core.Conga;
using System.Collections;
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

        private void Update()
        {
            if (match == null)
                return;

            match.Update(Time.deltaTime, inputManager.GetDirectionInput());
        }

        private void StartGame(EventStartGame context)
        {
            UIManager.Instance.DisplayViewTransition();
            UIManager.Instance.DisplayView<MatchView>();

            match = new Match(configManager.Participants, configManager.Rythm);
            match.OnAddParticipant += AddParticipant;
            match.OnStart += OnStartGame;
            match.OnDefeat += OnDefeat;

            audioManager.Setup(match.Rythm);
            audioManager.PlaySound(ESoundCode.MATCH_START);
            audioManager.SetMusicVolume(.1f);

            StartCoroutine(match.Launch());
        }

        private void ChangeDifficulty(EventChangeDifficulty context)
        {

        }

        private void OnStartGame()
        {

        }

        private void AddParticipant(IParticipant newParticipant, bool isAwaiting = true)
        {
            ParticipantBehaviour participant = pool.PullElement();
            participant.Setup(newParticipant, isAwaiting);
            participants.Add(participant);

            if (isAwaiting)
                audioManager.PlaySound(ESoundCode.PARTICIPANT_JOIN_CONGA);
        }

        private void OnDefeat()
        {
            audioManager.PlaySound(ESoundCode.MATCH_DEFEAT);
            audioManager.PlayMusicPitch(.5f, .25f);
            StartCoroutine(CloseGame());
        }

        private IEnumerator CloseGame()
        {

            yield return match.Close();
            match = null;

            yield return UIManager.Instance.DisplayViewTransitionAsync(true);

            var awaiting = participants[participants.Count - 1];
            awaiting.Recycle(pool);
            participants.Remove(awaiting);


            UIManager.Instance.DisplayView<DefeatView>(participants);
        }

        private void RestartGame(EventRestartGame context)
        {
            RecycleParticipants();
            StartGame(new EventStartGame());
        }

        private void BackToMenu(EventBackToMenu context)
        {
            RecycleParticipants();
            audioManager.PlayMusicPitch(1f, 1f);
            audioManager.SetMusicVolume(.02f);
            UIManager.Instance.DisplayView<MenuView>();
        }

        private void ExitGame(EventExitGame context)
        {
            Application.Quit();
        }

        private void RecycleParticipants()
        {
            foreach (ParticipantBehaviour participant in participants)
                participant.Recycle(pool);

            participants.Clear();
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