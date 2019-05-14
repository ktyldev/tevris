using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
using System.Linq;


// non-static
public class TetrisGame : MonoBehaviour
{
    public static TetrisGame Instance { get; private set; }

    public Piece[] piecePatterns;
    public GameObject tetromino;
    public GameObject grid;

    private TetrisBoard board_;
    private TetrisInput input_;

    private void Awake()
    {
        if (Instance != null)
            throw new System.Exception();

        Instance = this;

        board_ = grid.GetComponent<TetrisBoard>();
        input_ = GetComponent<TetrisInput>();
    }

    // Use this for initialization
    void Start()
    {
        input_.OnMove.AddListener(md => board_.MovePiece(md));
        input_.OnRotate.AddListener(rd => board_.RotatePiece(rd));
    }

    private int frames_ = 0;
    private bool dirty_ = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !dirty_)
        {
            board_.SpawnPiece(piecePatterns[0], Color.red);
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
        board_.Fall();
    }
}
