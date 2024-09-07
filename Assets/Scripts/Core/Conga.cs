using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;

namespace Core.Conga
{
    public interface IConga
    {
        public Vector2Int Direction { get; }
        public IParticipant First { get; }
        public List<IParticipant> Participants { get; }
        Action<Vector2Int, IParticipant> OnCrash { get; set; }

        void Setup(IParticipant participant);
        void AddParticipant(IParticipant participant);
        void StepOn(IBoard board, IRythm rythm);
        void Update(IBoard board, Vector2Int directionSetup);
        IEnumerator Crash(IBoard board, IRythm rythm);
    }
    public class Conga : IConga
    {
        private List<IParticipant> participants = new List<IParticipant>();
        private Vector2Int direction;
        private Vector2Int lockDirection;
        private Action<Vector2Int, IParticipant> crash;

        public IParticipant First => participants?.ToArray()[0];
        public List<IParticipant> Participants => participants?.ToList();
        public Vector2Int Direction => direction;
        public Action<Vector2Int, IParticipant> OnCrash { get => crash; set => crash = value; }

        public void Setup(IParticipant participant)
        {
            participants = new List<IParticipant>();
            participants.Insert(0, participant);
        }

        public void Update(IBoard board, Vector2Int directionSetup)
        {
            if (directionSetup == lockDirection)
                return;

            direction = directionSetup;
        }

        public void AddParticipant(IParticipant participant)
        {
            participants.Insert(0, participant);
            participant.OnJoinConga?.Invoke();
        }

        public void StepOn(IBoard board, IRythm rythm)
        {
            if (First == null)
                return;

            Vector2Int previousLocation = First.Location;

            First.Move(board, rythm, direction);

            for (int i = 1; i < participants.Count; i++)
            {
                Vector2Int followDirection = previousLocation - participants[i].Location;
                previousLocation = participants[i].Location;
                participants[i].Move(board, rythm, followDirection);
            }

            if (CheckCrash(out IParticipant crashParticipant))
            {
                crash?.Invoke(direction, crashParticipant);
                return;
            }

            lockDirection = -direction;
        }

        public IEnumerator Crash(IBoard board, IRythm rythm)
        {
            Vector2Int previousLocation = First.Location;
            float crashTime = .1f;// 5f * participants.Count / 81f;

            First.Crash(board, crashTime, direction);

            yield return null;

            for (int i = 1; i < participants.Count; i++)
            {
                Vector2Int d =  previousLocation - participants[i].Location;
                previousLocation = participants[i].Location;
                participants[i].Crash(board, crashTime, d);

                yield return new WaitForSeconds(crashTime);
            }
        }

        public bool CheckCrash(out IParticipant participant)
        {
            participant = participants.Find(x => x != First && x.Location == First.Location);
            return participant != null;
        }
    }
}