using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GallerySceneController : MonoBehaviour
{
    public List<RawImage> allRawImages; // all raw images in the scene
    public List<Transform> targetPositions; // target positions for the selected raw images

    private int selectionCount = 0;
    private List<RawImage> selectedRawImages = new List<RawImage>();
    //private Dictionary<string, PanelData> loadedData; // new

    private void Start()
    {
        foreach (RawImage rawImage in allRawImages)
        {
            rawImage.gameObject.SetActive(true);
        }

        if (targetPositions.Count != allRawImages.Count)
        {
            Debug.LogError("Please specify the correct number of target positions for each raw image.");
        }

        // Load JSON data at the start ---------- NEW
        //loadedData = SaveLoadController.LoadPanelData();
    }


    public void OnRawImageButtonClick(Button clickedButton)
    {
        RawImage rawImage = clickedButton.GetComponentInParent<RawImage>();

        if (rawImage != null && selectionCount < 2)
        {
            selectedRawImages.Add(rawImage);
            selectionCount++;

            // Set the position of the raw image
            int index = selectedRawImages.Count - 1;
            if (index < targetPositions.Count)
            {
                rawImage.rectTransform.position = targetPositions[index].position;

                // Set the size of the raw image based on the index
                Vector2 newSize;
                if (index == 0)
                {
                    // Set size for the first raw image -> remote participant
                    newSize = new Vector2(100f, 100f);

                    // Add tag to the first selected raw image
                    rawImage.gameObject.tag = "Selected1";
                }
                else if (index == 1)
                {
                    // Set different size for the second raw image -> local participants
                    newSize = new Vector2(250f, 100f);

                    // Add tag to the second selected raw image
                    rawImage.gameObject.tag = "Selected2";
                }
                else
                {
                    newSize = Vector2.zero; // Handle additional cases if needed
                }

                rawImage.rectTransform.sizeDelta = newSize;
            }

            else
            {
                Debug.LogError($"Target position not specified for raw image {index}.");
            }

            // Set the button inactive
            clickedButton.gameObject.SetActive(false);

            if (selectionCount == 2)
            {
                DisableUnselectedRawImages();
                FinalizeSelection();
            }
        }
    }

    /*public void OnRawImageButtonClick(Button clickedButton)
    {
        RawImage rawImage = clickedButton.GetComponentInParent<RawImage>();

        if (rawImage != null && selectionCount < 2)
        {
            selectedRawImages.Add(rawImage);
            selectionCount++;

            // Assign tags immediately after selection
            AssignTagToSelectedRawImage(rawImage);

            // Set the position and size of the raw image based on JSON data or default values
            SetRawImagePositionAndSize(rawImage);

            // Set the button inactive
            clickedButton.gameObject.SetActive(false);

            if (selectionCount == 2)
            {
                DisableUnselectedRawImages();
                FinalizeSelection();
            }
        }
    }

    private void AssignTagToSelectedRawImage(RawImage rawImage)
    {
        int index = selectedRawImages.Count - 1;

        if (index == 0)
        {
            rawImage.gameObject.tag = "Selected1";
        }
        else if (index == 1)
        {
            rawImage.gameObject.tag = "Selected2";
        }
        else
        {
            Debug.LogError($"Index {index} is out of range for assigning tags.");
        }
    }

    private void SetRawImagePositionAndSize(RawImage rawImage)
    {
        int index = selectedRawImages.Count - 1;

        if (loadedData.ContainsKey(rawImage.gameObject.tag))
        {
            // Set position and size based on JSON data
            PanelData panelData = loadedData[rawImage.gameObject.tag];
            rawImage.rectTransform.position = panelData.position;
            rawImage.rectTransform.localScale = panelData.scale;
        }
        else if (index < targetPositions.Count)
        {
            // Set position based on target positions
            rawImage.rectTransform.position = targetPositions[index].position;

            // Set the size of the raw image based on the index
            Vector2 newSize;
            if (index == 0)
            {
                // Set size for the first raw image -> remote participant
                newSize = new Vector2(100f, 100f);
            }
            else if (index == 1)
            {
                // Set different size for the second raw image -> local participants
                newSize = new Vector2(250f, 100f);
            }
            else
            {
                newSize = Vector2.zero; // Handle additional cases if needed
            }

            rawImage.rectTransform.sizeDelta = newSize;
        }
        else
        {
            Debug.LogError($"Target position not specified for raw image {index}.");
        }
    }*/

    private void DisableUnselectedRawImages()
    {
        foreach (RawImage rawImage in allRawImages)
        {
            if (!selectedRawImages.Contains(rawImage))
            {
                rawImage.gameObject.SetActive(false);

                Button buttonComponent = rawImage.GetComponentInParent<Button>();
                if (buttonComponent != null)
                {
                    buttonComponent.interactable = false;
                }
            }
        }
    }

    private void FinalizeSelection()
    {
        foreach (RawImage selectedRawImage in selectedRawImages)
        {
            // Get the tag of the selected raw image
            string selectedTag = selectedRawImage.gameObject.tag;

            // Instantiate the corresponding prefab based on the tag
            GameObject instantiatedPrefab = null;
            switch (selectedTag)
            {
                case "Selected1":
                    instantiatedPrefab = Instantiate(Resources.Load<GameObject>("ColliderRemote"), selectedRawImage.transform);
                    instantiatedPrefab.transform.localPosition = Vector3.zero;
                    instantiatedPrefab.transform.localRotation = Quaternion.identity;
                    instantiatedPrefab.transform.localScale = new Vector3(100f, 100f, 1f);
                    break;

                case "Selected2":
                    GameObject leftCollider = Instantiate(Resources.Load<GameObject>("ColliderLocal2"), selectedRawImage.transform);
                    GameObject rightCollider = Instantiate(Resources.Load<GameObject>("ColliderLocal1"), selectedRawImage.transform);

                    leftCollider.transform.localPosition = new Vector3(-100f, 0f, 0f); //-75.5f
                    leftCollider.transform.localRotation = Quaternion.identity;
                    leftCollider.transform.localScale = new Vector3(100f, 100f, 1f); ;

                    rightCollider.transform.localPosition = new Vector3(100f, 0f, 0f); //75.5f
                    rightCollider.transform.localRotation = Quaternion.identity;
                    rightCollider.transform.localScale = new Vector3(100f, 100f, 1f); ;
                    break;

                default:
                    Debug.LogError("Invalid tag for selected raw image: " + selectedTag);
                    break;
            }

            // Enable the button component for the selected raw image
            Button buttonComponent = selectedRawImage.GetComponentInParent<Button>();
            if (buttonComponent != null)
            {
                buttonComponent.interactable = true;
            }
        }
    }

}