﻿using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {
    public float speed = 0.5f;
    private float moveX = 0;
    private float moveY = 0f;
    public Inventory inventory;

    public Sprite up, down, left, right, attk_up, attk_down, attk_left, attk_right;


    /// <summary>
    /// Handles the player's collision
    /// </summary>
    /// <param name="other"></param>

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
         Vector2 moveDirection = GetComponent<Rigidbody2D>().velocity;
         if (moveDirection != Vector2.zero) {
             float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
             //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
         }
        //if (GetComponent<Rigidbody2D>().velocity.x < 0) {
        //    GetComponent<Sprite>(). .Rotate(-90,0,0);
        //}
        //if (GetComponent<Rigidbody2D>().velocity.x > 0) {
        //    GetComponent<Transform>().Rotate(-90, 0, 0);
        //}
        //if (GetComponent<Rigidbody2D>().velocity.y < 0) {
        //    GetComponent<Transform>().Rotate(-90, 0, 0);
        //}
        //if (GetComponent<Rigidbody2D>().velocity.y > 0) {
        //    GetComponent<Transform>().Rotate(-90, 0, 0);
        //}

        //Move Direction
         if (Input.GetKey("left"))              { GetComponent<SpriteRenderer>().sprite = left; }
         if (Input.GetKey("right"))             { GetComponent<SpriteRenderer>().sprite = right; }
         if (Input.GetKey("down"))              { GetComponent<SpriteRenderer>().sprite = down; }
         if (Input.GetKey("up"))                { GetComponent<SpriteRenderer>().sprite = up; }

        //Attack Direction
         if (GetComponent<SpriteRenderer>().sprite == left && Input.GetKey("space")) { GetComponent<SpriteRenderer>().sprite = attk_left; }
         if (GetComponent<SpriteRenderer>().sprite == right && Input.GetKey("space")) { GetComponent<SpriteRenderer>().sprite = attk_right; }
         if (GetComponent<SpriteRenderer>().sprite == down && Input.GetKey("space")) { GetComponent<SpriteRenderer>().sprite = attk_down; }
         if (GetComponent<SpriteRenderer>().sprite == up && Input.GetKey("space")) { GetComponent<SpriteRenderer>().sprite = attk_up; }


         if (Input.GetKeyDown(KeyCode.B))
         {
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

        //if (collision.gameObject.tag == "Enemy" && Input.GetKeyDown("space"))
        //{

        //    Destroy(collision.gameObject);
        //}
    }

    void FixedUpdate() {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        GetComponent<Rigidbody2D>().velocity = new Vector2(moveX * speed, moveY * speed);
    }

    

}
