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
    public int timerInSeconds;
    private int _timer;
    public void Reset()
    {
        ballSpawner.RemoveBalls();
        ballSpawner.RandomSpawnLocation();
        resetTime();
        resetAgent();
    }

    private void resetTime()
    {
        _timer = timerInSeconds;
        TimeText.text = ""+_timer+"Seconds left";
    }

    private void resetAgent()
    {
        currentAgent.transform.position = new Vector3(0,5,0);
    }
}
