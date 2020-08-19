using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class PiPiece : MonoBehaviour
{

    private bool isOver;
    private bool lerp;
    private Image thisImg;
    [HideInInspector]
    [SerializeField]
    private float innerRadius;
    [HideInInspector]
    [SerializeField]
    private float outerRadius;
    [HideInInspector]
    [SerializeField]
    private Color normalColor;
    [HideInInspector]
    [SerializeField]
    private Color highlightColor;
    [HideInInspector]
    [SerializeField]
    private Color disabledColor;
    [HideInInspector]
    [SerializeField]
    private UnityEvent clickEvent;
    [HideInInspector]
    [SerializeField]
    private UnityEvent onHoverEnter;
    [HideInInspector]
    [SerializeField]
    private UnityEvent onHoverExit;
    [SerializeField]
    private bool onHoverEvents;
    [SerializeField]
    [HideInInspector]
    PiUI parent;

    float scaledOR;


    private float maxAngle;
    private float minAngle;
    private Text sliceLabel;
    private Image sliceIcon;
    private bool isInteractable;
    private bool lastFrameIsOver;

    void OnEnable()
    {
        thisImg = GetComponent<Image>( );
        sliceIcon = transform.GetChild(0).GetComponent<Image>( );
        sliceLabel = GetComponentInChildren<Text>( );
    }

    private void Start()
    {
        thisImg.color= normalColor;

    }

    public void ManualUpdate()
    {
        Vector2 inputAxis = parent.joystickInput;
        sliceIcon.transform.position = Center( );
        sliceLabel.transform.position = Center( ) - new Vector2(0, sliceIcon.rectTransform.sizeDelta.y + parent.textVerticalOffset) * parent.scaleModifier * transform.lossyScale.magnitude;
        if (isInteractable)
        {
            if (isOver && transform.localScale.sqrMagnitude < (Vector2.one * parent.hoverScale).sqrMagnitude)
            {
                transform.localScale = Vector2.Lerp(transform.localScale, Vector2.one * parent.hoverScale, Time.deltaTime * 10f);
            }
            else if (transform.localScale.sqrMagnitude > 1 && !isOver)
            {
                transform.localScale = Vector2.Lerp(transform.localScale, Vector2.one, Time.deltaTime * 10f);
            }
            Vector2 mousePos = Input.mousePosition;
            Vector2 temp = mousePos - (Vector2)transform.position;
            float angle = (Mathf.Atan2(temp.y, temp.x) * Mathf.Rad2Deg);
            angle = (angle + 360) % 360;
            scaledOR = outerRadius;
            if (angle < maxAngle && angle > minAngle && temp.magnitude >= innerRadius && temp.magnitude <= scaledOR)
            {
                isOver = true;
            }
            else if (parent.useController && isInteractable)
            {
                temp = inputAxis;
                angle = (Mathf.Atan2(temp.y, temp.x) * Mathf.Rad2Deg);
                angle = (angle + 360) % 360;
                if (angle == 0)
                {
                    angle += 1;
                }
                if (angle < maxAngle && angle >= minAngle && inputAxis != Vector2.zero)
                {
                    isOver = true;
                }
                else
                {
                    isOver = false;
                    thisImg.color= Color.Lerp(thisImg.color, normalColor, Time.deltaTime * 10f);
                }

            }
            else
            {
                isOver = false;
                thisImg.color= Color.Lerp(thisImg.color, normalColor, Time.deltaTime * 10f);
            }
            if (!parent.interactable)
            {
                isOver = false;
                if (parent.fade)
                {
                    thisImg.color= Color.Lerp(thisImg.color, Color.clear, Time.deltaTime * 10f);
                }
            }
            if (isOver && parent.interactable)
            {
                scaledOR *= parent.hoverScale;
                transform.SetAsLastSibling( );
                thisImg.color= Color.Lerp(thisImg.color, highlightColor, Time.deltaTime * 10f);
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0) || parent.useController && parent.joystickButton)
                {
                    clickEvent.Invoke( );
                }
            }
        }
        else
        {
            thisImg.color = disabledColor;
            transform.localScale = Vector2.Lerp(transform.localScale, Vector2.one, Time.deltaTime * 10f);
        }
        if (transform.rotation.eulerAngles.z == 359f || transform.rotation.eulerAngles.z == 0)
        {
            transform.rotation = Quaternion.identity;
        }
        if (transform.rotation.eulerAngles.z == 359f || transform.rotation.eulerAngles.z == 0 && parent.openedMenu)
        {
            transform.rotation = Quaternion.identity;
            maxAngle = 359f;
            minAngle = 359f - (thisImg.fillAmount * 360);
        }
        else if (parent.interactable)
        {
            maxAngle = transform.rotation.eulerAngles.z;
            minAngle = transform.rotation.eulerAngles.z - (thisImg.fillAmount * 360);
        }
        sliceLabel.transform.rotation = Quaternion.identity;
        sliceIcon.transform.rotation = Quaternion.identity;
        if (lastFrameIsOver != isOver && isInteractable && parent.interactable && onHoverEvents)
        {
            if (isOver && onHoverEnter.GetPersistentEventCount() >= 0)
            {
                OnHoverEnter( );
            }
            else if (!isOver && onHoverEnter.GetPersistentEventCount( ) >= 0)
            {
                OnHoverExit( );
            }
        }
        if (isOver)
        {
            parent.overMenu = true;
        }
        lastFrameIsOver = isOver;
    }

    public Vector2 Center()
    {
        if (!thisImg)
        {
            thisImg = GetComponent<Image>( );
        }
        float temp = (innerRadius * parent.iconDistance / parent.scaleModifier + outerRadius / parent.scaleModifier) / 3f;
        temp *= transform.lossyScale.magnitude;
        float angleOfFill = thisImg.fillAmount * 360;
        Vector2 center = Quaternion.AngleAxis(transform.rotation.eulerAngles.z - angleOfFill / 2f, Vector3.forward) * new Vector2(temp, 0);
        center += (Vector2)parent.transform.position;
        return center;
    }

    public void SetData(PiUI.PiData piData, float iR, float oR, PiUI creator)
    {
        parent = creator;
        if (!thisImg || !sliceIcon || !sliceLabel)
        {
            thisImg = GetComponent<Image>( );
            sliceIcon = transform.GetChild(0).GetComponent<Image>( );
            sliceLabel = GetComponentInChildren<Text>( );
        }
        innerRadius = iR;
        outerRadius = oR;
        normalColor = piData.nonHighlightedColor;
        highlightColor = piData.highlightedColor;
        disabledColor = piData.disabledColor;
        clickEvent = piData.onSlicePressed;
        if (parent.fade)
        {
            thisImg.color= Color.clear;
        }
        maxAngle = transform.rotation.eulerAngles.z;
        minAngle = transform.rotation.eulerAngles.z - (thisImg.fillAmount * 360);
        if (transform.rotation.eulerAngles.z == 359f || transform.rotation.eulerAngles.z == 0)
        {
            transform.rotation = Quaternion.identity;
            maxAngle = 359f;
            minAngle = 359f - (thisImg.fillAmount * 360);
        }
        sliceIcon.rectTransform.sizeDelta = new Vector2(piData.iconSize, piData.iconSize);

        sliceLabel.text = piData.sliceLabel;
        sliceIcon.sprite = piData.icon;
        sliceIcon.transform.position = Center( );
        sliceLabel.transform.position = Center( ) - new Vector2(0, sliceIcon.rectTransform.sizeDelta.y + parent.textVerticalOffset) * parent.scaleModifier * transform.localScale.magnitude;
        isInteractable = piData.isInteractable;
        onHoverEvents = piData.hoverFunctions;
        if (onHoverEvents)
        {
            onHoverEnter = piData.onHoverEnter;
            onHoverExit = piData.onHoverExit;
        }
    }

    private void OnHoverEnter()
    {
        onHoverEnter.Invoke( );

    }

    private void OnHoverExit()
    {
        onHoverExit.Invoke( );
    }

}
