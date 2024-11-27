using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

public class RaycastController : MonoBehaviour
{
    public GameObject dotPrefab; // Prefab for the dot
    public float dotDistance = 2f; // Distance from the camera
    public float dotSize = 0.005f; // Size of the dot

    private OVRCameraRig ovrCameraRig; // Reference to the OVR Camera Rig
    private GameObject dot; // Instance of the dot GameObject

    private StreamWriter fileWriter;
    private DateTime startTime;
    private string currentPanelTag;

    private string[] panelTags = { "Panel1", "Panel2", "Panel3", "Laptop", "Slides" }; // Array of panel tags

    private void Start()
    {
        ovrCameraRig = GetComponent<OVRCameraRig>();

        // Instantiate the dot prefab
        if (dotPrefab != null)
        {
            dot = Instantiate(dotPrefab, transform);
            dot.transform.localScale = new Vector3(dotSize, dotSize, dotSize);
        }

        // Open a new file for writing with a unique session index
        string filePath = GetUniqueFilePath();
        Debug.Log("File Path: " + filePath);

        try
        {
            fileWriter = new StreamWriter(filePath, false); // Create a new file
            fileWriter.WriteLine("Tag,start_time,end_time"); // Write CSV header
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to open file for writing: " + ex.Message);
            return;
        }

        // Start the timer
        startTime = DateTime.Now;
    }

    private string GetUniqueFilePath()
    {
        string directoryPath = Application.persistentDataPath;
        string fileNamePattern = "buenas_session";
        string fileExtension = ".csv";
        int sessionIndex = GetNextSessionIndex(directoryPath, fileNamePattern, fileExtension);
        return Path.Combine(directoryPath, $"{fileNamePattern}{sessionIndex}{fileExtension}");
    }

    private int GetNextSessionIndex(string directoryPath, string fileNamePattern, string fileExtension)
    {
        int highestIndex = 0;

        // Get all files that match the naming pattern
        string[] files = Directory.GetFiles(directoryPath, $"{fileNamePattern}*{fileExtension}");

        foreach (string file in files)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            string indexPart = fileName.Substring(fileNamePattern.Length);

            if (int.TryParse(indexPart, out int index))
            {
                if (index > highestIndex)
                {
                    highestIndex = index;
                }
            }
        }

        return highestIndex + 1;
    }

    private void Update()
    {
        if (dot == null || ovrCameraRig == null)
        {
            return; // Exit if dot or OVR camera rig is not assigned
        }

        // Get the center eye camera from the OVR Camera Rig
        Camera vrCamera = ovrCameraRig.centerEyeAnchor.GetComponent<Camera>();

        // Cast a ray from the center of the camera's viewport
        Ray ray = new Ray(vrCamera.transform.position, vrCamera.transform.forward);

        // Calculate the position of the dot based on the ray direction
        Vector3 dotPosition = ray.origin + ray.direction * dotDistance;

        // Update the position and rotation of the dot
        dot.transform.position = dotPosition;
        dot.transform.rotation = Quaternion.LookRotation(ray.direction);

        // Check if the ray hits something
        RaycastHit hit;
        string newPanelTag = null; // Declare panelTag outside the loop
        float closestDistance = float.MaxValue; // To track the closest object

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Ray hit something.");

            // Check if the hit object is one of the boxes
            foreach (string tag in panelTags)
            {
                if (hit.collider.CompareTag(tag))
                {
                    // Update the closest distance and tag
                    if (hit.distance < closestDistance)
                    {
                        closestDistance = hit.distance;
                        newPanelTag = tag;
                    }
                }
            }
        }

        // If the user is looking at a different panel or the first frame
        if (newPanelTag != currentPanelTag)
        {
            Debug.Log("Panel changed.");

            // Calculate the time spent on the previous panel
            if (currentPanelTag != null)
            {
                DateTime endTime = DateTime.Now;
                Debug.Log("Calling WriteToLogFile with tag: " + currentPanelTag);
                WriteToLogFile(currentPanelTag, startTime, endTime); // Write data to file
            }

            // Update currentPanelTag and reset the timer
            Debug.Log("Updating currentPanelTag to: " + newPanelTag);
            currentPanelTag = newPanelTag;
            startTime = DateTime.Now;
        }
    }

    private void WriteToLogFile(string panelTag, DateTime entryTime, DateTime exitTime)
    {
        try
        {
            // Write data to the file with local time
            string data = $"{panelTag},{entryTime:HH:mm:ss.fff},{exitTime:HH:mm:ss.fff}";
            Debug.Log("Writing to file: " + data);
            fileWriter.WriteLine(data);
            fileWriter.Flush(); // Flush the data to the file immediately
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to write to file: " + ex.Message);
        }
    }

    private void OnApplicationQuit()
    {
        try
        {
            if (fileWriter != null)
            {
                fileWriter.Close();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to close file writer: " + ex.Message);
        }
    }
}
