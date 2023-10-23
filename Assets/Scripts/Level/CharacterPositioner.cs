using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterPositioner : MonoBehaviour
{
    [SerializeField] private ProceduralGeneration proceduralGeneration;
    [SerializeField] private Transform character;

    private IEnumerator Start()
    {
        yield return ProceduralGeneration.IsReady;

        Vector2Int pos = proceduralGeneration.GetHighestPoint();
        Vector3 position = new Vector3(pos.x, pos.y - 5, character.position.z);
        character.position = position;
    }
}
