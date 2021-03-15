using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using System.Linq;

public class BallReward : MonoBehaviour
{
    [SerializeField]
    private int _currentReward;

    private BouncingBallAgent _agent;

    private void Start()
    {
        _agent = GameObject.FindGameObjectsWithTag("Player").First().GetComponent<BouncingBallAgent>();
    }

    public void AddReward()
    {
        _agent.AddReward(_currentReward);
    }
}
