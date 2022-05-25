using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightJson;

public class JsonTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var textAsset = Resources.Load<TextAsset>("Json/Skill");
        string jsonText = textAsset.ToString();
        var json = JsonValue.Parse(jsonText)["skills"].AsJsonArray;
        Dictionary<string,int> skills = new Dictionary<string,int>();
        foreach(var i in json)
        {
            skills.Add(i["name"],i["power"]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
