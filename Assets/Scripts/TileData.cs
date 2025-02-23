using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Diagnostics;
using UnityEngine.SceneManagement;
using System.Collections;

public class TileData : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public bool IsSelected; // Track if tile is selected
    public bool IsDisabled; // Prevents selection of tiles used in valid words
    public string Letter;   // Letter assigned to this tile
    public int TileType;    // Tile type (not used here but can be for different types of tiles)
    public List<Image> dots; // List of dot images to indicate score
    public GameObject Bug;
    public bool IsBug;
    public GameObject Block;
    public Animator animator;

    private void Awake()
    {
        IsBug = false;
        IsSelected = false;
        IsDisabled = false; // Ensure tiles are selectable at start
    }

    // Called when the player touches a tile
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!IsSelected && !IsDisabled) // Only select if tile is active
        {
            WordSelectionManager.Instance.StartSelection();
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
            if (tile != null && !tile.IsSelected && !tile.IsDisabled)
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
        if (!IsSelected && !IsDisabled) // Prevents re-selection in the same swipe
        {
            IsSelected = true;
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
        IsDisabled = true; // This prevents future selection
        ChangeDotColor(Color.white); // Change dots to white for valid words
    }

    // Change the color of all dots
    public void ChangeDotColor(Color newColor)
    {
        Scene activeScene = SceneManager.GetActiveScene();
        if(activeScene.name == "Level")
        {
            foreach (Image dot in dots)
            {
                if (dot != null)
                {
                    dot.color = newColor;
                }
            }
        }
        else
        {
            animator.Play("FadeOut");
            StartCoroutine(Delay());
            animator.Play("FadeIn");
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.2f);
    }
}
