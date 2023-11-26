using Config;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using Random = UnityEngine.Random;

namespace Core
{
    public interface IMatch
    {
        Action<IParticipant> OnAddParticipant { get; set; }
        Action OnDefeat { get; set; }
        IEnumerator Launch();
        void Update(float time, Vector2Int directionInput);
    }

    public class Match :IMatch
    {
        private IBoard board;
        private IRythm rythm;
        private IConga conga;
        private IParticipant awaitingParticipant;
        private Action<IParticipant> addParticipant;

        private List<ParticipantConfig> config;

        private const int BOARD_SIZE = 9;
        public Action<IParticipant> OnAddParticipant { get => addParticipant; set => addParticipant = value; }
        public Action OnDefeat { get => conga.OnCrash; set => conga.OnCrash = value; }

        public Match(List<ParticipantConfig> participants)
        {
            config = participants;
            board = new Board(BOARD_SIZE);
            conga = new Conga();
            rythm = new Rythm();

            conga.Setup(GetRandomFactory().Build(board.GetEmptyLocation(conga.Participants)));
            awaitingParticipant = GetRandomFactory().Build(board.GetEmptyLocation(conga.Participants));
            rythm.OnStep += StepOn;
        }

        public IEnumerator Launch()
        {
            yield return null;

            OnAddParticipant?.Invoke(conga.First);
            OnAddParticipant?.Invoke(awaitingParticipant);
        }

        public void Update(float time, Vector2Int directionInput)
        {
            rythm.Update(time);

            if (directionInput != Vector2Int.zero)
                conga.Update(board, directionInput);
        }
        private void StepOn()
        {
            
            if (board.GetBoardLocation(conga.First.Location + conga.Direction) == awaitingParticipant.Location)
            {
                AddParticipant();
                return;
            }

            conga.StepOn(board);
        }

        private void AddParticipant()
        {
            conga.AddParticipant(awaitingParticipant);
            awaitingParticipant = GetRandomFactory().Build(board.GetEmptyLocation(conga.Participants));
            addParticipant?.Invoke(awaitingParticipant);
        }

        private IParticipantFactory GetRandomFactory(bool allowClone = true)
        {
            int index = Random.Range(0, config.Count);
            var factory = config[index];

            if (!allowClone)
                config.RemoveAt(index);

            return factory;
        }
    }
}