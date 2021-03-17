using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class TestAgent : Agent
{
    [SerializeField]
    private List<Transform> _targets = new List<Transform>();

    private Rigidbody _rb;
    private float _moveSpeed = 20;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        transform.position = new Vector3(0, 1, 0);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        foreach (Transform target in _targets)
        {
            sensor.AddObservation(target.position);
        }
        
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousAction = actionsOut.ContinuousActions;
        continuousAction[0] = Input.GetAxis("Horizontal");
        continuousAction[1] = Input.GetAxis("Vertical");
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        Vector3 position = new Vector3(-moveX, 0, -moveZ) * _moveSpeed;

        _rb.AddForce(position, ForceMode.Force);

        if (_rb.velocity.magnitude < _moveSpeed)
        {
            AddReward(-10);
        }
        if (_rb.velocity.magnitude >= _moveSpeed)
        {
            AddReward(-1/MaxStep);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "goal")
        {
            AddReward(100);
            EndEpisode();
        }
        if (other.gameObject.tag == "wall")
        {
            AddReward(-100);
            EndEpisode();
        }
    }
}
