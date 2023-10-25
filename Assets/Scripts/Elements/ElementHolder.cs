using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ElementHolder : MonoBehaviour
{
    [SerializeField] private GameObject dummySlot;
    [SerializeField] private GameObject evaluateManagerGO;
    [SerializeField] private Transform selectedSlot;
    [SerializeField] private Button finalizeButton;
    [SerializeField] private TMP_Text selectedElementText;
    private PeriodicTable.Element currentElement;

    private void Update()
    {
        dummySlot.SetActive(transform.childCount == 1);

        if(selectedSlot.childCount > 0)
        {
            finalizeButton.interactable = true;
            if(selectedSlot.GetChild(0).TryGetComponent<PeriodicTable.Element>(out PeriodicTable.Element element))
            {
                selectedElementText.text = $"Selected Element: {element.elementName} (#{element.atomicNumber} | {element.atomicMass}g)";
                currentElement = element;
            }
        }
        else
        {
            finalizeButton.interactable = false;
            currentElement = null;
            selectedElementText.text = "Selected Element: None";
        }
    }

    public void Evaluate()
    {
        evaluateManagerGO.SetActive(true);
        evaluateManagerGO.GetComponent<EvaluateManager>().Evaluate(currentElement);
    }
}