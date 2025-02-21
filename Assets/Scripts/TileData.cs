using UnityEngine;
using UnityEngine.EventSystems;

public class TileData : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public bool IsSelected;
    public string Letter;
    public int TileType;

    private void Awake()
    {
        IsSelected = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        WordSelectionManager.Instance.StartSelection();
        SelectTile();
    }

    public void OnDrag(PointerEventData eventData)
    {
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

    public void OnPointerUp(PointerEventData eventData)
    {
        WordSelectionManager.Instance.StopSelection();
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
