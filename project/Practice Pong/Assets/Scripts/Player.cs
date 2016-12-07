using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    // Some variable declarations
    Rigidbody2D rb;
    public float movementSpeed;
    
	void Start () {
        // Get the rigid body
        rb = GetComponent<Rigidbody2D>();
	}
	
    void Update () {
        // Increase speed on shift
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            movementSpeed *= 2.5f;
        }

        // Decrease speed on shift release
        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            movementSpeed *= .4f;
        }
    }
    
	void FixedUpdate () {
        // Get a direction and caculate new position
        Vector2 direction = new Vector2(0f, Input.GetAxisRaw("Vertical"));
        Vector2 newPosition = rb.position + direction * movementSpeed * Time.fixedDeltaTime;

        // Clamp the new position to a good space
        newPosition = new Vector2(newPosition.x, Mathf.Clamp(newPosition.y, -9.75f, 6.75f));

        // Apply the new position
        rb.MovePosition(newPosition);
	}
}
