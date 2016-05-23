using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PinchZoom : MonoBehaviour
{
    public RectTransform zoomableImage;

    // Use this for initialization
    void Start()
    {
        startSize = zoomableImage.sizeDelta;
    }
    private float CurrentZoom = 1f;

    private float ActualZoom = 1f;
    private Vector2 startSize;
    Vector2 anchordistance;
    Vector2 startposition;
    float startZ;
    Vector2 middle;
    private bool zooming = false;
    private float startdistance = 0.0f;

    void Update()
    {

        
        // If there are two touches on the device...
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);
            if (zooming == false)
            {
                GetComponent<ScrollRect>().enabled = false;
                zooming = true;
                startdistance = (touchZero.position - touchOne.position).magnitude;
                startposition = zoomableImage.position;
                startZ = zoomableImage.position.z;
                middle = (touchZero.position + ((touchOne.position - touchZero.position) * 0.5f));

                anchordistance = startposition - middle;
            }
            // Store both touches.
            
            float newdistance = (touchZero.position - touchOne.position).magnitude;
            
            float tempActualZoom  = CurrentZoom * (newdistance / startdistance);
            if (tempActualZoom > 0.5 && tempActualZoom < 4) {
                ActualZoom = tempActualZoom;
                //Vector3 newPosition = middle + anchordistance * (newdistance / startdistance);
                //newPosition.z = startZ;
                //zoomableImage.position = newPosition;
                zoomableImage.sizeDelta = startSize * ActualZoom;
            }
        }
        else if(Input.touchCount == 0)
        {
            if(zooming == true)
            {
                GetComponent<ScrollRect>().enabled = true;
                zooming = false;
                CurrentZoom = ActualZoom;
            }
            
        }

    }
}