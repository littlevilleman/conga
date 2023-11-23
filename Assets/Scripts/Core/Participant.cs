using Config;
using System;
using UnityEngine;

namespace Core
{
    public class Participant : IParticipant
    {
        private ParticipantConfig config;
        private Vector2Int location;
        private Action<Vector2Int, Vector2Int> move;

        public ParticipantConfig Config => config;
        public Vector2Int Location => location;
        public Action<Vector2Int, Vector2Int> OnMove { get => move; set => move = value; }

        public Participant(ParticipantConfig configSetup, Vector2Int locationSetup)
        {
            config = configSetup;
            location = locationSetup;
        }

        public void Move(IBoard board, Vector2Int direction)
        {
            location = board.OverrideLocation(location + direction);
            move?.Invoke(location, direction);
        }
    }

    public interface IParticipant
    {
        ParticipantConfig Config { get; }
        Vector2Int Location { get; }
        Action<Vector2Int, Vector2Int> OnMove { get; set; }

        void Move(IBoard board, Vector2Int location);
    }

    public interface IParticipantFactory
    {
        public IParticipant Build(Vector2Int location);
    }
}