using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCanvas : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private Transform leftControllerTransform;

    private bool canvasActive = true; // Initial state of the canvas
    private bool isAnchored = false; // State to track if the canvas is anchored
    private Vector3 anchoredPosition; // The anchored position of the canvas
    private Quaternion anchoredRotation; // The anchored rotation of the canvas

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch))
        {
            // Toggle the canvas active state
            canvasActive = !canvasActive;
            // Set the canvas active state based on the toggle
            canvas.SetActive(canvasActive);
        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
        {
            // Toggle the anchored state
            isAnchored = !isAnchored;

            if (isAnchored)
            {
                // Anchor the canvas to its current position
                anchoredPosition = canvas.transform.position;
                anchoredRotation = canvas.transform.rotation;
            }
            else
            {
                // Unanchor the canvas and attach it back to the controller
                canvas.transform.SetParent(leftControllerTransform);
                canvas.transform.localPosition = Vector3.zero;
                canvas.transform.localRotation = Quaternion.identity;
            }
        }

        // If the canvas is anchored, keep it at the anchored position
        if (isAnchored)
        {
            canvas.transform.position = anchoredPosition;
            canvas.transform.rotation = anchoredRotation;
        }
        else
        {
            // If not anchored, make sure the canvas follows the controller
            if (canvas.transform.parent != leftControllerTransform)
            {
                canvas.transform.SetParent(leftControllerTransform);
                canvas.transform.localPosition = Vector3.zero;
                canvas.transform.localRotation = Quaternion.identity;
            }
        }
    }
}
