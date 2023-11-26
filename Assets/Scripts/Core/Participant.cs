using Config;
using System;
using UnityEngine;

namespace Core
{
    public interface IParticipantFactory
    {
        public IParticipant Build(Vector2Int location);
    }

    public interface IParticipant
    {
        ParticipantConfig Config { get; }
        Vector2Int Location { get; }
        Action<Vector2Int, Vector2Int, float> OnMove { get; set; }

        void Move(IBoard board, IRythm rythm, Vector2Int location);
    }

    public class Participant : IParticipant
    {
        private ParticipantConfig config;
        private Vector2Int location;
        private Action<Vector2Int, Vector2Int, float> move;

        public ParticipantConfig Config => config;
        public Vector2Int Location => location;
        public Action<Vector2Int, Vector2Int, float> OnMove { get => move; set => move = value; }

        public Participant(ParticipantConfig configSetup, Vector2Int locationSetup)
        {
            config = configSetup;
            location = locationSetup;
        }

        public void Move(IBoard board, IRythm rythm, Vector2Int direction)
        {
            direction = board.GetBoardDirection(direction);

            Vector2Int toLocation = location + direction;
            location = board.GetBoardLocation(toLocation);
            move?.Invoke(toLocation, direction, rythm.Cadence / 2f);
        }
    }
}