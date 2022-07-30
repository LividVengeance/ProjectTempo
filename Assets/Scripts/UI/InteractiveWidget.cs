using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InteractiveWidget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private InputManager InputManager;
    private VirtualCursor VirtualCursor;

    [SerializeField] private GameObject HoverPopout;
    [SerializeField] private float OnHoverSpeed = 100.0f;

    private float PreHoverSpeed;

    private enum EWidgetState
    {
        EHovered,
        EUnhovered,
    };
    private EWidgetState CurrentWidgetState;

    private void Start()
    {
        InputManager = TempoManager.Instance.GetInputManager(); 
        VirtualCursor = InputManager.GetVirtualCursor();
        CurrentWidgetState = EWidgetState.EUnhovered;
    }

    private void Update()
    {
        if (HoverPopout != null && CurrentWidgetState == EWidgetState.EHovered)
        {
            Image PopoutImage = HoverPopout.GetComponent<Image>();
            if (PopoutImage != null)
            {
                RectTransform PopoutTransform = (RectTransform)PopoutImage.transform;
                float WidthOffset = PopoutTransform.rect.width / 2;
                float HeightOffset = PopoutTransform.rect.height / 2;

                Vector2 CurosrPostion = VirtualCursor.GetCursorPosition();
                Vector2 PopoutPosition = new Vector2(CurosrPostion.x + WidthOffset, CurosrPostion.y - HeightOffset);
                HoverPopout.transform.position = PopoutPosition;
            }
        }
    }

    private void OnDisable()
    {
        if (HoverPopout != null)
        {
            HoverPopout.SetActive(false);
        }

        VirtualCursor.ModifyVirtualCursorSpeed(VirtualCursor.GetDefaultCursorSpeed());
    }

    public virtual void OnHover()
    {
        PreHoverSpeed = VirtualCursor.GetVirtualCursorSpeed();
        VirtualCursor.ModifyVirtualCursorSpeed(OnHoverSpeed);
        CurrentWidgetState = EWidgetState.EHovered;

        if (HoverPopout != null)
        {
            HoverPopout.SetActive(true);    
        }
    }

    public virtual void OnUnhover()
    {
        VirtualCursor.ModifyVirtualCursorSpeed(PreHoverSpeed);
        CurrentWidgetState = EWidgetState.EUnhovered;

        if (HoverPopout != null)
        {
            HoverPopout.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData PointerData)
    {
        OnHover();
    }

    public void OnPointerExit(PointerEventData PointerData)
    {
        OnUnhover();
    }
}
