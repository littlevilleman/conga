using System.Collections.Generic;
using System.Linq;
using Vector2Int = UnityEngine.Vector2Int;
using Random = UnityEngine.Random;

namespace Core
{
    public interface IBoard
    {
        Vector2Int GetEmptyLocation(List<IParticipant> participants);
        Vector2Int OverrideLocation(Vector2Int lcoation);
    }

    public class Board : IBoard
    {
        public Vector2Int size;

        public int LocationsCount => size.x * size.y;

        public Board(int sizeSetup)
        {
            size = new Vector2Int(sizeSetup, sizeSetup);
        }

        public Vector2Int OverrideLocation(Vector2Int location)
        {
            if (location.x < 0)
                return location + Vector2Int.right * size.x;

            if (location.x >= size.x)
                return location - Vector2Int.right * size.x;

            if (location.y < 0)
                return location + Vector2Int.up * size.y;

            if (location.y >= size.y)
                return location - Vector2Int.up * size.y;

            return location;
        }

        //TODO
        public Vector2Int GetEmptyLocation(List<IParticipant> participants)
        {
            List<Vector2Int> emptyLocations = new List<Vector2Int>();
            List<Vector2Int> participantsLocations = participants.Select(x => x.Location).ToList();
            
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector2Int emptyLocation = new Vector2Int(x, y);
                    if (!participantsLocations.Contains(emptyLocation))
                        emptyLocations.Add(emptyLocation);
                }
            }

            int random = Random.Range(0, emptyLocations.Count);
            return emptyLocations[random];
        }
    }
}