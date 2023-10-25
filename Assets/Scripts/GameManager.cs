using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.XR;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private Transform pickedUpElementsUIParent;
    [SerializeField] private Transform elementHolderTransform;
    [SerializeField] private GameObject pickedUpElementPrefab;
    [SerializeField] private GameObject dragDropElementSlotPrefab;
    [SerializeField] private GameObject playerGameobject;
    [SerializeField] private GameObject elementSelectorGameobject;
    [SerializeField] private TMP_Text elementsLeftText;
    [SerializeField] private Button finishButton;

    private int spawnedElementCount;
    private List<PeriodicTable.Element> pickedUpElements = new();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Debug.LogError("Multiple gamemagers in the scene! Deleting the extra.");
            Destroy(this);
            return;
        }

        finishButton.interactable = false;
    }

#if UNITY_EDITOR
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            FinishCollection();
        }

        if(Input.GetKeyDown(KeyCode.K))
        {
            FindObjectOfType<SceneLoader>().LoadScene(0);
        }
    }
#endif

    public void SetSpawnedElementCount(int count)
    {
        spawnedElementCount = count;
        elementsLeftText.text = $"Elements Left: {spawnedElementCount - pickedUpElements.Count}";
    }

    public void AddPickedUpElement(PeriodicTable.Element element)
    {
        if(pickedUpElements.Contains(element))
        {
            return;
        }

        pickedUpElements.Add(element);
        if(pickedUpElements.Count == 1)
        {
            Destroy(pickedUpElementsUIParent.GetChild(0).gameObject);
        }

        GameObject instantiatedGO = Instantiate(pickedUpElementPrefab, pickedUpElementsUIParent);
        TMP_Text text = instantiatedGO.GetComponentInChildren<TMP_Text>();
        text.text = $"{element.symbol}<sup>{element.atomicMass.ToString("F")}g</sup>";
        if(element.elementColor == new Color(0,0,0,0))
        {
            element.elementColor = Color.white;
        }
        text.color = element.elementColor;
        elementsLeftText.text = $"Elements Left: {spawnedElementCount - pickedUpElements.Count}";

        if (pickedUpElements.Count == spawnedElementCount)
        {
            finishButton.interactable = true;
        }
    }

    public void FinishCollection()
    {
        playerGameobject.SetActive(false);
        elementSelectorGameobject.SetActive(true);

        for (int i = 0; i < pickedUpElements.Count; i++)
        {
            PeriodicTable.Element element = pickedUpElements[i];
            GameObject prefab = Instantiate(dragDropElementSlotPrefab, elementHolderTransform);
            PeriodicTable.Element instantiatedElement = prefab.AddComponent<PeriodicTable.Element>();
            instantiatedElement.listIndex = element.listIndex;
            instantiatedElement.atomicNumber = element.atomicNumber;
            instantiatedElement.elementName = element.elementName;
            instantiatedElement.symbol = element.symbol;
            instantiatedElement.atomicMass = element.atomicMass;
            instantiatedElement.elementColor = element.elementColor;

            TMP_Text text = prefab.GetComponentInChildren<TMP_Text>();
            text.text = $"{element.symbol}<sup>{element.atomicMass.ToString("F")}g</sup>";
        }
    }
}
