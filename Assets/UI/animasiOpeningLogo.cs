using UnityEngine;
using UnityEngine.Video;

public class animasiLogo : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public VideoClip introClip;
    public VideoClip loopClip;
    public bool startOnAwake = true;

    void Start()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        if (startOnAwake)
        {
            PlayIntro();
        }
    }

    public void PlayIntro()
    {
        if (videoPlayer == null || introClip == null)
        {
            return;
        }

        videoPlayer.clip = introClip;
        videoPlayer.isLooping = false;
        videoPlayer.loopPointReached += OnIntroFinished;
        videoPlayer.Play();
    }

    void OnIntroFinished(VideoPlayer vp)
    {
        vp.loopPointReached -= OnIntroFinished;
        PlayLoopLogo();
    }

    public void PlayLoopLogo()
    {
        if (videoPlayer == null || loopClip == null)
        {
            return;
        }

        videoPlayer.clip = loopClip;
        videoPlayer.isLooping = true;
        videoPlayer.Play();
    }
}
