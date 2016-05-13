using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour
{
   

    public GameObject playerStatus;
    public bool showBowls = true;
    public bool showUI = false;
    static int boardSize = 15;

   
    public bool blacksTurn = true;
    float speed = 5.0f;
    private float zoomSpeed = 6.0f;
    private float minX = 270.0f;
    private float maxX = 360.0f;

    private float minY = -45.0f;
    private float maxY = 45.0f;

    private float sensX = 100.0f;
    private float sensY = 100.0f;

    float rotationY = 0.0f;
    float rotationX = 0.0f;
    float scroll;
    Vector3 lookAt = new Vector3(7, 0, 10);
   
 
    public void resetCamera()
    {
        Camera.main.fieldOfView = 39.0f;
    }

    // Use this for initialization
    void Start()
    {


        gamePieces = GetComponent<GamePieces>();
        Debug.Log("GamePieces Instaniated");
        Debug.Log("multitouch:" + Input.multiTouchEnabled);
        
        resetCamera();
    }

    public void interact()
    {
        Debug.Log("Interact");
    }
   

    
    void rotateCamera()
    {
        //if mouse is on one side the screen rotate that corresponding direction
        if (Input.GetMouseButton(2))
            transform.Rotate(0, 0, 1);
        

        /*
      else if (Input.GetMouseButton(1) && Input.mousePosition.y < (Screen.height / 4))
          transform.Translate(Vector3.down * (Time.deltaTime) * 5);
      else if (Input.GetMouseButton(1) && Input.mousePosition.x > (Screen.width/4))             
          transform.Translate(Vector3.right * (Time.deltaTime)*5);
      else if (Input.GetMouseButton(1) && Input.mousePosition.x < (Screen.width / 4))
          transform.Translate(Vector3.left * (Time.deltaTime) * 5);
     */
        /*
           rotationX += Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
           rotationY += Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;
           rotationY = Mathf.Clamp(rotationY, minY, maxY);
           Debug.Log("Rotate: " + rotationX + "," + rotationY);
           transform.localEulerAngles = new Vector3(45, rotationY, rotationX);//-rotationY, rotationX,0 );
           */

    }
    void panRotateCamera()
    {

        //pan when mouse hits edge of screen
        // Debug.Log("MousePos: " + Input.mousePosition.ToString());
      
            if (Input.GetKey(KeyCode.UpArrow) ||  (Input.mousePosition.x < (Screen.width - (Screen.width / 4)) &&
               Input.mousePosition.x > (Screen.width / 4) &&
               Input.mousePosition.y > (Screen.height - (Screen.height / 6))))
                transform.Translate(Vector3.up * (Time.deltaTime) * speed);


            if (Input.GetKey(KeyCode.DownArrow) || (Input.mousePosition.x < (Screen.width - (Screen.width / 4)) &&
                Input.mousePosition.x > (Screen.width / 4) &&
                Input.mousePosition.y < ((Screen.height / 6))))
                transform.Translate(Vector3.down * (Time.deltaTime) * speed);

            if (Input.GetKey(KeyCode.UpArrow) || (Input.mousePosition.x > (Screen.width - (Screen.width / 4)) &&
               Input.mousePosition.y < (Screen.height - (Screen.height / 6)) &&
                Input.mousePosition.y > ((Screen.height / 6))))
                transform.Translate(Vector3.right * (Time.deltaTime) * speed);

            if (Input.GetKey(KeyCode.LeftArrow) || (Input.mousePosition.x < ((Screen.width / 4)) &&
               Input.mousePosition.y < (Screen.height - (Screen.height / 6)) &&
                Input.mousePosition.y > ((Screen.height / 6))))
                transform.Translate(Vector3.left * (Time.deltaTime) * speed);
        
    }
    void zoomCamera()
    {
        //zoom in with scroll wheel
        scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
            Camera.main.fieldOfView -= scroll * zoomSpeed;
    }

    public float rotationSpeed = 100.0F;



    private GamePieces gamePieces;
    private bool hasFired = false;
    private bool pieceJustPlaced = false;
    private float mouseDownStart;
    private Vector3 lastHitPos;


   
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
