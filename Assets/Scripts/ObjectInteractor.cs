using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractor : MonoBehaviour {
    public float interactRange;
    public GameObject carryObject;
    private Camera cam;
    private RaycastHit hit;
    private IInteractive hitInteract;
    private Dictionary<string, string> actions;

	void Start () {
        cam = Camera.main;
	}
	
	void Update () {
        // Cast a ray to look for some object
        Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, interactRange);

        // Check if some object was hit
        if (hit.transform) {
            // Check if hit object is interactive
            hitInteract = hit.transform.GetComponent<IInteractive>();

            if (hitInteract != null) {
                actions = hitInteract.GetHitActions(gameObject, carryObject);

                if (actions != null) {
                    // Show possible actions in the UI
                    ShowActionsUI(actions);

                    // Execute player action on the hitInteract (if any)
                    bool actionExecuted = HandleHitActions(hitInteract, actions);
                    if (actionExecuted) return;
                }
            }
        }

        // Check if carry object is interactive
        if (carryObject != null) {
            Debug.Log("CarryObject not null");
            IInteractive carryInteract = carryObject.GetComponent<IInteractive>();
            if (carryInteract != null) {
            Debug.Log("CarryInteract not null");
                actions = carryInteract.GetCarryActions(gameObject);

                if (actions != null) {
                    Debug.Log("Actions not null");
                    // Show possible actions in the UI
                    ShowActionsUI(actions);

                    // Execute player action on the carryInteract (if any)
                    HandleCarryActions(carryInteract, actions);
                }
            }
        }
	}

    bool HandleHitActions(IInteractive interact, Dictionary<string, string> actions)
    {
        bool executed = false;
        string actionKey = FindActionKeyDown(actions);

        if (actionKey != null) {
            interact.ExecuteHitAction(actions[actionKey], gameObject, carryObject);
            executed = true;
        }

        return executed;
    }

    bool HandleCarryActions(IInteractive interact, Dictionary<string, string> actions)
    {
        Debug.Log("HandleCarryActions");
        bool executed = false;
        string actionKey = FindActionKeyDown(actions);

        if (actionKey != null) {
            Debug.Log("HandleCarryActions: actionKey not null");
            interact.ExecuteCarryAction(actions[actionKey], gameObject);
            executed = true;
        }

        return executed;
    }


    string FindActionKeyDown(Dictionary<string, string> actions)
    {
        if (actions.ContainsKey("E") && Input.GetKeyDown(KeyCode.E)) return "E";
        if (actions.ContainsKey("R") && Input.GetKeyDown(KeyCode.R)) return "R";

        return null;
    }

    void ShowActionsUI(Dictionary<string, string> actions)
    {

    }
}
