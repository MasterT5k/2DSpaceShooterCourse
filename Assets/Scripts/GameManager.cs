using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isgameOver = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isgameOver == true)
            SceneManager.LoadScene(1);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            EditorApplication.ExitPlaymode();
        }
    }

    public void GameOver()
    {
        _isgameOver = true;
    }
}
