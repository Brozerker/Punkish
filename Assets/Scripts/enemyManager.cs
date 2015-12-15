using UnityEngine;
using System.Collections;

struct Node {
    Vector3 position;
    ArrayList availableNodes;
}

public class enemyManager : MonoBehaviour {
    private Transform target;
    private Vector2 toMove;
    private stateMachine FSM;
    private GameObject[] nodes;
    public float speed = 0.5f;
    public ArrayList nodesToCopy;
    public float range = 15f;
    
	// Use this for initialization
	void Start () {
        nodes = GameObject.FindGameObjectsWithTag("Node");
    }

    // Update is called once per frame
    void Update() {
        //FSM.Update(this, nodes);
        target = GameObject.FindWithTag("Player").transform;
        Debug.Log(Vector2.Distance(transform.position, target.position));
       // if (Vector2.Distance(transform.position, target.position) < range) {
            Debug.DrawLine(transform.position, target.position);
            //Debug.DrawLine(transform.position);
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        //}
        Vector2 moveDirection = GetComponent<Rigidbody2D>().velocity;
        if (moveDirection != Vector2.zero) {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
