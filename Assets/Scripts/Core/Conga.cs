using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Core
{
    public class Conga : IConga
    {
        private List<IParticipant> participants = new List<IParticipant>();
        private Vector2Int direction;
        private Vector2Int lockDirection;
        private Action crash;

        public int Size => participants.Count;
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
        }

        public void StepOn(IBoard board)
        {
            if (First == null)
                return;

            Vector2Int previousLocation = First.Location;
            Vector2Int dir = direction;
            lockDirection = -direction;

            if (CheckCrash())
            {
                crash?.Invoke();
                return;
            }

            foreach (IParticipant participant in participants)
            {
                dir = participant == First ? dir : previousLocation - participant.Location;
                previousLocation = participant.Location;
                participant.Move(board, dir);
            }
        }

        private bool CheckCrash()
        {
            return participants.Any(x => x != First && x.Location == First.Location);
        }
    }


    public interface IConga
    {
        public Vector2Int Direction { get; }
        public IParticipant First { get; }
        public List<IParticipant> Participants { get; }
        Action OnCrash { get; set; }

        void Setup(IParticipant participant);
        void AddParticipant(IParticipant participant);
        void StepOn(IBoard board);
        void Update(IBoard board, Vector2Int directionSetup);
    }
}