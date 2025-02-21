using System.Collections.Generic;
using UnityEngine;

public class WordSelectionManager : MonoBehaviour
{
    public static WordSelectionManager Instance; // Singleton for global access
    private List<string> selectedTiles = new List<string>();

    void Awake()
    {
        Instance = this;
    }

    public void AddTile(TileData tile)
    {
        if (!selectedTiles.Contains(tile.Letter))
        {
            selectedTiles.Add(tile.Letter);
            Debug.Log("Selected Tiles: " + string.Join(", ", selectedTiles));
        }
    }
}
