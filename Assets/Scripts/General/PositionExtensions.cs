using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Extensions
{
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    public static class PositionExtensions
    {
        private static readonly Vector2Int[] Directions = new[]
        {
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left
        };

        public static Vector2Int GetNeighbour(this Vector2Int pos, Direction direction)
        {
            return pos + GetVector(direction);
        }

        public static Vector2Int GetVector(this Direction direction)
        {
            return Directions[(int)direction];
        }
    }
}