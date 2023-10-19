using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    LineRenderer line;

    public Node current;
    Unit selectedUnit;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        line = transform.GetChild(0).GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && selectedUnit != null && current != null)
        {
            selectedUnit.MoveUnit(AStar.Path(selectedUnit.currentPosition, current));
            Reset();
        }

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.collider.gameObject.GetComponent<Unit>() != null)
                {
                    selectedUnit = hit.collider.gameObject.GetComponent<Unit>();
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
    }
}
