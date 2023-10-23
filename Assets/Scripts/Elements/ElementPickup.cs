using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ElementPickup : MonoBehaviour
{
    [SerializeField] private GameObject rootGameObject;
    [SerializeField] private GameObject popupPrefab;
    [SerializeField] private Animator animator;
    private bool pickedUp;

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        RectTransform rectTransform = transform.parent.GetComponent<RectTransform>();
        boxCollider.size = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "Player" && !pickedUp)
        {
            PeriodicTable.Element element = rootGameObject.GetComponent<PeriodicTable.Element>();
            GameManager.Instance.AddPickedUpElement(element);

            GameObject prefab = Instantiate(popupPrefab, transform.position + new Vector3(0, 0, -0.1f), Quaternion.identity);
            prefab.GetComponent<PickupPopup>().SetText($"Picked up {element.elementName}!");

            animator.SetTrigger("Pickup");
            pickedUp = true;
        }
    }
}
