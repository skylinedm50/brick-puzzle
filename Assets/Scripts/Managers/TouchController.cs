using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TouchController : MonoBehaviour {

    public delegate void TouchEventHandler(Vector2 swipe);
    public static event TouchEventHandler DragEvent;
    public static event TouchEventHandler SwipeEvent;
    public static event TouchEventHandler TapEvent;
   
    Vector2 m_TouchMovement;

    [Range(50, 150)]
    public int m_minDragDistance = 100;

    [Range(50,250)]
    public int m_minSwipeDistance = 200;

    float m_tabTimeMax = 0;
    public float m_tabTimeWindow = 0.1f;


    void OnTap() {
        if (TapEvent != null) {
            TapEvent(m_TouchMovement);
        }
    }

    void OnDrag() {
        if (DragEvent != null) {
            DragEvent(m_TouchMovement);
        }        
    }

    void OnSwipeEnd() {
        if (SwipeEvent != null)
        {
            SwipeEvent(m_TouchMovement);
        }
    }
    

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];

            if (touch.phase == TouchPhase.Began)
            {
                m_TouchMovement = Vector2.zero;
                m_tabTimeMax = Time.time + m_tabTimeWindow;
                
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                m_TouchMovement += touch.deltaPosition;

                if (m_TouchMovement.magnitude > m_minDragDistance)
                {
                    OnDrag();
                  //  Diagnostic("Drag detected", m_touchMovement.ToString() + " " + m_touchMovement.ToString());
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (m_TouchMovement.magnitude > m_minDragDistance)
                {
                    OnSwipeEnd();
                 //   Diagnostic("Swipe detected", m_touchMovement.ToString() + " " + m_touchMovement.ToString());
                }
                else if (Time.time < m_tabTimeMax)
                {
                    OnTap();
                //    Diagnostic("Tag detected", m_touchMovement.ToString() + " " + m_touchMovement.ToString());
                }
            }
        }
    }
}
