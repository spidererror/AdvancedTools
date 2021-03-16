using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;
using System.Threading;

public class BouncingBallArea : MonoBehaviour
{
    public BouncingBallAgent currentAgent;
    public Text TimeText;
    public BallSpawner ballSpawner;
    public int maxTimerInSeconds;
    private int _timer;

    public void Reset()
    {
        ballSpawner.RepositionBalls();
        resetTime();
        resetAgent();
    }

    public int GetTimer()
    {
        return _timer;
    }
    private void Start()
    {
        StartCoroutine(countDown());
    }

    public IEnumerator countDown()
    {
        while (true)
        {
            _timer--;
            TimeText.text = "" + _timer + " Sec";
            yield return new WaitForSeconds(1);
        }
        
    }

    private void resetTime()
    {
        _timer = maxTimerInSeconds;
        TimeText.text = ""+(int)_timer+"Sec";
    }

    private void resetAgent()
    {
        currentAgent.transform.position = new Vector3(0,5,0);
    }
}
