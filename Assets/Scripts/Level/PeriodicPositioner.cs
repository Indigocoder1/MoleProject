using System.Collections;
using UnityEngine;
using TMPro;
using static PeriodicTable;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class PeriodicPositioner : MonoBehaviour
{
    private const int maxIterationNum = 10;

    [SerializeField] private ProceduralGeneration proceduralGeneration;
    [SerializeField] private GameObject elementPrefab;
    [Range(0, 250f)]
    [SerializeField] private float minSpacingDistance;

    private List<Vector2Int> spawnedPointPositions = new List<Vector2Int>();
    private bool exactQuantityAdded;
    private List<int> elementsAdded = new List<int>();
    private int elementsAddedSoFar;
    private int elementsToSpawn;
    private int currentIterationNum;

    private IEnumerator Start()
    {
        yield return ProceduralGeneration.IsReady;

        elementsToSpawn = Random.Range(SettingsManager.minElements, SettingsManager.maxElements);
        GameManager.Instance.SetSpawnedElementCount(elementsToSpawn);
        for (int i = 0; i < elementsToSpawn; i++)
        {
            float minSpacingDistance = proceduralGeneration.GetMapSize().magnitude * (this.minSpacingDistance / 1000);
            Vector2Int position = GetSpacedPosition(minSpacingDistance, 0);

            spawnedPointPositions.Add(position);
            Element element = GetRandomElement(out int elementNumber);

            SpawnElement(element, elementNumber, position);
        }
    }

    public void SpawnElement(Element element, int elementNumber, Vector2 position)
    {
        GameObject prefab = Instantiate(elementPrefab, new Vector3(position.x, position.y, -0.1f), Quaternion.identity);
        Element prefabElement = prefab.AddComponent<Element>();
        prefabElement.listIndex = elementNumber;
        prefabElement.atomicNumber = element.atomicNumber;
        prefabElement.symbol = element.symbol;
        prefabElement.elementName = element.elementName;
        prefabElement.atomicMass = element.atomicMass;
        prefabElement.elementColor = element.elementColor;

        if (!exactQuantityAdded && Random.Range(0f, 100f) > 50)
        {
            exactQuantityAdded = true;
        }
        else if (elementsAddedSoFar == elementsToSpawn - 1 && !exactQuantityAdded)
        {
            exactQuantityAdded = true;
        }
        else
        {
            float variation = MathF.Round(prefabElement.atomicMass + Random.Range(-5f, 5f), 2);
            prefabElement.atomicMass = variation;
        }

        TMP_Text text = prefab.GetComponentInChildren<TMP_Text>();
        text.text = $"{prefabElement.elementName}<sup>{prefabElement.atomicMass.ToString("F")}g</sup>";
        elementsAddedSoFar++;
    }

    private Vector2Int GetSpacedPosition(float minSpacingDistance, int iterationNum)
    {
        Vector2Int pos = proceduralGeneration.GetSpawnPosition();
        Vector2Int highestPoint = proceduralGeneration.GetHighestPoint();
        Vector3 characterPosition = new Vector3(highestPoint.x, highestPoint.y - 5, 0);

        if (iterationNum > maxIterationNum)
        {
            return pos;
        }

        for (int i = 0; i < spawnedPointPositions.Count; i++)
        {
            if(Vector2.Distance(pos, spawnedPointPositions[i]) <= minSpacingDistance)
            {
                pos = GetSpacedPosition(minSpacingDistance, iterationNum + 1);
            }

            if(Vector2.Distance(pos, characterPosition) <= minSpacingDistance)
            {
                pos = GetSpacedPosition(minSpacingDistance, iterationNum + 1);
            }
        }

        return pos;
    }

    private Element GetRandomElement(out int elementIndex)
    {
        Element element = null;
        int elementNumber = Random.Range(1, SettingsManager.maxAtomicNumber);

        if(!elementsAdded.Contains(elementNumber))
        {
            element = Table.elements[elementNumber];
            elementsAdded.Add(elementNumber);
        }
        else
        {
            element = GetRandomElement(out elementNumber);
        }

        elementIndex = elementNumber;
        return element;
    }
}
