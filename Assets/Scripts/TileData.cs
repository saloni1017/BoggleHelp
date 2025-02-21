using UnityEngine;
using UnityEngine.EventSystems;

public class TileData : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public bool IsSelected; // Track if tile is selected
    public bool IsDisabled; // Prevents selection of tiles used in valid words
    public string Letter;   // Letter assigned to this tile
    public int TileType;    // Tile type (not used here but can be for different types of tiles)

    private void Awake()
    {
        IsSelected = false;
        IsDisabled = false; // Initially, all tiles are selectable
    }

    // Called when the player touches a tile
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!IsSelected && !IsDisabled) // Prevent selecting already selected or disabled tiles
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
            if (tile != null && !tile.IsSelected && !tile.IsDisabled) // Select only if not selected or disabled
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
        if (!IsSelected && !IsDisabled)
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

    // Disable tile (used for valid words so they can't be selected again)
    public void DisableTile()
    {
        IsDisabled = true;
    }
}
