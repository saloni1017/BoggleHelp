using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WordSelectionManager : MonoBehaviour
{
    public static WordSelectionManager Instance; // Singleton for global access

    private List<TileData> selectedTiles = new List<TileData>(); // Stores selected tiles for the current swipe
    private List<string> finalWordsList = new List<string>(); // Stores only valid words

    private HashSet<string> validWords = new HashSet<string>(); // Fast lookup for valid words
    private bool isSelecting = false; // Track if the player is actively swiping

    private string wordListPath = @"C:\Sameer\BoggleHelp\Assets\wordlist.txt"; // Path to the word list file

    [SerializeField]
    private TextMeshProUGUI ScoreCountText;
    [SerializeField]
    private TextMeshProUGUI FoundCount;
    [SerializeField]
    private GameObject GameOverScreen;

    void Awake()
    {
        Instance = this;
        LoadValidWords(); // Load valid words at the start
        GameOverScreen.SetActive(false);
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
            if (!tile.IsDisabled) // Ensure already disabled tiles are not modified
            {
                tile.DisableTile(); // Prevents re-selection of these tiles
            }
        }
    }


    // Print all valid words stored so far
    private void PrintFinalWordsList()
    {
        Debug.Log("Final List of Valid Words: " + string.Join(", ", finalWordsList));
        int letterCount = 0;

        // Count total letters in all valid words
        foreach (string word in finalWordsList)
        {
            letterCount += word.Length;
        }

        FoundCount.text = finalWordsList.Count.ToString();
        ScoreCountText.text = (letterCount * 3).ToString();
        Scene activeScene = SceneManager.GetActiveScene();
        
        if (activeScene.name == "Level" && finalWordsList.Count == GridCreator.GCInstance.WordToWin)
            GameOverScreen.SetActive(true);
        if(activeScene.name == "Endless" && finalWordsList.Count == EndlessGrid.Instance.WordToWin)
        {
            GameOverScreen.SetActive(true);
        }
    }
}
