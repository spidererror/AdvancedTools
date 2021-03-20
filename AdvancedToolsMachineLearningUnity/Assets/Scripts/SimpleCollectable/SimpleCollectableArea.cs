using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
public class SimpleCollectableArea : MonoBehaviour
{
    public SimpleCollectableAgent agent;
    public Transform collectable;
    public List<Transform> allWalls = new List<Transform>();
    private Vector3 _startPositionAgent;

    private void Start()
    {
        _startPositionAgent = new Vector3(1.5f,1,-12);//agent.gameObject.transform.position;
    }

    public void Reset()
    {
        resetSetPosition();
    }

    private void resetSetPosition()
    {
        agent.gameObject.transform.position = _startPositionAgent;
    }
}
