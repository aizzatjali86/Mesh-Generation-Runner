using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rb.AddForce(-transform.up * 50);
        //rb.velocity = new Vector3(50, 0, 0);
        rb.AddForce(transform.forward * 200);
    }
}
