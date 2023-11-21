using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

// This script is used for placing the chessboard on a detected plane

public class ARChessboardManager : MonoBehaviour
{
    [SerializeField]
    private GameObject chessboardPrefab; // Reference to the chessboard prefab
    [SerializeField]
    private ARPlaneManager arPlaneManager; // Reference to the ARPlaneManager
    [SerializeField]
    private ARRaycastManager raycastManager; // Reference to the ARRaycastManager
    [SerializeField]
    private ARAnchorManager anchorManager; // Reference to the ARAnchorManager
    
    private bool _isChessboardPlaced; // Bool for checking if the chessboard has been placed

    void Update()
    {
        // If the chessboard has not been placed, try to place it
        if (!_isChessboardPlaced)
        {
            // Check if the user has touched the screen
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                // Try to place the chessboard
                TryPlaceChessboard(Input.GetTouch(0).position);
            }
        }
    }
    
    // Method for placing the chessboard
    private void TryPlaceChessboard(Vector2 touchPosition)
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>(); // Create a list of ARRaycastHits
        
        // Check if the user has touched a plane
        if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose; // Get the pose of the hit
            ARAnchor anchor = CreateAnchor(hits[0]); // Create an anchor using the hit information
            
            // Check if an anchor was successfully created
            if (anchor != null)
            {
                // Instantiate the chessboard and parent it to the anchor
                GameObject instantiatedChessboard = Instantiate(chessboardPrefab, hitPose.position, hitPose.rotation);
                instantiatedChessboard.transform.parent = anchor.transform;
                
                _isChessboardPlaced = true; // Set the chessboard placed bool to true

                // Disable the visualization of all planes once the chessboard is placed
                foreach (var plane in arPlaneManager.trackables)
                {
                    plane.gameObject.SetActive(false); // Disable the plane
                }

                arPlaneManager.enabled = false; // Disable the ARPlaneManager after the chessboard has been placed
            }
            else
            {
                Debug.LogError("Failed to create anchor.");
            }
        }
    }
    
    // Method to create an AR anchor
    ARAnchor CreateAnchor(ARRaycastHit hit)
    {
        Debug.Log("Hit an AR Plane");
        
        // Create an anchor using the ARAnchorManager
        return anchorManager.AttachAnchor((ARPlane)hit.trackable, hit.pose);
    }
}