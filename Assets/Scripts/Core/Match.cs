using Config;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core
{
    public interface IMatch
    {
        Action OnStart { get; set; }
        Action<IParticipant, bool> OnAddParticipant { get; set; }
        Action OnDefeat { get; set; }
        IRythm Rythm { get;}

        IEnumerator Launch();
        void Update(float time, Vector2Int directionInput);
    }

    public class Match :IMatch
    {
        private IBoard board;
        private IRythm rythm;
        private IConga conga;

        private IParticipant awaitingParticipant;
        private Action start;
        private Action<IParticipant, bool> addParticipant;

        private List<ParticipantConfig> config;
        private List<ParticipantConfig> inUseConfig = new ();

        public IRythm Rythm => rythm;
        public Action OnStart { get => start; set => start = value; }
        public Action<IParticipant, bool> OnAddParticipant { get => addParticipant; set => addParticipant = value; }
        public Action OnDefeat { get => conga.OnCrash; set => conga.OnCrash = value; }

        public Match(List<ParticipantConfig> participantsConfig, RythmConfig rythmConfig)
        {
            config = new List<ParticipantConfig>(participantsConfig);
            rythm = rythmConfig.Build();
            board = new Board();
            conga = new Conga();

            conga.Setup(GetRandomFactory().Build(board.CenterLocation));
            awaitingParticipant = GetRandomFactory().Build(board.GetEmptyLocation(conga.Participants));

            rythm.OnStep += StepOn;
        }

        public IEnumerator Launch()
        {
            //wait for view transition
            OnAddParticipant?.Invoke(conga.First, false);
            yield return new WaitForSeconds(1f);

            OnAddParticipant?.Invoke(awaitingParticipant, true);

            yield return new WaitForSeconds(1f);

            start?.Invoke();
        }

        public void Update(float time, Vector2Int directionInput)
        {
            rythm.Update(time, conga.Participants.Count);

            if (directionInput != Vector2Int.zero)
                conga.Update(board, directionInput);
        }
        private void StepOn()
        {
            if (board.GetBoardLocation(conga.First.Location + conga.Direction) == awaitingParticipant.Location)
            {
                conga.AddParticipant(awaitingParticipant);
                awaitingParticipant = GetRandomFactory().Build(board.GetEmptyLocation(conga.Participants));
                addParticipant?.Invoke(awaitingParticipant, true);
                return;
            }

            conga.StepOn(board, rythm);
        }

        private IParticipantFactory GetRandomFactory()
        {
            if (config.Count == 0)
            {
                config.AddRange(inUseConfig);
                inUseConfig.Clear();
            }

            int index = Random.Range(0, config.Count);
            var factory = config[index];

            config.RemoveAt(index);
            inUseConfig.Add(factory);

            return factory;
        }
    }
}