using UnityEngine;
using System.Collections;

public class PlayPiece : MonoBehaviour {



    // Update is called once per frame
    
    private GamePieces gamePieces;
    private bool hasFired = false;
    private bool pieceJustPlaced = false;
    private float mouseDownStart;
    private Vector3 lastHitPos;


    // Use this for initialization
    void Start()
    {
        gamePieces = GetComponent<GamePieces>();

    }
    void Update()
    {
        // transform.LookAt(lookAt);
        Ray toMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rhInfo;
        bool didHit = Physics.Raycast(toMouse, out rhInfo, 500.0f);

        if (didHit && rhInfo.collider.tag == "board")
        {
            Vector3 gamePeiceLocation = rhInfo.collider.bounds.center;
            gamePieces.drawTempPiece(gamePeiceLocation);

        }

        if (Input.GetMouseButtonDown(0))
        {

            hasFired = false;
            mouseDownStart = Time.time;
        }
        if (Input.GetMouseButton(0))
        {
            if (!hasFired && (Time.time - mouseDownStart) >= 0.35)
            {
                // 3 seconds has passed with the mouse down             

                pieceJustPlaced = true;
                Debug.Log("Mouse Up");
                gamePieces.removeTempPiece();

                toMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
                didHit = Physics.Raycast(toMouse, out rhInfo, 500.0f);
                Vector3 gamePeiceLocation = rhInfo.collider.bounds.center;
                hasFired = true;
                if (lastHitPos != gamePeiceLocation)
                {
                    mouseDownStart = Time.time;
                    lastHitPos = gamePeiceLocation;
                    hasFired = false;

                }
                else if (didHit && rhInfo.collider.tag == "board")
                {

                    bool results = gamePieces.addGamePiece(gamePeiceLocation);
                    // lookAt = gamePeiceLocation;
                    pieceJustPlaced = true;


                }
            }
        }
    }

}
