using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {
    public float speed = 0.5f;
    private float moveX = 0;
    private float moveY = 0f;
    public Inventory inventory;
    public Sprite Up, Down, Left, Right;



    /// <summary>
    /// Handles the player's collision
    /// </summary>
    /// <param name="other"></param>

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 moveDirection = GetComponent<Rigidbody2D>().velocity;
        //Debug.DrawLine(transform.position, moveDirection);
        //if (moveDirection != Vector2.zero) {
        //    float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        //    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //}
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            GetComponent<SpriteRenderer>().sprite = Left;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            GetComponent<SpriteRenderer>().sprite = Right;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            GetComponent<SpriteRenderer>().sprite = Down;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            GetComponent<SpriteRenderer>().sprite = Up;
        }
        if(Input.GetKeyDown(KeyCode.RightControl)) {
            
        }
        if (Input.GetKeyDown(KeyCode.B)) {
            inventory.Open();
        }
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Item") //If we collide with an item that we can pick up
        {
            inventory.AddItem(other.GetComponent<Item>()); //Adds the item to the inventory.
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Item") //If we collide with an item that we can pick up
        {
            inventory.AddItem(collision.gameObject.GetComponent<Item>()); //Adds the item to the inventory.

            Destroy(collision.gameObject);
        }
    }

    void FixedUpdate() {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        GetComponent<Rigidbody2D>().velocity = new Vector2(moveX * speed, moveY * speed);
    }

    

}
