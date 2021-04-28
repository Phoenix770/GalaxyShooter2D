using UnityEngine;
using UnityEngine.SceneManagement;

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
    }

    public void GameOver(bool gameOver)
    {
        _isGameOver = gameOver;
    }

    public bool IsGameOver()
    {
        return _isGameOver;
    }
}

