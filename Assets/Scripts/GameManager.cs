using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    bool _isGameOver = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (IsGameOver() && Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    public void GameOver(bool gameOver)
    {
        _isGameOver = gameOver;
    }

    public bool IsGameOver()
    {
        return _isGameOver;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}

