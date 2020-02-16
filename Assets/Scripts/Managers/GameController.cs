using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    Board m_Board;

    ScoreManager score_manager;

    Spawner m_Spawner;

    Shape m_activeShape;

    GhostShape m_ghostShape;

    GameOver m_GameOverPanel;

    public float m_dropInterval = 0.1f;

    float m_dropIntervalModded;

    float m_timeToDrop; 

    float m_timeToNextKeyLeftRight;

    HolderShape m_holder;

    [Range(0.02f, 1f)]
    public float m_keyRepeatRateLeftRight = 0.25f;

    float m_timeToNextKeyDown;

    [Range(0.01f, 0.5f)]
    public float m_keyRepeatRateDown = 0.01f;

    float m_timeToNextKeyRotate;

    [Range(0.02f, 1f)]
    public float m_keyRepeatRateRotate = 0.25f;

    bool m_gameOver = false;

  //  public GameObject OverLoadPanel;

    SoundManager m_soundManager;

    public bool m_isPaused = false;

    public GameObject m_PausedPanel;

    enum Direction { none , left , right , down};
    Direction m_DragDirection = Direction.none;
    Direction m_SwipeDirection = Direction.none;

    float m_timeToNextSwipe;
    float m_timeToNextDrag;

    [Range(0.05f, 1f)]
    public float m_minTimeToDrag = 0.15f;

    [Range(0.05f, 1f)]
    public float m_minTimeToSwipe = 0.3f;

    bool didTap = false;

    // AdsScript ads;

    // Use this for initialization
    void Start () {
        //    this.ads = GameObject.FindObjectOfType<AdsScript>();
        //   OverLoadPanel.SetActive(false);
        Screen.orientation = ScreenOrientation.Portrait;
        this.m_GameOverPanel = GameObject.FindObjectOfType<GameOver>();
        m_PausedPanel.SetActive(false);
        this.m_timeToNextKeyLeftRight = Time.time+ m_keyRepeatRateLeftRight;
        this.m_timeToNextKeyDown = Time.time+ m_keyRepeatRateDown;
        this.m_timeToNextKeyRotate = Time.time+ m_keyRepeatRateRotate;
        this.score_manager = GameObject.FindObjectOfType<ScoreManager>();
        this.m_Board = GameObject.FindObjectOfType<Board>();
        this.m_Spawner = GameObject.FindObjectOfType<Spawner>();
        this.m_soundManager = GameObject.FindObjectOfType<SoundManager>();
        this.m_ghostShape = GameObject.FindObjectOfType<GhostShape>();
        this.m_holder = GameObject.FindObjectOfType<HolderShape>();
        this.m_GameOverPanel.HideGameOverPanel();

        if (m_activeShape == null) {
            m_activeShape = m_Spawner.SpawnShape();
        }
        m_Spawner.transform.position = VectorF.Round(m_Spawner.transform.position);
        m_dropIntervalModded = m_dropInterval;
    }


	// Update is called once per frame
	void Update () 
    {
        if (!m_Board || !m_Spawner || !m_activeShape || m_gameOver || !m_soundManager || !score_manager)
        {
            return;
        }
        else {
            playerInputs();
        }        
	}

    void LateUpdate() {
        if (m_ghostShape) {
            m_ghostShape.DrawGhost(m_activeShape , m_Board);
        }
    }

    void MoveRight() {
        m_activeShape.MoveRight();
        m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;
        if (!m_Board.isValidPosition(m_activeShape))
        {
            
            m_activeShape.MoveLeft();

        }
        else
        {
            if (m_activeShape.rotateIn2)
            {
                m_activeShape.rotateIn2 = false;
            }

            if (m_activeShape.rotateIn9 && m_activeShape.transform.position.x < 10)
            {
                m_activeShape.rotateIn9 = false;
            }

            PlayClip(m_soundManager.m_MoveSound);
        }
    }

    void MoveLeft() {
        m_activeShape.MoveLeft();
        m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;
        if (!m_Board.isValidPosition(m_activeShape))
        {
            m_activeShape.MoveRight();
        }
        else
        {

            if (m_activeShape.rotateIn9)
            {
                m_activeShape.rotateIn9 = false;
            }

            if (m_activeShape.rotateIn2 && m_activeShape.transform.position.x > 0)
            {
                m_activeShape.rotateIn2 = false;
            }
            PlayClip(m_soundManager.m_MoveSound);
        }
    }

    void RightInputRotate() {
        m_activeShape.RotateRight(m_activeShape);
        m_timeToNextKeyRotate = Time.time + m_keyRepeatRateRotate;
        if (!m_Board.isValidPosition(m_activeShape))
        {
            if (!m_Board.IsLeftOrRightOut(m_activeShape))
            {         
                if (m_Board.BelowTheBoard(m_activeShape))
                {                    
                    m_activeShape.RotateLeft(m_activeShape);
                    m_activeShape.PutInBoard(m_activeShape);
                }
                else
                {                    
                    m_activeShape.PutInBoard(m_activeShape);
                }
            }
            else
            {                
                m_activeShape.RotateLeft(m_activeShape);
                m_activeShape.PutInBoard(m_activeShape);
            }
        }
    }

    void MoveDown() {
        m_timeToDrop = Time.time + m_dropIntervalModded;
        m_timeToNextKeyDown = Time.time + m_keyRepeatRateDown;
        if (m_activeShape)
        {
            m_activeShape.MoveDown();
        }

        if (!m_Board.isValidPosition(m_activeShape))
        {
            if (!m_Board.LimitIsOver(m_activeShape))
            {
                LandShape();
            }
            else
            {
                GameOver();
            }
        }
    }

    void playerInputs() {
        if (!m_Board || !m_Spawner )
        {
            return;
        }

        if ((Input.GetButton("MoveRight") && Time.time > m_timeToNextKeyLeftRight) || Input.GetButtonDown("MoveRight"))
        {
            MoveRight();
        }
        else if ((Input.GetButton("MoveLeft") && Time.time > m_timeToNextKeyLeftRight) || Input.GetButtonDown("MoveLeft"))
        {
            MoveLeft();
        }
        else if ((Input.GetButtonDown("Rotate") && Time.time > m_timeToNextKeyRotate))
        {
            RightInputRotate();
        }
        else if ((Input.GetButton("MoveDown") && Time.time > m_timeToNextKeyDown) || Time.time > m_timeToDrop)
        {
            MoveDown();
        }
        //Controls for touch...
        else if ((m_SwipeDirection == Direction.right && Time.time > m_timeToNextSwipe) ||
                 (m_DragDirection == Direction.right && Time.time > m_timeToNextDrag))
        {
            MoveRight();
            m_timeToNextDrag = Time.time + m_minTimeToDrag;
            m_timeToNextSwipe = Time.time + m_minTimeToSwipe;
        }
        else if ((m_SwipeDirection == Direction.left && Time.time > m_timeToNextSwipe) ||
                 (m_DragDirection == Direction.left && Time.time > m_timeToNextDrag))
        {
            MoveLeft();
            m_timeToNextDrag = Time.time + m_minTimeToDrag;
            m_timeToNextSwipe = Time.time + m_minTimeToSwipe;
        }
        else if ((m_DragDirection == Direction.down && Time.time > m_timeToNextKeyDown))
        {
            MoveDown();
            m_timeToNextDrag = Time.time + m_minTimeToDrag;
            m_timeToNextSwipe = Time.time + m_minTimeToSwipe;

        } else if (didTap) {
            RightInputRotate();
            didTap = false;
        }

        m_SwipeDirection = Direction.none;
        m_DragDirection = Direction.none;
        didTap = false;
    }

    public void HoldShape() {
        if (!m_holder)
        {
            return;
        }


        if (!m_holder.m_heldShape) {
            m_holder.Catch_Shape(m_activeShape);
            m_activeShape = m_Spawner.SpawnShape();
            PlayClip(m_soundManager.m_HoldSound);
        }
        else if(m_holder.canRelese)
        {           
            Shape nextToHold = m_activeShape;
            m_activeShape = m_holder.ReleseHold();
            m_activeShape.transform.position = m_Spawner.transform.position;
            m_holder.Catch_Shape(nextToHold);
            PlayClip(m_soundManager.m_HoldSound);
        }

        if (m_ghostShape) {
            m_ghostShape.Reset();
        }
    }

    void PlayClip(AudioClip clip ) {
        if (m_soundManager.m_FxEnabled && clip)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, m_soundManager.m_FxVolume);
        }
    }


    void LandShape() {

        this.m_timeToNextKeyLeftRight =Time.time;
        this.m_timeToNextKeyDown = Time.time;
        this.m_timeToNextKeyRotate = Time.time;

        m_activeShape.LandShapeFx();

        if (m_ghostShape) {
            m_ghostShape.Reset();
        }

        m_holder.canRelese = true;
        m_activeShape.MoveUp();
        m_Board.StoreShapeInGrid(m_activeShape);
        
         m_Board.StartCoroutine("ClearAllRows");

        PlayClip(m_soundManager.m_DropSound);
        if (m_Board.CountrowsDeletes>0)
        {
            score_manager.scoreLines(m_Board.CountrowsDeletes);
            if (score_manager.isLevelUp){
                score_manager.isLevelUp = false;
                m_dropIntervalModded = Mathf.Clamp( m_dropInterval - (((float)score_manager.m_level - 1) * 0.01f) , 0.05f , 1 );
                PlayClip(m_soundManager.m_LevelUp);
            }           
            PlayClip(m_soundManager.m_ClearRowsSound);            
            m_Board.rowsDeleted = false;            
        }
        m_activeShape = m_Spawner.SpawnShape();

    }

    public void RightRotate() {

        if (Time.time > m_timeToNextKeyRotate)
        {
            m_activeShape.RotateRight(m_activeShape);
            m_timeToNextKeyRotate = Time.time + m_keyRepeatRateRotate;
            if (!m_Board.isValidPosition(m_activeShape))
            {
                if (!m_Board.IsLeftOrRightOut(m_activeShape))
                {
                    if (m_Board.BelowTheBoard(m_activeShape))
                    {
                        m_activeShape.RotateLeft(m_activeShape);
                        m_activeShape.PutInBoard(m_activeShape);
                    }
                    else
                    {
                        m_activeShape.PutInBoard(m_activeShape);
                    }
                }
                else
                {
                    m_activeShape.RotateLeft(m_activeShape);
                    m_activeShape.PutInBoard(m_activeShape);
                }
            }

        }
    }

    public void LeftRotate()
    {
        didTap = false;
        if (Time.time > m_timeToNextKeyRotate)
        {
            m_activeShape.RotateLeft(m_activeShape);
            m_timeToNextKeyRotate = Time.time + m_keyRepeatRateRotate;
            if (!m_Board.isValidPosition(m_activeShape))
            {
                if (!m_Board.IsLeftOrRightOut(m_activeShape))
                {
                    if (m_Board.BelowTheBoard(m_activeShape))
                    {
                        m_activeShape.RotateRight(m_activeShape);
                        m_activeShape.PutInBoard(m_activeShape);
                    }
                    else
                    {
                        m_activeShape.PutInBoard(m_activeShape);
                    }
                }
                else
                {
                    m_activeShape.RotateRight(m_activeShape);
                    m_activeShape.PutInBoard(m_activeShape);
                }
            }
        }
    }

    public void SetGamePause() {
        m_isPaused = !m_isPaused;
        if (m_isPaused)
        {
            m_soundManager.pauseMusic(true);
            m_PausedPanel.SetActive(true);
        }
        else {
            m_soundManager.pauseMusic(false);
            m_PausedPanel.SetActive(false);
        }
        Time.timeScale = (m_isPaused) ? 0 : 1;
    }

    void GameOver()
    {
        m_activeShape.MoveUp();
        m_gameOver = true;
        score_manager.SaveScore();
        PlayClip(m_soundManager.m_GameOverSound);
        m_GameOverPanel.ShowPanelGameOver(score_manager.GetCurrentScore());
    
    }

    public void Restart() {
        m_isPaused = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = (m_isPaused) ? 0 : 1;
    }

    void OnEnable() {
        TouchController.DragEvent += DragHandler;
        TouchController.SwipeEvent += SwipeHandler;
        TouchController.TapEvent += TapHandler;
    }

    void OnDisable() {
        TouchController.DragEvent -= DragHandler;
        TouchController.SwipeEvent -= SwipeHandler;
        TouchController.TapEvent -= TapHandler;
    }

    public void DragHandler(Vector2 dragMovement)
    {
        m_DragDirection = GetDirection(dragMovement);
    }

    public void SwipeHandler(Vector2 swipeMovement) {
        m_SwipeDirection = GetDirection(swipeMovement);
    }

    public void TapHandler(Vector2 TapeMovement)
    {

        didTap = true;
    }

    Direction GetDirection(Vector2 swipeMovement) {
        Direction swipeDir = Direction.none;
        // Horizontal
        if (Mathf.Abs(swipeMovement.x) > Mathf.Abs(swipeMovement.y))
        {
            swipeDir = (swipeMovement.x >= 0) ? Direction.right : Direction.left;
        }
        //Vertical
        else {
            swipeDir = (swipeMovement.y >= 0) ? Direction.none : Direction.down;
        }

        return swipeDir;
    }


}
