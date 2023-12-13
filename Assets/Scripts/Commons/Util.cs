using UnityEngine;

public class Util
{
    public static T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        if(!go.TryGetComponent<T>(out T component))
        {
            component = go.AddComponent<T>();
        }

        return component;
    }

    public T[,] Copy<T>(T[,] source)
    {
        int row = source.GetLength(0);
        int col = source.GetLength(1);

        T[,] copy = new T[row--, col--];

        for(; row > -1; row--)
        {
            for(; col > -1; col--)
            {
                copy[row, col] = source[row, col];
            }
        }

        return copy;
    }


    public static ItemType GetItemType(int id) => id switch
    {
        (>= 11000) and (< 12000) => ItemType.DishType,
        (>= 12000) and (< 13000) => ItemType.EtcType,
        (>= 13000) and (< 14000) => ItemType.FarmingitemType,
        (>= 14000) and (< 15000) => ItemType.MedicineType,
        (>= 15000) and (< 16000) => ItemType.SeedsType,
        (>= 16000) and (< 17000) => ItemType.BlueprintType,
        _ => ItemType.NoneType
    };


    public static void TurnOffGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }

    public static int BooleanToInt(bool value)
    {
        return value ? 1 : 0;
    }

    public static int GetBlueprintItemCode(BuildingType type)
    {
        var datas = Managers.Instance.DataManager.Items;
        foreach (var data in datas)
        {
            var item = data.Value;
            if (item.Id < 16000) 
                continue;
            if (item is not BlueprintData blueprint)
                continue;
            if (blueprint.BuildingType != type)
                continue;

            return item.Id;
        }
        return -1;
    }
}
