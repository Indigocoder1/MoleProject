using UnityEngine;

public class MoleMovement : MonoBehaviour
{
    [SerializeField] private float defaultSpeed;
    [SerializeField] private float superSpeed;
    [SerializeField] private Transform visualTransform;
    [SerializeField] private ProceduralGeneration proceduralGeneration;
    [SerializeField] private ParticleSystem dirtParticleSystem;
    private Vector3 previousPos;
    private ParticleSystem.EmissionModule emissionModule;

    private void Start()
    {
        previousPos = transform.position;
        emissionModule = dirtParticleSystem.emission;
        emissionModule.enabled = false;
    }

    private void Update()
    {
        if (!ProceduralGeneration.IsReady)
        {
            return;
        }

        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target.z = transform.position.z;

        float speed = SettingsManager.superspeed ? superSpeed : defaultSpeed;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        HandleRotation(target);
        HandleParticles();

        if(Vector3ToInt(previousPos) != Vector3ToInt(transform.position))
        {
            HandlePositionLimits();
        }

        previousPos = transform.position;
    }

    private Vector3Int Vector3ToInt(Vector3 v)
    {
        int x = (int)v.x;
        int y = (int)v.y;
        int z = (int)v.z;
        return new Vector3Int(x, y, z);
    }

    private void HandleRotation(Vector3 target)
    {
        Vector3 look = visualTransform.InverseTransformPoint(target);
        float angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg;
        visualTransform.Rotate(0, 0, angle);
    }

    private void HandlePositionLimits()
    {
        Vector3 pos = transform.position;

        if (pos.x < 1 || pos.x > proceduralGeneration.GetMapSize().x ||
            pos.y > proceduralGeneration.GetPerlinHeightArray()[(int)pos.x] || pos.y < 1)
        {
            transform.position = new Vector3(previousPos.x, previousPos.y, transform.position.z);
        }
        else
        {
            proceduralGeneration.SetTile(new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z), false);
        }
    }

    private void HandleParticles()
    {
        if ((previousPos - transform.position).magnitude >= 0.02f)
        {
            emissionModule.enabled = true;
        }
        else
        {
            emissionModule.enabled = false;
        }
    }
}
