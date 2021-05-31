using UnityEngine;

public class Bird : MonoBehaviour
{
    private Rigidbody2D birdRigidBody;
    private const int jumpForce = 16;

    private void Start()
    {
        birdRigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Jump();
        }
    }

    private void Jump()
    {
       birdRigidBody.velocity = Vector2.up * jumpForce;
    }
}
