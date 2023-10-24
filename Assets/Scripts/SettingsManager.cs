using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Slider mapWidthSlider;
    [SerializeField] private Slider mapHeightSlider;
    [SerializeField] private Slider mapSeedSlider;
    [SerializeField] private Slider minElementsSlider;
    [SerializeField] private Slider maxElementsSlider;
    [SerializeField] private Slider maxAtmoicNumberSlider;
    [SerializeField] private Toggle superspeedToggle;
    [SerializeField] private Toggle fastRadarToggle;

    public static int mapWidth = 500;
    public static int mapHeight = 150;
    public static int mapSeed = 51;
    public static int minElements = 4;
    public static int maxElements = 8;
    public static int maxAtomicNumber = 100;
    public static bool superspeed = false;
    public static bool fastRadar = false;

    private int previousSceneIndex = -1;

    private void Start()
    {
        TryLoadFromPlayerPrefs();
    }

    private void Update()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentIndex == 0 && currentIndex != previousSceneIndex)
        {
            Debug.Log("In main menu");

            mapWidthSlider.value = mapWidth;
            mapHeightSlider.value = mapHeight;
            mapSeedSlider.value = mapSeed;
            minElementsSlider.value = minElements;
            maxElementsSlider.value = maxElements;
            maxAtmoicNumberSlider.value = maxAtomicNumber;
            superspeedToggle.isOn = superspeed;
            fastRadarToggle.isOn = fastRadar;
        }

        previousSceneIndex = currentIndex;
    }

    public void SaveSettings()
    {
        mapWidth = (int)mapWidthSlider.value;
        mapHeight = (int)mapHeightSlider.value;
        mapSeed = (int)mapSeedSlider.value;
        minElements = (int)minElementsSlider.value;
        maxElements = (int)maxElementsSlider.value;
        maxAtomicNumber = (int)maxAtmoicNumberSlider.value;
        superspeed = superspeedToggle.isOn;
        fastRadar = fastRadarToggle.isOn;
        SaveToPlayerPrefs();
    }

    private void TryLoadFromPlayerPrefs()
    {
        if(TryGetPrefValue("mapWidth", out int prefMapWidth)) mapWidth = prefMapWidth;
        if (TryGetPrefValue("mapHeight", out int prefMapHeight)) mapHeight = prefMapHeight;
        if (TryGetPrefValue("mapSeed", out int prefMapSeed)) mapSeed = prefMapSeed;
        if(TryGetPrefValue("minElements", out int prefMinElements)) minElements = prefMinElements;
        if(TryGetPrefValue("maxElements", out int prefMaxElements)) maxElements = prefMaxElements;
        if(TryGetPrefValue("maxAtomic", out int prefMaxAtomic)) maxAtomicNumber = prefMaxAtomic;
        if (TryGetPrefValue("superspeed", out int prefSuperspeed)) superspeed = prefSuperspeed == 1 ? true : false;
        if (TryGetPrefValue("fastRadar", out int prefFastRadar)) fastRadar = prefFastRadar == 1 ? true : false;
    }

    private void SaveToPlayerPrefs()
    {
        PlayerPrefs.SetInt("mapWidth", mapWidth);
        PlayerPrefs.SetInt("mapHeight", mapHeight);
        PlayerPrefs.SetInt("mapSeed", mapSeed);
        PlayerPrefs.SetInt("minElements", minElements);
        PlayerPrefs.SetInt("maxElements", maxElements);
        PlayerPrefs.SetInt("maxAtomic", maxAtomicNumber);
        PlayerPrefs.SetInt("superspeed", superspeed ? 1 : 0);
        PlayerPrefs.SetInt("fastRadar", fastRadar ? 1 : 0);
    }

    private bool TryGetPrefValue(string key, out int value)
    {
        if(PlayerPrefs.HasKey(key))
        {
            value = PlayerPrefs.GetInt(key);
            return true;
        }
        else
        {
            value = -1;
            return false;
        }
    }
}
