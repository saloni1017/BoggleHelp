using UnityEngine;
using UnityEngine.EventSystems;

public class TileData : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public bool IsSelected;
    public string Letter;
    public int TileType;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SelectTile();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Check if dragging over another tile
        RaycastHit2D hit = Physics2D.Raycast(eventData.position, Vector2.zero);
        if (hit.collider != null)
        {
            TileData tile = hit.collider.GetComponent<TileData>();
            if (tile != null && !tile.IsSelected)
            {
                tile.SelectTile();
            }
        }
    }

    private void SelectTile()
    {
        if (!IsSelected)
        {
            IsSelected = true;
            WordSelectionManager.Instance.AddTile(this);
        }
    }
}
