using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class PiUI : MonoBehaviour
{

    [Tooltip("Default pi Slice to instantiate suggested to be a child gameobject to make adjusting values easier.")]
    public PiPiece piCut;

    [Tooltip("Adjust this to match the inner radius of the piCut Sprite.")]
    public float innerRadius;

    [Tooltip("Adjust this to match the outer radius of the piCut Sprite.")]
    public float outerRadius;

    [Header("Color Settings")]
    [Space(10)]

    [Tooltip("All slices will share a color.")]
    public bool syncColors;

    [Tooltip("The synced non-highlighted color.")]
    [SerializeField]
    private Color syncNormal;

    [SerializeField]
    [Tooltip("The synced highlighted color.")]
    private Color syncSelected;

    [Tooltip("The synced disabled color.")]
    [SerializeField]
    private Color syncDisabled;

    [Tooltip("All slices will have an outline.")]
    public bool outline;

    [Tooltip("Color of outline.")]
    [SerializeField]
    private Color outlineColor;

    [Header("Transition Settings")]
    [Space(10)]

    [Tooltip("Will this fade in and out.")]
    public bool fade;

    [Tooltip("How fast this piUI opens/closes.")]
    [SerializeField]
    float transitionSpeed;

    [Tooltip("The style this piUI opens.")]
    [SerializeField]
    public TransitionType openTransition;

    [Tooltip("The style this piUI closes.")]
    [SerializeField]
    public TransitionType closeTransition;

    [Header("Platform Settings")]
    [Space(10)]

    [Tooltip("Will scale based off of the default resolution to fit all device screens.")]
    public bool dynamicallyScaleToResolution;

    [SerializeField]
    [Tooltip("Resolution to match.")]
    private Vector2 defaultResolution;

    [Tooltip("Label vertical offset to move the text further or closer to the icon")]
    public float textVerticalOffset;

    [Tooltip("Enable Controller Support")]
    public bool useController;

    [HideInInspector]
    public bool joystickButton;
    [HideInInspector]
    public Vector2 joystickInput;

    [Tooltip("How the slices scale on hover")]
    [Range(1, 3)]
    public float hoverScale;

    [HideInInspector]
    public float scaleModifier = 1;
    [HideInInspector]
    public bool interactable = false;
    public readonly List<PiPiece> piList = new List<PiPiece>();

    [Header("Slice Data")]
    [Space(10)]

    [Tooltip("Icon position on slices, 0 is at the inner radius, 2 is at the outer radius")]
    [Range(0f, 2f)]
    public float iconDistance;
    [Tooltip("Slices are all equal.")]
    public bool equalSlices;
    [HideInInspector]
    [Range(1, 30)]
    public int sliceCount;

    [HideInInspector]
    public PiData[] piData;


    [HideInInspector]
    public bool openedMenu;
    [HideInInspector]
    public bool worldSpace;

    /// <summary>
    /// To add a transition type add it's name in the below enum, then add it to the switch statements in update and open menu, and make a function for it
    /// </summary>
    public enum TransitionType { Scale };

    private Vector2 menuPosition;
    [SerializeField]
    [HideInInspector]
    private float[] angleList;
    [HideInInspector]
    public bool overMenu;


    private void Awake()
    {
        if (transform.parent.GetComponent<Canvas>().renderMode == RenderMode.WorldSpace)
        {
            worldSpace = true;
        }
        else
        {
            worldSpace = false;
        }

        if (dynamicallyScaleToResolution)
        {
            if (Screen.width > Screen.height)
            {
                scaleModifier = Screen.height / defaultResolution.y;
            }
            else
            {
                scaleModifier = Screen.width / defaultResolution.x;
            }
        }
        innerRadius *= scaleModifier;
        outerRadius *= scaleModifier;
        GeneratePi(new Vector2(-1000, -1000));
    }


    /// <summary>
    /// Clear menu and make a new pi with the updated pidata and position
    /// </summary>
    /// <param name="screenPosition">Screen position that the pi will be made at</param>
    public void GeneratePi(Vector2 screenPosition)
    {
        sliceCount = piData.Length;
        if (piList.Count > 1)
        {
            ClearMenu();
        }
        transform.position = screenPosition;
        float lastRot = 0;
        if (syncColors)
        {
            SyncColor();
        }
        angleList = new float[sliceCount];
        for (int i = 0; i < sliceCount; i++)
        {
            PiPiece currentPi = Instantiate(piCut);
            Image currentImage = currentPi.GetComponent<Image>();
            if (outline)
            {
                currentPi.GetComponent<Outline>().effectColor = outlineColor;
            }
            else
            {
                currentPi.GetComponent<Outline>().enabled = outline;

            }
            float fillPercentage = (1f / sliceCount);
            float angle = fillPercentage * 360;
            if (!equalSlices)
            {
                angle = piData[i].angle;
                fillPercentage = piData[i].angle / 360;
            }
            currentImage.fillAmount = fillPercentage;
            angle = (angle + 360) % 360;
            if (angle == 0)
            {
                angle = 360;
            }
            currentPi.transform.SetParent(transform);
            int rot = Mathf.Clamp((int)(angle + lastRot), 0, 360);
            if (rot == 360)
            {
                rot = 0;
            }
            currentPi.transform.rotation = Quaternion.Euler(0, 0, rot);
            lastRot += angle;
            angleList[i] = rot;
            //currentPi.gameObject.SetActive(true);
            currentImage.rectTransform.localPosition = Vector2.zero;
            currentPi.SetData(piData[i], innerRadius, outerRadius, this);
            piList.Add(currentPi);
        }
        openedMenu = false;
    }

    private void Update()
    {
        overMenu = false;
        //Open the menu with the selected opening transition, or if !openmenu close menu with selected closing transition
        if (openedMenu)
        {
            switch (openTransition)
            {
                case TransitionType.Scale:
                    Scale();
                    transform.position = menuPosition;
                    break;

            }
        }
        else if (!openedMenu)
        {
            interactable = false;
            switch (closeTransition)
            {
                case TransitionType.Scale:
                    Scale();
                    transform.position = menuPosition;
                    break;

            }
        }
        foreach (PiPiece pi in piList)
        {
            if (pi.gameObject.activeInHierarchy)
            {
                pi.ManualUpdate();
            }
        }
    }

    public void CloseMenu()
    {
        openedMenu = false;
    }

    /// <summary>
    /// Open menu and if the menu isn't created create the slices, then do their transition.
    /// </summary>
    /// <param name="screenPos">Place in screen position to open the menu</param>
    public void OpenMenu(Vector2 screenPos)
    {
        menuPosition = screenPos;
        openedMenu = true;
        foreach (PiPiece pi in piList)
        {
            pi.gameObject.SetActive(true);
        }
        if (piList.Count == 0)
        {
            GeneratePi(screenPos);
        }
        else
        {
            ResetPiRotation();
        }
        Vector2 tempPos = menuPosition;
        switch (openTransition)
        {
            case TransitionType.Scale:
                transform.localScale *= 0;
                break;

        }
    }

    /// <summary>
    /// Clear menu and destroy all pi slices
    /// </summary>
    public void ClearMenu()
    {
        foreach (PiPiece pi in piList)
        {
            if (pi == null)
            {
                piList.Clear();
                break;
            }
            DestroyImmediate(pi.gameObject);
        }
        piList.Clear();
    }

    /// <summary>
    /// Set all pi slice colours for selected and not selected
    /// </summary>
    public void SyncColor()
    {
        foreach (PiData pi in piData)
        {
            pi.nonHighlightedColor = syncNormal;
            pi.highlightedColor = syncSelected;
            pi.disabledColor = syncDisabled;
        }
        foreach (PiPiece pi in piList)
        {
            pi.SetData(piData[piList.IndexOf(pi)], innerRadius, outerRadius, this);
        }
    }

    #region TRANSITIONS
    private void Scale()
    {
        if (openedMenu)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, Vector2.one * scaleModifier, Time.deltaTime * transitionSpeed);
            if (Mathf.Abs((Vector2.one * scaleModifier).sqrMagnitude - transform.localScale.sqrMagnitude) < .05f)
            {
                interactable = true;
            }
        }
        else if (!openedMenu)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, Vector2.zero, Time.deltaTime * transitionSpeed);
            if (transform.localScale.x < .05f)
            {
                transform.localScale = Vector2.zero;
                foreach (PiPiece pi in piList)
                {
                    pi.gameObject.SetActive(false);
                }
            }
        }
    }

    #endregion

    /// <summary>
    /// Set pi rotation to proper rotation, useful for ensuring rotational transitions work.
    /// </summary>
    private void ResetPiRotation()
    {
        float lastRot = 0;
        for (int i = 0; i < piList.Count; i++)
        {

            float fillPercentage = (1f / sliceCount);
            float angle = fillPercentage * 360;
            if (!equalSlices)
            {
                angle = piData[i].angle;
            }
            int rot = Mathf.Clamp((int)(angle + lastRot), 0, 359);
            Vector3 rotVec = new Vector3(0, 0, rot);
            piList[i].transform.rotation = Quaternion.Euler(rotVec);
            lastRot += angle;
        }
    }

    /// <summary>
    /// Set Pis Rotation to zero
    /// </summary>
    private void PiRotationToNil()
    {
        foreach (PiPiece pi in piList)
        {
            pi.transform.rotation = Quaternion.identity;
        }
    }

    /// <summary>
    /// Use This Function To Update the Slices When pi count maintains the same count
    /// </summary>
    public void UpdatePiUI()
    {
        foreach (PiPiece currentPi in piList)
        {
            if (syncColors)
            {
                SyncColor();
            }
            Image sliceIcon = currentPi.transform.GetChild(0).GetComponent<Image>();
            Text sliceText = currentPi.transform.GetChild(1).GetComponent<Text>();
            sliceIcon.sprite = piData[piList.IndexOf(currentPi)].icon;
            sliceText.text = piData[piList.IndexOf(currentPi)].sliceLabel;
            currentPi.SetData(piData[piList.IndexOf(currentPi)], innerRadius, outerRadius, this);
        }

    }

    [System.Serializable]
    public class PiData
    {
        [Range(20, 360)]
        public float angle;
        public string sliceLabel;
        public Sprite icon;
        public Color nonHighlightedColor;
        public Color highlightedColor;
        public Color disabledColor;
        public UnityEvent onSlicePressed;
        public int iconSize;
        public bool isInteractable = true;
        public bool hoverFunctions;
        public UnityEvent onHoverEnter;
        public UnityEvent onHoverExit;
        public int order;
        private Texture2D angleTexture;
        private int angleTextureSize = 64;

        public void SetValues(PiData newData)
        {
            nonHighlightedColor = newData.nonHighlightedColor;
            highlightedColor = newData.highlightedColor;
            disabledColor = newData.disabledColor;
            icon = newData.icon;
            sliceLabel = newData.sliceLabel;
            angle = newData.angle;
            iconSize = newData.iconSize;
            isInteractable = newData.isInteractable;
        }

#if UNITY_EDITOR
        public void OnInspectorGUI(SerializedProperty sprop, PiUI menu, System.Action AddSlice, System.Action<PiData> RemoveSlice, System.Action<int> angleUpdate)
        {

            float angleStart = angle;
            if (icon == null)
                icon = Sprite.Create(angleTexture, new Rect(0, 0, angleTextureSize, angleTextureSize), Vector2.zero, 0, 0, SpriteMeshType.FullRect, Vector4.zero, false);
            order = Mathf.Clamp(order, 0, menu.piData.Length);
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.BeginHorizontal();
            if (order > 0 && GUILayout.Button("▲", GUILayout.Width(32)))
            {
                order = Mathf.Clamp(order - 1, 0, menu.piData.Length);
                foreach (PiData pi in menu.piData)
                {
                    if (pi != this && pi.order == order)
                    {
                        pi.order = Mathf.Clamp(pi.order + 1, 0, menu.piData.Length);
                        break;
                    }
                }
            }
            if (order < menu.piData.Length - 1 && GUILayout.Button("▼", GUILayout.Width(32)))
            {
                order = Mathf.Clamp(order + 1, 0, menu.piData.Length);
                foreach (PiData pi in menu.piData)
                {
                    if (pi != this && pi.order == order)
                    {
                        pi.order = Mathf.Clamp(pi.order - 1, 0, menu.piData.Length);
                        break;
                    }
                }
            }
            sliceLabel = EditorGUILayout.TextField(sliceLabel);
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("+", GUILayout.Width(32)))
                AddSlice.Invoke();
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("-", GUILayout.Width(32)))
                RemoveSlice.Invoke(this);
            GUI.backgroundColor = Color.white;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            var rect = EditorGUILayout.GetControlRect(GUILayout.Width(64), GUILayout.Height(64));
            if (icon != null)
            {
                GUI.DrawTexture(rect, Texture2D.blackTexture, ScaleMode.ScaleToFit);

                GUI.DrawTexture(rect, icon.texture, ScaleMode.ScaleToFit);
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Space(8);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Icon", GUILayout.Width(96));
            icon = (Sprite)EditorGUILayout.ObjectField(icon, typeof(Sprite), false, GUILayout.Height(16));
            GUILayout.EndHorizontal();
            GUILayout.BeginVertical();
            if (!menu.syncColors)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Non Selected Color");
                nonHighlightedColor = EditorGUILayout.ColorField(nonHighlightedColor, GUILayout.Height(16), GUILayout.Width(50));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Selected Color");
                highlightedColor = EditorGUILayout.ColorField(highlightedColor, GUILayout.Height(16), GUILayout.Width(50));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Not Interactable Color");
                disabledColor = EditorGUILayout.ColorField(disabledColor, GUILayout.Height(16), GUILayout.Width(50));
                GUILayout.EndHorizontal();

            }
            GUILayout.BeginHorizontal();
            if (!menu.equalSlices)
            {
                angle = EditorGUILayout.FloatField("Angle", angle, GUILayout.Width(192));
                GUILayout.FlexibleSpace();
            }
            Rect angleRect = EditorGUILayout.GetControlRect(GUILayout.Width(32), GUILayout.Height(32));
            GUI.DrawTexture(angleRect, angleTexture, ScaleMode.ScaleToFit);
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Icon Size", GUILayout.Width(96));
            iconSize = EditorGUILayout.IntField(iconSize);
            if (iconSize < 8)
                iconSize = 8;
            else if (iconSize > 256)
                iconSize = 256;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Interactable ?", GUILayout.Width(96));
            isInteractable = EditorGUILayout.Toggle(isInteractable);
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            EditorGUI.indentLevel++;
            hoverFunctions = EditorGUILayout.Foldout(hoverFunctions, "Hover Events", true);
            EditorGUI.indentLevel--;

            if (hoverFunctions)
            {
                EditorGUILayout.PropertyField(sprop.FindPropertyRelative("onHoverEnter"));
                EditorGUILayout.PropertyField(sprop.FindPropertyRelative("onHoverExit"));
            }
            EditorGUILayout.PropertyField(sprop.FindPropertyRelative("onSlicePressed"));
            GUILayout.Label(order.ToString(), GUILayout.Width(10));

            GUILayout.EndVertical();
            if (!menu.equalSlices && angle != angleStart)
            {
                angleUpdate.Invoke(order);
            }

            angleTexture = new Texture2D(angleTextureSize, angleTextureSize);
            Color32 resetColor = new Color32(255, 255, 255, 0);
            Color32[] resetColorArray = angleTexture.GetPixels32();

            float sumOfBefore = (360f / menu.piData.Length) * order;
            float endAngle = (360f / menu.piData.Length) * (order + 1);

            for (int i = 0; i < resetColorArray.Length; i++)
            {
                resetColorArray[i] = resetColor;
            }
            angleTexture.SetPixels32(resetColorArray);
            angleTexture.Apply();
            if (!menu.equalSlices)
            {
                sumOfBefore = 0;
                for (int i = 0; i < order; i++)
                {
                    sumOfBefore += menu.piData[i].angle;
                }
                endAngle = sumOfBefore + angle;
            }
            Vector2 origin = new Vector2(angleTextureSize / 2, angleTextureSize / 2);
            for (int i = Mathf.RoundToInt(sumOfBefore); i <= Mathf.Round(endAngle); i++)
            {
                Vector2 dir = new Vector2(Mathf.Cos(i * Mathf.Deg2Rad), Mathf.Sin(i * Mathf.Deg2Rad));
                dir.Normalize();
                for (int j = 0; j < angleTextureSize / 2; j++)
                {
                    Vector2 temp = dir * j;
                    temp += origin;
                    if (j != (angleTextureSize / 2) - 1)
                        angleTexture.SetPixel(Mathf.RoundToInt(temp.x), Mathf.RoundToInt(temp.y), Color.white);

                }
            }
            angleTexture.Apply();
        }
#endif

    }
}
