using UnityEngine;
using System.Collections;

struct Node {
    Vector3 position;
    ArrayList availableNodes;
}

public class enemyManager : MonoBehaviour {
    public Sprite up, down, left, right;
    private RaycastHit2D rayHit;
    private Transform target;
    private Vector2 toMove;
    private stateMachine FSM;
    private GameObject[] nodes;
    public float speed = 0.5f;
    public ArrayList nodesToCopy;
    public float sensorRange = 15f;
    private bool arrived = false;
    public GameObject myTarget = null;
	// Use this for initialization
	void Start () {
        nodes = GameObject.FindGameObjectsWithTag("Node");
        FSM = new stateMachine();
    }

    // Update is called once per frame
    void Update() {
        Vector2 moveDirection = GetComponent<Rigidbody2D>().velocity;
        if (moveDirection != Vector2.zero) {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        if (moveDirection.x < 0) {
            GetComponent<SpriteRenderer>().sprite = left;
        }
        if (moveDirection.x > 0) {
            GetComponent<SpriteRenderer>().sprite = right;
        }
        if (moveDirection.y < 0) {
            GetComponent<SpriteRenderer>().sprite = down;
        }
        if (moveDirection.y > 0) {
            GetComponent<SpriteRenderer>().sprite = up;
        }
        FSM.Update(this, nodes);
    }

    
}
