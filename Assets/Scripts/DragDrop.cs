using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Animator animator;
    private Transform parentAfterDrag;
    private Transform originalParent;

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        animator = GetComponent<Animator>();
        originalParent = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        animator.SetBool("BeingHeld", true);
        canvasGroup.blocksRaycasts = false;

        parentAfterDrag = transform.parent;
        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position += (Vector3)eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        animator.SetBool("BeingHeld", false);

        bool overSlot = false;
        for (int i = 0; i < eventData.hovered.Count; i++)
        {
            if (eventData.hovered[i].GetComponent<ItemSlot>() != null)
            {
                overSlot = true;
                break;
            }
        }

        if(!overSlot)
        {
            ResetParent();
            canvasGroup.blocksRaycasts = true;
        }

        transform.SetParent(parentAfterDrag);
    }

    public void SetItemParent(Transform parent)
    {
        transform.SetParent(parent);
        parentAfterDrag = parent;
    }

    public void ResetParent()
    {
        transform.SetParent(originalParent);
        parentAfterDrag = originalParent;
    }
}
