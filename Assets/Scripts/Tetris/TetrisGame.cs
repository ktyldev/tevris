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

        StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        bool gameOver = false;
        while (!gameOver)
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
        board_.Fall();

        if (!board_.HasActivePiece)
        {
            board_.SpawnPiece(piecePatterns[0], Color.red);
        }
    }
}
