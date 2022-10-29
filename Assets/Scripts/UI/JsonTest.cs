using System.Collections;
using System.Collections.Generic;
using System.IO;
using Bright.Serialization;
using UnityEngine;
using SimpleJSON;

public class JsonTest : MonoBehaviour
{
    Skill test;
    // Start is called before the first frame update
    void Start()
    {
        var tables = new cfg.Tables(file =>
        JSON.Parse(File.ReadAllText(Application.dataPath+ "/MiniTemplate/output_json" + "/" + file + ".json")
    ));
        cfg.test.SkillParam skillInfo = tables.TbSkillParam.Get(1001);
        //Debug.Log(skillInfo.AttackAnimationEndTime);
        var textAsset = Resources.Load<TextAsset>("Json/test_tbskillparam");
        string json = textAsset.ToString();
        //var jsonParse = JsonValue.Parse(json);
        //Debug.Log(test.sp.AttackAnimationEndTime);
  
        //JsonUtility.FromJsonOverwrite(json, test.sp);
        //Debug.Log(test.sp.AttackAnimationEndTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
