using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] LineRenderer line;
    bool selecting = true;

    public Node startingNode;
    public Node endingNode;
    public Node current;

    void Update()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            //Checks if targeted node has changed since last frame
            if(current != hit.collider.gameObject.GetComponent<Node>() && hit.collider.gameObject.GetComponent<Node>() != null)
            {
                current = hit.collider.gameObject.GetComponent<Node>();
                //draw line from starting node to current node

                if (startingNode != null)
                {                    
                    DrawPath(AStar.Path(startingNode, current));
                }
            }        
        }

        //on mouse click
        //if no node actively selected...
        //select starting node

        //if start selected but end is null...
        //select end
        if (Input.GetMouseButtonDown(0))
        {
            if (startingNode == null)
            {
                startingNode = current;
            }
            else if(endingNode == null)
            {
                endingNode = current;
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            startingNode = null;
            endingNode = null;
            line.positionCount = 0;
        }
    }

    void DrawPath(List<Node> path)
    {
        line.SetWidth(.5f, .5f);
        line.useWorldSpace = true;
        line.positionCount = (path.Count);
        for (int i = 0; i < path.Count; i++)
        {
            Vector3 pos = new Vector3(path[i].gameObject.transform.position.x, path[i].gameObject.transform.position.y + 1, path[i].gameObject.transform.position.z);
            line.SetPosition(i, pos);
        }
    }
}
