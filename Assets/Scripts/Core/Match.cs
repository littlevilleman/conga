using Config;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class Match
    {
        private IConga conga;
        private IBoard board;
        private IParticipant awaitingParticipant;
        private Rythm rythm;

        private List<ParticipantConfig> config;
        public Action<IParticipant> addParticipant;

        private const int BOARD_SIZE = 9;


        public Match(Vector2Int location, List<ParticipantConfig> participants)
        {
            config = participants;

            conga = new Conga();
            conga.Setup(GetRandomFactory().Build(location));

            rythm = new Rythm();
            rythm.step += StepConga;

            board = new Board(BOARD_SIZE);

            awaitingParticipant = GetRandomFactory().Build(GetEmptyLocation());
            //addParticipant?.Invoke(awaitingParticipant);
        }

        public IEnumerator Launch()
        {
            yield return null;

            addParticipant.Invoke(conga.First);

            yield return null;

            addParticipant.Invoke(awaitingParticipant);
        }

        public void Update(float time, Vector2Int directionInput)
        {
            rythm.Update(time);

            if (directionInput != Vector2Int.zero)
                conga.Update(directionInput);
        }

        private void StepConga()
        {
            conga.StepOn(board);

            if (conga.First.Location == awaitingParticipant.Location)
            {
                conga.AddParticipant(awaitingParticipant);
                awaitingParticipant = GetRandomFactory().Build(GetEmptyLocation());
                addParticipant?.Invoke(awaitingParticipant);
            }
        }

        private Vector2Int GetEmptyLocation()
        {
            return board.GetEmptyLocation(conga.Participants);
        }

        private IParticipantFactory GetRandomFactory(bool allowClone = true)
        {
            int index = UnityEngine.Random.Range(0, config.Count);
            var factory = config[index];

            if (!allowClone)
                config.RemoveAt(index);

            return factory;
        }
    }
}