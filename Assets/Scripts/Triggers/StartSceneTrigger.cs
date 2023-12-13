using UnityEngine;

public class StartSceneTrigger : MonoBehaviour
{
    private void Awake()
    {
        SetGameObject();
        SetMusic();
        Managers.Instance.UIManager.OpenUI<TitleUI>();
    }

    private void SetGameObject()
    {
        var obj = new GameObject("StartSceneController");
        obj.AddComponent<StartSceneController>();

        obj = Resources.Load<GameObject>(Const.Prefabs_TitleBG);
        obj = Instantiate(obj);
        obj.name = "TitleBG";
    }

    private void SetMusic()
    {
        Managers.Instance.SoundManager.PlayMusic(MusicSource.LetTheAdventureBegin);
    }
}
