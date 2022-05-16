using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleHandSword : MonoBehaviour
{
    private Vector3 positionOffset = new Vector3(-0.0876f,0.2076f,0.0463f);
    private Vector3 rotateOffset=new Vector3(-13.352f,-169.541f,-4.059f);
    private Vector3 scale=new Vector3(0.2f,0.2f,0.2f);

    public Vector3 PositionOffset { get => positionOffset; set => positionOffset = value; }
    public Vector3 RotateOffset { get => rotateOffset; set => rotateOffset = value; }
    public Vector3 Scale { get => scale; set => scale = value; }

    public DoubleHandSword()
    {

    }

}
