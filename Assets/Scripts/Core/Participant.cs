using Config;
using System;
using UnityEngine;

namespace Core
{
    public class Participant : IParticipant
    {
        private ParticipantConfig config;
        private Vector2Int location;
        private Action<Vector2Int, Vector2Int, bool> move;

        public ParticipantConfig Config => config;
        public Vector2Int Location => location;
        public Action<Vector2Int, Vector2Int, bool> OnMove { get => move; set => move = value; }

        public Participant(ParticipantConfig configSetup, Vector2Int locationSetup)
        {
            config = configSetup;
            location = locationSetup;
        }

        public void Move(IBoard board, Vector2Int direction)
        {
            if (Mathf.Abs(direction.x) > 1)
                direction.x -= Math.Sign(direction.x) * 9;
            else if (Mathf.Abs(direction.y) > 1)
                direction.y -= Math.Sign(direction.y) * 9;

            var absoluteLocation = location + direction;

            location = board.GetBoardLocation(absoluteLocation);
            move?.Invoke(absoluteLocation, direction, location != absoluteLocation);
        }

        public void Follow(IBoard board, Vector2Int toLocation)
        {
            var dir = toLocation - location;


            location = board.GetBoardLocation(toLocation);
            move?.Invoke(toLocation, dir, location != toLocation);
        }
    }

    public interface IParticipant
    {
        ParticipantConfig Config { get; }
        Vector2Int Location { get; }
        Action<Vector2Int, Vector2Int, bool> OnMove { get; set; }

        void Follow(IBoard board, Vector2Int toLocation);
        void Move(IBoard board, Vector2Int location);
    }

    public interface IParticipantFactory
    {
        public IParticipant Build(Vector2Int location);
    }
}