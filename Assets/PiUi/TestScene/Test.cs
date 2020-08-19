using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Test : MonoBehaviour
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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            piUi.ChangeMenuState("Normal Menu", new Vector2(Screen.width / 2f, Screen.height / 2f));
        }
        //Update the menu and add the Testfunction to the button action if s or Fire1 axis is pressed


        //Set joystick input on the normal menu which the piPieces check
        normalMenu.joystickInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //Set the bool to detect if the controller button has been pressed
        normalMenu.joystickButton = Input.GetButtonDown("Fire1");
        //If the button isnt pressed check if has been released
        if (!normalMenu.joystickButton)
        {
            normalMenu.joystickButton = Input.GetButtonUp("Fire1");
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
