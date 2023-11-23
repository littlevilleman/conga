using System.Collections.Generic;
using System.Linq;
using Vector2Int = UnityEngine.Vector2Int;
using Random = UnityEngine.Random;

namespace Core
{
    public interface IBoard
    {
        Vector2Int GetEmptyLocation(List<IParticipant> participants);
        bool IsValidLocation(Vector2Int lcoation);
        Vector2Int OverrideDirection(Vector2Int location, Vector2Int direction);
    }

    public class Board : IBoard
    {
        public Vector2Int size;

        public int LocationsCount => size.x * size.y;

        private static Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.left, Vector2Int.down, };

        public Board(int sizeSetup)
        {
            size = new Vector2Int(sizeSetup, sizeSetup);
        }

        public bool IsValidLocation(Vector2Int location)
        {
            if (location.x >= size.x || location.x < 0)
                return false;

            if (location.y >= size.y || location.y < 0)
                return false;

            return true;
        }

        public Vector2Int OverrideDirection(Vector2Int location, Vector2Int direction)
        {
            if (IsValidLocation(location + direction))
                return direction;

            foreach (var d in directions)
            {
                if (IsValidLocation(location + d))
                    return d;
            }

            return direction;
        }

        public Vector2Int GetEmptyLocation(List<IParticipant> participants)
        {
            List<Vector2Int> emptyLocations = new List<Vector2Int>();
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector2Int emptyLocation = new Vector2Int(x, y);
                    if (!participants.Any(x => x.Location == emptyLocation))
                        emptyLocations.Add(emptyLocation);
                }
            }

            int random = Random.Range(0, emptyLocations.Count);
            return emptyLocations[random];
        }
    }
}