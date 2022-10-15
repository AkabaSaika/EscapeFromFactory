using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUlitity : MonoBehaviour
{
    public GameObject player;
    public GameObject room1Pos;
    public GameObject corridor2Pos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        //if(GUI.Button(new Rect(new Vector2(50, 50),new Vector2(10,20)),"移动到起始点"))
        if (GUI.Button(new Rect(20,200,200,50), "移动到起始点"))
        {
            player.transform.position = room1Pos.transform.position;
        }
        if (GUI.Button(new Rect(20, 300, 200, 50), "移动到走廊2"))
        {
            player.transform.position = corridor2Pos.transform.position;
        }
    }
}
