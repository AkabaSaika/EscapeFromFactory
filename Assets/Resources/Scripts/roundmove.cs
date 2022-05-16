using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roundmove : MonoBehaviour
{
    private float posX;
    private float posY;
    private float angle = 0.0f;
    private float period = 1.0f;
    Transform cube;

    // Start is called before the first frame update
    void Start()
    {
        cube = GameObject.Find("Cube").transform;
    }

    // Update is called once per frame
    void Update()
    {
        angle += (2.0f * Mathf.PI / period) * Time.deltaTime;
        posX = Mathf.Cos(angle) * 5.0f + cube.position.x;
        posY = Mathf.Sin(angle) * 5.0f+cube.position.z;
        this.transform.position = new Vector3(posX, this.transform.position.y, posY);
        if(angle>=2.0f*Mathf.PI)
        {
            angle -= Mathf.PI * 2.0f;
        }
    }
}
