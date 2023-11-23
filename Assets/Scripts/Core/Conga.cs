using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class Conga : IConga
    {
        private Queue<IParticipant> participants;
        private Vector2Int direction;

        public int Size => participants.Count;
        public IParticipant First => participants?.ToArray()[0];
        public List<IParticipant> Participants => participants.ToList();
        public Vector2Int Direction => direction;

        public void Setup(IParticipant participant)
        {
            participants = new Queue<IParticipant>();
            participants.Enqueue(participant);
        }

        public void Update(Vector2Int directionSetup)
        {
            if (direction == -directionSetup)
                return;

            direction = directionSetup;
        }

        public void AddParticipant(IParticipant participant)
        {
            participants.Enqueue(participant);
        }

        public void StepOn(IBoard board)
        {
            if (First == null)
                return;

            Vector2Int dir = board.OverrideDirection(First.Location, direction);
            Vector2Int previousLocation = First.Location;

            foreach (IParticipant participant in participants)
            {
                dir = participant == First ? dir : previousLocation - participant.Location;
                previousLocation = participant.Location;
                participant.Move(dir);
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

        void Update(Vector2Int direction);
    }
}