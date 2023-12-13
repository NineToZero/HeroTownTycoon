using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Scriptable Object/Building")]
public class BuildingSO : ScriptableObject
{
    public List<BuildingData> BaseList;
    public List<PassiveBuildingData> PassiveList;

    public BuildingData GetData(BuildingType type)
    {
        int typeNumber = (int)type;
        return BaseList[typeNumber];
    }

    [ContextMenu("RefreshFromJson")]
    private void RefreshFromJson()
    {
        BaseList.Clear();

        var json = File.ReadAllText(Path.Combine($"{Application.dataPath}/Resources/{Const.Jsons_BaseBuilding}.json"));
        BaseList = JsonConvert.DeserializeObject<List<BuildingData>>(json);
        json = File.ReadAllText(Path.Combine($"{Application.dataPath}/Resources/{Const.Jsons_PassiveBuilding}.json"));
        PassiveList = JsonConvert.DeserializeObject<List<PassiveBuildingData>>(json);
    }
}