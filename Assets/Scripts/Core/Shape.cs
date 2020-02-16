using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour {

    public bool m_canRotate = true;
    public Vector3 m_queuedOffset;
    public GameObject[] m_glowSquareFx;
    public string m_glowSquareTag;
    public int mWidth = 10;
    public int indexShape;
    public bool rotateIn2;
    public bool rotateIn9;

    public void LandShapeFx() {

        int i = 0;
        foreach (Transform Child in gameObject.transform) {
            if (m_glowSquareFx[i]) {
                  m_glowSquareFx[i].transform.position = new Vector3(Child.position.x , Child.position.y +1, -1) - new Vector3(0, 0, 0.1f);              
                ParticlePlayer particlePlayer = m_glowSquareFx[i].GetComponent<ParticlePlayer>();                
                if (particlePlayer) {
                    particlePlayer.Play();                    
                }                
                i++;
            }
        }

    }

    void Move(Vector3 moveDirection) {
        transform.position += moveDirection;
    }

    public bool PutInBoard(Shape shape)
    {
        foreach (Transform transform in shape.transform) {
            Vector3 position = VectorF.Round(transform.position);            
            if (position.x < 0)
            {
                MoveRight();
                if (PutInBoard(shape))
                {
                    if (shape.indexShape == 0) {
                        shape.rotateIn9 = false;
                        shape.rotateIn2 = true;
                    }
                    break;
                }
            }
            else if (position.x >= mWidth) {
                MoveLeft();
                if (PutInBoard(shape))
                {
                    if (shape.indexShape == 0) {
                        shape.rotateIn2 = false;
                        shape.rotateIn9 = true;
                    }
                    break;
                }
            }
        }
        
        return true;       
    }

    
    public void MoveLeft() {
        Move(new Vector3(-1, 0, 0));
    }


   public void MoveRight()
    {
        Move(new Vector3(1, 0, 0));
    }


    public void MoveUp()
    {
        Move(new Vector3(0, 1, 0));
    }

   public void MoveDown()
    {
        Move(new Vector3(0, -1, 0));
    }

   public void RotateLeft(Shape shape) {
        if (m_canRotate) {
            if (shape.rotateIn2)
            {
                shape.rotateIn2 = false;
                shape.rotateIn9 = false;
                transform.position = new Vector3(1, transform.position.y, transform.position.z);                                
            } else if (shape.rotateIn9) {
                shape.rotateIn2 = false;
                shape.rotateIn9 = false;
                transform.position = new Vector3(8, transform.position.y, transform.position.z);                
            }           
            transform.Rotate(0, 0, 90);           
        }        
    }

    public  void RotateRight(Shape shape) {
        if (m_canRotate) {
            if (shape.rotateIn2)
            {
                shape.rotateIn2 = false;
                shape.rotateIn9 = false;
                transform.position = new Vector3(1, transform.position.y, transform.position.z);            
            }
            else if (shape.rotateIn9) {
                shape.rotateIn2 = false;
                shape.rotateIn9 = false;
                transform.position = new Vector3(8, transform.position.y, transform.position.z);
            }           
            transform.Rotate(0, 0, -90);                       
        }        
    }
    

    // Use this for initialization
    void Start () {

        if (m_glowSquareTag != null && m_glowSquareTag != "") {
            m_glowSquareFx = GameObject.FindGameObjectsWithTag(m_glowSquareTag);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
