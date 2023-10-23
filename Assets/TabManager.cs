using System.Collections.Generic;
using UnityEngine;

public class TabManager : MonoBehaviour
{
    [SerializeField] private List<CanvasGroup> tabs = new List<CanvasGroup>();
    [SerializeField] private float fadeSpeed;
    private CanvasGroup currentTab;
    private CanvasGroup targetTab;

    private void Start()
    {
        currentTab = tabs[0];
        targetTab = currentTab;
        SetActiveTab(0);
        currentTab.blocksRaycasts = true;

        for (int i = 1; i < tabs.Count; i++)
        {
            tabs[i].blocksRaycasts = false;
            tabs[i].alpha = 0;
        }
    }

    private void Update()
    {
        if(targetTab == currentTab)
        {
            return;
        }

        if (currentTab.alpha > 0)
        {
            currentTab.alpha -= fadeSpeed * Time.deltaTime;
        }

        if(targetTab.alpha < 1)
        {
            targetTab.alpha += fadeSpeed * Time.deltaTime;
        }

        if(targetTab.alpha >= 1 && currentTab.alpha <= 0)
        {
            currentTab.blocksRaycasts = false;
            currentTab = targetTab;
            targetTab.blocksRaycasts = true;
        }
    }

    public void SetActiveTab(int tabIndex)
    {
        currentTab.blocksRaycasts = true;
        targetTab = tabs[tabIndex];
        targetTab.blocksRaycasts = false;
    }
}
