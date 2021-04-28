using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Text _scoreText;
    [SerializeField] Image _livesImage;
    [SerializeField] Sprite[] _liveSprites;

    void Start()
    {
        _scoreText.text = "Score: " + 0;
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        _livesImage.sprite = _liveSprites[currentLives];
    }
}
