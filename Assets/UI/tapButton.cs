using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

// Attach this script to a UI Button or Image GameObject.
// In the Button component, add the OnClick event and assign TapButton->OnTap,
// or attach this script to an Image and handle pointer clicks directly.
public class TapButton : MonoBehaviour, IPointerClickHandler
{
    [Tooltip("Name of the scene to load when the button is tapped.")]
    public string sceneName;

    [Tooltip("Optional: set to true to load the scene asynchronously.")]
    public bool loadAsync = false;

    public void OnTap()
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("TapButton: sceneName is empty. Please set a scene name to load.");
            return;
        }

        if (loadAsync)
        {
            SceneManager.LoadSceneAsync(sceneName);
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnTap();
    }
}
