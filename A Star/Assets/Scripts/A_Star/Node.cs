using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public bool walkable = true;
    public Node parent;
    public int fCost
    {
        get { return hCost + gCost; }
    }
    public int hCost;
    public int gCost;
}
