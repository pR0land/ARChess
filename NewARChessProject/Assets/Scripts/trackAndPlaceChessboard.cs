using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class trackAndPlaceChessboard : MonoBehaviour
{
    //reference to ARtrackedImageManager
    private ARTrackedImageManager trackedImageManager;
    // Our ARprefabs (chessboard and or pices)
    [SerializeField] private GameObject[] ARObjectPrefabs;
    // The objects we have instantiated
    private readonly Dictionary<string, GameObject> instantatedARObjects = new Dictionary<string, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        //gets the ARtrackedImageManager
        trackedImageManager = GetComponent<ARTrackedImageManager>();
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
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs imageChangeEvents)
    {
        //when new images are detecked in the scene
        foreach(var trackedImage in imageChangeEvents.added)
        {
            //get reference to what image is detected
            var imageName = trackedImage.referenceImage.name;
            //check if there is an object corresponding to that image, and the prefab is not present somwhere else *if you want to instantiate more of the same change this*
            foreach(var prefab in ARObjectPrefabs)
            {
                if(string.Compare(prefab.name,imageName,System.StringComparison.OrdinalIgnoreCase) == 0 && !instantatedARObjects.ContainsKey(imageName)){
                    var ARObject = Instantiate(prefab, trackedImage.transform);
                    instantatedARObjects[imageName] = ARObject;
                }
            }
        }
        //Set prefabs active if they apear in image again, or not active if they are not in the image.
        foreach(var trackedImage in imageChangeEvents.updated)
        {
            instantatedARObjects[trackedImage.referenceImage.name].SetActive(trackedImage.trackingState == TrackingState.Tracking);
        }

        //handle if the ARsubsystem stops looking for the image *destroys the AR object and reference to it*
        foreach(var trackedImage in imageChangeEvents.removed)
        {
            Destroy(instantatedARObjects[trackedImage.referenceImage.name]);
            instantatedARObjects.Remove(trackedImage.referenceImage.name);
        }


    }
}
