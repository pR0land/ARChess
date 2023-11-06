using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[System.Serializable]
public struct ARObjectPrefab
{
    public string name;
    public GameObject prefab;
}

[RequireComponent(typeof(ARTrackedImageManager))]
public class trackAndPlaceChessboard : MonoBehaviour

{
    //reference to ARtrackedImageManager
    private ARTrackedImageManager trackedImageManager;
    // Our ARprefabs (chessboard and or pices)
    public List<ARObjectPrefab> objectPrefabs = new List<ARObjectPrefab>();
    // The objects we have instantiated
    private Dictionary<string, GameObject> instantiatedObjects;
    
    
    
    void Start()
    {
        //gets the ARtrackedImageManager
        trackedImageManager = GetComponent<ARTrackedImageManager>();
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        instantiatedObjects = new Dictionary<string, GameObject>();
    }

    void OnEnable()
    {
        //subscribes to changes in tracked images alows us to handle what will happen in the event there is a change in whats being tracked
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        //desubscribe if we for some reason dont want to track anymore
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    //our eventhandler, handles when there is a change in the images being tracked
    
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added) 
        { 
            foreach (ARObjectPrefab obj in objectPrefabs) 
            { 
                if ((obj.name == trackedImage.referenceImage.name) && (!instantiatedObjects.ContainsKey(obj.name))) 
                {
                    instantiatedObjects[obj.name] = Instantiate(obj.prefab, trackedImage.transform);
                }
            }

        }
    
        foreach (ARTrackedImage trackedImage in eventArgs.updated) 
        { 
            if (trackedImage.trackingState == TrackingState.Tracking) 
            { 
                instantiatedObjects[trackedImage.referenceImage.name].SetActive(true);
                Debug.Log($"FOUND {trackedImage.referenceImage.name}");
            } 
            else 
            { 
                instantiatedObjects[trackedImage.referenceImage.name].SetActive(false); 
            } 
        }
        foreach (ARTrackedImage trackedImage in eventArgs.removed) 
        { 
            Destroy(instantiatedObjects[trackedImage.referenceImage.name]);

            instantiatedObjects.Remove(trackedImage.referenceImage.name);
        }
    
    }
}
