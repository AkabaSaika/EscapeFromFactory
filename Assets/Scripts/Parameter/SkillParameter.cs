using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightJson;

public class SkillParameter : MonoBehaviour
{
    private TextAsset jsonText;
    public Dictionary<string,int> skillDic =new Dictionary<string, int>();
    // Start is called before the first frame update
    void Awake()
    {
        jsonText = Resources.Load<TextAsset>("Json/Skill");
        string jsonString = jsonText.ToString();
        var skillArr = JsonValue.Parse(jsonString)["skills"].AsJsonArray;
        foreach(var i in skillArr)
        {
            skillDic.Add(i["name"],i["power"]);
        }  
    }
}
