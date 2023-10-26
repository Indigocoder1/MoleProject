using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Image progressBarMask;
    [SerializeField] private GameObject progressBarCanvas;
    [SerializeField] private TextMeshProUGUI headerText;
    private CanvasGroup canvasGroup;
    private float targetAlpha;
    private const float fadeSpeed = 3f;

    private void Start()
    {
        canvasGroup = progressBarCanvas.GetComponent<CanvasGroup>();
    }

    public void LoadScene(int index)
    {
        SetText("Loading...");
        SceneManager.LoadScene(index);
    }

    public void LoadScene(string name)
    {
        SetText("Loading...");
        SceneManager.LoadScene(name);
    }

    public void LoadSceneAsync(int index)
    {
        SetText("Loading...");
        canvasGroup.alpha = 0f;
        StartCoroutine(LoadAsynchronously(index));
    }

    public void EnableLoadingScreen(bool fade = true)
    {
        if(!fade)
        {
            canvasGroup.alpha = 1;
        }

        targetAlpha = 1;
    }

    public void DisableLoadingScreen()
    {
        targetAlpha = 0;
    }

    public void SetText(string text)
    {
        headerText.text = text;
    }

    public bool ReachedTargetAlpha()
    {
        return Mathf.Approximately(canvasGroup.alpha, targetAlpha);
    }

    private IEnumerator LoadAsynchronously(int index)
    {
        if(canvasGroup.alpha < 1)
        {
            EnableLoadingScreen();
        }

        yield return canvasGroup.alpha >= 1;
        yield return new WaitForSeconds(0.5f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(index);

        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressBarMask.fillAmount = progress;

            yield return null;
        }
    }

    private void Update()
    {
        if(canvasGroup.alpha != targetAlpha)
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, Time.deltaTime * fadeSpeed);
        }
    }
}
