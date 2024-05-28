using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoEndScript : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd; // Add listener for video end event
        }
        else
        {
            Debug.LogError("No VideoPlayer component found on this GameObject.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene("level 1"); // Replace "NextSceneName" with the actual scene name you want to load
    }
}
