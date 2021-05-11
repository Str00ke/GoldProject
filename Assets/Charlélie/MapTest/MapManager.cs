using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public GameObject mapCanvas;
    float tmpMouseZoom = 0;
    Vector2 startPos;
    Vector2 direction;
    Vector2 currPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            // Handle finger movements based on TouchPhase
            switch (touch.phase)
            {
                //When a touch has first been detected, change the message and record the starting position
                case TouchPhase.Began:
                    // Record initial touch position.
                    currPos = touch.position;
                    break;

                //Determine if the touch is a moving touch
                case TouchPhase.Moved:
                    // Determine direction by comparing the current touch position with the initial one
                    Vector2 vec = (touch.position + touch.deltaPosition) - touch.position;
                    mapCanvas.transform.position = Vector2.MoveTowards(mapCanvas.transform.position, vec + (Vector2)mapCanvas.transform.position, (vec.magnitude * Time.deltaTime) * 10);
                    currPos = vec;
                    break;

                case TouchPhase.Ended:
                    // Report that the touch has ended when it ends
                    currPos = Vector2.zero;
                    break;
            }
            
        }
        
        else if (Input.touchCount == 2)
        {
            Vector2 vec = Input.GetTouch(1).position - Input.GetTouch(0).position;
            
            if (tmpMouseZoom == 0)
            {
                tmpMouseZoom = vec.magnitude;
            } else if (mapCanvas.transform.localScale.x > 0.2f)
            {
                if (vec.magnitude < tmpMouseZoom - 1 && mapCanvas.transform.localScale.x - 0.08f > 0.2f)
                {
                    mapCanvas.transform.localScale = new Vector3(mapCanvas.transform.localScale.x - 0.08f, mapCanvas.transform.localScale.y - 0.08f, 1);
                    tmpMouseZoom = vec.magnitude;
                }                   
                else if (vec.magnitude > tmpMouseZoom + 1 && mapCanvas.transform.localScale.x + 0.08f < 3.5f)
                {
                    mapCanvas.transform.localScale = new Vector3(mapCanvas.transform.localScale.x + 0.08f, mapCanvas.transform.localScale.y + 0.08f, 1);
                    tmpMouseZoom = vec.magnitude;
                }
                    
            } 
        } else
        {
            tmpMouseZoom = 0;
        }
    }
}
