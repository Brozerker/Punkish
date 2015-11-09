using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {
    public float speed = 0.5f;
    private float moveX = 0;
    private float moveY = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<Rigidbody2D>().velocity.x < 0) {
        }
        if (GetComponent<Rigidbody2D>().velocity.x > 0) {
        }
        if (GetComponent<Rigidbody2D>().velocity.y < 0) {
        }
        if (GetComponent<Rigidbody2D>().velocity.y > 0) {
        }
	}
    void FixedUpdate() {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        GetComponent<Rigidbody2D>().velocity = new Vector2(moveX * speed, moveY * speed);
    }
}
