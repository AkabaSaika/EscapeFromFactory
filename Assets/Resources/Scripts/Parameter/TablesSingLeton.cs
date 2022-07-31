using System.Collections;
using System.Collections.Generic;
using System.IO;
using Bright.Serialization;
using UnityEngine;
using SimpleJSON;
using cfg;

public class TablesSingLeton:MonoSingleton<TablesSingLeton>
{
    private cfg.Tables tables;

    public Tables Tables { get => tables; set => tables = value; }

    private void Awake()
    {
        tables = new cfg.Tables(file =>
        JSON.Parse(File.ReadAllText(Application.dataPath + "/MiniTemplate/output_json" + "/" + file + ".json")
        ));
    }
}
