using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;
using Random = UnityEngine.Random;

public class GridCreator : MonoBehaviour
{
    public static GridCreator GCInstance;

    public TextMeshProUGUI CountText;
    public int WordToWin;

    [System.Serializable]
    public class GridSize
    {
        public int x; // Columns
        public int y; // Rows
    }
    [System.Serializable]
    public class Tile
    {
        public int tileType;
        public string letter;
    }

    [System.Serializable]
    public class GridData
    {
        public int bugCount;
        public int wordCount;
        public int timeSec;
        public int totalScore;
        public GridSize gridSize;
        public List<Tile> gridData;
    }

    [System.Serializable]
    public class RootObject
    {
        public List<GridData> data;
    }
    public GameObject gridPrefeb;
    private int spacing = 220;
    public GameObject parent;
    private string json;
    RootObject root;
    private bool IsInitialised = false;
    // Start is called before the first frame update

    void Awake()
    {
        GCInstance = this;
    }

    void Start()
    {
        string filePath = "C:\\Sameer\\BoggleHelp\\Assets\\LevelData.json";
        json = File.ReadAllText(filePath);
        root = JsonUtility.FromJson<RootObject>(json);
        IsInitialised = false;
        CreateGrid();
    }

    void CreateGrid()
    {
        if (!IsInitialised)
        {
            var index = Random.Range(0, 10);
            var grid = root.data[index];
            int totalTiles = grid.gridSize.x * grid.gridSize.y;
            int row = grid.gridSize.y;
            int col = grid.gridSize.x;
            string[,] rowCol = new string[row, col];
            for (int i = 0; i < grid.gridData.Count; i++)
            {
                rowCol[i / col, i % col] = grid.gridData[i].letter;
            }
            int BugCount = grid.bugCount;
            CountText.text = grid.wordCount.ToString();
            WordToWin = grid.wordCount;
            IsInitialised = true;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    Vector3 position = new Vector3(j * spacing, i * spacing, 0);

                    var tileObj = Instantiate(gridPrefeb, position, Quaternion.identity);
                    tileObj.GetComponentInChildren<TextMeshProUGUI>().text = rowCol[i, j];
                    tileObj.GetComponent<TileData>().IsSelected = false;
                    tileObj.GetComponent<TileData>().Letter = rowCol[i, j];
                    if (BugCount > 0)
                    {
                        tileObj.GetComponent<TileData>().Bug.SetActive(true);
                        tileObj.GetComponent<TileData>().IsBug = true;
                        BugCount--;
                    }
                    /// assign tile type here in tile data class
                    tileObj.transform.SetParent(parent.transform);
                }
            }
        }
    }
}
