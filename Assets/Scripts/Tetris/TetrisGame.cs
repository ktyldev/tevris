using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public enum Direction
{
    Up,
    Right,
    Down,
    Left
}
// non-static
public class TetrisGame : MonoBehaviour
{
    private static readonly Vector2Int[] Directions = new[] 
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };
    public static Vector2Int GetDirection(Direction dir) => Directions[(int)dir];

    public static TetrisGame Instance { get; private set; }

    public int rows, columns;
    public GameObject tetromino;
    public GameObject grid;

    private Tetromino[,] tetrominos_;

    private void Awake()
    {
        tetrominos_ = new Tetromino[columns, rows];

        if (Instance != null)
            throw new System.Exception();

        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        SpawnTetronimo();
    }

    private int frames_ = 0;
    private bool dirty_ = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !dirty_)
        {
            SpawnTetronimo();
            dirty_ = true;
        }

        if (frames_ % 30 == 0)
        {
            Tick();
            dirty_ = false;
        }
        frames_++;
    }

    private void Tick()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                var pos = new Vector2Int(col, row);

                if (!IsOccupied(pos))
                    continue;

                // fall
                if (pos.y >= 1)   // can't fall on the bottom row
                {
                    if (!IsNeighbourOccupied(pos, Direction.Down))
                    {
                        MoveTetromino(pos, Vector2Int.down);
                    }
                }
            }
        }
    }

    private void MoveTetromino(Vector2Int start, Vector2Int dir)
    {
        if (!IsOccupied(start))
            throw new System.Exception("no tetronimo at " + start);

        var end = start + dir;

        SetTetrominoPosition(start, end);
    }

    private void SetTetrominoPosition(Vector2Int start, Vector2Int end)
    {
        if (IsOccupied(end))
            throw new System.Exception();

        int x = start.x;
        int y = start.y;

        var t = tetrominos_[x, y];
        tetrominos_[x, y] = null;

        x = end.x;
        y = end.y;

        tetrominos_[x, y] = t;
        t.transform.position = new Vector3(x, y);
    }

    private void SpawnTetronimo()
    {
        int x = columns / 2 - 1;
        int y = rows - 1;

        var t = this.Instantiate<Tetromino>(
            TetrisGame.Instance.tetromino,
            Vector3.zero,
            Quaternion.identity,
            grid.transform);

        tetrominos_[x, y] = t;
        t.transform.position = new Vector3(x, y);

        t.Colour = Color.red;
    }

    public bool IsNeighbourOccupied(Vector2Int pos, Direction dir)
    {
        return IsOccupied(pos + GetDirection(dir));
    }

    public bool IsOccupied(Vector2Int pos)
    {
        return IsOccupied(pos.x, pos.y);
    }
            
    public bool IsOccupied(int x, int y)
    {
        if (!IsPositionValid(x, y))
        {
            string message = string.Format("({0},{1}) is not a valid position", x, y);
            throw new System.Exception(message);
        }
            
        return tetrominos_[x, y] != null;
    }

    public bool IsPositionValid(int x, int y)
    {
        return x.InRange(0, columns) && y.InRange(0, rows);
    }
}
