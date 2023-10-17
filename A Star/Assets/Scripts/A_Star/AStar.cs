using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar 
{
    List<Node> openList = new List<Node>();
    List<Node> closedList = new List<Node>();
    Node currentNode;
    Node endNode; 

    public AStar (Node startNode, Node _endNode)
    {
        openList = new List<Node>();
        closedList = new List<Node>();
        openList.Add(startNode);
        endNode = _endNode;
        
        while(openList.Count > 0)
        {
            currentNode = openList[0];
            //get lowest f cost node
            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].fCost < currentNode.fCost || openList[i].fCost == currentNode.fCost && openList[i].hCost < currentNode.hCost)
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if(currentNode == endNode)
            {
                return;
            }

            AdjacentNodes();            
        }

    }
    public List<Node> GetPath()
    {
        return closedList;
    }
    void AdjacentNodes()
    {
        Collider[] hit = Physics.OverlapBox(currentNode.transform.position, new Vector3(6, 0, 6));
        List<Node> adjacentNodes = new List<Node>();

        //get reference to all adjacent nodes
        foreach (Collider col in hit)
        {
            if (col.gameObject.GetComponent<Node>().walkable)
            {
                adjacentNodes.Add(col.gameObject.GetComponent<Node>());
            }
        }

        foreach(Node node in adjacentNodes)
        {
            //if node isn't walkable or has already been added, skip it 
            if (closedList.Contains(node) || !node.walkable)
            {
                if(node == endNode)
                {
                    return;
                }
                continue;
            }
            else
            {
                int newCost = currentNode.gCost + GetDistance(currentNode, node);
                if (newCost < node.gCost || !openList.Contains(node))
                {
                    node.gCost = newCost;
                    node.hCost = GetDistance(node, endNode);
                    node.parent = currentNode;

                    //IF adjacent_cell is not in OPEN_LIST
                    //ADD adjacent_cell to OPEN_LIST 
                    if (!openList.Contains(node))
                    {
                        openList.Add(node);
                    }
                }
            }
        }
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = (int) Mathf.Abs(nodeA.transform.position.x - nodeB.transform.position.x);
        int dstY = (int) Mathf.Abs(nodeA.transform.position.y - nodeB.transform.position.y);

        if(dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }
        return 14 * dstX + 10 * (dstY - dstX);

    }
}
