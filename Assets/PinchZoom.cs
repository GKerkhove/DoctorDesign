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
            }
            // Store both touches.
            
            float newdistance = (touchZero.position - touchOne.position).magnitude;
            ActualZoom  = CurrentZoom * (newdistance / startdistance);
            zoomableImage.sizeDelta = startSize * ActualZoom;
        }
        else
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