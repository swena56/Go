using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;

public class GameSetup : MonoBehaviour
{

    
    static int boardSize = 15;
    //TODO make this an array
    public Texture2D bottomEdge;
    public Texture2D bottomLeftCorner;
    public Texture2D bottomRightCorner;
    public Texture2D leftEdge;
    public Texture2D middle;
    public Texture2D cross;
    public Texture2D rightEdge;
    public Texture2D topEdge;
    public Texture2D topLeftCorner;
    public Texture2D topRightCorner;
    public GameObject playerStatus;
    
    public bool showBowls = true;  //TODO make this toggle
    public bool showUI = false;
   
    public GameObject endGameScorePanel;
    private List<GameObject> boardParts = new List<GameObject>();
    public GamePieces gamePieces;
    public bool isBlacksTurn = true;

    public enum ThemeType
    {
        WOOD
    };
    public ThemeType theme = ThemeType.WOOD;
    public enum GType
    {
        TWO_PLAYER,
        TWO_PLAYER_NETWORK,
        ONE_PLAYER_EASY,
        ONE_PLAYER_MEDIUM,
        ONE_PLAYER_HARD
    };
    public GType GameType = GType.TWO_PLAYER;

    public void setBackGroundColor(Color color)
    {
        Camera.main.backgroundColor = color;
    }
    void Start()
    {
        boardParts = new List<GameObject>();
      
        if (showBowls)
        {
            GameObject blackbowl = (GameObject)Instantiate(Resources.Load("blackbowl"));
            GameObject whitebowl = (GameObject)Instantiate(Resources.Load("whitebowl"));
        }

        DrawGameBoard();
    }
    public void DrawGameBoard(bool playable = true)
    {
        destroyBoard();
        gamePieces.clearBoard();
        Debug.Log("Draw board of size " + boardSize);
        for (int x = 1; x <= boardSize; x++)
        {
            for (int y = 1; y <= boardSize; y++)
            {
                             
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                BoxCollider boxCollider = (BoxCollider)cube.gameObject.AddComponent(typeof(BoxCollider));
                
                boxCollider.center = new Vector3(0, 0, 0);
                boxCollider.size = new Vector3(1, 1, 1);

                //cube.AddComponent<Collider>()
                cube.transform.position = new Vector3(x, -0.1f, y);
                cube.name = "(" + x + "," + y + ")";

                if (playable) 
                    cube.tag = "board";           

                if (x > 1 && x < boardSize && y == 1)                   
                    cube.GetComponent<Renderer>().material.mainTexture = bottomEdge;
                else if (x == 1 && y == 1)                             
                    cube.GetComponent<Renderer>().material.mainTexture = bottomLeftCorner;
                //bottomRightCorner;
                else if (x == boardSize && y == 1)
                    cube.GetComponent<Renderer>().material.mainTexture = bottomRightCorner;
                //leftEdge;
                else if (y > 1 && y < boardSize && x == 1)
                    cube.GetComponent<Renderer>().material.mainTexture = leftEdge;      
               
                //rightEdge;
                else if (y > 1 && y < boardSize && x == boardSize)
                    cube.GetComponent<Renderer>().material.mainTexture = rightEdge;
                //topEdge;
                else if (x > 1 && x < boardSize && y == boardSize)
                    cube.GetComponent<Renderer>().material.mainTexture = topEdge;
                //topLeftCorner;
                else if (x == 1 && y == boardSize)
                    cube.GetComponent<Renderer>().material.mainTexture = topLeftCorner;
                //topRightCorner;
                else if (x == boardSize && y == boardSize)
                    cube.GetComponent<Renderer>().material.mainTexture = topRightCorner;
                else if (x == 4 && y == 4)
                    cube.GetComponent<Renderer>().material.mainTexture = cross;
                else if (x == 4 && y == 8)
                    cube.GetComponent<Renderer>().material.mainTexture = cross;
                 else if (x == 4 && y == 12)
                    cube.GetComponent<Renderer>().material.mainTexture = cross;
                else if (x == 8 && y == 8)
                    cube.GetComponent<Renderer>().material.mainTexture = cross;
                else if (x == 8 && y == 4)
                    cube.GetComponent<Renderer>().material.mainTexture = cross;
                else if (x == 8 && y == 4)
                    cube.GetComponent<Renderer>().material.mainTexture = cross;
                else if (x == 4 && y == 12)
                    cube.GetComponent<Renderer>().material.mainTexture = cross;
                else if (x == 8 && y == 12)
                    cube.GetComponent<Renderer>().material.mainTexture = cross;
                else if (x == 12 && y == 12)
                    cube.GetComponent<Renderer>().material.mainTexture = cross;
                else if (x == 12 && y == 8)
                    cube.GetComponent<Renderer>().material.mainTexture = cross;
                else if (x == 12 && y == 4)
                    cube.GetComponent<Renderer>().material.mainTexture = cross;
            
                else
                    cube.GetComponent<Renderer>().material.mainTexture = middle;
                cube.name = "(" + x + "," + y + ")";

                boardParts.Add(cube);         
            }
        }
    }


