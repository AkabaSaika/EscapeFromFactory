using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    private MeshRenderer mr;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SmallDamage()
    {
        mr.material.color = Color.green;
        
    }

    public void MediumDamage()
    {
        mr.material.color = Color.yellow;
    }

    public void BigDamage()
    {
        mr.material.color = Color.red;
        rb.AddForce(new Vector3(0, 0, -1));
    }
}
