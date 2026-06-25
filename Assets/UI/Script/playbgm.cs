using UnityEngine;

public class PlayBGM : MonoBehaviour
{
    void Awake()
    {
        SetupBGM();
    }

    void SetupBGM()
    {
        if (FindObjectsByType<PlayBGM>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
