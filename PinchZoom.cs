using UnityEngine;

public class PinchZoom : MonoBehaviour
{
    public float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
    public float orthoZoomSpeed = 0.5f;        // The rate of change of the orthographic size in orthographic mode.


    public GameObject onScreenUI;
    private Camera camera;
    public GamePieces gamePieces;
    static Vector3 offset = new Vector3(-1.6f, 0.5f, 2.2f);
    float speed = 5.0f;
    float scroll;
    private float zoomSpeed = 8.0f;
    private Vector3 firstClickPos;
    private Vector3 secondClickPos;
    Vector3 lookAt = new Vector3(7, 0, 10);
    private Vector2 lastHitPos;


    public void detectAspectRatio()
    {
        //force aspect ratio
       // Camera.main.aspect = 5 / 4;


        if (Camera.main.aspect >= 1.7)
        {
            Debug.Log("16:9");
        }
        else if (Camera.main.aspect >= 1.5)
        {
            Debug.Log("3:2");
        }
       
        else
        {
            Debug.Log("4:3");
            Camera.main.fieldOfView = 102.7999f;
            Camera.main.transform.position = new Vector3(8.125001f, 6.717163f, 9.324996f);
        }
    }
    public void resetCamera()
    {
        //camera.fieldOfView = 39.0f;
    }

   

    // Use this for initialization
    void Start()
    {
        camera = Camera.main;
       
        Debug.Log("GamePieces Instaniated");
        Debug.Log("multitouch:" + Input.multiTouchEnabled);

        detectAspectRatio();
        resetCamera();
    }

    void panRotateCamera()
    {

        //pan when mouse hits edge of screen
        // Debug.Log("MousePos: " + Input.mousePosition.ToString());
        
        if (Input.GetKey(KeyCode.UpArrow) || (Input.mousePosition.x < (Screen.width - (Screen.width / 4)) &&
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

    bool doubleClickDetected = false;
    bool mouseClicksStarted = false;
    int mouseClicks = 0;
    float mouseTimerLimit = .25f;

    private Vector3 getboardHit()
    {
       // Camera.main.GetComponents<GameSetup>();
       // GetComponentsInParent<GameSetup>();
        gamePieces.removeTempPiece();
        Ray toMouse;
       
            toMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
      
        RaycastHit rhInfo;
        bool didHit = Physics.Raycast(toMouse, out rhInfo, 500.0f);
        if (didHit)
            return rhInfo.collider.bounds.center;
        else
            return new Vector3(0, 0, 0);
    }

    private Vector2 getboardHitXY()
    {
        
        gamePieces.removeTempPiece();
        Ray toMouse;
       
            toMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
       

        RaycastHit rhInfo;
        bool didHit = Physics.Raycast(toMouse, out rhInfo, 500.0f);
        if (didHit)
        {
            Vector3 temp = rhInfo.collider.bounds.center + offset;
            return new Vector2(temp.x, temp.z);
        }
        else
            return new Vector3(0, 0, 0);
    }
    private void checkMouseDoubleClick()
    {
        if (mouseClicks > 1)
        {
           Debug.Log("Double Clicked" + lastHitPos);
            firstClickPos = getboardHit();
            lastHitPos = getboardHitXY();
            doubleClickDetected = true;
            
        } else
        {
            doubleClickDetected = false;
            firstClickPos = getboardHit();
            lastHitPos = new Vector2(-1, -1);
            //  Debug.Log("single Clickedd");
        }

        mouseClicksStarted = false;
        mouseClicks = 0;
    }

    public void OnClick()
    {
        mouseClicks++;
        if (mouseClicksStarted)
            return;

        mouseClicksStarted = true;
        
        Invoke("checkMouseDoubleClick", mouseTimerLimit);     
    }

    private Vector2 getLastHitLocation()
    {
        return lastHitPos;
    }

    private bool onBoard()
    {
       
        Ray toMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rhInfo;
        bool didHit = Physics.Raycast(toMouse, out rhInfo, 500.0f);
       
        if (didHit && rhInfo.collider.tag == "board")
        {
            Vector2 boardHitLocation = new Vector2(rhInfo.collider.bounds.center.x, rhInfo.collider.bounds.center.z);
            return true;        
        }
        else
        {
            doubleClickDetected = false;         
            return false;
        }
    }

    

    private void drawTemp()
    {
       if(onBoard())
        {

            

            gamePieces.removeTempPiece();
            gamePieces.drawTempPiece(getboardHit());
            
        }
        /*
        if (didHit && rhInfo.collider.tag == "board")
        {
            gamePeiceLocation = rhInfo.collider.bounds.center;

            if (lastHitPos != getOnBoardPos(gamePeiceLocation))
            {
                doubleClickDetected = false;
                lastHitPos = getOnBoardPos(gamePeiceLocation);
                gamePieces.removeTempPiece();
                gamePieces.drawTempPiece(gamePeiceLocation);
            }
        }
        */
    }

   

    void Update()
    {
        //
        //panRotateCamera();
        // If there are two touches on the device...

       // if(!Input.touchSupported)
            zoomCamera();

        if (Input.touchCount > 1)
        {
            gamePieces.removeTempPiece();
            Debug.Log("Zoom");
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // If the camera is orthographic...
            if (camera.orthographic)
            {
                // ... change the orthographic size based on the change in distance between the touches.
                camera.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

                // Make sure the orthographic size never drops below zero.
                camera.orthographicSize = Mathf.Max(camera.orthographicSize, 0.1f);
            }
            else
            {
                // Otherwise change the field of view based on the change in distance between the touches.
                camera.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

                // Clamp the field of view to make sure it's between 0 and 180.
                camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, 0.1f, 179.9f);
            }
        }



        // didItHitTheBoard(Input.mousePosition);

        



        //get click pos
        Vector3 clickPos = new Vector3(-1,-1,-1);

        // if (Input.touchSupported && Input.touchCount == 1)
        //   clickPos = Input.GetTouch(1).position;
        //else 
        if (Input.GetMouseButtonDown(0))
                clickPos = Input.mousePosition;



               

            if (Input.GetMouseButtonDown(0))           
                OnClick();

            if(doubleClickDetected && onBoard())
            {
                Debug.Log("Double Click");

            

                Vector3 checkMe = getboardHit();
            checkMe.y = 0;

            
            if (firstClickPos != (new Vector3(-1, -1, -1))  && lastHitPos != (new Vector2(-1, -1)) && getboardHitXY() == lastHitPos )
            {
               
                gamePieces.removeTempPiece();
                gamePieces.addGamePiece(firstClickPos);
                // new Vector3(-1, -1, -1);
                firstClickPos = new Vector3(-1, -1, -1);
                lastHitPos = new Vector2(-1, -1);
            }
           
        }


        drawTemp();

        /*
            doubleClickDetected = false;
            gamePieces.removeTempPiece();

            getOnBoardPos(Input.mousePosition);


            bool results = gamePieces.addGamePiece(gamePeiceLocation);
            //lookAt = gamePeiceLocation;
            lastHitPos = getOnBoardPos(gamePeiceLocation);
        }


        if (didHit && rhInfo.collider.tag == "board")
        {
            gamePeiceLocation = rhInfo.collider.bounds.center;

            if (lastHitPos != getOnBoardPos(gamePeiceLocation))
            {
                doubleClickDetected = false;
                lastHitPos = getOnBoardPos(gamePeiceLocation);
                gamePieces.removeTempPiece();
                gamePieces.drawTempPiece(gamePeiceLocation);
            }
        }
        */



    }
}