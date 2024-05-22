using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordGenerator : MonoBehaviour {

private static string[] wordList = {
    // Fire Chamber
    "fire", "light", "flame", "candle",
    // Water Chamber
    "water", "ocean", "rain", "lake",
    // Earth Chamber
    "earth", "mountain", "tree", "plant",
    // Air Chamber
    "air", "sky", "cloud", "wind"
};

	public static string GetRandomWord ()
	{
		int randomIndex = Random.Range(0, wordList.Length);
		string randomWord = wordList[randomIndex];

		return randomWord;
	}

}
