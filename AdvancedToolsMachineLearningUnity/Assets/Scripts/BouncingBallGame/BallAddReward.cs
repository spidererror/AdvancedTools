using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAddReward : MonoBehaviour
{
    private int _amountOfBalls = 0;
    private int _ballLayer = 6;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _ballLayer)
        {
            other.gameObject.GetComponent<BallReward>().AddReward();
            other.gameObject.GetComponent<SphereCollider>().material = null;
            _amountOfBalls++;
        }
    }

    public int GetAmountOfBalls()
    {
        return _amountOfBalls;
    }
}
