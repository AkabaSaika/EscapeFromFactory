using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionGroupTriggerController : MonoBehaviour
{
    private ExplosionGroupController egc;   
    // Start is called before the first frame update
    void Start()
    {
        egc = GetComponentInParent<ExplosionGroupController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        egc.SendMessage("RandomFuse");
    }
}
