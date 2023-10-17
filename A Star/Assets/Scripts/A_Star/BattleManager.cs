using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] Node startNode;
    [SerializeField] LineRenderer line;
    bool selecting = true;

    void Update()
    {
        /*if(Input.GetMouseButtonDown(0))
        {*/
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if(hit.collider.gameObject.GetComponent<Node>())
                {
                    AStar astar = new AStar(startNode, hit.collider.gameObject.GetComponent<Node>());
                    List<Node> path = astar.GetPath();
                    DrawPath(path);
                }
            }
        //}
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
