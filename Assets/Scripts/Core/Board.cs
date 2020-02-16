using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    public Transform emptySquare;
    public int mHeight = 31;
    public int mWidth = 10;
    public int mHeader = 8;
    Transform[,] m_grid;
    SoundManager m_soundManager;
    public bool rowsDeleted = false;
    public int CountrowsDeletes = 0;
    public ParticlePlayer[] m_rowGlowFx = new ParticlePlayer[4];


    void Awake() {
       m_grid = new Transform[mWidth, mHeight];
    }

	// Use this for initialization
	void Start () {
        this.m_soundManager = GameObject.FindObjectOfType<SoundManager>();
        drawEmptySquare();

    }

	
	// Update is called once per frame
	void Update () {
		
	}

    bool IsWithInTheBoard(int x , int y) {
        return (x >= 0 && x < this.mWidth && y >= 1);
    }


   public bool IsLeftOrRightOut(Shape shape) {
        foreach (Transform child in shape.transform) {
            Vector3 position = VectorF.Round(child.position);
            if (!IsWithInTheBoard((int)position.x , (int)position.y)) {
                return false;
            }
        }
        return true;
    }



    public bool BelowTheBoard(Shape shape) {
        foreach (Transform child in shape.transform) {
            Vector3 position = VectorF.Round(child.position);
            if ((int)position.y <= 0) {
                return true;
            }
        }
        return false;
    }


    public bool isValidPosition(Shape shape)
    {
        foreach (Transform child in shape.transform)
        {            
            Vector3 position = VectorF.Round(child.position);            
            if (!IsWithInTheBoard((int)position.x, (int)position.y) || isOccupied((int)position.x, (int)position.y , shape) )
            {                
                return false;
            }
        }
        return true;
    }

    bool isOccupied(int x, int y, Shape shape) {
        return (m_grid[x,y] != null && m_grid[x,y].parent != shape.transform);
    }

    public void StoreShapeInGrid(Shape shape) {
        foreach (Transform chield in shape.transform) {
            Vector3 position = VectorF.Round(chield.transform.position);
            m_grid[(int)position.x, (int)position.y] = chield;
        }
    }


   void drawEmptySquare(){
        if (emptySquare) {
            for (int y = 1; y < mHeight - mHeader; y++){
                for (int x = 0; x < mWidth; x++){
                    Transform clone;
                    clone = Instantiate(emptySquare, new Vector2(x, y), Quaternion.identity) as Transform;
                    clone.name = "BoardName ( x = " + x.ToString() + " , y=" + y.ToString() + " ) ";
                    clone.transform.parent = transform;
                }
            }
        }   
    }

    bool isCompletly(int y) {
        for (int x = 0; x < mWidth; x++) {
            if (m_grid[x, y] == null) {
                return false;
            }
        }
        return true;
    }

    void ClearRows(int y) {
        for (int x = 0; x < mWidth; x++) {
            if (m_grid[x, y] != null) {
                Destroy(m_grid[x, y].gameObject);
                m_grid[x, y] = null;
            }
        }
    }

    void ShiftRowDown(int y) {
        for (int x = 0; x < mWidth; x++) {
            if (m_grid[x, y] != null) {
                m_grid[x, y - 1] = m_grid[x, y];
                m_grid[x, y] = null;
                m_grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    void ShiftAllRowDown(int Starty)
    {
        for (int y = Starty; y < mHeight - mHeader; y++)
        {
            ShiftRowDown(y);
        }
    }


    public IEnumerator ClearAllRows()
    {
        CountrowsDeletes = 0;

        for (int y = 0; y < mHeight; ++y)
        {
            if (isCompletly(y))
            {
                ClearRowFx(CountrowsDeletes, y);
                CountrowsDeletes++;
            }
        }
        yield return new WaitForSeconds(0.3f);

        for (int y = 0; y < mHeight; ++y)
        {
            if (isCompletly(y))
            {
                ClearRows(y);
                ShiftAllRowDown(y + 1);
                rowsDeleted = true;
                yield return new WaitForSeconds(0.25f);
                y--;
               
            }

        }
    }

    public bool LimitIsOver(Shape shape) {

        foreach (Transform chield in shape.transform) {
            if (chield.transform.position.y >= ((mHeight - mHeader) - 1)) {
                m_soundManager.StopBackgroundMusic();
                return true;
            }
        }
        return false;
    }

    void ClearRowFx(int index , int y) {
        if (m_rowGlowFx[index]) {
            m_rowGlowFx[index].transform.position = new Vector3(0,y,-1.1f);
            m_rowGlowFx[index].Play();
        }
    }

}


