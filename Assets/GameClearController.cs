using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            GameManager.Instance.gameClearHandler.Invoke();
        }
    }
}
