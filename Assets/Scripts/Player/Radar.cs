using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    [SerializeField] LayerMask allowedLayers;
    [SerializeField] private GameObject pingPrefab;
    [SerializeField] private Transform sweepTransform;
    [SerializeField] private float defaultRotationSpeed;
    [SerializeField] private float fastRotationSpeed;
    [SerializeField] private float radarDistance;
    private float rotationSpeed;
    private float angleLastFrame;

    private List<Collider2D> colliderList = new List<Collider2D>();

    private void Start()
    {
        rotationSpeed = SettingsManager.fastRadar ? fastRotationSpeed : defaultRotationSpeed;
        angleLastFrame = sweepTransform.eulerAngles.z;
    }

    private void Update()
    {
        float previousRotation = (sweepTransform.eulerAngles.z % 360) - 180;
        sweepTransform.eulerAngles -= new Vector3(0, 0, rotationSpeed * Time.deltaTime);
        float currentRotation = (sweepTransform.eulerAngles.z % 360) - 180;

        if(previousRotation < 0 && currentRotation >= 0)
        {
            colliderList.Clear();
        }

        for (float i = sweepTransform.eulerAngles.z; i < angleLastFrame; i += 0.5f)
        {
            float angleRad = i * Mathf.Deg2Rad;
            Vector3 angleVector = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, angleVector, radarDistance, allowedLayers);

            for (int g = 0; g < hits.Length; g++)
            {
                RaycastHit2D hit2D = hits[g];
                if (hit2D.collider == null)
                {
                    return;
                }

                if (!colliderList.Contains(hit2D.collider))
                {
                    colliderList.Add(hit2D.collider);

                    GameObject prefab = Instantiate(pingPrefab, hit2D.point, Quaternion.identity);
                    if (hit2D.collider.TryGetComponent<PingIdentifier>(out PingIdentifier identifier))
                    {
                        prefab.GetComponent<RadarPing>().SetColor(identifier.GetPingColor());
                    }
                }
            }
        }

        angleLastFrame = sweepTransform.eulerAngles.z;
    }
}
