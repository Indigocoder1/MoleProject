using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [Tooltip("Not needed if you don't use Async loading in the scene")]
    [SerializeField] private Image progressBarMask;

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void LoadSceneAsync(int index)
    {
        StartCoroutine(LoadAsynchronously(index));
    }

    private IEnumerator LoadAsynchronously(int index)
    {
        yield return new WaitForSeconds(0.75f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(index);

        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressBarMask.fillAmount = progress;

            yield return null;
        }
    }
}
