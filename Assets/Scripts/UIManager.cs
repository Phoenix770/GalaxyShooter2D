using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Text _scoreText;
    [SerializeField] Image _livesImage;
    [SerializeField] Sprite[] _liveSprites;
    [SerializeField] Text _gameOverText;
    [SerializeField] Text _restartGameText;
    [SerializeField] Text _ammoText;

    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartGameText.gameObject.SetActive(false);
        _ammoText.text = "Ammo: " + 15 + " / " + 15;
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        if (currentLives < 0)
            currentLives = 0;
        _livesImage.sprite = _liveSprites[currentLives];
        if (currentLives == 0)
            StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        _gameOverText.gameObject.SetActive(true);
        _restartGameText.gameObject.SetActive(true);

        while (true)
        {
            _gameOverText.color = new Color32(255, 255, 255, 255);
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.color = new Color32(92, 1, 255, 255);
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void UpdateAmmo(int currAmmo, int maxAmmo)
    {
        _ammoText.text = "Ammo: " + currAmmo + " / " + maxAmmo;
    }
}
