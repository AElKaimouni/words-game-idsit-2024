using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;


public class Main : MonoBehaviour
{
    // Game Settings
    [SerializeField] private int gameTimeLimitSeconds;
    [SerializeField] private int xBounds;
    [SerializeField] private int yBounds;

    // Game Object References
    [SerializeField] private AudioSource musicSource, fxSource;
    [SerializeField] private List<AudioClip> sfx;
    [SerializeField] private Tilemap tiles;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject lineObject;
    [SerializeField] private GameObject resultsBackboard;
    [SerializeField] private GameObject playBackBoard;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI wordCountText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI resTextScores_Ref;
    [SerializeField] private TextMeshProUGUI resTextWords_Ref;
    [SerializeField] private Button replayButton;

    private LineRenderer chainLine;
    private char[,] predefinedGrid = new char[,]
    {
        { 'B', 'I', 'R', 'D' },
        { 'E', 'A', 'R', 'T' },
        { 'C', 'A', 'R', 'E' },
        { 'H', 'O', 'M', 'E' }
    };

    private Dictionary<string, int> validWords;
    private Dictionary<string, int> wordsSpelled;
    private string word;
    private int totalScore, wordCount;
    private float timer;
    private Color baseColor;
    private HashSet<GameObject> usedTiles;
    private GameObject[,] tileGrid;
    private bool gameOver;
    private Vector3Int prevCell;
    private Coroutine timerCoroutineReference;

    void Start()
    {
        word = "";
        gameOver = true;
        baseColor = new Color(1.0f, 0.85f, 0.59f);
        validWords = new Dictionary<string, int>();
        wordsSpelled = new Dictionary<string, int>();
        usedTiles = new HashSet<GameObject>();
        tileGrid = new GameObject[xBounds, yBounds];
        chainLine = lineObject.GetComponent<LineRenderer>();

        LoadValidWords();

        createTiles();
    }

