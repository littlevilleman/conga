using System;
using UnityEngine;

namespace Core.Tetris
{
    public interface IBoard
    {
    }

    public class Board : IBoard
    {
        private Vector2Int size;
        public Vector2Int Size => size;

        public Board(Vector2Int size)
        {
            this.size = size;
        }
    }


    public interface IPiece
    {
        void Rotate(int direction);
        void Push();
        void Setup(PieceType type);
    }

    public  class Piece : IPiece
    {
        public Vector2Int[] blocks;

        public Piece(PieceConfig config)
        {

        }

        public void Push()
        {
        }

        public void Rotate(int direction)
        {
        }

        public void Setup(PieceType type)
        {
            blocks = BuildBlocks(type);
        }

        private Vector2Int[] BuildBlocks(PieceType type)
        {
            switch (type)
            {
                case PieceType.SQUARE: return new Vector2Int[] { Vector2Int.zero, Vector2Int.up, Vector2Int.up + Vector2Int.right, Vector2Int.right};
                case PieceType.LINE: return new Vector2Int[] { Vector2Int.zero, Vector2Int.up, Vector2Int.up * 2, Vector2Int.up * 3};
                case PieceType.L_RIGHT: return new Vector2Int[] { Vector2Int.zero, Vector2Int.up, Vector2Int.up * 2, Vector2Int.up * 2 + Vector2Int.right};
                case PieceType.L_LEFT: return new Vector2Int[] { Vector2Int.zero, Vector2Int.up, Vector2Int.up * 2, Vector2Int.up * 2 + Vector2Int.left };
                case PieceType.Z_RIGHT: return new Vector2Int[] {};
                case PieceType.Z_LEFT: return new Vector2Int[] {};
            }

            return new Vector2Int[]
            {


            };
        }
    }

    public class PieceConfig : ScriptableObject, IPieceFactory
    {
        public PieceType type;
        public Color color;

        public IPiece Build(Vector2Int location)
        {
            IPiece piece = new Piece(this);
            piece.Setup(type);
            return piece;
        }
    }
    public interface IPieceFactory
    {
        public IPiece Build(Vector2Int location);

    }

    public enum PieceType
    {
        SQUARE, L_LEFT, L_RIGHT, Z_LEFT, Z_RIGHT, LINE
    }
}