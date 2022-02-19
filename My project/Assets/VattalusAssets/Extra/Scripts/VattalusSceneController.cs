using System;
using Unity.Collections;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VattalusSceneController : MonoBehaviour
{
    // This script acts as a central hub for all other important scripts in the demo scene and can be accessed easily from all other scripts

    [Header("Player / Camera / Spaceship")]
    //Player related variables
    public VattalusFirstPersonCamera firstPersonController;
    //Different player control types
    public enum ControlModeTypes
    {
        Walking,
        Seated,
        Flying
    }
    private ControlModeTypes controlMode;

    //Different camera behaviour types
    public enum CameraModes
    {
        Player,
        ShipOrbit
    }
    private CameraModes cameraMode;
    public CameraModes GetCamMode { get { return cameraMode; } }

    //orbit camera that rotates around the spaceship
    public VattalusOrbitCamera orbitCamera;

    //Spaceship related variables
    public VattalusSpaceshipController spaceshipController;


    [Header("Inputs")]
    //Interaction variables
    public KeyCode interactionKey = KeyCode.E; // what key is used to interact with objects
    public KeyCode standUpKey = KeyCode.X; //key used to stand up from seats
    public KeyCode cameraKey = KeyCode.C;
    public KeyCode hideUIKey = KeyCode.Tab;

    [Header("UI References")]
    public GameObject reticle;
    public GameObject KeyPromptsParent;
    public GameObject UI_Background;

    [Space]
    // references to the UI elements of key prompts
    public VattalusKeyPrompt interactPrompt;
    public VattalusKeyPrompt standUpPrompt;

    [Space]
    //ship controls prompts
    public VattalusKeyPrompt hologramPrompt;
    public VattalusKeyPrompt landingGearPrompt;
    public VattalusKeyPrompt rampPrompt;

    [Space]
    public VattalusKeyPrompt shipControlsPrompt;
    public Text pitchDownPrompt;
    public Text pitchUpPrompt;
    public Text yawLeftPrompt;
    public Text yawRightPrompt;
    public Text rollLeftPrompt;
    public Text rollRightPrompt;
    public Text acceleratePrompt;
    public Text deceleratePrompt;

    [Space]
    public VattalusKeyPrompt cameraPrompt;
    public VattalusKeyPrompt hideUIPrompt;


    private VattalusInteractible lookingAtInteractible = null;
    private VattalusInteractible currentlyOccupiedSeat = null; //reference to the seat the player is currently sitting in

    void Start()
    {
        Application.targetFrameRate = 60;

        //Check important references and throw warnings for you in case you forgot something
        if (firstPersonController == null) Debug.Log("color=#FF0000>VattalusAssets: [SceneController] Missing reference to first person camera controller</color>");
        if (spaceshipController == null) Debug.Log("color=#FF0000>VattalusAssets: [SceneController] Missing reference to the ship controller</color>");

        if (interactPrompt == null) Debug.Log("<color=#FF0000>VattalusAssets: [SceneController] Missing reference to UI component: Interaction key prompt</color>");
        if (standUpPrompt == null) Debug.Log("<color=#FF0000>VattalusAssets: [SceneController] Missing reference to UI component: Stamp up key prompt</color>");
        if (hologramPrompt == null) Debug.Log("<color=#FF0000>VattalusAssets: [SceneController] Missing reference to UI component: Hologram key prompt</color>");
        if (landingGearPrompt == null) Debug.Log("<color=#FF0000>VattalusAssets: [SceneController] Missing reference to UI component: Landing gear key prompt</color>");
        if (rampPrompt == null) Debug.Log("<color=#FF0000>VattalusAssets: [SceneController] Missing reference to UI component: ramp key prompt</color>");
        if (cameraPrompt == null) Debug.Log("<color=#FF0000>VattalusAssets: [SceneController] Missing reference to UI component: camera key prompt</color>");

        if (shipControlsPrompt == null) Debug.Log("<color=#FF0000>VattalusAssets: Missing reference to UI component: ship controls prompts</color>");


        //set the key prompt values on the UI
        if (spaceshipController != null)
        {
            if (hologramPrompt != null) hologramPrompt.UpdateKeyPromptTexts("Hologram", spaceshipController.hologramKey.ToString());
            if (landingGearPrompt != null) landingGearPrompt.UpdateKeyPromptTexts("Landing Gear", spaceshipController.landingGearKey.ToString());

            if (pitchDownPrompt != null) pitchDownPrompt.text = spaceshipController.pitchDown.ToString();
            if (pitchUpPrompt != null) pitchUpPrompt.text = spaceshipController.pitchUp.ToString();
            if (yawLeftPrompt != null) yawLeftPrompt.text = spaceshipController.yawLeft.ToString();
            if (yawRightPrompt != null) yawRightPrompt.text = spaceshipController.yawRight.ToString();
            if (rollLeftPrompt != null) rollLeftPrompt.text = spaceshipController.rollLeftInputKey.ToString();
            if (rollRightPrompt != null) rollRightPrompt.text = spaceshipController.rollRightInputKey.ToString();
            if (acceleratePrompt != null) acceleratePrompt.text = spaceshipController.accelerateInputKey.ToString();
            if (deceleratePrompt != null) deceleratePrompt.text = spaceshipController.decelerateInputKey.ToString();
        }


        if (cameraPrompt != null) cameraPrompt.UpdateKeyPromptTexts("Camera", cameraKey.ToString());
        if (hideUIPrompt != null) hideUIPrompt.UpdateKeyPromptTexts("Hide Controls", hideUIKey.ToString());

        SetPlayerControl(ControlModeTypes.Walking);
        SetCameraMode(CameraModes.Player);
    }

    void Update()
    {
        lookingAtInteractible = null;

        //Check if the cursor is looking at an interactible object
        if (firstPersonController != null && firstPersonController.cameraComponent != null && firstPersonController.cameraComponent.gameObject.activeInHierarchy)
        {
            VattalusInteractible interactibleObj = null;
            RaycastHit hit;
            var cameraCenter = firstPersonController.cameraComponent.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, firstPersonController.cameraComponent.nearClipPlane));
            if (Physics.Raycast(cameraCenter, firstPersonController.cameraComponent.transform.forward, out hit, 2f))
            {
                interactibleObj = hit.collider.GetComponent<VattalusInteractible>();
            }

            //player is looking at an interactible object
            if (interactibleObj != null)
            {
                lookingAtInteractible = interactibleObj;
            }
        }

        //when looking at an interactible and pressing the interaction button
        if (lookingAtInteractible != null && lookingAtInteractible.CanInteract && Input.GetKeyDown(interactionKey))
        {
            lookingAtInteractible.Interact();
            if (lookingAtInteractible.isSeat)
            {
                //if the seat is unoccupied, tell player to sit down. If its occupied, tell player to stand up
                if (lookingAtInteractible.isActivated)
                    SitPlayerDown(lookingAtInteractible);
                else
                    StandPlayerUp();
            }

            //some custom checks very specific to this example scene
            if (spaceshipController != null)
            {
                if (lookingAtInteractible == spaceshipController.pilotSeat)
                {
                    spaceshipController.SitInPilotSeat();
                }
            }
        }

        //When pressing the 'stand up' key, check if we are currently sitting down, then tell player to stand up
        if (currentlyOccupiedSeat != null && Input.GetKeyDown(standUpKey))
        {
            currentlyOccupiedSeat.Interact();
            StandPlayerUp();
        }

        //When seated, always show a special UI hint to stand up
        standUpPrompt.gameObject.SetActive(currentlyOccupiedSeat != null);
        interactPrompt.gameObject.SetActive(lookingAtInteractible != null);

        bool inPilotSeat = false;

        if (currentlyOccupiedSeat != null)
        {
            string standUpPromptText = currentlyOccupiedSeat.isActivated ? currentlyOccupiedSeat.deactivateText : currentlyOccupiedSeat.activateText;
            if (string.IsNullOrEmpty(standUpPromptText)) standUpPromptText = "Stand Up";

            standUpPrompt.UpdateKeyPromptTexts(standUpPromptText, standUpKey.ToString(), currentlyOccupiedSeat.CanInteract);

            //if below conditions are met, we are sitting in the pilot seat
            if (spaceshipController != null && currentlyOccupiedSeat == spaceshipController.pilotSeat)
            {
                inPilotSeat = true;
            }
        }

        if (lookingAtInteractible != null)
        {
            string interactionPromptText = lookingAtInteractible.isActivated ? lookingAtInteractible.deactivateText : lookingAtInteractible.activateText;
            if (string.IsNullOrEmpty(interactionPromptText)) interactionPromptText = "Interact";

            interactPrompt.UpdateKeyPromptTexts(interactionPromptText, interactionKey.ToString(), lookingAtInteractible.CanInteract);
        }

        //When sitting in the pilot seat, enable additional controls, and update the ui to show prompts
        if (UI_Background != null) UI_Background.SetActive(inPilotSeat);

        if (hologramPrompt != null) hologramPrompt.gameObject.SetActive(inPilotSeat);
        if (landingGearPrompt != null) landingGearPrompt.gameObject.SetActive(inPilotSeat);
        if (rampPrompt != null) rampPrompt.gameObject.SetActive(inPilotSeat);
        if (cameraPrompt != null) cameraPrompt.gameObject.SetActive(inPilotSeat);
        if (hideUIPrompt != null) hideUIPrompt.gameObject.SetActive(inPilotSeat);

        if (shipControlsPrompt != null) shipControlsPrompt.gameObject.SetActive(inPilotSeat);

        if (inPilotSeat)
        {
            if (landingGearPrompt != null) landingGearPrompt.UpdateKeyPromptTexts("Landing Gear", spaceshipController.landingGearKey.ToString(), !spaceshipController.IsLandingGearAnimating());
            if (rampPrompt != null) rampPrompt.UpdateKeyPromptTexts("Ramp", spaceshipController.rampKey.ToString(), spaceshipController.ramp ? spaceshipController.ramp.CanInteract : true);
        }

        //only enable spaceship controls when sitting in the pilot seat
        if (spaceshipController != null) spaceshipController.enableMovement = inPilotSeat;

        //When pressing the camera key, switch between camera modes
        if (inPilotSeat && Input.GetKeyDown(cameraKey))
        {
            CameraModes newCamMode = CameraModes.Player;
            if (cameraMode == CameraModes.Player) newCamMode = CameraModes.ShipOrbit;
            if (cameraMode == CameraModes.ShipOrbit) newCamMode = CameraModes.Player;

            SetCameraMode(newCamMode);
        }

        //disable reticle when in orbit camera
        if (reticle != null) reticle.SetActive(cameraMode == CameraModes.Player);


        //Hide UI
        if (Input.GetKeyDown(hideUIKey) && KeyPromptsParent != null)
        {
            KeyPromptsParent.SetActive(!KeyPromptsParent.activeSelf);
        }

