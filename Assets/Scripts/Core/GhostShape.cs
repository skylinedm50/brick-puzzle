using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostShape : MonoBehaviour {

    Shape _GhostShape = null;
    bool hitBottom = false;
    public Color m_color = new Color(1f , 1f , 1f , 0.5f);

    public void DrawGhost(Shape originalShape , Board gameBoard) {

        if (!_GhostShape)
        {
            _GhostShape = Instantiate(originalShape, originalShape.transform.position, originalShape.transform.rotation) as Shape;
            _GhostShape.gameObject.name = "GhostShape";
            SpriteRenderer[] allRendered = _GhostShape.GetComponentsInChildren<SpriteRenderer>();

            foreach (SpriteRenderer rendered in allRendered)
            {
                rendered.color = m_color;
            }
        }
        else {
            _GhostShape.transform.position = originalShape.transform.position;
            _GhostShape.transform.rotation = originalShape.transform.rotation;
        }

        hitBottom = false;

        while (!hitBottom) {
            _GhostShape.MoveDown();
            if (!gameBoard.isValidPosition(_GhostShape)) {
                _GhostShape.MoveUp();
                hitBottom=true;
            }
        }
    }

    public void Reset() {
        Destroy(_GhostShape.gameObject);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
