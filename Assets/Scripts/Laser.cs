using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] float _speed = 9f;

    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > 7f)
        {
            if (transform.parent.name == "TripleLaserShot(Clone)")
                Destroy(transform.parent.gameObject);
            else
                Destroy(gameObject);
        }
    }
}
