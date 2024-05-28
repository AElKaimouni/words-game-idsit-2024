using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class GameManagerLvl2 : MonoBehaviour
{
    public static GameManagerLvl2 Instance { get; private set; }
    [SerializeField] DragScriptLvl2 hurufPrefab;
    [SerializeField] Transform slotAwal, slotAkhir;
    [SerializeField] GameObject winning;
    [SerializeField] GameObject button;
    [SerializeField] string[] listKataKata;

    [SerializeField] public AudioClip[] audioList;

    private static List<char> availableLetters = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
    private int poinKata, poin;
    public int level = 0;

    public static char targetLetter;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        InitKata(listKataKata[0]);
    }

    public static string GetRandomLetter()
    {
        int randomIndex = Random.Range(0, availableLetters.Count);

        char letter = availableLetters[randomIndex];
        availableLetters.RemoveAt(randomIndex);

        return letter.ToString();
    }

    void InitKata(string kata)
    {
        char[] hurufKata = kata.ToCharArray();
        char[] hurufAcak = new char[hurufKata.Length];

        List<char> hurufKataCopy = new List<char>();
        hurufKataCopy = hurufKata.ToList();
        int letter = Random.Range(0, hurufKataCopy.Count);
        int position = Random.Range(0, 3);


        targetLetter = hurufKata[letter];

        for (int i = 0; i < 3 + level; i++)
        {
            string letterStr;
            // int randomIndex = Random.Range(0, hurufKataCopy.Count);
            // hurufAcak[i] = hurufKataCopy[randomIndex];
            // hurufKataCopy.RemoveAt(randomIndex);

            availableLetters.Remove(hurufKata[letter]);

            if (i == position)
            {
                letterStr = hurufKata[letter].ToString();
            }
            else
            {
                letterStr = GetRandomLetter();
            }

            DragScriptLvl2 temp = Instantiate(hurufPrefab, slotAwal);
            temp.initialiser(slotAwal, letterStr, false);

        }



        for (int i = 0; i < hurufKata.Length; i++)
        {
            DragScriptLvl2 temp = Instantiate(hurufPrefab, slotAkhir);
            temp.initialiser(slotAkhir, letter == i ? "?" : hurufKata[i].ToString(), true);
        }

        poinKata = hurufKata.Length;

    }

    public void NextLevel()
    {
        if(level == listKataKata.Length)
        {
            winning.SetActive(true);
            button.SetActive(false);
        } else
        {
            level++;
            availableLetters = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

            for (int i = slotAkhir.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(slotAkhir.transform.GetChild(i).gameObject);
            }

            for (int i = slotAwal.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(slotAwal.transform.GetChild(i).gameObject);
            }

            if (level < listKataKata.Length)
            {
                InitKata(listKataKata[level]);
            }
            else
            {
                winning.SetActive(true);
            }
        }

    }

}