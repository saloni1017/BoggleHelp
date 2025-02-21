using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WordSelectionManager : MonoBehaviour
{
    public static WordSelectionManager Instance;

    private List<string> selectedTiles = new List<string>(); // Stores current word
    private List<string> finalWordsList = new List<string>(); // Stores completed words

    private bool isSelecting = false; // Tracks active swipe

    void Awake()
    {
        Instance = this;
    }

    public void StartSelection()
    {
        if (!isSelecting)
        {
            isSelecting = true;
            selectedTiles.Clear(); // Clear for new word
        }
    }

    public void AddTile(TileData tile)
    {
        if (!selectedTiles.Contains(tile.Letter))
        {
            selectedTiles.Add(tile.Letter);
            Debug.Log("Current Word: " + string.Join("", selectedTiles));
        }
    }

    public void StopSelection()
    {
        if (isSelecting && selectedTiles.Count > 0)
        {
            string finalWord = string.Join("", selectedTiles);
            finalWordsList.Add(finalWord); // Store completed word

            Debug.Log("Word Stored: " + finalWord);
            PrintFinalWordsList(); // Print all words so far

            selectedTiles.Clear(); // Reset for new word
            isSelecting = false;
        }
    }

    private void PrintFinalWordsList()
    {
        Debug.Log("Final List of Words: " + string.Join(", ", finalWordsList));
    }
}
