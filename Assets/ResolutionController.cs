using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionController : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_Dropdown resolution;
    // Start is called before the first frame update
    void Start()
    {
        resolution = gameObject.GetComponentInChildren<TMPro.TMP_Dropdown>();
        resolution.onValueChanged.AddListener(ResolutionChange.SetResolution);
    }


}
