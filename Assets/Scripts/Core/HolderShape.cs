using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolderShape : MonoBehaviour {

    public Transform m_HolderXForm;
    public Shape m_heldShape = null;
    float m_scale = 0.5f;
    public bool canRelese = false;

    public void Catch_Shape(Shape shape) {
        if (m_HolderXForm)
        {
            shape.transform.position = m_HolderXForm.position + shape.m_queuedOffset;
            shape.transform.localScale = new Vector3(m_scale , m_scale , m_scale);
            shape.transform.rotation = Quaternion.identity;
            m_heldShape = shape;
        }
    }

    public Shape ReleseHold() {
      
        m_heldShape.transform.localScale = Vector3.one;
        Shape shapeRelesed = m_heldShape;
        m_heldShape = null;
        canRelese = false;
        return shapeRelesed; 
    }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
