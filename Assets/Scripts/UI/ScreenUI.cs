using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenUI : MonoBehaviour
{
    public GameObject gameOverMenu;

	void Start ()
    {
        var tetris = TetrisGame.Instance;
        tetris.OnGameOver.AddListener(() => gameOverMenu.SetActive(true));

        gameOverMenu.SetActive(false);
	}
	
	void Update ()
    {
		
	}

    public void Exit()
    {
        Application.Quit();
    }
}
