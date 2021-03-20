using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class SimpleCollectableAgent : Agent
{
    [SerializeField]
    private float _moveSpeed = 50f;
    [SerializeField]
    private float _turnSpeed = 180f;

    [SerializeField]
    private SimpleCollectableArea _area;
    private Rigidbody _rb;
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        _area.Reset();
        _rb.velocity = Vector3.zero;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        foreach (Transform wall in _area.allWalls)
        {
            sensor.AddObservation(wall);
        }
        sensor.AddObservation(_area.collectable);
        sensor.AddObservation(transform);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuous = actionsOut.ContinuousActions;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        continuous[0] = horizontal;
        continuous[1] = vertical;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        Vector3 velocity =transform.forward*actions.ContinuousActions[1]*_moveSpeed;
        Vector3 turnSpeed = new Vector3(0,actions.ContinuousActions[0],0)*_turnSpeed;

        _rb.AddForce(velocity);//AddForce(velocity,ForceMode.VelocityChange);
        _rb.AddTorque(turnSpeed);

        if (_rb.velocity.magnitude <= 0)
        {
            AddReward(-2);
        }
        if (_rb.velocity.magnitude > 0)
        {
            AddReward(2);
        }
        if ((_area.collectable.transform.position - transform.position).magnitude < 2)
        {
            AddReward(1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "goal")
        {
            AddReward(100);
            EndEpisode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "wall")
        {
            AddReward(-10);
            EndEpisode();
        }
    }
}
