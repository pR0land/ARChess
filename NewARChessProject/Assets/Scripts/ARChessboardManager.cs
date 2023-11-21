using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;


public class ARChessboardManager : MonoBehaviour
{
    [SerializeField]
    private ARPlaneManager arPlaneManager; // Reference to the ARPlaneManager
    [SerializeField]
    private ARRaycastManager raycastManager; // Reference to the ARRaycastManager
    [SerializeField]
    private GameObject chessboardPrefab; // Reference to the chessboard prefab
    
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
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        
        // Check if the user has touched a plane
        if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose; // Get the pose of the hit
            Instantiate(chessboardPrefab, hitPose.position, hitPose.rotation); // Instantiate the chessboard
            _isChessboardPlaced = true; // Set the _isChessboardPlaced bool to true

            // Once the chessboard has been placed, disable the visualization of all planes
            foreach (var plane in arPlaneManager.trackables)
            {
                plane.gameObject.SetActive(false); // Disable plane
            }
            
            arPlaneManager.enabled = false; // Disable the ARPlaneManager after the chessboard has been placed
        }
    }
}