using System;
using System.Collections.Generic;
using UnityEngine;

public static class AStar
{
    public static List<Node> Path(Node startNode, Node endNode)
    {
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();
        startNode.gCost = 0;
        startNode.hCost = GetDistance(startNode, endNode);
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];
            //get lowest f cost node
            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].fCost < currentNode.fCost || (openList[i].fCost == currentNode.fCost && openList[i].hCost < currentNode.hCost))
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == endNode)
            {
                break;
            }

            List<Node> adjacentNodes = AdjacentNodes(currentNode, endNode, openList, closedList);
            foreach (Node node in adjacentNodes)
            {
                openList.Add(node);
            }
        }

        return GetPath(endNode);

    }
    public static List<Node> GetPath(Node endNode)
    {
        Node node = endNode;
        List<Node> path = new List<Node>();

        while (node.parent != null)
        {
            Debug.Log(node);
            path.Add(node);
            node = node.parent;
        }

        //add start node back after loop finishes
        path.Add(node);

        return path;
    }
    static List<Node> AdjacentNodes(Node currentNode, Node endNode, List<Node> openList, List<Node> closedList)
    {
        Collider[] hit = Physics.OverlapBox(currentNode.transform.position, new Vector3(7, 0, 7));
        List<Node> adjacentNodes = new List<Node>();
        List<Node> nodesToAdd = new List<Node>();

        //get reference to all adjacent nodes
        foreach (Collider col in hit)
        {
            if (col.gameObject.GetComponent<Node>())
            {
                adjacentNodes.Add(col.gameObject.GetComponent<Node>());
            }
        }

        foreach (Node node in adjacentNodes)
        {
            //if node has already been added, skip it 
            if (closedList.Contains(node))
            {
                continue;
            }

            int newCost = currentNode.gCost + GetDistance(currentNode, node);
            if (newCost < node.gCost || !openList.Contains(node))
            {
                //cost to move from current node to node this node
                node.gCost = newCost;
                //cost to move from this node to end node
                node.hCost = GetDistance(node, endNode);
                node.parent = currentNode;

                //IF adjacent_cell is not in OPEN_LIST
                //ADD adjacent_cell to OPEN_LIST 
                if (!openList.Contains(node))
                {
                    nodesToAdd.Add(node);
                }
            }
        }

        return nodesToAdd;
    }

    static int GetDistance(Node nodeA, Node nodeB)
    {
        float dstX = Mathf.Abs(nodeA.transform.position.x - nodeB.transform.position.x);
        float dstY = Mathf.Abs(nodeA.transform.position.z - nodeB.transform.position.z);

        if (dstX > dstY)
        {
            return (int)(14 * dstY + 10 * (dstX - dstY));
        }
        return (int)(14 * dstX + 10 * (dstY - dstX));
    }
}
