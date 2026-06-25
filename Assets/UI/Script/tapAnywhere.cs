using UnityEngine;
using UnityEngine.SceneManagement;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class TapAnywhere : MonoBehaviour
{
    [Tooltip("Optional next scene name. If empty, the next scene in build settings will be loaded.")]
    public string nextSceneName;

    void Update()
    {
#if ENABLE_INPUT_SYSTEM
        if (IsTapDetectedWithInputSystem())
        {
            LoadNextScene();
        }
#else
        if (Application.isMobilePlatform)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                LoadNextScene();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                LoadNextScene();
            }
        }
#endif
    }

#if ENABLE_INPUT_SYSTEM
    bool IsTapDetectedWithInputSystem()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            return true;

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            return true;

        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
            return true;

        return false;
    }
#endif

    void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            Debug.LogWarning("No next scene in build settings. Set nextSceneName or add the scene to the build settings.");
        }
    }
}
