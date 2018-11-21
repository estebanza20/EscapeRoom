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
    private List<string> buttons;

	void Start () {
        cam = Camera.main;
        buttons = new List<string> {
            "Button_X",
            "Button_Circle",
            "Button_Square",
            "Button_Triangle"
        };
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
            IInteractive carryInteract = carryObject.GetComponent<IInteractive>();
            if (carryInteract != null) {
                actions = carryInteract.GetCarryActions(gameObject);

                if (actions != null) {
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
        bool executed = false;
        string actionKey = FindActionKeyDown(actions);

        if (actionKey != null) {
            interact.ExecuteCarryAction(actions[actionKey], gameObject);
            executed = true;
        }

        return executed;
    }


    string FindActionKeyDown(Dictionary<string, string> actions)
    {
        foreach (string button in buttons) {
            if (actions.ContainsKey(button) && 
                Input.GetButtonDown(button))
                return button;
        }
        return null;
    }

    void ShowActionsUI(Dictionary<string, string> actions)
    {

    }
}
