using System.Collections;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [Tooltip("Will be the main camera if not set")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Vector2 parallaxEffectMultiplier;
    [SerializeField] private float smoothSpeed;
    private Vector3 lastCameraPosition;
    private float textureUnitSizeX;
    private bool initialized;

    private IEnumerator Start()
    {
        yield return ProceduralGeneration.IsReady;
        yield return new WaitForEndOfFrame();

        if (!cameraTransform)
        {
            cameraTransform = Camera.main.transform;
        }

        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        Vector2 baseResolution = new Vector2(1920, 1080);
        float reductionValue = screenSize.magnitude / baseResolution.magnitude;
        parallaxEffectMultiplier = new Vector2(parallaxEffectMultiplier.x * reductionValue, parallaxEffectMultiplier.y * reductionValue * 1.25f);

        yield return new WaitForEndOfFrame();
        lastCameraPosition = cameraTransform.position;
        transform.position = new Vector3(lastCameraPosition.x, lastCameraPosition.y, transform.position.z);

        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Sprite sprite = spriteRenderer.sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;

        initialized = true;
    }

    private void LateUpdate()
    {
        if(!initialized)
        {
            return;
        }

        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        deltaMovement.z = 0;
        Vector3 deltaPos = new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, transform.position + deltaPos, smoothSpeed * Time.deltaTime); 
        lastCameraPosition = cameraTransform.position;

        if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
        {
            float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, transform.position.y);
        }
    }

    public void SetPosition(Vector3 position)
    {
        lastCameraPosition = new Vector3(position.x, position.y, lastCameraPosition.z);
        transform.position = position;
    }
}
