using System.Collections.Generic;
using System.Linq;
using Vector2Int = UnityEngine.Vector2Int;
using Random = UnityEngine.Random;
using UnityEngine;
using System;

namespace Core
{
    public interface IBoard
    {
        Vector2Int Size { get; }
        Vector2Int CenterLocation { get; }
        Vector2Int GetEmptyLocation(List<IParticipant> participants);
        Vector2Int GetBoardLocation(Vector2Int location);
        Vector2Int GetBoardDirection(Vector2Int direction);
    }

    public class Board : IBoard
    {
        public Vector2Int size;

        private const int BOARD_SIZE = 9;
        public int LocationsCount => size.x * size.y;

        public Vector2Int Size => size;
        public Vector2Int CenterLocation => new Vector2Int(Mathf.FloorToInt(size.x / 2f), Mathf.FloorToInt(size.y / 2f));

        public Board()
        {
            size = new Vector2Int(BOARD_SIZE, BOARD_SIZE);
        }

        public Vector2Int GetBoardLocation(Vector2Int location)
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

        public Vector2Int GetBoardDirection(Vector2Int direction)
        {
            if (Mathf.Abs(direction.x) > 1)
                direction.x -= Math.Sign(direction.x) * size.x;

            else if (Mathf.Abs(direction.y) > 1)
                direction.y -= Math.Sign(direction.y) * size.x;

            return direction;
        }
    }
}