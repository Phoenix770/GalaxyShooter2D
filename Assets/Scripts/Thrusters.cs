using UnityEngine;
using UnityEngine.UI;

public class Thrusters : MonoBehaviour
{
    [SerializeField] Text _overheatedText;

    Slider _thrusterBar;

    private void Start()
    {
        _thrusterBar = GetComponent<Slider>();
        _thrusterBar.maxValue = 250;
        _thrusterBar.value = 250;
        _overheatedText.gameObject.SetActive(false);
    }

    public void SetValue(int value)
    {
        _thrusterBar.value = value;
    }

    public void Overheated(bool value)
    {
        _overheatedText.gameObject.SetActive(value);
    }
}