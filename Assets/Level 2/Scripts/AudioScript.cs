using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AudioScript : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip clickSound; // Sound to play when the button is clicked

    void Start()
    {
        Button button = GetComponent<Button>();

        button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {

        // Assign the audio clip to the AudioSource component
        audioSource.clip = GameManagerLvl2.Instance.audioList[GameManagerLvl2.Instance.level]; ;

        audioSource.Play();
    }

}