    void Update()
    {
        if (!gameOver)
        {
            // Game logic
            timer -= Time.deltaTime;
            updateTime(timer);

            // Move finger, add letter
            if (Input.GetMouseButton(0))
            {
                Vector2 worldPt = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int curCell = tiles.WorldToCell(worldPt);

                if (withinBounds(curCell) && (word.Length == 0 || isAdjacent(curCell, prevCell)))
                {
                    GameObject curTile = tileGrid[curCell.x, curCell.y];

                    if (!usedTiles.Contains(curTile))
                    {
                        usedTiles.Add(curTile);
                        prevCell = curCell;

                        float lerp = 1.15f;
                        curTile.transform.localScale = new Vector3(lerp, lerp, lerp);

                        char letter = predefinedGrid[curCell.x, curCell.y];
                        word += letter;

                        if (validWords.ContainsKey(word))
                        {
                            if (wordsSpelled.ContainsKey(word))
                            {
                                fxSource.PlayOneShot(sfx[0]);
                                colorUsedTiles(Color.yellow);
                                colorLine(Color.white);
                            }
                            else
                            {
                                fxSource.PlayOneShot(sfx[2]);
                                colorUsedTiles(Color.green);
                                colorLine(Color.white);
                            }
                        }
                        else
                        {
                            fxSource.PlayOneShot(sfx[0]);
                            colorUsedTiles(Color.white);
                            colorLine(Color.red);
                        }

                        lineObject.SetActive(true);
                        Vector3 pos = worldPtToCentered(tiles.CellToWorld(curCell));
                        for (int i = word.Length - 1; i < 16; i++)
                        {
                            chainLine.SetPosition(i, pos);
                        }
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                int fx_toplay = 1;
                word = word.ToUpper();

                if (word.Length >= 2)
                {
                    if (validateWord(word))
                    {
                        wordsSpelled.Add(word, validWords[word]);
                        totalScore += validWords[word];
                        wordCount++;

                        scoreText.text = totalScore.ToString();
                        wordCountText.text = wordCount.ToString();
                        fx_toplay = word.Length > 6 ? 6 : word.Length;
                    }

                    fxSource.PlayOneShot(sfx[fx_toplay]);
                }

                colorUsedTiles(baseColor);
                descaleUsedTiles();
                lineObject.SetActive(false);
                word = "";
                usedTiles.Clear();
            }
        }
    }

    private void LoadValidWords()
    {
        validWords.Add("BIRD", 400);
        validWords.Add("EAR", 600);
        validWords.Add("CARE", 300);
        validWords.Add("HOME", 500);
        validWords.Add("BE", 100);
        validWords.Add("ARE", 200);
        validWords.Add("CAR", 200);
        validWords.Add("ART", 500);
    }


    private void createTiles()
    {
        for (int i = 0; i < xBounds; i++)
        {
            for (int j = 0; j < yBounds; j++)
            {
                Vector3Int loc = new Vector3Int(i, j, 0);
                GameObject newTile = Instantiate(tilePrefab, tiles.CellToWorld(loc), Quaternion.identity);
                newTile.transform.parent = tilePrefab.transform.parent;
                newTile.GetComponentInChildren<TextMeshProUGUI>().text = predefinedGrid[i, j].ToString();
                tileGrid[i, j] = newTile;
            }
        }
    }

    private IEnumerator gameTimer()
    {
        yield return new WaitForSeconds(gameTimeLimitSeconds - 5f);
        fxSource.PlayOneShot(sfx[9]);
        yield return new WaitForSeconds(5f);
        gameOver = true;
        updateResults();
        fxSource.PlayOneShot(sfx[10]);
    }

    public void replayGame()
    {
        try
        {
            StopCoroutine(timerCoroutineReference);
        }
        catch (System.NullReferenceException) { }

        fxSource.PlayOneShot(sfx[8]);
        resetVars();

        resultsBackboard.SetActive(false);
        playBackBoard.SetActive(true);

        timerCoroutineReference = StartCoroutine(gameTimer());
    }

    private void resetVars()
    {
        wordCount = 0;
        wordCountText.text = "0";
        totalScore = 0;
        scoreText.text = "0000";
        timer = gameTimeLimitSeconds + 1f;
        wordsSpelled.Clear();
        gameOver = false;
    }

    private void updateTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private bool isAdjacent(Vector3Int a, Vector3Int b)
    {
        return Mathf.Abs(b.x - a.x) <= 1 && Mathf.Abs(b.y - a.y) <= 1 && Mathf.Abs(b.z - a.z) <= 1;
    }

    private void updateResults()
    {
        resultsBackboard.SetActive(true);
        playBackBoard.SetActive(false);

        string resTextWords = "";
        string resTextScores = "";
        foreach (KeyValuePair<string, int> entry in wordsSpelled)
        {
            resTextWords += entry.Key + "\n";
            resTextScores += entry.Value.ToString() + "\n";
        }
        resTextScores_Ref.text = resTextScores;
        resTextWords_Ref.text = resTextWords;
    }

    private bool withinBounds(Vector3Int cell)
    {
        return cell.x >= 0 && cell.y >= 0 && cell.x < xBounds && cell.y < yBounds;
    }

    private Vector3 worldPtToCentered(Vector3 worldPos)
    {
        Vector3 pos = worldPos;
        pos.x += 7f;
        pos.y += 6f;
        return pos;
    }

    private bool validateWord(string word)
    {
        return validWords.ContainsKey(word) && !wordsSpelled.ContainsKey(word);
    }

    private void colorUsedTiles(Color newColor)
    {
        foreach (GameObject tile in usedTiles)
        {
            tile.GetComponent<SpriteRenderer>().color = newColor;
        }
    }

    private void descaleUsedTiles()
    {
        foreach (GameObject tile in usedTiles)
        {
            tile.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void colorLine(Color newColor)
    {
        chainLine.startColor = newColor;
        chainLine.endColor = newColor;
    }
}
