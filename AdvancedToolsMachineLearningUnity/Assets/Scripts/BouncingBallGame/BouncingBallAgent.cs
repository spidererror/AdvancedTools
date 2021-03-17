using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class BouncingBallAgent : Agent
{
    [SerializeField]
    private float _moveSpeed = 10f;
    [SerializeField]
    private float _turnSpeed = 180f;
    [SerializeField]
    private int _maxAmountOfBalls = 20;
    [SerializeField]
    private BouncingBallArea _BouncingBallArea;
    [SerializeField]
    private BallAddReward _ballAddReward;

    private Rigidbody _rb;

    public override void Initialize()
    {
        base.Initialize();
        _rb = GetComponent<Rigidbody>();

    }
    //----------------------------------NOT MY PART OF THE CODE-----------------------------//
    //THIS IS FROM: https://www.immersivelimit.com/tutorials/reinforcement-learning-penguins-part-2-unity-ml-agents
    
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Convert the first action to forward movement
        float forwardAmount = actionBuffers.DiscreteActions[0];

        // Convert the second action to turning left or right
        float turnAmount = 0f;
        if (actionBuffers.DiscreteActions[1] == 1f)
        {
            turnAmount = -1f;
        }
        else if (actionBuffers.DiscreteActions[1] == 2f)
        {
            turnAmount = 1f;
        }

        // Apply movement
        _rb.MovePosition(transform.position + transform.forward * forwardAmount * _moveSpeed);
        transform.Rotate(transform.up * turnAmount * _turnSpeed);

        // Apply a tiny negative reward every step to encourage action
        if (MaxStep > 0) AddReward(-1f / MaxStep);
        //if (_rb.velocity.magnitude <= 0) AddReward(-10);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        int forwardAction = 0;
        int turnAction = 0;
        if (Input.GetKey(KeyCode.W))
        {
            // move forward
            forwardAction = 1;
            Debug.Log("MOVEFORWARD");
        }
        if (Input.GetKey(KeyCode.A))
        {
            // turn left
            turnAction = 1;
            Debug.Log("LEFT");

        }
        else if (Input.GetKey(KeyCode.D))
        {
            // turn right
            turnAction = 2;
            Debug.Log("RIGHT");
        }

        // Put the actions into the array
        actionsOut.DiscreteActions.Array[0] = forwardAction;
        actionsOut.DiscreteActions.Array[1] = turnAction;
    }
    //----------------------------------------------------------------------------------------------------//

    public override void OnEpisodeBegin()
    {
        _BouncingBallArea.Reset();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //store the direction the agent is facing
        sensor.AddObservation(transform.forward);

        //Store the distance to each active ball
       /* foreach (GameObject ball in _BouncingBallArea.ballSpawner.currentActiveBalls)
        {
            sensor.AddObservation(Vector3.Distance(ball.transform.position,transform.position));
        }*/

        //check how many balls we have collected inorder to see that more balls equals more rewards.
        sensor.AddObservation(_ballAddReward.GetAmountOfBalls());

        //store the position of your basket detection this way the network should now where the balls need to go.
        sensor.AddObservation(_ballAddReward.gameObject.transform.position);
    }
    private void Update()
    {
        EndGame();
    }
    private void EndGame()
    {
        if (_ballAddReward.GetAmountOfBalls() >= _maxAmountOfBalls || _BouncingBallArea.GetTimer() <= 0)
        {
            EndEpisode();
        }
    }
}
