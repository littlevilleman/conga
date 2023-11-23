using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class Conga : IConga
    {
        private List<IParticipant> participants = new List<IParticipant>();
        private Vector2Int direction;

        public int Size => participants.Count;
        public IParticipant First => participants?.ToArray()[0];
        public List<IParticipant> Participants => participants?.ToList();
        public Vector2Int Direction => direction;


        public void Setup(IParticipant participant)
        {
            participants = new List<IParticipant>();
            participants.Insert(0, participant);
        }

        public void Update(IBoard board, Vector2Int directionSetup)
        {
            if (direction == - directionSetup)
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

            foreach (IParticipant participant in participants)
            {
                dir = participant == First ? dir : previousLocation - participant.Location;
                previousLocation = participant.Location;
                participant.Move(board, dir);
            }
        }
    }

    public interface IConga
    {
        public Vector2Int Direction { get; }
        public IParticipant First { get; }
        public List<IParticipant> Participants { get; }
        void Setup(IParticipant participant);
        void AddParticipant(IParticipant participant);
        void StepOn(IBoard board);

        void Update(IBoard board, Vector2Int directionSetup);
    }
}