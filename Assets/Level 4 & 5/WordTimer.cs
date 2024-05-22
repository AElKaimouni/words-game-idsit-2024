using System.Collections;
using UnityEngine;

public class WordTimer : MonoBehaviour
{
    public WordManager wordManager;

    public float wordDelay = 1.5f;
    private float nextWordTime = 0f;
    private bool hasPassed15Seconds = false;

    private void Update()
    {
        if (!hasPassed15Seconds && Time.time >= nextWordTime)
        {
            hasPassed15Seconds = true;
            StartCoroutine(AddWordWithDelay());
        }
    }

    private IEnumerator AddWordWithDelay()
    {
        wordManager.AddWord();
        yield return new WaitForSeconds(5f);
        hasPassed15Seconds = false;
    }
}
