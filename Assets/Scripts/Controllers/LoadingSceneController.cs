using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    private static SceneType _nextScene;
    [SerializeField] private Slider _progressBar;

    public static void LoadScene(SceneType type)
    {
        _nextScene = type;
        SceneManager.LoadScene(2);
    }

    private void Start()
    {
        Managers.Instance.SoundManager.PlaySFX(SFXSource.SceneChange);
        StartCoroutine(LoadSceneProgress());
    }

    public IEnumerator LoadSceneProgress()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync((int)_nextScene);
        //기껏 만든 로딩씬 좀 쓰려고 아래 페이크 로딩용 겸 애셋번들 로딩용.
        //리소스 로딩이 끝나기 전 씬으로 옮겨간다면 에셋이 깨질 것
        //아래로 설정하면 씬을 90까지만 로딩함.
        op.allowSceneActivation = false;

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;

            if (op.progress < 0.9f)
            {
                _progressBar.value = op.progress;

            }
            else
            {
                timer += Time.unscaledDeltaTime;
                _progressBar.value = Mathf.Lerp(0.9f, 1f, timer);
                if (_progressBar.value >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}