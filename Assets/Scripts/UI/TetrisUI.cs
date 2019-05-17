using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TetrisUI : MonoBehaviour
{
    public GameObject tetris;
    public GameObject level;
    public GameObject linesCleared;
    public GameObject score;

    private TetrisGame tetris_;
    private Text level_;
    private Text linesCleared_;
    private Text score_;
        
    void Start()
    {
        tetris_ = tetris.GetComponent<TetrisGame>();
        level_ = level.GetComponent<Text>();
        linesCleared_ = linesCleared.GetComponent<Text>();
        score_ = score.GetComponent<Text>();
    }

    private void OnGUI()
    {
        level_.text = (tetris_.Level + 1).ToString();
        linesCleared_.text = tetris_.LinesCleared.ToString();
        score_.text = tetris_.Score.ToString();
    }
}
