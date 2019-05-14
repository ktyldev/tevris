﻿using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBoard : MonoBehaviour
{

    public int rows;
    public int columns;
    public Color borderColour;

    private Tetromino[,] tetrominos_;
    private Piece activePiece_;

    public Vector2Int SpawnPos => new Vector2Int(columns / 2, rows - 1);
    public bool HasActivePiece => activePiece_ != null;

    private void Awake()
    {
        tetrominos_ = new Tetromino[columns, rows];
    }

    // Use this for initialization
    void Start()
    {
        CreateBorder();
    }

    private void CreateBorder()
    {
        var borders = new GameObject();
        borders.transform.SetParent(transform);
        borders.name = "Borders";

        var botBorder = GameObject.CreatePrimitive(PrimitiveType.Cube);
        botBorder.transform.position = new Vector3(columns / 2 - 0.5f, -1);
        botBorder.transform.localScale = new Vector3(columns + 2, 1, 1);
        botBorder.transform.SetParent(borders.transform);

        var topBorder = Instantiate(botBorder);
        topBorder.transform.position = new Vector3(columns / 2 - 0.5f, rows);
        topBorder.transform.SetParent(borders.transform);

        var leftBorder = GameObject.CreatePrimitive(PrimitiveType.Cube);
        leftBorder.transform.position = new Vector3(-1, rows / 2 - 0.5f);
        leftBorder.transform.localScale = new Vector3(1, rows, 1);
        leftBorder.transform.SetParent(borders.transform);

        var rightBorder = Instantiate(leftBorder);
        rightBorder.transform.position = new Vector3(columns, rows / 2 - 0.5f);
        rightBorder.transform.SetParent(borders.transform);

        foreach (var renderer in borders.GetComponentsInChildren<Renderer>())
        {
            renderer.material.color = borderColour;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Fall()
    {
        if (activePiece_ == null)
            return;

        var result = MovePiece(Direction.Down);
        if (!result)
        {
            DeactivatePiece();
        }
    }

    public void SpawnPiece(Piece pattern)
    {
        int x = SpawnPos.x;
        int y = SpawnPos.y;

        var newPiece = new Piece();
        newPiece.Position = new Vector2Int(x, y);
        newPiece.relativePositions = new Vector2Int[4];
        for (int i = 0; i < 4; i++)
        {
            newPiece.relativePositions[i] = pattern.relativePositions[i];
        }

        var tPositions = newPiece.TetrominoPositions;
        for (int i = 0; i < 4; i++)
        {
            var pos = tPositions[i];
            SpawnTetromino(pos.x, pos.y, pattern.colour);
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

        ClearLines();
    }

    public void ClearLines()
    {
        // check for cleared lines

        // start at the top
        for (int row = rows - 1; row >= 0; row--)
        {
            bool rowFull = true;
            for (int col = 0; col < columns; col++)
            {
                if (tetrominos_[col, row] == null)
                {
                    rowFull = false;
                }                
            }
            if (rowFull)
            {
                // if a line can be cleared... 
                print("clear row " + row);
                for (int col = 0; col < columns; col++)
                {
                    // ...remove it... 
                    var mino = tetrominos_[col, row];
                    Destroy(mino.gameObject);
                    tetrominos_[col, row] = null;
                }

                // ...and move all higher lines down
                for (int r = row + 1; r < rows; r++)
                {
                    for (int col = 0; col < columns; col++)
                    {
                        if (!IsOccupied(col, r))
                            continue;

                        MoveTetromino(new Vector2Int(col, r), Direction.Down);
                    }
                }
            }
        }

    }

    public void DropPiece()
    {
        if (activePiece_ == null)
            return;

        bool canDrop = true;
        while (canDrop)
        {
            canDrop = MovePiece(Direction.Down);
        }

        DeactivatePiece();
    }

    public bool MovePiece(MoveDirection mDir) =>
        MovePiece(mDir == MoveDirection.Left
            ? Direction.Left
            : Direction.Right);

    public bool MovePiece(Direction dir)
    {
        if (activePiece_ == null)
            return false;

        var oldPositions = new Vector2Int[4];
        Array.Copy(activePiece_.TetrominoPositions, oldPositions, 4);

        for (int i = 0; i < 4; i++)
        {
            var pos = oldPositions[i];
            var newPos = pos.GetNeighbour(dir);
            if (!IsPositionValid(newPos))
                return false;

            if (IsOccupied(newPos))
                return false;
        }

        // move is ok
        activePiece_.Position = activePiece_.Position.GetNeighbour(dir);

        // get the minos and remove them from the grid
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
        var newPositions = activePiece_.TetrominoPositions;
        for (int i = 0; i < 4; i++)
        {
            var pos = newPositions[i];
            tetrominos[i].transform.position = new Vector3(pos.x, pos.y);
            tetrominos_[pos.x, pos.y] = tetrominos[i];
        }

        return true;
    }

    public bool RotatePiece(RotateDirection rDir)
    {
        if (activePiece_ == null)
            return false;

        var rotation = rDir == RotateDirection.Anticlockwise
            ? Quaternion.AngleAxis(90, Vector3.forward)
            : Quaternion.AngleAxis(-90, Vector3.forward);

        var rotatedRelativePositions = new Vector2Int[4];
        for (int i = 0; i < 4; i++)
        {
            var pos = activePiece_.relativePositions[i];
            var relPos = new Vector3(pos.x, pos.y);
            relPos = rotation * relPos;

            rotatedRelativePositions[i] = new Vector2Int
            {
                x = Mathf.RoundToInt(relPos.x),
                y = Mathf.RoundToInt(relPos.y)
            };
        }

        // if the piece is at the top of the board it bumps into the upper
        // border. this can be accounted for by calculating how far past the 
        // border it is and moving it down 
        int moveOffset = 0;
        for (int i = 0; i < 4; i++)
        {
            var newPos = activePiece_.Position + rotatedRelativePositions[i];
            if (newPos.y >= rows)
            {
                moveOffset = Math.Max(moveOffset, newPos.y - rows + 1);
                continue;
            }

            if (IsOccupied(newPos))
                return false;
        }

        print(moveOffset);
        for (int i = 0; i < moveOffset; i++)
        {
            MovePiece(Direction.Down);
        }

        // all validation passed, time to move the pieces
        var tetrominos = new Tetromino[4];
        var oldPositions = activePiece_.TetrominoPositions;
        for (int i = 0; i < 4; i++)
        {
            var pos = oldPositions[i];
            var t = tetrominos_[pos.x, pos.y];
            if (t == null)
                throw new NullReferenceException();

            tetrominos[i] = t;
            tetrominos_[pos.x, pos.y] = null;
        }

        // rotate piece
        for (int i = 0; i < 4; i++)
        {
            activePiece_.relativePositions[i] = rotatedRelativePositions[i];
        }

        // put minos in new positions
        var newPositions = activePiece_.TetrominoPositions;
        for (int i = 0; i < 4; i++)
        {
            var pos = newPositions[i];
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
            return false;

        // ignore minos in the active piece
        if (activePiece_ != null)
        {
            var tPositions = activePiece_.TetrominoPositions;
            for (int i = 0; i < 4; i++)
            {
                var pos = tPositions[i];
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
