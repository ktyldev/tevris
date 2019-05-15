using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
using System.Linq;
using UnityEngine.Events;


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
    private float lastTick_ = 0;
    private float nextTick_ = 0;

    private void Awake()
    {
        if (Instance != null)
            throw new System.Exception();

        Instance = this;

        board_ = grid.GetComponent<TetrisBoard>();
        input_ = GetComponent<TetrisInput>();
    }

    private float lastInput_ = 0;
    // Use this for initialization
    void Start()
    {
        input_.OnStartGame.AddListener(StartGame);
        input_.OnEndGame.AddListener(GameOver);

        input_.OnMove.AddListener(md =>
        {
            board_.MovePiece(md);
            lastInput_ = Time.time;
        });
        input_.OnRotate.AddListener(rd =>
        {
            board_.RotatePiece(rd);
            lastInput_ = Time.time;
        });
        input_.OnDrop.AddListener(() =>
        {
            board_.DropPiece();
            lastInput_ = Time.time;
        });
    }

    private void Update()
    {
        if (input_.SoftDrop)
        {
            nextTick_ = lastTick_ + softDropTickLength;
        }

        if (Time.time < nextTick_)
            return;

        lastTick_ = Time.time;
        nextTick_ = lastTick_ + tickLength;

        if (board_.ActivePieceLanded)
        {
            if (Time.time - lastInput_ < tickLength)
                return;

            board_.DeactivatePiece();
        }
            
        Tick();
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

    private bool pieceLanded_ = false;
    private void Tick()
    {
        if (gameOver_)
            return;

        board_.Fall();

        if (!board_.HasActivePiece)
        {
            int r = Random.Range(0, piecePatterns.Length);
            if (!board_.SpawnPiece(piecePatterns[r]))
            {
                StartCoroutine(DoGameOver());
                return;
            }
        }

        ticks_++;
    }

    public void StartGame()
    {
        gameOver_ = false;
    }

    public IEnumerator DoGameOver()
    {
        bool done = false;
        UnityAction listener = () => done = true;

        input_.OnContinue.AddListener(listener);
        gameOver_ = true;

        // TODO: start pieces flashing

        yield return new WaitUntil(() => done);

        board_.Clear();
        input_.OnContinue.RemoveListener(listener);
    }

    public void GameOver()
    {
        gameOver_ = true;
        board_.Clear();
    }
}
