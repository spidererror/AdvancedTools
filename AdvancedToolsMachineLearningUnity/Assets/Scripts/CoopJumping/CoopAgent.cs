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

    private void Update()
    {
        resetJump();
        resetOutOfBounds();
       // detectJump();
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
            AddReward(20);
        }
        if (Input.GetKey(KeyCode.S))
        {
            forward = -1;
            AddReward(10);
        }
        if (Input.GetKey(KeyCode.A))
        {
            turnSpeed = -1;
            AddReward(-10);
        }
        if (Input.GetKey(KeyCode.D))
        {
            turnSpeed = 1;
            AddReward(-10);
        }
        else if (Input.GetKey(KeyCode.Space) && _canJump)
        {
            jump = 1;
            AddReward(1+(-1/MaxStep));
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
        
        //First the cumulativeReward needs to be higher than 10 I hope rotating agents will be less at this point
        if (GetCumulativeReward() > 10)
        {
            //Rotate agent;
            // transform.Rotate(new Vector3(0,turnSpeed*_turnSpeed*Time.deltaTime,0));
            _rb.AddTorque(new Vector3(0, turnSpeed * _turnSpeed, 0), ForceMode.Force);
        }
        
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
        
        
    }

   /* private void detectJump()
    {
        if (!Physics.Raycast(transform.position, transform.up * -.51f))
        {
            jump = 0;
        }
    }*/

    private void resetJump()
    {
        if (transform.position.y > area.agentOneStartPosition.position.y + 5)
        {
            _canJump = false;
            AddReward(-1000);
        }
    }

    private void resetOutOfBounds()
    {
        //fail safe for out of bounds agents.
        float maxHeight = 5;
        float minHeight = -12;
        if (transform.position.y > maxHeight || transform.position.y < minHeight)
        {
            AddReward(-1000);
            EndEpisode();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "wall")
        {
            AddReward(-2);
            EndEpisode();
        }
        if (other.gameObject.tag == "goal")
        {
            AddReward(2000);
            EndEpisode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "walkableSurface")
        {
            _canJump = true;
            AddReward(10);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "walkableSurface")
        {
            _canJump = false;
        }
        if (collision.gameObject.tag == "agent")
        {
            _canJump = false;
        }
    }
}
