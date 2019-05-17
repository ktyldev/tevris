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
    public float baseTickLength;
    public float softDropTickLength;
    public int linesPerLevel;
    [Range(0, 1)]
    public float levelTickMultiplier;
 
    private TetrisBoard board_;
    private static SoundEngine soundEngine_;
    private TetrisInput input_;
    private int ticks_ = 0;
    private bool gameOver_;
    private float lastTick_ = 0;
    private float nextTick_ = 0;

    public int Level { get; private set; }

    public UnityEvent OnGameOver { get; set; } = new UnityEvent();

    private void Awake()
    {
        if (Instance != null)
            throw new System.Exception();

        Instance = this;

        board_ = grid.GetComponent<TetrisBoard>();
        input_ = GetComponent<TetrisInput>();

        tickLength_ = baseTickLength;

        PrintLevel();
        board_.LineCleared.AddListener(i =>
        {
            if (i % linesPerLevel == 0)
            {
                Level++;
                tickLength_ = baseTickLength * Mathf.Pow(levelTickMultiplier, Level);
                PrintLevel();
            }
        });
    }

    private void PrintLevel()
    {
        var msg = string.Format("level {0}\ttick length: {1}", Level, tickLength_);
        print(msg);
    }

    private float lastInput_ = 0;
    // Use this for initialization
    void Start()
    {
        input_.OnStartGame.AddListener(StartGame);
        input_.OnEndGame.AddListener(GameOver);
        soundEngine_ = SoundEngine.GetEngine();

        input_.OnMove.AddListener(md =>
        {
            board_.MovePiece(md);
            lastInput_ = Time.time;
            soundEngine_.PlaySFX(GameConstants.SFXTetrisMove, false);
        });
        input_.OnRotate.AddListener(rd =>
        {
            board_.RotatePiece(rd);
            lastInput_ = Time.time;
            soundEngine_.PlaySFX(GameConstants.SFXTetrisRotate, false);
        });
        input_.OnDrop.AddListener(() =>
        {
            board_.DropPiece();
            lastInput_ = Time.time;
            soundEngine_.PlaySFX(GameConstants.SFXTetrisDrop);
        });
    }

    private float tickLength_;
    private void Update()
    {
        if (input_.SoftDropStart)
            soundEngine_.PlaySFX(GameConstants.SFXTetrisSoftDrop);

        if (input_.SoftDrop)
        {
            nextTick_ = lastTick_ + softDropTickLength;
        }

        if (Time.time < nextTick_)
            return;

        lastTick_ = Time.time;
        nextTick_ = lastTick_ + tickLength_;

        if (board_.ActivePieceLanded)
        {
            if (Time.time - lastInput_ < tickLength_)
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
                : baseTickLength;

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

    public void RestartGame()
    {
        board_.Clear();
        gameOver_ = false;
    }

    public IEnumerator DoGameOver()
    {
        bool done = false;
        UnityAction listener = () => done = true;

        input_.OnContinue.AddListener(listener);
        gameOver_ = true;

        // TODO: start pieces flashing
        OnGameOver.Invoke();

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
