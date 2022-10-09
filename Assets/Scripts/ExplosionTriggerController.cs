using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTriggerController : MonoBehaviour
{
    private ExplosionController explosionController;
    private void Start()
    {
        explosionController = GetComponentInParent<ExplosionController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        explosionController.GetComponent<ExplosionController>().SendMessage("ExplosionCountDown");
    }
}
