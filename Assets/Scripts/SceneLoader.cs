using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Image progressBarMask;
    [SerializeField] private GameObject progressBarCanvas;

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
        CanvasGroup group = progressBarCanvas.GetComponent<CanvasGroup>();
        group.alpha = 0f;
        StartCoroutine(LoadAsynchronously(index, group));
    }

    private IEnumerator LoadAsynchronously(int index, CanvasGroup group)
    {
        while(group.alpha < 1)
        {
            group.alpha += Time.deltaTime * 3f;
            yield return null;
        }

        yield return new WaitForSeconds(0.25f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(index);

        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressBarMask.fillAmount = progress;

            yield return null;
        }
    }
}
