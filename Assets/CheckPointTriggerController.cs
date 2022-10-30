using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointTriggerController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().GetHeal(100);
            GameManager.Instance.lastCheckPoint=transform.parent.gameObject;
        }
    }
}
