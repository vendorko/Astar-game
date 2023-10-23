using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Material highlightMat;
    public Material neutralMat;
    public bool walkable = true;
    public Node parent;
    public int fCost
    {
        get { return hCost + gCost; }
    }
    public int hCost;
    public int gCost;

    public void Highlight(bool isLit = true)
    {
        if(isLit)
        {
            gameObject.GetComponent<MeshRenderer>().material = highlightMat;
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material = neutralMat;
        }
    }
}
