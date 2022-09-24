//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;
using SimpleJSON;



namespace cfg.test
{

public sealed partial class TbRespawnPonts
{
    private readonly Dictionary<int, test.Respawn> _dataMap;
    private readonly List<test.Respawn> _dataList;
    
    public TbRespawnPonts(JSONNode _json)
    {
        _dataMap = new Dictionary<int, test.Respawn>();
        _dataList = new List<test.Respawn>();
        
        foreach(JSONNode _row in _json.Children)
        {
            var _v = test.Respawn.DeserializeRespawn(_row);
            _dataList.Add(_v);
            _dataMap.Add(_v.Id, _v);
        }
        PostInit();
    }

    public Dictionary<int, test.Respawn> DataMap => _dataMap;
    public List<test.Respawn> DataList => _dataList;

    public test.Respawn GetOrDefault(int key) => _dataMap.TryGetValue(key, out var v) ? v : null;
    public test.Respawn Get(int key) => _dataMap[key];
    public test.Respawn this[int key] => _dataMap[key];

    public void Resolve(Dictionary<string, object> _tables)
    {
        foreach(var v in _dataList)
        {
            v.Resolve(_tables);
        }
        PostResolve();
    }

    public void TranslateText(System.Func<string, string, string> translator)
    {
        foreach(var v in _dataList)
        {
            v.TranslateText(translator);
        }
    }
    
    
    partial void PostInit();
    partial void PostResolve();
}

}