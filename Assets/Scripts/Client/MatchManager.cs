using Client;
using Config;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class MatchManager : MonoBehaviour
    {
        [SerializeField] private List<ParticipantConfig> config;
        [SerializeField] private List<ParticipantBehaviour> participants;
        [SerializeField] private ParticipantPool pool;
        [SerializeField] private MenuView menuView;
        [SerializeField] private DefeatView defeatView;

        private IMatch match;

        // Start is called before the first frame update
        void Start()
        {
            menuView.startGame += StartGame;
            menuView.exitGame += ExitGame;
            menuView.Display();

            defeatView.restart += RestartGame;
            defeatView.back += BackToMenu;
        }

        private void RecycleParticipants()
        {
            foreach (ParticipantBehaviour participant in participants)
            {
                participant.Recycle();
            }

            participants.Clear();
        }

        private void StartGame()
        {
            match = new Match(config);
            match.OnJoinParticipant += AddParticipant;
            match.OnDefeat += Defeat;

            StartCoroutine(match.Launch());
        }

        private void RestartGame()
        {
            RecycleParticipants();
            StartGame();
        }

        private void BackToMenu()
        {
            RecycleParticipants();
            menuView.Display();
        }

        private void ExitGame()
        {
            Application.Quit();
        }

        private void Defeat()
        {
            match = null;
            defeatView.Display();
        }

        private void AddParticipant(IParticipant newParticipant)
        {
            ParticipantBehaviour participant = pool.PullElement();
            participant.Setup(newParticipant, pool);
            participants.Add(participant);
        }

        private void Update()
        {
            if (match == null)
                return;

            match.Update(Time.deltaTime, GetDirectionInput());
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
    }
}
