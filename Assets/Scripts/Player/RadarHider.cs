using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarHider : MonoBehaviour
{
    [SerializeField] private Animator radarVisualAnimator;
    private bool open = true;

    private void Start()
    {
        radarVisualAnimator.SetBool("Open", open);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            ToggleRadarVisual();
        }
    }

    public void ToggleRadarVisual()
    {
        open = !open;
        radarVisualAnimator.SetBool("Open", open);
    }
}
