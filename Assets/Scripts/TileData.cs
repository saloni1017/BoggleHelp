using UnityEngine;
using UnityEngine.EventSystems;

public class TileData : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public bool IsSelected; // Track if tile is selected
    public string Letter;   // Letter assigned to this tile
    public int TileType;    // Tile type (not used here but can be for different types of tiles)

    private void Awake()
    {
        IsSelected = false; // Ensure tile is selectable at the start
    }

    // Called when the player touches a tile
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!IsSelected) // Prevent selecting already selected tiles
        {
            WordSelectionManager.Instance.StartSelection(); // Start a new word selection
            SelectTile();
        }
    }

    // Called when the player drags over a tile
    public void OnDrag(PointerEventData eventData)
    {
        RaycastHit2D hit = Physics2D.Raycast(eventData.position, Vector2.zero);
        if (hit.collider != null)
        {
            TileData tile = hit.collider.GetComponent<TileData>();
            if (tile != null && !tile.IsSelected) // Select the tile if it's not selected already
            {
                tile.SelectTile();
            }
        }
    }

    // Called when the player releases the drag (stops selection)
    public void OnPointerUp(PointerEventData eventData)
    {
        WordSelectionManager.Instance.StopSelection();
    }

    // Mark tile as selected
    public void SelectTile()
    {
        if (!IsSelected)
        {
            IsSelected = true; // Prevents re-selection in the same swipe
            WordSelectionManager.Instance.AddTile(this);
        }
    }

    // Reset tile selection (only for invalid words)
    public void ResetTile()
    {
        IsSelected = false; // Allows the tile to be used again
    }
}
