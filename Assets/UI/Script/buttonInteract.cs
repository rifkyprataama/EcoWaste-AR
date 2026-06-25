using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class buttonInteract : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Image targetImage;
    public Color normalColor = Color.white;
    public Color pressedColor = Color.gray;

    void Reset()
    {
        targetImage = GetComponent<Image>();
    }

    void Start()
    {
        if (targetImage != null)
        {
            targetImage.color = normalColor;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (targetImage != null)
        {
            targetImage.color = pressedColor;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (targetImage != null)
        {
            targetImage.color = normalColor;
        }
    }
}
