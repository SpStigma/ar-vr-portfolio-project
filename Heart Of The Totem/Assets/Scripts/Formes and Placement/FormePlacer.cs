using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class FormePlacer : MonoBehaviour
{
    public ARRaycastManager arRaycastManager;
    public FormeSelector formeSelector;

    private GameObject activeForme;
    private bool isPlacing = false;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private Vector2 initialPinchDistance;
    private float initialScale;

    void Update()
    {
        if (isPlacing && activeForme != null)
        {
            HandlePlacement();
            HandleScaling();
        }
        else
        {
            HandleSpawn();
        }
    }

    private void HandleSpawn()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 touchPosition = Input.GetTouch(0).position;

            if (arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;

                GameObject selectedForme = formeSelector.GetSelectedForme();

                if (selectedForme != null)
                {
                    activeForme = Instantiate(selectedForme, hitPose.position, hitPose.rotation);
                    isPlacing = true;
                }
            }
        }
    }

    private void HandlePlacement()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (arRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;
                activeForme.transform.position = hitPose.position;
            }
        }
    }

    private void HandleScaling()
    {
        if (Input.touchCount == 2 && activeForme != null)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                initialPinchDistance = touch1.position - touch2.position;
                initialScale = activeForme.transform.localScale.x;
            }
            else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                Vector2 currentPinchDistance = touch1.position - touch2.position;
                float scaleDelta = currentPinchDistance.magnitude / initialPinchDistance.magnitude;

                switch (formeSelector.GetSelectedForme().name.ToLower())
                {
                    case "square":
                        activeForme.transform.localScale = new Vector3(initialScale * scaleDelta, initialScale * scaleDelta, initialScale * scaleDelta);
                        break;

                    case "rectangle":
                        float newZ = initialScale * scaleDelta / 2f;
                        activeForme.transform.localScale = new Vector3(initialScale * scaleDelta, initialScale * scaleDelta, newZ);
                        break;

                    case "circle":
                        activeForme.transform.localScale = new Vector3(initialScale * scaleDelta, activeForme.transform.localScale.y, initialScale * scaleDelta);
                        break;

                    default:
                        break;
                }

                Parameters.objectScale = activeForme.transform.localScale;
            }
        }
    }

    public void ValidatePlacement()
    {
        if (activeForme != null)
        {
            isPlacing = false;
        }
    }

    public void CancelPlacement()
    {
        if (activeForme != null)
        {
            Destroy(activeForme);
            activeForme = null;
            isPlacing = false;
        }
    }
}
