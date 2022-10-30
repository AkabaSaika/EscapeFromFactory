using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorController : MonoBehaviour
{
    private DoorController doorController;

    private void Start() {
        doorController=GetComponentInParent<DoorController>();
    }
    private void OnTriggerEnter(Collider other) {
        if(!doorController.isOpen)
        doorController.SendMessage("OpenDoor");
    }
}
