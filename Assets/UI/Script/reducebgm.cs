using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ReduceBGM : MonoBehaviour
{
    [Tooltip("The AudioSource to reduce and restore volume for.")]
    public AudioSource audioSource;

    [Tooltip("The trigger or target GameObject that will activate the reduced volume.")]
    public GameObject volumeTriggerObject;

    [Tooltip("Volume level when inside the trigger object.")]
    [Range(0f, 1f)]
    public float reducedVolume = 0.2f;

    private float normalVolume;
    private bool isReduced;

    private void Awake()
    {
        audioSource ??= GetComponent<AudioSource>();
        audioSource ??= GetComponentInChildren<AudioSource>();

        if (audioSource != null)
        {
            normalVolume = audioSource.volume;
        }
        else
        {
            Debug.LogWarning("ReduceBGM: AudioSource not assigned and none found on this GameObject or its children.", this);
        }
    }

    private bool IsTriggerMatch(Collider other)
    {
        if (volumeTriggerObject == null || volumeTriggerObject == gameObject)
        {
            return true;
        }

        if (other.gameObject == volumeTriggerObject)
        {
            return true;
        }

        if (other.transform.root.gameObject == volumeTriggerObject)
        {
            return true;
        }

        if (other.transform.IsChildOf(volumeTriggerObject.transform))
        {
            return true;
        }

        return false;
    }

    private void SetReducedVolume(bool active)
    {
        if (audioSource == null || isReduced == active)
        {
            return;
        }

        isReduced = active;
        audioSource.volume = active ? reducedVolume : normalVolume;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsTriggerMatch(other))
        {
            SetReducedVolume(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsTriggerMatch(other))
        {
            SetReducedVolume(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsTriggerMatch(collision.collider))
        {
            SetReducedVolume(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (IsTriggerMatch(collision.collider))
        {
            SetReducedVolume(false);
        }
    }
}
