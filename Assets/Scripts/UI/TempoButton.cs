using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TempoButton : Button, IPointerEnterHandler, IPointerExitHandler
{
    public delegate void PointerEventHandler(PointerEventData eventData);
    public PointerEventHandler PointerEnter;
    public PointerEventHandler PointerExit;
    public PointerEventHandler PointerDown;
    public PointerEventHandler PointerUp;
    
    private bool bHovered = false;

    public bool IsHovered => bHovered;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        bHovered = true;
        if (PointerEnter != null)
        { 
            PointerEnter.Invoke(eventData);
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        bHovered = false;
        if (PointerExit != null)
        {
            PointerExit.Invoke(eventData);
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        if (PointerUp != null)
        {
            PointerUp.Invoke(eventData);
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if (PointerDown != null)
        {
            PointerDown.Invoke(eventData);
        }
    }

    public void SetImageSize(Vector3 ImageScalar)
    {
        image.rectTransform.localScale += ImageScalar;
    }
}
