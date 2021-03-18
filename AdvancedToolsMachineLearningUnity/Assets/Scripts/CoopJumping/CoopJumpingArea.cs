using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
public class CoopJumpingArea : MonoBehaviour
{
    public float randomXRangeGoal;
    public CoopAgent agentOne;
    public CoopAgent agentTwo;
    public GameObject Wall;
    public GameObject goal;

    [HideInInspector]
    public Transform agentOneStartPosition;
    [HideInInspector]
    public Transform agentTwoStartPosition;

    private void Start()
    {
        agentOneStartPosition = agentOne.transform;
        agentTwoStartPosition = agentTwo.transform;
    }

    public void Reset()
    {
        ResetPositionAgents();
        RandomGoalPosition();
    }

    private void ResetPositionAgents()
    {
        agentOne.transform.localPosition = new Vector3(-12,-8,-9);//agentOneStartPosition.position;
        agentTwo.transform.localPosition = new Vector3(-15, -8, -9);//agentTwoStartPosition.position;
    }

    private void RandomGoalPosition()
    {
        goal.transform.position = new Vector3(randomXRangeGoal, goal.transform.position.y, goal.transform.position.z);
    }
}
