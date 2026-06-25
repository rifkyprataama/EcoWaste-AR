using UnityEngine;
using UnityEngine.UI;

public class ChangeMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject newUI;
    [SerializeField] private Button switchButton;

    private void Start()
    {
        if (mainMenuCanvas != null)
        {
            mainMenuCanvas.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Main menu canvas is not assigned in the inspector.");
        }

        if (newUI != null)
        {
            newUI.SetActive(false);
        }
        else
        {
            Debug.LogWarning("New UI is not assigned in the inspector.");
        }

        if (switchButton != null)
        {
            switchButton.onClick.RemoveListener(SwitchToNewUI);
            switchButton.onClick.AddListener(SwitchToNewUI);
        }
        else
        {
            Debug.LogWarning("Switch button is not assigned in the inspector.");
        }
    }

    public void SwitchToNewUI()
    {
        if (mainMenuCanvas == null || newUI == null)
        {
            Debug.LogWarning("SwitchToNewUI could not run because one or more UI references are missing.", this);
            return;
        }

        mainMenuCanvas.SetActive(false);
        newUI.SetActive(true);
    }
}
