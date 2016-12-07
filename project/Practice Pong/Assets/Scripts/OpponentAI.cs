using UnityEngine;
using System.Collections;

public class OpponentAI : MonoBehaviour {

    // Declare some variables
    GameObject ball;
    Ball ballScript;
    Rigidbody2D rbBall;
    Rigidbody2D rb;

    // The calculated point to which the ball will go
    float collisionPointY;

    void Start () {
        // Find the ball and get its components
        ball = GameObject.FindGameObjectWithTag("Balls");
        rbBall = ball.GetComponent<Rigidbody2D>();
        ballScript = ball.GetComponent<Ball>();

        // Get the paddle rigidbody
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        // Reset the prediction point during the frame where the rally is starting
        if (GameManager.instance.rallyStart) {
            collisionPointY = -1.5f;
        }
    }

	void FixedUpdate () {
        // If ball just changed direction towards the AI
        if (ballScript.justChangedDirection && ballScript.leftRight == -1) {
            // Caculate the total Y distance the ball will travel
            float velocityAngle = Mathf.Deg2Rad * Vector2.Angle(new Vector2(-1f, 0f), rbBall.velocity);
            float rawMovementX = rbBall.position.x + 21.5f;
            float rawMovementY = rawMovementX * Mathf.Tan(velocityAngle);

            // Use that distance to calculate a point in world space
            if (rbBall.velocity.y > 0) {
                collisionPointY = rbBall.position.y + rawMovementY;
            } else {
                collisionPointY = rbBall.position.y - rawMovementY;
            }

            // While the point isn't in the bounds, reflect it inwards across y = 8.75 or y = -11.75
            while (collisionPointY > 8.75 || collisionPointY < -11.75) {
                if (collisionPointY > 8.75) {
                    collisionPointY = -collisionPointY + 17.5f;
                } else {
                    collisionPointY = -collisionPointY - 23.5f;
                }
            }
            
            // The collision point is now calculated

            // Change the trigger back to false
            ballScript.justChangedDirection = false;
        }

        // Declare a frame-specific velocity
        Vector2 velocity;

        // If the collision point is further than 2 away, set the velocity to 1
        if (collisionPointY - rb.position.y > 2) {
            velocity = new Vector2(0f, 1f);
        } else if (collisionPointY - rb.position.y < -2) {
            velocity = new Vector2(0f, -1f);
        } 

        // If the collision point is close, slow the movement down
        else {
            velocity = new Vector2(0f, (collisionPointY - rb.position.y) / 2);
        }

        // Caculate and apply the new position
        Vector2 newPosition = rb.position + velocity * 15 * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
	}
}
