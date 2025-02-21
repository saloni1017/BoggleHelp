using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WordSelectionManager : MonoBehaviour
{
    public static WordSelectionManager Instance; // Singleton for global access

    private List<TileData> selectedTiles = new List<TileData>(); // Stores selected tiles for the current swipe
    private List<string> finalWordsList = new List<string>(); // Stores only valid words

    private HashSet<string> validWords = new HashSet<string>(); // Fast lookup for valid words
    private bool isSelecting = false; // Track if the player is actively swiping

    private string wordListPath = @"C:\New folder\BoggleWord-main\Assets\wordlist.txt"; // Path to the word list file

    void Awake()
    {
        Instance = this;
        LoadValidWords(); // Load valid words at the start
    }

    // Load words from wordlist.txt into a HashSet for quick validation
    private void LoadValidWords()
    {
        if (File.Exists(wordListPath))
        {
            string[] words = File.ReadAllLines(wordListPath);
            foreach (string word in words)
            {
                validWords.Add(word.Trim().ToUpper()); // Convert to uppercase for case-insensitive matching
            }
            Debug.Log($"Loaded {validWords.Count} valid words.");
        }
        else
        {
            Debug.LogError($"Word list file not found: {wordListPath}");
        }
    }

    // Start a new word selection (clears previous selection)
    public void StartSelection()
    {
        isSelecting = true;
        selectedTiles.Clear(); // Clear the previous selection
    }

    // Add a tile to the selected word
    public void AddTile(TileData tile)
    {
        if (!selectedTiles.Contains(tile)) // Avoid duplicate letters in a single word
        {
            selectedTiles.Add(tile);
            Debug.Log("Current Word: " + GetCurrentWord());
        }
    }

    // Stop selection and validate the word
    public void StopSelection()
    {
        if (isSelecting && selectedTiles.Count > 0)
        {
            string finalWord = GetCurrentWord().ToUpper(); // Convert to uppercase

            if (validWords.Contains(finalWord)) // Check if the word is valid
            {
                finalWordsList.Add(finalWord); // Store valid words permanently
                Debug.Log("Valid Word Stored: " + finalWord);
                DisableValidTiles(); // Prevent re-selection of these tiles
            }
            else // Word is invalid → Reset tiles so they can be used again
            {
                Debug.Log("Invalid Word (Not Stored): " + finalWord);
                ResetInvalidTiles();
            }

            PrintFinalWordsList(); // Print all valid words stored so far
            selectedTiles.Clear(); // Reset selection for the next word
            isSelecting = false;
        }
    }

    // Get the word formed by the selected tiles
    private string GetCurrentWord()
    {
        return string.Join("", selectedTiles.ConvertAll(tile => tile.Letter));
    }

    // Reset tiles from an invalid word so they can be selected again
    private void ResetInvalidTiles()
    {
        foreach (TileData tile in selectedTiles)
        {
            tile.ResetTile(); // Unlock tile for future use
        }
    }

    // Disable tiles from a valid word so they can't be selected again
    private void DisableValidTiles()
    {
        foreach (TileData tile in selectedTiles)
        {
            tile.DisableTile(); // Prevents re-selection of these tiles
        }
    }

    // Print all valid words stored so far
    private void PrintFinalWordsList()
    {
        Debug.Log("Final List of Valid Words: " + string.Join(", ", finalWordsList));
    }
}
