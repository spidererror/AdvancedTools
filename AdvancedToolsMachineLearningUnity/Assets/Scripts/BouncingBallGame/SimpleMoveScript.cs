using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMoveScript : MonoBehaviour
{
    [SerializeField]
    private float speed = 5.0f;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        MoveAgent();
    }

    private void MoveAgent()
    {
        Vector3 vertical = new Vector3(0,0,Input.GetAxis("Vertical"));
        Vector3 horizontal = new Vector3(Input.GetAxis("Horizontal"),0,0);

        Vector3 velocity = (horizontal + vertical)*speed;
        _rb.AddForce(velocity,ForceMode.Force);
    }
}
