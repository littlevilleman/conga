using Config;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core
{
    public interface IMatch
    {
        Action<IParticipant> OnJoinParticipant { get; set; }
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

        private List<ParticipantConfig> config;
        private Action<IParticipant> joinParticipant;
        private Action defeat;

        private const int BOARD_SIZE = 9;
        public Action<IParticipant> OnJoinParticipant { get => joinParticipant; set => joinParticipant = value; }
        public Action OnDefeat { get => defeat; set => defeat = value; }

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

            joinParticipant?.Invoke(conga.First);
            joinParticipant?.Invoke(awaitingParticipant);
        }

        public void Update(float time, Vector2Int directionInput)
        {
            rythm.Update(time);

            if (directionInput != Vector2Int.zero)
                conga.Update(board, directionInput);
        }

        private void StepOn()
        {
            Vector2Int location = board.OverrideLocation(conga.First.Location + conga.Direction);

            if (CheckDefeat(location))
            {
                defeat?.Invoke();
                return;
            }
            
            if (CheckJoin(location))
            {
                joinParticipant?.Invoke(awaitingParticipant);
                return;
            }

            conga.StepOn(board);
        }

        private bool CheckJoin(Vector2Int location)
        {
            if (location != awaitingParticipant.Location)
                return false;
            
            conga.AddParticipant(awaitingParticipant);
            awaitingParticipant = GetRandomFactory().Build(board.GetEmptyLocation(conga.Participants));
            return true;
        }

        private bool CheckDefeat(Vector2Int location)
        {
            return conga.Participants.Count > 1 && conga.Participants.Any(x => x.Location == location);
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