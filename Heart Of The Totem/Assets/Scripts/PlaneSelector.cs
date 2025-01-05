using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaneSelector : MonoBehaviour
{
    public ARRaycastManager arRaycastManager;
    public ARPlaneManager arPlaneManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    public GameObject uiPanel;

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 touchPosition = Input.GetTouch(0).position;

            if (arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;
                ARPlane selectedPlane = hits[0].trackable as ARPlane;

                if (selectedPlane != null)
                {
                    DisableOtherPlanes(selectedPlane);
                    arPlaneManager.enabled = false;

                    if(uiPanel != null)
                    {
                        uiPanel.SetActive(true);
                    }

                }
            }
        }
    }

    void DisableOtherPlanes(ARPlane selectedPlane)
    {
        foreach (var plane in arPlaneManager.trackables)
        {
            if (plane != selectedPlane)
            {
                plane.gameObject.SetActive(false);
            }
        }
    }

    public void DisableEveryPlanes()
    {
        foreach (var plane in arPlaneManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
    }
}
