using UnityEngine;

public class ShieldBehavior : MonoBehaviour
{
    bool _shieldsActive = false;
    int _shieldsHP = 3;

    void Start()
    {
        _shieldsHP = gameObject.GetComponent<Animator>().GetParameter(0).defaultInt;
    }

    public bool AreShieldsActive()
    {
        return _shieldsActive;
    }

    public void ActivateShields()
    {
        gameObject.SetActive(_shieldsActive = true);
        gameObject.GetComponent<Animator>().SetInteger("ShieldStrength", _shieldsHP = 3);
    }

    public void DamageShields()
    {
        _shieldsHP--;
        if (_shieldsHP == 0)
            gameObject.SetActive(_shieldsActive = false);
        gameObject.GetComponent<Animator>().SetInteger("ShieldStrength", _shieldsHP);
    }
}
