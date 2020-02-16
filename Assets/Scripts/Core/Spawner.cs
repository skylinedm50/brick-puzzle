using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public Shape[] m_allShapes;
    public Transform[] m_queuedXForms = new Transform[3];
    Shape[] m_queuedShape = new Shape[3];
    float m_queuedScale = 0.5f;

    // Use this for initialization
    void Start () {
        InitQueued();        	
	}

    Shape getRamdonShape()
    {
        int i = Random.Range(0,m_allShapes.Length);
        if (m_allShapes[i]) {            
            m_allShapes[i].m_canRotate = (i == 3) ? false : true;
            return m_allShapes[i];
        }
        else{
            return null;
        }
    }

    public Shape SpawnShape() {
        Shape shape=null;
        shape = GetQueueShape();
        shape.transform.position = transform.position;
        shape.transform.localScale = Vector3.one;
       // shape = Instantiate(getRamdonShape(), transform.position, Quaternion.identity);        
        return shape;
    }


	// Update is called once per frame
	void Update () {
		
	}

    void InitQueued() {
        for (int i = 0; i < m_queuedShape.Length; i++) {
            m_queuedShape[i] = null;
        }
        FillQueued();
    }

    void FillQueued() {
        for (int i = 0; i < m_queuedShape.Length; i++) {
            if (!m_queuedShape[i]) {
                m_queuedShape[i] = Instantiate(getRamdonShape(), transform.position, Quaternion.identity) as Shape;
                m_queuedShape[i].transform.position = m_queuedXForms[i].transform.position +m_queuedShape[i].m_queuedOffset;
                m_queuedShape[i].transform.localScale = new Vector3(m_queuedScale , m_queuedScale , m_queuedScale);
            }
        }

    }

    Shape GetQueueShape() {
        Shape firstShape = null;

        if (m_queuedShape[0]) {
            firstShape = m_queuedShape[0];
        }
        for (int i = 1; i < m_queuedShape.Length; i++) {
            m_queuedShape[i - 1] = m_queuedShape[i];
            m_queuedShape[i - 1].transform.position = m_queuedXForms[i-1].position + m_queuedShape[i].m_queuedOffset;
        }

        m_queuedShape[m_queuedShape.Length - 1] = null;
        FillQueued();
        return firstShape;
    }



}
