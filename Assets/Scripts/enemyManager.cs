using UnityEngine;
using System.Collections;

struct Node {
    Vector3 position;
    ArrayList availableNodes;
}

public class enemyManager : MonoBehaviour {
    stateMachine FSM;
    public ArrayList nodesToCopy;
    ArrayList nodes;
	// Use this for initialization
	void Start () {
        for (int i = 0; i < nodesToCopy.Count; ++i) {
            for (int j = 0; j < nodesToCopy.Count; ++j) {

            }
        }
	}

    // Update is called once per frame
    void Update() {
        
	}
}