#if !UNITY_EDITOR
                if (Input.GetKeyDown(KeyCode.Escape))
                    Application.Quit();
#endif
    }

    private void SetPlayerControl(ControlModeTypes newMode)
    {
        controlMode = newMode;
        if (firstPersonController != null) firstPersonController.SetPlayerControl(newMode);
    }

    //Call this method to tell the player character to sit down in a seat
    public void SitPlayerDown(VattalusInteractible interactibleSeat)
    {
        controlMode = ControlModeTypes.Seated;
        if (firstPersonController != null) firstPersonController.SetPlayerControl(ControlModeTypes.Seated, interactibleSeat.seatCameraAnchor, interactibleSeat.seatCamAngleConstraints);
        currentlyOccupiedSeat = lookingAtInteractible;
    }

    //Call this method to tell the player character to stand up from seat
    public void StandPlayerUp()
    {
        SetPlayerControl(ControlModeTypes.Walking);
        SetCameraMode(CameraModes.Player);

        /////////some custom checks very specific to this example scene, can be safely deleted
        if (spaceshipController != null && currentlyOccupiedSeat == spaceshipController.pilotSeat)
        {
            spaceshipController.SitInPilotSeat();
        }
        /////////


        currentlyOccupiedSeat = null;
    }

    public void SetCameraMode(CameraModes newMode)
    {
        cameraMode = newMode;

        if (firstPersonController != null) firstPersonController.gameObject.SetActive(cameraMode == CameraModes.Player);
        if (orbitCamera != null) orbitCamera.gameObject.SetActive(cameraMode != CameraModes.Player);
    }
}
