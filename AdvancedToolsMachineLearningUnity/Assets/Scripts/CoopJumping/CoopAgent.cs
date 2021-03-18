using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CoopAgent : Agent
{
    public CoopJumpingArea area;
    [SerializeField]
    private float _moveSpeed = 10;
    [SerializeField]
    private float _turnSpeed = 180;
    [SerializeField]
    private float _jumpHeight = 10;

    private bool _canJump = false;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }


    public override void OnEpisodeBegin()
    {
        area.Reset();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (area.agentOne == this)
        {
            sensor.AddObservation(area.agentTwo.transform);
        }
        if(area.agentOne != this)
        {
            sensor.AddObservation(area.agentOne.transform);
        }
        sensor.AddObservation(transform);
        sensor.AddObservation(area.goal.transform);
        sensor.AddObservation(area.Wall.transform);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        int forward = 0;
        int turnSpeed = 0;
        int jump = 0;
        if (Input.GetKey(KeyCode.W))
        {
            forward = 1;
            AddReward(4);
        }
        if (Input.GetKey(KeyCode.A))
        {
            turnSpeed = -1;
            AddReward(3);
        }
        if (Input.GetKey(KeyCode.D))
        {
            turnSpeed = 1;
            AddReward(3);
        }
        if (!Physics.Raycast(transform.position, transform.up * -.51f))
        {
            jump = 0;
        }
        else if (Input.GetKey(KeyCode.Space) && (transform.position.y <= _jumpHeight||_canJump))
        {
            jump = 1;
            AddReward(2);
        }
        
        actionsOut.DiscreteActions.Array[0] = forward;
        actionsOut.DiscreteActions.Array[1] = turnSpeed;
        actionsOut.DiscreteActions.Array[2] = jump;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveForward = actions.DiscreteActions[0];
        float turnSpeed = actions.DiscreteActions[1];
        float jump = actions.DiscreteActions[2];

        //move agent forward when W is pressed
        _rb.AddForce(transform.forward*moveForward*_moveSpeed,ForceMode.Force);
        //Rotate agent;
        _rb.AddTorque(new Vector3(0,turnSpeed*_turnSpeed,0),ForceMode.Force);
        //Make agent jump when space is pressed
        _rb.AddForce(Vector3.up*jump*_jumpHeight, ForceMode.Impulse);
        //make sure the agent gets punished for inactivity in order to move more
        if (_rb.velocity.magnitude < _moveSpeed)
        {
            AddReward(-10);
        }
        if (_rb.velocity.magnitude >= _moveSpeed)
        {
            AddReward(-1 / MaxStep);
        }
        if (transform.position.y > 7)
        {
            AddReward(-20000);
            EndEpisode();
        }
        if (transform.position.y < 7)
        {
            AddReward(-20000);
            EndEpisode();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "wall")
        {
            AddReward(-20);
            EndEpisode();
        }
        if (other.gameObject.tag == "goal")
        {
            AddReward(200);
            EndEpisode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "walkableSurface")
        {
            _canJump = true;
            AddReward(1);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "agent")
        {
            _canJump = false;
        }
    }
}