    public void pieceExamplesOfConnected()
    {
        DrawGameBoard(true);
        List<Vector2> black = new List<Vector2>();
        black.Add(new Vector2(8,4));
        black.Add(new Vector2(9,7));
        black.Add(new Vector2(8,11));
        black.Add(new Vector2(8,8));
        black.Add(new Vector2(8,12));
        black.Add(new Vector2(7,9));
        black.Add(new Vector2(8,13));
        black.Add(new Vector2(9,12));    
        black.Add(new Vector2(7,12));


        foreach (Vector2 pos in black)
        {
            gamePieces.isBlacksTurn = false;
            gamePieces.addPieces(pos);
            
            // System.Threading.Thread.Sleep(5000);
        }
        gamePieces.onlyBlack = true;
    }

    public void pieceExamplesOf()
    {
        gamePieces.onlyBlack = false;

        DrawGameBoard(true);
        List<Vector2> black = new List<Vector2>();
        black.Add(new Vector2(6, 5));
        black.Add(new Vector2(7, 7));
        black.Add(new Vector2(7, 6));
        black.Add(new Vector2(5, 7));
        black.Add(new Vector2(5, 6));
        black.Add(new Vector2(6, 8));
        black.Add(new Vector2(6, 7));
        // black.Add(new Vector2(5, 6));

        gamePieces.isBlacksTurn = true;
        foreach (Vector2 pos in black)
        {
            gamePieces.addPieces(pos);
          // System.Threading.Thread.Sleep(5000);
        }
        
    }
    public void pieceExamples()
    {
        DrawGameBoard(true);
        List<Vector2> black = new List<Vector2>();
        black.Add(new Vector2(1, 1));
        black.Add(new Vector2(6, 6));
        black.Add(new Vector2(1, 1));
        black.Add(new Vector2(1, 1));
        black.Add(new Vector2(1, 1));
        black.Add(new Vector2(1, 1));


        foreach (Vector2 pos in black)
        {
            gamePieces.addPieces(pos);
            gamePieces.isBlacksTurn = true;
            //System.Threading.Thread.Sleep(5000);
        }
        gamePieces.onlyBlack = true;

    }

    public void destroyBoard()
    {
        foreach(GameObject part in boardParts)
            Destroy(part);
    }
    public void pass()
    {
        Debug.Log("Pass button pressed");
        gamePieces.pass();
        
        gamePieces.removeTempPiece();

        if (endGameScorePanel != null)
        {
           // endGameScorePanel.GetComponent<InnerPanel>();

            if (gamePieces.isGameOver)
                endGameScorePanel.SetActive(true);
            else
                endGameScorePanel.SetActive(false);
        }

       
    }
 
    public void UpdateUI()
    {
        gamePieces.updateUI();
    }

    public void toggleCanAddPieces(bool on=true)
    {
        gamePieces.canAddPiecesOn = on;

        gamePieces.updateUI();
    }

    public void newGame()
    {
        toggleCanAddPieces(true);
        gamePieces.onlyBlack = false;

        Debug.Log("New Game");
        destroyBoard();
        DrawGameBoard(true);
        gamePieces.newGame();
        gamePieces.clearBoard();
    }
    public void newGameAiEasy()
    {
        Debug.Log("New Game newGameAiEasy");
        toggleCanAddPieces(true);
        gamePieces.onlyBlack = false;
        destroyBoard();
        DrawGameBoard(true);
        gamePieces.newGame();
        gamePieces.clearBoard();
        gamePieces.setGameType(GamePieces.GType.ONE_PLAYER_EASY);
    }
}
