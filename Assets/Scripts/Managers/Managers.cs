using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers _instance;
    public static Managers Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Managers>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("@Managers");
                    _instance = go.AddComponent<Managers>();
                }
                _instance.Init();
            }
            return _instance;
        }
    }

    //all singletone nono he
    public DataManager DataManager { get; private set; }
    public DayManager DayManager { get; private set; }
    public ItemManager ItemManager { get; private set; }
    public UIManager UIManager { get; private set; }
    public OptionManager OptionManager { get; private set; }
    public SoundManager SoundManager { get; private set; }

    private void Init()
    {
        OptionManager = _instance.gameObject.AddComponent<OptionManager>();
        DataManager = _instance.gameObject.AddComponent<DataManager>();
        ItemManager = _instance.gameObject.AddComponent<ItemManager>();
        DayManager = _instance.gameObject.AddComponent<DayManager>();
        UIManager = _instance.gameObject.AddComponent<UIManager>();
        SoundManager = _instance.gameObject.AddComponent<SoundManager>();
    }
}
