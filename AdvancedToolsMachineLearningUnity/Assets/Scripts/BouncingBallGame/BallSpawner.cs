using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField]
    private PhysicMaterial _physicsMaterial;
    [SerializeField]
    private int _maxAmountOfBalls = 30;
    [SerializeField]
    private int _radius = 30;
    [SerializeField]
    private List<GameObject> _differentBalls = new List<GameObject>();
    public List<GameObject> currentActiveBalls = new List<GameObject>();
    private List<Transform> currentStartTransforms = new List<Transform>();

    private void Start()
    {
        RandomSpawnLocation();
    }



    private void RandomSpawnLocation()
    {
        while (currentActiveBalls.Count < _maxAmountOfBalls)
        {
            currentActiveBalls.Add(Instantiate(_differentBalls[Random.Range(0,_differentBalls.Count)],transform));
        }
        foreach (GameObject ball in currentActiveBalls)
        {
            currentStartTransforms.Add(ball.transform);
            Vector3 randomPosition = transform.position + new Vector3(Random.Range(-_radius,_radius),0,Random.Range(-_radius,_radius));
            ball.transform.position = randomPosition;
        }
    }

    public void RepositionBalls()
    {
        for (int i = 0; i< currentActiveBalls.Count; i++)
        {
            Vector3 randomPosition = currentStartTransforms[i].position + new Vector3(Random.Range(-_radius, _radius), 0, Random.Range(-_radius, _radius));
            currentActiveBalls[i].transform.position = randomPosition;
            //change the material incase it was changed.
            currentActiveBalls[i].GetComponent<SphereCollider>().material = _physicsMaterial;
            //make sure the ball has no velocity other wise it won't fall perfectly anymore.
            currentActiveBalls[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
