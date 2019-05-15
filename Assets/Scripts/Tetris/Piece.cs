using Extensions;
using System;
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

    private List<Vector2Int> neighbourPositions_ = new List<Vector2Int>();
    public Vector2Int[] NeighbourPositions
    {
        get
        {
            neighbourPositions_.Clear();

            var dirs = Enum.GetValues(typeof(Direction));
            var tPositions = TetrominoPositions;

            // for each direction
            for (int d = 0; d < 4; d++)
            {
                var direction = (Direction)dirs.GetValue(d);

                // for each mino
                for (int t = 0; t < 4; t++)
                {
                    var neighbour = tPositions[t].GetNeighbour(direction);
                    bool success = true;
                    // test that the neighbour position isn't the piece itself
                    for (int t2 = 0; t2 < 4; t2++)
                    {
                        if (neighbour == tPositions[t2])
                        {
                            success = false;
                            break;
                        }
                    }

                    if (!success || neighbourPositions_.Contains(neighbour))
                        continue;

                    neighbourPositions_.Add(neighbour);
                }
            }

            return neighbourPositions_.ToArray();
        }
    }
}
