using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoToSceneOnClick : MonoBehaviour
{
    public string sceneName; // Assign the name of the scene to go to in the Unity Editor

    void Start()
    {
        // Get the Button component from the GameObject
        Button button = GetComponent<Button>();

        // Add a listener for when the button is clicked
        button.onClick.AddListener(GoToScene);
    }

    void GoToScene()
    {
        // Load the scene with the specified name
        SceneManager.LoadScene(sceneName);
    }
}
