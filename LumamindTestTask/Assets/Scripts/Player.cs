using UnityEngine;

public class Player : MonoBehaviour
{
    private const int speed = 700;

    [SerializeField]
    private Joystick joystick;
    [SerializeField]
    private Rigidbody2D rb;

    private void Update()
    {
        rb.velocity = new Vector2(joystick.Horizontal, joystick.Vertical) * speed * Time.deltaTime;
    }
}
