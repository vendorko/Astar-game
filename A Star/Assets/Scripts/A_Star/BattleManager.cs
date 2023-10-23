using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] Transform background;
    [SerializeField] List<GameObject> tiles = new List<GameObject>();
    [SerializeField] int moveRange = 1;
    public static BattleManager instance;
    LineRenderer line;

    List<Node> nodesInRange = new List<Node>();

    public Node current;
    Unit selectedUnit;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        line = transform.GetChild(0).GetComponent<LineRenderer>();
        PopulateTiles();
    }

    void PopulateTiles()
    {
        foreach(Transform child in background)
        {
            tiles.Add(child.gameObject);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && selectedUnit != null && current != null && nodesInRange.Contains(current))
        {
            if(selectedUnit != current)
            {
                selectedUnit.MoveUnit(AStar.Path(selectedUnit.currentPosition, current));
                Reset();
            }
            else
            {
                selectedUnit = null;
                Reset();
            }
        }

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.collider.gameObject.GetComponent<Unit>() != null && selectedUnit == null)
                {
                    selectedUnit = hit.collider.gameObject.GetComponent<Unit>();
                    GetTilesInRange(selectedUnit.currentPosition);
                }
            }
            if (hit.collider.gameObject.GetComponent<Node>() != null && selectedUnit != null)
            {
                current = hit.collider.gameObject.GetComponent<Node>();
            }
            if(selectedUnit != null && current != null)
            {
                DrawPath(AStar.Path(selectedUnit.currentPosition, current));
            }
        }
    }

    void DrawPath(List<Node> path)
    {
        line.useWorldSpace = true;
        line.positionCount = (path.Count);
        for (int i = 0; i < path.Count; i++)
        {
            Vector3 pos = new Vector3(path[i].gameObject.transform.position.x, path[i].gameObject.transform.position.y + 1, path[i].gameObject.transform.position.z);
            line.SetPosition(i, pos);
        }
    }

    public void Reset()
    {
        selectedUnit = null;
        current = null;
        line.positionCount = 0;

        foreach (GameObject tile in tiles)
        {
            if (tile.GetComponent<Node>())
            {
                Node node = tile.GetComponent<Node>();
                node.gCost = 0;
                node.hCost = 0;
                node.parent = null;
            }
        }
        foreach(Node node in nodesInRange)
        {
            node.Highlight(false);
        }
        nodesInRange.Clear();
    }

    void GetTilesInRange(Node startNode)
    {
        nodesInRange.Add(startNode);
        int range = moveRange;              

        while(range > 0)
        {
            List<Node> adjacentNodes = new List<Node>();
            foreach (Node node in nodesInRange)
            {

                Collider[] hit = Physics.OverlapBox(node.transform.position, new Vector3(7, 0, 7));
                //get reference to all adjacent nodes
                foreach (Collider col in hit)
                {
                    if (col.gameObject.GetComponent<Node>() && !nodesInRange.Contains(col.gameObject.GetComponent<Node>()))
                    {
                        if(AStar.GetDistance(node, col.gameObject.GetComponent<Node>()) == 50)
                        {
                            adjacentNodes.Add(col.gameObject.GetComponent<Node>());
                        }
                    }
                }
            }
            
            foreach(Node node in adjacentNodes)
            {
                nodesInRange.Add(node);
            }
            range--;
        }

        foreach (Node node in nodesInRange)
        {
            node.Highlight();
        }

    }
}
