using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public Node currentPosition;
    NavMeshAgent ai;

    private void Awake()
    {
        ai = GetComponent<NavMeshAgent>();
        AssignCurrentPosition();
    }

    public void MoveUnit(List<Node> path)
    {
        StartCoroutine(Move(path));
    }
    IEnumerator Move(List<Node> path)
    {
        int pos = 0;
        path.Reverse();

        Vector3 newPosition = new Vector3(path[pos].transform.position.x, transform.position.y, path[pos].transform.position.z);
        while (pos < path.Count)
        {
            if (Vector3.Distance(transform.position, newPosition) < 1f)
            {
                newPosition = new Vector3(path[pos].transform.position.x, transform.position.y, path[pos].transform.position.z);
                ai.destination = newPosition;
                pos++;
            }
            yield return new WaitForSeconds(.1f);
        }
        AssignCurrentPosition();
        BattleManager.instance.Reset();
    }

    public void AssignCurrentPosition()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, -transform.up, out hitInfo, 4f))
        {
            currentPosition = hitInfo.collider.gameObject.GetComponent<Node>();
        }
    }

}
