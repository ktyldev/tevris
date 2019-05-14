using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Piece
{
    public string name;
    public Color colour;
    public Vector2Int[] relativePositions;
    public Vector2Int Position { get; set; } // root position of the piece
    public Vector2Int[] TetrominoPositions
    {
        get
        {
            var result = new Vector2Int[4];
            for (int i = 0; i < 4; i++)
            {
                result[i] = Position + relativePositions[i];
            }

            return result;
        }
    }
}
