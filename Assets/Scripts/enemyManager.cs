using UnityEngine;
using System.Collections;

struct Node {
    Vector3 position;
    ArrayList availableNodes;
}

public class enemyManager : MonoBehaviour {
    private Vector2 playerPos;
    private Vector2 toMove;
    private stateMachine FSM;
    private GameObject[] nodes;
    public float speed = 3.0f;
    public ArrayList nodesToCopy;
    
	// Use this for initialization
	void Start () {
        nodes = GameObject.FindGameObjectsWithTag("Node");
	}

    void Update() {
        //FSM.Update(this, nodes);
    }

    // Update is called once per frame
    void FixedUpdate() {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        GetComponent<Rigidbody2D>().velocity = new Vector2(playerPos.x * speed, playerPos.y * speed);
        Debug.Log(playerPos.ToString(), gameObject);
	}
}
