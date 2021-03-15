using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField]
    private int _maxAmountOfBalls = 30;
    [SerializeField]
    private int _radius = 30;
    [SerializeField]
    private List<GameObject> _differentBalls = new List<GameObject>();
    private List<GameObject> _currentActiveBalls = new List<GameObject>();

    private void Start()
    {
        RandomSpawnLocation();
    }



    public void RandomSpawnLocation()
    {
        while (_currentActiveBalls.Count < _maxAmountOfBalls)
        {
            _currentActiveBalls.Add(Instantiate(_differentBalls[Random.Range(0,_differentBalls.Count)],transform));
        }
        foreach (GameObject balls in _currentActiveBalls)
        {
            Vector3 randomPosition = transform.position + new Vector3(Random.Range(-_radius,_radius),0,Random.Range(-_radius,_radius));
            balls.transform.position = randomPosition;
        }
    }

    public void RemoveBalls()
    {
        _currentActiveBalls.Clear();
    }
}
