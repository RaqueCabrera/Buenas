using System.Collections;
using UnityEngine;

public class SelectedObjectsInput : MonoBehaviour
{
    public OVRInput.Controller leftController = OVRInput.Controller.LTouch;
    public OVRInput.Controller rightController = OVRInput.Controller.RTouch;

    // Movement and scaling speed
    public float moveSpeed = 0.009f;
    public float scaleSpeed = 2f; // Increased for better responsiveness with button press

    private GameObject selectedObject1;
    private GameObject selectedObject2;

    private bool mirroringApplied = false; // Flag to track if mirroring has been applied
    public float mirroringDelay = 5f; // Time in seconds to wait before applying mirroring

    private void Start()
    {
        // Start a coroutine to continuously search for the tagged objects
        StartCoroutine(FindSelectedObjectsWithTag());
    }

    private System.Collections.IEnumerator FindSelectedObjectsWithTag()
    {
        // Keep searching until both tagged objects are found
        while (selectedObject1 == null || selectedObject2 == null)
        {
            // Find the game objects with the specified tags
            if (selectedObject1 == null)
                selectedObject1 = GameObject.FindGameObjectWithTag("Selected1"); // remote
            if (selectedObject2 == null)
                selectedObject2 = GameObject.FindGameObjectWithTag("Selected2"); // local

            // Wait for the next frame before checking again
            yield return null;
        }

        // Once both objects are found, start handling input
        StartCoroutine(HandleInput());

        // Start the automatic mirroring coroutine for the relevant object
        StartCoroutine(ApplyMirroringAfterDelay(selectedObject1, mirroringDelay));
        // If you want the mirroring to apply to selectedObject2, switch the object in the coroutine above
    }

    private System.Collections.IEnumerator HandleInput()
    {
        // Continuously handle input
        while (true)
        {
            // Handle input for left hand controller (Selected1)
            if (selectedObject1 != null)
            {
                HandleControllerInput(leftController, selectedObject1, OVRInput.Button.Three, OVRInput.Button.Four); // X and Y buttons
            }

            // Handle input for right hand controller (Selected2)
            if (selectedObject2 != null)
            {
                HandleControllerInput(rightController, selectedObject2, OVRInput.Button.One, OVRInput.Button.Two); // A and B buttons
            }

            // Wait for the next frame before handling input again
            yield return null;
        }
    }

    private void HandleControllerInput(OVRInput.Controller controller, GameObject selectedObject, OVRInput.Button scaleUpButton, OVRInput.Button scaleDownButton)
    {
        // Get joystick input for the specified controller
        Vector2 joystickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controller);

        // Translate joystick input to movement
        float moveAmountX = joystickInput.x * moveSpeed;
        float moveAmountY = joystickInput.y * moveSpeed;

        // Apply movement to the selected object
        selectedObject.transform.position += new Vector3(moveAmountX, moveAmountY, 0f);

        // Handle scaling with buttons
        if (OVRInput.GetDown(scaleUpButton))
        {
            ScaleObject(selectedObject, scaleSpeed);
        }
        if (OVRInput.GetDown(scaleDownButton))
        {
            ScaleObject(selectedObject, -scaleSpeed);
        }
    }

    private void ScaleObject(GameObject obj, float scaleAmount)
    {
        Vector3 currentScale = obj.transform.localScale;
        float newScaleX = Mathf.Abs(currentScale.x) + scaleAmount;
        float newScaleY = currentScale.y + scaleAmount;

        // Maintain mirroring if enabled by keeping the x scale negative if it was negative
        if (currentScale.x < 0)
        {
            newScaleX = -newScaleX;
        }

        // Apply uniform scaling to the selected object
        obj.transform.localScale = new Vector3(newScaleX, newScaleY, currentScale.z);
    }

    private System.Collections.IEnumerator ApplyMirroringAfterDelay(GameObject selectedObject, float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Apply mirroring if not already applied
        if (!mirroringApplied)
        {
            ApplyMirroring(selectedObject);
            mirroringApplied = true; // Set the flag to indicate mirroring has been applied
        }
    }

    private void ApplyMirroring(GameObject selectedObject)
    {
        // Apply mirroring to the raw image
        Vector3 currentScale = selectedObject.transform.localScale;
        selectedObject.transform.localScale = new Vector3(-currentScale.x, currentScale.y, currentScale.z);
    }
}
