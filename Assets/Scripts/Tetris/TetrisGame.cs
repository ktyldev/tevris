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
    public float tickLength;
    public float softDropTickLength;

    private TetrisBoard board_;
    private TetrisInput input_;
    private int ticks_ = 0;
    private bool gameOver_;

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
        input_.OnStartGame.AddListener(StartGame);
        input_.OnEndGame.AddListener(GameOver);

        input_.OnMove.AddListener(md => board_.MovePiece(md));
        input_.OnRotate.AddListener(rd => board_.RotatePiece(rd));
        input_.OnDrop.AddListener(board_.DropPiece);
    }

    private IEnumerator Run()
    {
        while (!gameOver_)
        {
            float waitTime = input_.SoftDrop 
                ? softDropTickLength
                : tickLength;

            float elapsed = 0;
            while (elapsed < waitTime)
            {
                if (input_.SoftDropStart)
                    break;

                elapsed += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            Tick();
        }
    }

    private void Tick()
    {
        if (gameOver_)
            return;

        board_.Fall();

        if (!board_.HasActivePiece)
        {
            int r = Random.Range(0, piecePatterns.Length);
            board_.SpawnPiece(piecePatterns[r]);
        }

        ticks_++;
    }

    public void StartGame()
    {
        gameOver_ = false;
        StartCoroutine(Run());
    }

    public void GameOver()
    {
        gameOver_ = true;
        board_.Clear();
    }
}
