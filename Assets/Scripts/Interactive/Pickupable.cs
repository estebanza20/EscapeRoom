using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour, IInteractive {
    Dictionary<string, string> hitActions, carryActions;
    public Vector3 CarryAngles;
    public Vector3 CarryPosition;

    public void Start()
    {
        // Setup hitActions
        hitActions = new Dictionary<string, string>
        {
            { "Button_X", "PickUp" }
        };

        // Setup carryActions
        carryActions = new Dictionary<string, string>
        {
            { "Button_Circle", "Drop" }
        };

        // Setup Rigidbody
        GetComponent<Rigidbody>().useGravity = true;
    }

    /**
     * ====================
     * H I T  A C T I O N S
     * ====================
     */
    public Dictionary<string, string> GetHitActions(GameObject interactor, GameObject other)
    {
        return hitActions;
    }

    public void ExecuteHitAction(string actionName, GameObject interactor, GameObject other)
    {
        switch(actionName) {
            case "PickUp":
                PickUpAction(interactor);
                break;
            default:
                Debug.Log("Invalid HitAction");
                break;
        }
    }

    void PickUpAction(GameObject interactor)
    {
        // Drop object if carrying
        GameObject carryObject = interactor.GetComponent<ObjectInteractor>().carryObject;
        if (carryObject != null) {
            carryObject.GetComponent<IInteractive>().ExecuteCarryAction("Drop", interactor);
        }

        // Adjust object physics for pick up
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;
        Transform guide = interactor.GetComponent<Transform>().Find("CarryGuide");
        if (guide != null) {
            // Move object to player's hand
            Transform transform = GetComponent<Transform>();
            guide.position += CarryPosition;
            transform.position = guide.position;
            //transform.rotation = guide.rotation;
            transform.SetParent(interactor.transform);
            transform.eulerAngles = CarryAngles;

            // Set current object as carry reference
            interactor.GetComponent<ObjectInteractor>().carryObject = gameObject;
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }

    /**
     * ========================
     * C A R R Y  A C T I O N S
     * ========================
     */
    public Dictionary<string, string> GetCarryActions(GameObject interactor)
    {
        return carryActions;
    }

    public void ExecuteCarryAction(string actionName, GameObject interactor)
    {
        switch(actionName) {
            case "Drop":
                Debug.Log("Executing Drop Action");
                DropAction(interactor);
                break;
            default:
                Debug.Log("Invalid CarryAction");
                break;
        }
    }

    void DropAction(GameObject interactor)
    {
        // Adjust object physics for drop
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().useGravity = true;
       Transform guide = interactor.GetComponent<Transform>().Find("CarryGuide");
        if (guide != null) {
            // Drop object from player's hand
            Transform transform = GetComponent<Transform>();
            transform.SetParent(null);
            transform.position = guide.position;
            // transform.rotation = guide.rotation;
            transform.eulerAngles = CarryAngles;
            guide.position -= CarryPosition;

            // Remove current object from carry reference
            interactor.GetComponent<ObjectInteractor>().carryObject = null;
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
}
