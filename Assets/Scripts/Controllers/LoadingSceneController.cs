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
        //�ⲯ ���� �ε��� �� ������ �Ʒ� ����ũ �ε��� �� �ּ¹��� �ε���.
        //���ҽ� �ε��� ������ �� ������ �Űܰ��ٸ� ������ ���� ��
        //�Ʒ��� �����ϸ� ���� 90������ �ε���.
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