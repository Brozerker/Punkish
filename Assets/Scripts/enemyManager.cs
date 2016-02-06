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
    public float losRange = 1.0f;
    private bool arrived = false;
    public GameObject myTarget = null;
	// Use this for initialization
	void Start () {
        nodes = GameObject.FindGameObjectsWithTag("Node");
        FSM = new stateMachine();
    }

    // Update is called once per frame
    void Update() {
        Vector3 moveDirection = GetComponent<Rigidbody2D>().velocity;
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

    public bool lineOfSight(GameObject target) {
        Vector3 start = transform.position;
        Vector3 end = target.transform.position;
        Vector3 direction = (end - start).normalized;
        start += direction;
        rayHit = Physics2D.Raycast(start, direction, losRange);
        Debug.DrawRay(start, direction*losRange, Color.red);
        Debug.Log(rayHit.collider.gameObject.name);
        if (rayHit.collider != null) {
            if (rayHit.collider.gameObject == target)
                return true;
        }
        return false;
    }    
}
