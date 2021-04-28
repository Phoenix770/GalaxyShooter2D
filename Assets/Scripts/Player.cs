using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _playerSpeed = 7f;

    void Start()
    {
        transform.position = new Vector3(0, -4f, 0);
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, vertical, 0);

        transform.Translate(direction * _playerSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.8f, 4.8f), 0f); ;

        if (transform.position.x > 11f || transform.position.x < -11f)
            transform.position = new Vector3(-transform.position.x, transform.position.y, 0);
    }
}
