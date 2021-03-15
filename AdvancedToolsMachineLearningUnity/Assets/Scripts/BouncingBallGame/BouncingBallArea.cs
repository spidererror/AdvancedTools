using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;
public class BouncingBallArea : MonoBehaviour
{
    public BouncingBallAgent currentAgent;
    public Text TimeText;
    public BallSpawner ballSpawner;
    private float _timer;

    public void Reset()
    {

    }
}
