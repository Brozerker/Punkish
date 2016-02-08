using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {
    public bool walkable;
    public Vector3 worldPosition;

    public int gCost;
    public int hCost;

    public Node(bool mWalkable, Vector3 mWorlPos) {
        walkable = mWalkable;
        worldPosition = mWorlPos;
    }

    public int fCost {
        get {
            return gCost + hCost;
        }
    }
}
