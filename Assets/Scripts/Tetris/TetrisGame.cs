using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
using System.Linq;


// non-static
public class TetrisGame : MonoBehaviour
{
    public static TetrisGame Instance { get; private set; }

    public int rows, columns;
    public Piece[] piecePatterns;
    public GameObject tetromino;
    public GameObject grid;

    private Tetromino[,] tetrominos_;
    private Tetromino[,] fallenTetrominos_;
    private Piece? activePiece_;

    private void Awake()
    {
        tetrominos_ = new Tetromino[columns, rows];
        fallenTetrominos_ = new Tetromino[columns, rows];

        if (Instance != null)
            throw new System.Exception();

        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
    }

    private int frames_ = 0;
    private bool dirty_ = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !dirty_)
        {
            SpawnPiece();
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
        Fall();
    }
    
    private void Fall()
    {
        if (!activePiece_.HasValue)
            return;

        var result = MovePiece(Direction.Down);
        if (!result)
        {
            for (int i = 0; i < 4; i++)
            {
                var pos = activePiece_.Value.tetrominoPositions[i];
                fallenTetrominos_[pos.x, pos.y] = tetrominos_[pos.x, pos.y];
            }

            activePiece_ = null;
        }
    }

    private bool MovePiece(Direction dir)
    {
        if (!activePiece_.HasValue)
            return false; 

        var positions = new Vector2Int[4];
        for (int i = 0; i < 4; i++)
        {
            positions[i] = activePiece_.Value.tetrominoPositions[i];
        }

        foreach (var pos in positions)
        {
            var newPos = pos.GetNeighbour(dir);
            if (!IsPositionValid(newPos))
                return false;

            // can't move that way, don't do anything
            if (IsNeighbourOccupied(pos, dir))
                return false;
        }

        // get the tetrominos for the active piece
        var tetrominos = new Tetromino[4];
        var newPositions = new Vector2Int[4];
        for (int i = 0; i < 4; i++)
        {
            var pos = positions[i];
            newPositions[i] = pos.GetNeighbour(dir);

            var t = tetrominos_[pos.x, pos.y];
            if (t == null)
                throw new System.Exception(string.Format("{0} is null", pos));

            tetrominos[i] = t;
            tetrominos_[pos.x, pos.y] = null;
        }

        // put the tetrominos in the new position
        for (int i = 0; i < 4; i++)
        {
            var pos = newPositions[i];
            activePiece_.Value.tetrominoPositions[i] = pos;
            tetrominos[i].transform.position = new Vector3(pos.x, pos.y);
            tetrominos_[pos.x, pos.y] = tetrominos[i];
        }

        return true;
    }

    private void MoveTetromino(Vector2Int start, Direction dir)
    {
        MoveTetromino(start, start.GetNeighbour(dir));
    }

    private void MoveTetromino(Vector2Int start, Vector2Int dir)
    {
        SetTetrominoPosition(start, start + dir);
    }

    private void SetTetrominoPosition(Vector2Int start, Vector2Int end)
    {
        if (IsOccupied(end))
            throw new System.Exception();

        int x = start.x;
        int y = start.y;

        var t = tetrominos_[x, y];
        if (t == null)
            throw new System.Exception();

        tetrominos_[x, y] = null;

        x = end.x;
        y = end.y;

        tetrominos_[x, y] = t;
        t.transform.position = new Vector3(x, y);
    }

    private void SpawnPiece()
    {
        var pattern = piecePatterns[0];

        int x = columns / 2 - 1;
        int y = rows - 1;

        Piece newPiece = new Piece();
        newPiece.tetrominoPositions = new Vector2Int[4];
        for (int i = 0; i < 4; i++)
        {
            var pos = pattern.tetrominoPositions[i];

            int tx = x + pos.x;
            int ty = y + pos.y;

            SpawnTetronimo(tx, ty);
            newPiece.tetrominoPositions[i] = new Vector2Int(tx, ty);
        }

        activePiece_ = newPiece;
    }

    private void SpawnTetronimo(int x, int y)
    {
        this.Instantiate<Tetromino>(
            TetrisGame.Instance.tetromino,
            Vector3.zero,
            Quaternion.identity,
            grid.transform,
            t =>
            {
                tetrominos_[x, y] = t;
                t.transform.position = new Vector3(x, y);

                t.Colour = Color.red;
            });
    }

    public bool IsNeighbourOccupied(Vector2Int pos, Direction dir)
    {
        return IsOccupied(pos.GetNeighbour(dir));
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

        return fallenTetrominos_[x, y] != null;
    }

    public bool IsPositionValid(Vector2Int pos)
    {
        return IsPositionValid(pos.x, pos.y);
    }

    public bool IsPositionValid(int x, int y)
    {
        return x.InRange(0, columns) && y.InRange(0, rows);
    }
}
