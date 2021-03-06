﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NewtonVR.Example;
using UnityEngine.UI;
using Valve.VR;

public class ChessMenu : MonoBehaviour {
    ChessGame game;
    bool isMenuActive = false;
    string sideActive = "";

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        string menuPress = PressedMenu();

        if (menuPress != null)
        {
            ToggleMenu(menuPress);
        }
	}

    /**
     * Toggles the menu on each hand. If the menu button is pressed then the menu will
     * initialize on the hand which the button was pressed. The menu will disappear if 
     * the menu button is pressed while the menu is active. 
     */
    void ToggleMenu(string side)
    {
        game = GameObject.Find("Chess_set").GetComponent<ChessGame>();

        if (isMenuActive)
        {
            GameObject leftController = this.transform.Find("Controller (left)").gameObject;
            GameObject rightController = this.transform.Find("Controller (right)").gameObject;

            leftController.GetComponent<NVRExampleTeleporter>().enabled = true;
            rightController.GetComponent<NVRExampleTeleporter>().enabled = true;

            leftController.transform.Find("Menu").gameObject.SetActive(false);
            rightController.transform.Find("Menu").gameObject.SetActive(false);

            leftController.transform.GetComponent<ChessMenuPointer>().enabled = false;
            leftController.transform.GetComponent<ChessMenuPointer>().enabled = false;

            isMenuActive = false;
            sideActive = "";
        }
        else
        {
            if (side == "left")
            {
                GameObject leftController = this.transform.Find("Controller (left)").gameObject;
                GameObject rightController = this.transform.Find("Controller (right)").gameObject;

                leftController.GetComponent<NVRExampleTeleporter>().enabled = false;
                rightController.GetComponent<NVRExampleTeleporter>().enabled = false;

                leftController.transform.Find("Menu").gameObject.SetActive(true);
                rightController.transform.GetComponent<ChessMenuPointer>().enabled = true;

                sideActive = "left";
            }
            if (side == "right")
            {
                GameObject leftController = this.transform.Find("Controller (left)").gameObject;
                GameObject rightController = this.transform.Find("Controller (right)").gameObject;

                leftController.GetComponent<NVRExampleTeleporter>().enabled = false;
                rightController.GetComponent<NVRExampleTeleporter>().enabled = false;

                rightController.transform.Find("Menu").gameObject.SetActive(true);
                leftController.transform.GetComponent<ChessMenuPointer>().enabled = true;

                sideActive = "right";
            }

            isMenuActive = true;
        }
    }

    string PressedMenu()
    {
        bool rightPadPressed = false;
        bool leftPadPressed = false;
        
        int rightControllerIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.FarthestRight);
        int leftControllerIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.FarthestLeft);

        // Checks if controller is valid/detected
        if (rightControllerIndex != -1)
        {
            rightPadPressed = SteamVR_Controller.Input(rightControllerIndex).GetPressDown(EVRButtonId.k_EButton_ApplicationMenu);
        }
        if (leftControllerIndex != -1)
        {
            leftPadPressed = SteamVR_Controller.Input(leftControllerIndex).GetPressDown(EVRButtonId.k_EButton_ApplicationMenu);
        }

        if (rightPadPressed)
        {
            return "right";
        }
        else if (leftPadPressed)
        {
            return "left";
        }
        else
        {
            return null;
        }
    }

    public void NewGame ()
    {
        GameObject activeController = this.transform.Find("Controller (" + sideActive + ")").gameObject;
        GameObject menu = activeController.transform.Find("Menu").gameObject;

        menu.transform.Find("Main Menu").gameObject.SetActive(false);
        menu.transform.Find("Setup Menu").gameObject.SetActive(true);

        // Default values
        ResetDifficultyColors();
        menu.transform.Find("Setup Menu/Levels/1").transform.Find("DifficultyText").gameObject.GetComponent<Text>().color = Color.green;
        ResetSideColors();
        menu.transform.Find("Setup Menu/Sides/White").transform.Find("SideText").gameObject.GetComponent<Text>().color = Color.green;
    }

    public void SetHumanColor(string color)
    {
        if (color.Equals("black"))
        {
            game.SetHumanColor("black");
        }
        else
        {
            game.SetHumanColor("white");
        }
    }

    public void SetDifficulty(int level)
    {
        game.SetDifficulty(level);
    }

    public void Reset()
    {
        game.Reset();
    }

    public void Quit()
    {
        game.Quit();
    }

    public void Confirm()
    {
        game.StartGame();

        GameObject activeController = this.transform.Find("Controller (" + sideActive + ")").gameObject;
        GameObject menu = activeController.transform.Find("Menu").gameObject;

        menu.transform.Find("Setup Menu").gameObject.SetActive(false);
        menu.transform.Find("Playing Menu").gameObject.SetActive(true);
    }

    public void Undo()
    {
        game.Undo();
    }

    public void ResetDifficultyColors()
    {
        GameObject activeController = this.transform.Find("Controller (" + sideActive + ")").gameObject;
        GameObject menu = activeController.transform.Find("Menu").gameObject;
        GameObject levels = menu.transform.Find("Setup Menu").gameObject.transform.Find("Levels").gameObject;

        foreach (Transform child in levels.transform)
        {
            child.gameObject.transform.Find("DifficultyText").GetComponent<Text>().color = Color.white;          
        }
    }

    public void ResetSideColors()
    {
        GameObject activeController = this.transform.Find("Controller (" + sideActive + ")").gameObject;
        GameObject menu = activeController.transform.Find("Menu").gameObject;
        GameObject sides = menu.transform.Find("Setup Menu").gameObject.transform.Find("Sides").gameObject;

        foreach (Transform child in sides.transform)
        {
            if (child.gameObject.name == "Black")
            {
                child.gameObject.transform.Find("SideText").GetComponent<Text>().color = Color.black;
            }
            else if (child.gameObject.name == "White")
            {
                child.gameObject.transform.Find("SideText").GetComponent<Text>().color = Color.white;
            }
        }
    }
}
