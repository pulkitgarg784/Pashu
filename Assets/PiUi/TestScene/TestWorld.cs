using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TestWorld : MonoBehaviour
{

    [SerializeField]
    PiUIManager piUi;
    private bool menuOpened;
    private PiUI normalMenu;
    // Use this for initialization
    void Start()
    {
        //Get menu for easy not repetitive getting of the menu when setting joystick input
        normalMenu = piUi.GetPiUIOf("Normal Menu");
    }

    // Update is called once per frame
    void Update()
    {
        //Bool function that returns true if on a menu
        if (piUi.OverAMenu())
            Debug.Log("You are over a menu");
        else
            Debug.Log("You are not over a menu");
        //Just open the normal Menu if A is pressed
        if (Input.GetKeyDown(KeyCode.A))
        {
            Vector3 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            temp.z = 0;
            piUi.ChangeMenuState("Normal Menu", temp);
        }
        //Update the menu and add the Testfunction to the button action if s or Fire1 axis is pressed
        if (Input.GetKeyDown(KeyCode.S) || Input.GetButtonDown("Fire1"))
        {
            //Ensure menu isnt currently open on update just for a cleaner look
            if (!piUi.PiOpened("Normal Menu"))
            {
                int i = 0;
                //Iterate through the piData on normal menu
                foreach (PiUI.PiData data in normalMenu.piData)
                {
                    //Changes slice label
                    data.sliceLabel = "Test" + i.ToString();
                    //Creates a new unity event and adds the testfunction to it
                    data.onSlicePressed = new UnityEngine.Events.UnityEvent();
                    data.onSlicePressed.AddListener(TestFunction);
                    i++;
                }
                //Since PiUI.sliceCount or PiUI.equalSlices didnt change just calling update
                piUi.UpdatePiMenu("Normal Menu");
            }
            //Open or close the menu depending on it's current state at the center of the screne
            piUi.ChangeMenuState("Normal Menu", new Vector2(Screen.width / 2f, Screen.height / 2f));
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            //Ensure menu isnt currently open on regenerate so it doesnt spasm
            if (!piUi.PiOpened("Normal Menu"))
            {

                //Make all angles equal 
                normalMenu.equalSlices = true;
                normalMenu.iconDistance = 0f;
                //Changes the piDataLength and adds new piData
                normalMenu.piData = new PiUI.PiData[10];
                for (int j = 0; j < 10; j++)
                {
                    normalMenu.piData[j] = new PiUI.PiData();
                }
                //Turns of the syncing of colors
                normalMenu.syncColors = false;
                //Changes open/Close animations

                int i = 0;
                foreach (PiUI.PiData data in normalMenu.piData)
                {
                    //Turning off the interactability of a slice
                    if (i % 2 == 0)
                    {
                        data.isInteractable = false;
                    }
                    //Set new highlight/non highlight colors
                    data.nonHighlightedColor = new Color(1 - i / 10f, 0, 0, 1);
                    data.highlightedColor = new Color(0, 0, 1 - i / 10f, 1);
                    data.disabledColor = Color.grey;
                    //Changes slice label
                    data.sliceLabel = "Test" + i.ToString();
                    //Creates a new unity event and adds the testfunction to it
                    data.onSlicePressed = new UnityEngine.Events.UnityEvent();
                    data.onSlicePressed.AddListener(TestFunction);
                    i += 1;
                    //Enables hoverFunctions
                    data.hoverFunctions = true;
                    //Creates a new unity event to adds on hovers function
                    data.onHoverEnter = new UnityEngine.Events.UnityEvent();
                    data.onHoverEnter.AddListener(OnHoverEnter);
                    data.onHoverExit = new UnityEngine.Events.UnityEvent();
                    data.onHoverExit.AddListener(OnHoverExit);
                }
                piUi.RegeneratePiMenu("Normal Menu");
            }
            piUi.ChangeMenuState("Normal Menu", new Vector2(Screen.width / 2f, Screen.height / 2f));
        }

        //Set joystick input on the normal menu which the piPieces check
        normalMenu.joystickInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //Set the bool to detect if the controller button has been pressed
        normalMenu.joystickButton = Input.GetButtonDown("Fire1");
        //If the button isnt pressed check if has been released
        if (Input.GetButtonUp("Fire1"))
        {
            normalMenu.joystickButton = Input.GetButtonUp("Fire1");
            normalMenu.CloseMenu();
        }
    }
    //Test function that writes to the console and also closes the menu
    public void TestFunction()
    {
        //Closes the menu
        piUi.ChangeMenuState("Normal Menu");
        Debug.Log("You Clicked me!");
    }

    public void OnHoverEnter()
    {
        Debug.Log("Hey get off of me!");
    }
    public void OnHoverExit()
    {
        Debug.Log("That's right and dont come back!");
    }
}
