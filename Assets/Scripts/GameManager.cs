using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    bool _isGameOver = false;

    private void Awake()
    {
        Instance = this;
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

