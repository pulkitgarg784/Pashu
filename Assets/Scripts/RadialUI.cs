using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialUI : MonoBehaviour
{
    public Camera UICamera;
    public GameObject backgroundPanel;
    public GameObject circleMenuElement;
    public bool gradient;

    public Color normalColor;
    public Color highlightColor;
    public Gradient highlightColorGradient = new Gradient();

    public Image centerBackground;
    public Text itemName;
    public Text itemDescription;
    public Image itemIcon;

    private int currentMenuItem;
    private int previousMenuItem;
    private float calculateMenuIndex;
    private float currentAngle;
    private Vector3 currentMousePosition;
    private List<CircleMenuElement> menuElements = new List<CircleMenuElement>();

    private static RadialUI instance;

    public static RadialUI Instance
    {
        get { return instance; }
    }
    public bool Active
    {
        get { return backgroundPanel.activeSelf; }
    }

    public List<CircleMenuElement> MenuElements
    {
        get { return menuElements; }
        set { menuElements = value; }
    }

    public void Initialize()
    {
        float rotationalIncrement = 360f / menuElements.Count;
        float currentRotationalValue = 0;
        float fillPercentageValue = 1f / menuElements.Count;
        for (int i = 0; i < menuElements.Count; i++)
        {
            GameObject menuElementGameObject = Instantiate(circleMenuElement);
            menuElementGameObject.name = i + ":" + currentRotationalValue;
            menuElementGameObject.transform.SetParent(backgroundPanel.transform);

            MenuButton menuButton = menuElementGameObject.GetComponent<MenuButton>();

            menuButton.rectTransform.localScale = Vector3.one;
            menuButton.rectTransform.localPosition = Vector3.zero;
            menuButton.rectTransform.rotation= Quaternion.Euler(0,0,currentRotationalValue);
        }
    }
}
