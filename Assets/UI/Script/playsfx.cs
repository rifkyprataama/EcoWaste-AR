using UnityEngine;
using UnityEngine.UI;

public class playsfx : MonoBehaviour
{
    public AudioClip sfxClip;
    public Button button;

    void Start()
    {
        button?.onClick.AddListener(PlaySfx);
    }

    public void PlaySfx()
    {
        if (sfxClip == null)
            return;

        var listenerPosition = Camera.main != null ? Camera.main.transform.position : Vector3.zero;
        AudioSource.PlayClipAtPoint(sfxClip, listenerPosition);
    }
}
