using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPositioner : MonoBehaviour
{
    [SerializeField] private ProceduralGeneration proceduralGeneration;
    [SerializeField] private Transform character;
    [SerializeField] private List<ParallaxBackground> parallaxBackgrounds = new List<ParallaxBackground>();

    public void PositionCharacter()
    {
        Vector2Int pos = proceduralGeneration.GetHighestPoint();
        Vector3 position = new Vector3(pos.x, pos.y - 5, character.position.z);
        character.position = position;

        foreach (ParallaxBackground background in parallaxBackgrounds)
        {
            background.SetPosition(position);
            Debug.Log($"Setting {background.transform.name}'s potition to {position}");
        }
    }
}
