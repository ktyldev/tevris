using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    public void LoadMain()
    {
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }
}
