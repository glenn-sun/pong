using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

    // Some variable declarations
    Rigidbody2D rb;
    public float startVelocityMultiplier;
    public float velocityScale;

    // Variables used in opponent AI
    [HideInInspector] public bool justChangedDirection;
    [HideInInspector] public int leftRight;

    bool isTimeToLoad;
    bool isTimeToPlay;

    int debug = 0;

	void Start () {
        // Get the rigidbody
        rb = GetComponent<Rigidbody2D>();
        
        StartCoroutine(GameManager.instance.ReadyGo());
        isTimeToLoad = false;
        isTimeToPlay = true;
        StartCoroutine(GameManager.instance.RallyIsStarting());
	}

    void Update() {
        if (isTimeToLoad && GameManager.instance.gameIsRunning) {
            ReloadScene();
            isTimeToLoad = false;
            isTimeToPlay = true;
            StartCoroutine(GameManager.instance.ReadyGo());
            
        } else if (isTimeToPlay && GameManager.instance.gameIsRunning) {
            PlayBall();
            isTimeToPlay = false;
            
        }
    }

    void PlayBall () {
        // Generate a normalized starting velocity
        leftRight = (Random.Range(0, 2) == 0) ? -1 : 1;
        Vector2 startVelocity = new Vector2((Random.Range(0, .5f) + .5f) * leftRight, Random.Range(-.3f, .3f));

        // Apply the starting velocity
        rb.velocity = startVelocity.normalized * startVelocityMultiplier;

        // Make sure y velocity is never 0
        if (rb.velocity.y == 0) {
            rb.velocity = new Vector2(rb.velocity.x, 0.001f);
        }
        
        // Tell AI that the direction changed
        justChangedDirection = true;
    }

    void ReloadScene () {
        // Reset the velocity and position of the ball
        rb.velocity = new Vector2(0f, 0f);
        gameObject.transform.position = new Vector2(0f, -1.5f);

        // Start the rally
        StartCoroutine(GameManager.instance.RallyIsStarting());
    }

    // To execute directly after bouncing off
    void OnCollisionExit2D(Collision2D coll) {
        // If collision is with a paddle
        if (coll.gameObject.tag == "Player" || coll.gameObject.tag == "Opponent") {

            // Get the difference in contact points
            Vector2 contact = coll.contacts[0].point;
            Vector2 center = coll.gameObject.transform.position;
            float difference = contact.y - center.y;

            // Identify new general direction
            leftRight = (center.x < 0) ? 1 : -1;

            // Apply difference to a velocity augmentation
            Vector2 newVelocity = new Vector2(1.5f * leftRight, difference);

            // Generate a speed factor
            float speedFactor = (Mathf.Abs(difference) / 2 + .85f);

            // Augment current velocity with directional velocity
            rb.velocity = (rb.velocity * (1 - velocityScale) + newVelocity.normalized * startVelocityMultiplier * velocityScale) * speedFactor;

            // Make sure y velocity is never 0
            if (rb.velocity.y == 0) {
                rb.velocity = new Vector2(rb.velocity.x, 0.001f);
            }

            // Tell AI that the direction changed
            justChangedDirection = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        // If the ball enters opponent bounds
        if (other.tag == "Opponent Bounds") {
            // Give the player a point
            GameManager.instance.IncrementPlayerScore();

            //StartCoroutine(GameManager.instance.PauseGameFor(1f));

            // Reload scene
            isTimeToLoad = true;
        } 
        // If the ball enters player bounds
        else if (other.tag == "Player Bounds") {
            // Give the opponent a point
            GameManager.instance.IncrementOpponentScore();

            //StartCoroutine(GameManager.instance.PauseGameFor(1f));

            // Reload scene
            isTimeToLoad = true;
        }
    } 
}
