using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionGroupController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> explosionObjects=new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RandomFuse()
    {
        foreach (var i in explosionObjects)
        {
            if (Random.value < 0.4f)
            {
                i.SendMessage("ExplosionCountDown");
            }
        }
    }
}
