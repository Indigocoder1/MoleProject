using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    private DragDrop previousDragDrop;

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null && eventData.pointerDrag.TryGetComponent<DragDrop>(out DragDrop dragDrop))
        {
            if(transform.childCount != 0 && previousDragDrop != null)
            {
                previousDragDrop.GetComponent<CanvasGroup>().blocksRaycasts = true;
                previousDragDrop.ResetParent();
            }

            dragDrop.SetItemParent(transform);
            dragDrop.GetComponent<CanvasGroup>().blocksRaycasts = false;
            previousDragDrop = dragDrop;
        }
    }
}
