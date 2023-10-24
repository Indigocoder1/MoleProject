using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EvaluateManager : MonoBehaviour
{
    [SerializeField] private TMP_Text header;
    [SerializeField] private TMP_Text body;
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private Button button;
    [SerializeField] private SceneLoader sceneLoader;

    public void Evaluate(PeriodicTable.Element element)
    {
        if(element.atomicMass == PeriodicTable.Table.elements[element.listIndex].atomicMass)
        {
            EvaluateCorrect();
        }
        else
        {
            EvaluateIncorrect();
        }
    }

    private void EvaluateCorrect()
    {
        header.color = Color.green;
        header.text = "Nice Job!";
        body.text = "You guessed correctly!";
        buttonText.text = "Main Menu";
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(GoToMainMenu);
    }

    private void EvaluateIncorrect()
    {
        header.color = Color.red;
        header.text = "Almost!";
        body.text = "You guessed wrong. Try again!";
        buttonText.text = "Close";
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(CloseTab);
    }

    private void GoToMainMenu()
    {
        sceneLoader.LoadSceneAsync(0);
    }

    private void CloseTab()
    {
        gameObject.SetActive(false);
    }
}
