using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ManagerKata : MonoBehaviour
{
    public static ManagerKata Instance { get; private set; }
    [SerializeField] DragScript hurufPrefab;
    [SerializeField] Transform slotAwal, slotAkhir;
    [SerializeField] GameObject winning;
    [SerializeField] string[] listKataKata;

    private int poinKata, poin, level = 0;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        InitKata(listKataKata[0]);
    }

    void InitKata(string kata)
    {
        char[] hurufKata = kata.ToCharArray();
        char[] hurufAcak = new char[hurufKata.Length];

        List<char> hurufKataCopy = new List<char>();
        hurufKataCopy = hurufKata.ToList();

        for (int i = 0; i < hurufAcak.Length; i++)
        {
            int randomIndex = Random.Range(0, hurufKataCopy.Count);
            hurufAcak[i] = hurufKataCopy[randomIndex];
            hurufKataCopy.RemoveAt(randomIndex);

            DragScript temp = Instantiate(hurufPrefab, slotAwal);
            temp.initialiser(slotAwal, hurufAcak[i].ToString(), false);

        }


        for (int i = 0; i < hurufKata.Length; i++)
        {
            DragScript temp = Instantiate(hurufPrefab, slotAkhir);


            temp.initialiser(slotAkhir, hurufKata[i].ToString(), true);
        }

        poinKata = hurufKata.Length;

    }

    public void TambahPoin()
    {
        poin++;
        if (poin == poinKata)
        {
            poin = 0;
            level++;

            for (int i = slotAkhir.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(slotAkhir.transform.GetChild(i).gameObject);
            }

            if (level < listKataKata.Length)
            {
                InitKata(listKataKata[level]);
            } else
            {
                winning.SetActive(true);
            }
        }
    }

}