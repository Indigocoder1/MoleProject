using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingIdentifier : MonoBehaviour
{
    [SerializeField] private Color pingColor;

    public Color GetPingColor()
    {
        return pingColor;
    }
}
