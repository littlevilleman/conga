using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Core
{
    public interface IConga
    {
        public Vector2Int Direction { get; }
        public IParticipant First { get; }
        public List<IParticipant> Participants { get; }
        Action OnCrash { get; set; }

        void Setup(IParticipant participant);
        void AddParticipant(IParticipant participant);
        void StepOn(IBoard board, IRythm rythm);
        void Update(IBoard board, Vector2Int directionSetup);
    }
    public class Conga : IConga
    {
        private List<IParticipant> participants = new List<IParticipant>();
        private Vector2Int direction;
        private Vector2Int lockDirection;
        private Action crash;

        public IParticipant First => participants?.ToArray()[0];
        public List<IParticipant> Participants => participants?.ToList();
        public Vector2Int Direction => direction;
        public Action OnCrash { get => crash; set => crash = value; }

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

            if (CheckCrash())
            {
                crash?.Invoke();
                return;
            }

            Vector2Int previousLocation = First.Location;
            First.Move(board, rythm, direction);

            for (int i = 1; i < participants.Count; i++)
            {
                Vector2Int followDirection = previousLocation - participants[i].Location;
                previousLocation = participants[i].Location;
                participants[i].Move(board, rythm, followDirection);
            }

            lockDirection = -direction;
        }

        private bool CheckCrash()
        {
            if (participants.Any(x => x != First && x.Location == First.Location))
                return true;

            return false;
        }
    }
}