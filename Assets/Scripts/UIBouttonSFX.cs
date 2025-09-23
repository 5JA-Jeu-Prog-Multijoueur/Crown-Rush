using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonSFX : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (NetworkSFXManager.Instance != null)
        {
            NetworkSFXManager.Instance.JoueBouttonHover();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (NetworkSFXManager.Instance != null)
        {
            NetworkSFXManager.Instance.JoueBouttonClick();
        }
    }
}
