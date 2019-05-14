using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBoard : MonoBehaviour {

    public int rows;
    public int columns;

    private Tetromino[,] tetrominos_;   
    private Piece? activePiece_;

    public Vector2Int SpawnPos => new Vector2Int(columns / 2, rows - 1);

    private void Awake()
    {
        tetrominos_ = new Tetromino[columns, rows];
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Fall()
    {
        if (!activePiece_.HasValue)
            return;

        var result = MovePiece(Direction.Down);
        if (!result)
        {
            DeactivatePiece();
        }
    }

    public void SpawnPiece(Piece pattern, Color colour)
    {
        int x = SpawnPos.x;
        int y = SpawnPos.y;

        var newPiece = new Piece();
        newPiece.tetrominoPositions = new Vector2Int[4];
        for (int i = 0; i < 4; i++)
        {
            var pos = pattern.tetrominoPositions[i];

            int tx = x + pos.x;
            int ty = y + pos.y;

            SpawnTetromino(tx, ty, colour);
            newPiece.tetrominoPositions[i] = new Vector2Int(tx, ty);
        }

        activePiece_ = newPiece;
    }

    public void SpawnTetromino(int x, int y, Color colour)
    {
        this.Instantiate<Tetromino>(
            TetrisGame.Instance.tetromino,
            Vector3.zero,
            Quaternion.identity,
            transform,
            t =>
            {
                tetrominos_[x, y] = t;
                t.transform.position = new Vector3(x, y);

                t.Colour = colour;
            });
    }

    public void DeactivatePiece()
    {
        activePiece_ = null;
    }

    public bool MovePiece(Direction dir)
    {
        if (!activePiece_.HasValue)
            return false;

        var oldPositions = new Vector2Int[4];
        Array.Copy(activePiece_.Value.tetrominoPositions, oldPositions, 4);

        var newPositions = new Vector2Int[4];
        for (int i = 0; i < 4; i++)
        {
            var pos = oldPositions[i];
            var newPos = pos.GetNeighbour(dir);
            if (!IsPositionValid(newPos))
                return false;

            if (IsOccupied(newPos))
                return false;

            newPositions[i] = newPos;
        }

        // new positions have been set, time to get the minos and move them
        var tetrominos = new Tetromino[4];
        for (int i = 0; i < 4; i++)
        {
            var pos = oldPositions[i];
            var t = tetrominos_[pos.x, pos.y];
            if (t == null)
                throw new NullReferenceException();

            tetrominos[i] = t;
            tetrominos_[pos.x, pos.y] = null;
        }

        // put minos in new positions
        for (int i = 0; i < 4; i++)
        {
            var pos = newPositions[i];
            activePiece_.Value.tetrominoPositions[i] = pos;
            tetrominos[i].transform.position = new Vector3(pos.x, pos.y);
            tetrominos_[pos.x, pos.y] = tetrominos[i];
        }

        return true;
    }

    public void MoveTetromino(Vector2Int start, Direction dir) =>
        MoveTetromino(start, dir.GetVector());

    public void MoveTetromino(Vector2Int start, Vector2Int dir) =>
        SetTetrominoPositon(start, start + dir);

    public void SetTetrominoPositon(Vector2Int start, Vector2Int end)
    {
        if (IsOccupied(end))
            throw new Exception();

        int x = start.x;
        int y = start.y;

        var t = tetrominos_[x, y];
        if (t == null)
            throw new Exception();

        tetrominos_[x, y] = null;

        x = end.x;
        y = end.y;

        tetrominos_[x, y] = t;
        t.transform.position = new Vector3(x, y);
    }
        
    public bool IsNeighbourOccupied(Vector2Int pos, Direction dir) =>
        IsOccupied(pos.GetNeighbour(dir));

    public bool IsOccupied(Vector2Int pos) =>
        IsOccupied(pos.x, pos.y);

    public bool IsOccupied(int x, int y)
    {
        if (!IsPositionValid(x, y))
        {
            var msg = string.Format("({0},{1}) is not a valid position", x, y);
            throw new System.Exception(msg);
        }

        // ignore minos in the active piece
        if (activePiece_.HasValue)
        {
            for (int i = 0; i < 4; i++)
            {
                var pos = activePiece_.Value.tetrominoPositions[i];
                if (pos.x == x && pos.y == y)
                    return false;
            }
        }

        return tetrominos_[x, y] != null;
    }

    public bool IsPositionValid(Vector2Int pos) =>
        IsPositionValid(pos.x, pos.y);

    public bool IsPositionValid(int x, int y) =>
        x.InRange(0, columns) && y.InRange(0, rows);
}
