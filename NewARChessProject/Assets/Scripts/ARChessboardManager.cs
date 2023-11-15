using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;


public class ARChessboardManager : MonoBehaviour
{
    [SerializeField]
    private ARPlaneManager arPlaneManager;
    [SerializeField]
    private ARRaycastManager raycastManager;
    [SerializeField]
    private GameObject chessboardPrefab;
    
    private GameObject placedChessboard;
    private bool isChessboardPlaced = false;

    void Update()
    {
        if (!isChessboardPlaced)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                TryPlaceChessboard(Input.GetTouch(0).position);
            }
        }
    }

    private void TryPlaceChessboard(Vector2 touchPosition)
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;
            placedChessboard = Instantiate(chessboardPrefab, hitPose.position, hitPose.rotation);
            isChessboardPlaced = true;

            // Once the chessboard has been placed, disable the visualization of all planes
            foreach (var plane in arPlaneManager.trackables)
            {
                plane.gameObject.SetActive(false);
            }
            
            arPlaneManager.enabled = false; // Disable the ARPlaneManager after the chessboard has been placed
        }
    }
}