using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.Init();
        GameManager.Instance.Init();
        UIManager.Instance.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
