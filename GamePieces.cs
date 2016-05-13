using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class GamePieces : MonoBehaviour {

    class BoardSnapshot
    {

        //potential snapshot

        //snapshots get saved when a capture takes place

        //before removing the pieces compare to

        public BoardSnapshot()
        {
            //private List<Vector2> blackPlays = new List<Vector2>();
    //private List<Vector2> whitePlays = new List<Vector2>();
        }


        //before move snapshot
        //after move snapshot
        //current snapshot

            //if after move snapshot
        public void takeSnapShot()
        {

        }

        public void isValidMove()
        {
            
        }

        public void setCurrentSnapshot()
        {

        }
    }

    static int boardSize = 16;
    static Vector3 offset = new Vector3(-1.6f, 0.5f, 2.2f);
    public bool isBlacksTurn = true;
    public bool isGameOver = false;
    public UnityEngine.UI.Text blackScore;
    public UnityEngine.UI.Text whiteScore;
    public UnityEngine.UI.Text endGameScore;
    private int numBlackCapturedStones = 0;
    private int numWhiteCapturedStones = 0;
    public bool onlyBlack = false;

    public bool canAddPiecesOn = true;
    public enum GType
    {
        TWO_PLAYER,
        TWO_PLAYER_NETWORK,
        ONE_PLAYER_EASY,
        ONE_PLAYER_MEDIUM,
        ONE_PLAYER_HARD       
    };
    public GType GameType = GType.TWO_PLAYER;

    private int timesPassed = 0;
    private GameObject[,] array = new GameObject[boardSize, boardSize];
    private List<GameObject> allGameObjects = new List<GameObject>();  
    private List<List<Vector2>> blackPieces = new List<List<Vector2>>();
    private List<List<Vector2>> whitePieces = new List<List<Vector2>>();
    private List<Vector2> blackPlays = new List<Vector2>();
    private List<Vector2> whitePlays = new List<Vector2>();
    private GameObject tempDisplayStone = null;
       
    public void clearBoard()
    {
        isBlacksTurn = true;
        //new game
        foreach (GameObject piece in allGameObjects)
        {
            Destroy(piece);
        }
        blackPieces = new List<List<Vector2>>();
        whitePieces = new List<List<Vector2>>();
        

        blackPlays = new List<Vector2>();
        whitePlays = new List<Vector2>();
        array = new GameObject[boardSize, boardSize];  
    }    
    GameObject InstansitateGamePeice(Vector3 pos, bool isBlack)
    {       
        if (isBlack)
        {
            GameObject go = (GameObject)Instantiate(Resources.Load("blackStone"));
            go.transform.position = new Vector3(0,0,0) + offset;
            go.transform.position = pos + offset;
            go.name = "blackStoneAt(" + pos.x + "," + pos.z + ")";
            return go;
        }
        else
        {
            GameObject go = (GameObject)Instantiate(Resources.Load("whiteStone"));
            go.transform.position = new Vector3(0, 0, 0) + offset;
            go.transform.position = pos + offset;
            go.name = "whiteStoneAt(" + pos.x + "," + pos.z + ")";
            return go;
        }
    }    
    public void addPieces(Vector2 pos,  int delay=0, bool loop= false)
    {     
        
        //foreach (Vector2 move in list)
            addGamePiece(new Vector3(pos.x, -0.2f, pos.y));
    }   
    List<Vector2> getConnectingSpots(Vector2 pos)
    {
        List<Vector2> adjcentSpots = new List<Vector2>();
  
            if (pos.x > 1)
                adjcentSpots.Add(new Vector2(pos.x - 1, pos.y));

            if (pos.x < (boardSize-1))
                adjcentSpots.Add(new Vector2(pos.x + 1, pos.y));

            if (pos.y > 1)
                adjcentSpots.Add(new Vector2(pos.x, pos.y - 1));

            if (pos.y < (boardSize-1))
                adjcentSpots.Add(new Vector2(pos.x, pos.y + 1));
        
       // Debug.Log("onboardLocation: " + pos + " has " + adjcentSpots.Count + " adjcent positions");

        return adjcentSpots;
    }
    void getWhiteGroups()
    {
        List<Vector2> groups = new List<Vector2>();

        foreach (Vector2 value in whitePlays)
            foreach (Vector2 adjSpot in getConnectingSpots(value))        
                if(whitePlays.Contains(adjSpot))
                    groups.Add(adjSpot);
             
        string output = "";
        foreach(Vector2 vec in groups)
            output = output + "(" + vec.x + "," + vec.y + ")";
        Debug.Log("WhiteGroups: " + output);
    }
    void getBlackGroups()
    {
        List<Vector2> groups = new List<Vector2>();

        foreach (Vector2 value in blackPlays)
            foreach (Vector2 adjSpot in getConnectingSpots(value))
                if (blackPlays.Contains(adjSpot))
                    groups.Add(adjSpot);

        string output = "";
        foreach (Vector2 vec in groups)
            output = output + "(" + vec.x + "," + vec.y + ")";
        Debug.Log("BlackGroups: " + output);
    }
    string whichPlayerOwns(Vector2 pos)
    {
        if (blackPlays.Contains(pos))
            return "black";
        else if (whitePlays.Contains(pos))
            return "white";
        else
            return "empty";     
    }
    int numberOfLiberties(List<Vector2> list)
    {
        int numberOfLiberties = 0;
        foreach(Vector2 pos in list)
            foreach(Vector2 vec in getConnectingSpots(pos))
                if (whichPlayerOwns(vec) == "empty")
                    numberOfLiberties++;
        return numberOfLiberties;
    }
    List<Vector2> getPiecesThatConnectWith(Vector2 pos)
    {
        List<Vector2> list = new List<Vector2>();     
    
        if (whichPlayerOwns(pos) == "black")
        {
            list.Add(pos);

            Queue<Vector2> queue = new Queue<Vector2>();
            foreach(Vector2 spot in getConnectingSpots(pos))
                queue.Enqueue(spot);
                
            do
            {
                Vector2 checkMe = queue.Dequeue();

                if (whichPlayerOwns(checkMe) == "black" && !list.Contains(checkMe))
                {
                    list.Add(checkMe);
                    foreach (Vector2 newPos in getConnectingSpots(checkMe))                
                        if (whichPlayerOwns(checkMe) == "black")
                            queue.Enqueue(newPos);       
                }                   
            }
            while (queue.Count > 0) ;     
        }
        else if (whichPlayerOwns(pos) == "white")
        {
            list.Add(pos);

            Queue<Vector2> queue = new Queue<Vector2>();
            foreach (Vector2 spot in getConnectingSpots(pos))
                queue.Enqueue(spot);

            do
            {
                Vector2 checkMe = queue.Dequeue();

                if (whichPlayerOwns(checkMe) == "white" && !list.Contains(checkMe))
                {
                    list.Add(checkMe);
                    foreach (Vector2 newPos in getConnectingSpots(checkMe))
                        if (whichPlayerOwns(checkMe) == "white")
                            queue.Enqueue(newPos);
                }
            }
            while (queue.Count > 0);
        }

        return list;
    }
    List<Vector2> getConnectedEmptySpots(Vector2 pos)
    {
        List<Vector2> list = new List<Vector2>();
        Queue<Vector2> queue = new Queue<Vector2>();
        if (whichPlayerOwns(pos) == "empty")
        {
            list.Add(pos);

            
            foreach (Vector2 spot in getConnectingSpots(pos))
                queue.Enqueue(spot);

            Debug.Log("start");
            do
            {
                Vector2 checkMe = queue.Dequeue();

                if (whichPlayerOwns(checkMe) == "empty" && !list.Contains(checkMe))
                {
                    list.Add(checkMe);
                    foreach (Vector2 newPos in getConnectingSpots(checkMe))
                        if (whichPlayerOwns(newPos) == "empty")
                            queue.Enqueue(newPos);
                }
            }
            while (queue.Count > 0);
        }
        

        return list;
    }
    void removePiece(Vector2 pos)
    {
        GameObject exp = (GameObject)Resources.Load("Detonator-Tiny", typeof(GameObject));
        exp.transform.position = new Vector3(0, 0, 0);
       
        if (whichPlayerOwns(pos) == "white")
        {
            Debug.Log("white group removed, has " + whitePlays.Count + " pieces on the board");
            List<Vector2> removeThese = getPiecesThatConnectWith(pos);

            foreach (Vector2 remove in removeThese)
            {
                whitePlays.Remove(remove);
                exp.transform.position = new Vector3(remove.x,0.2f, remove.y);
                Instantiate(exp);
                
                GameObject stone = GameObject.Find("whiteStoneAt(" + remove.x + "," + remove.y + ")");
                stone.AddComponent<Rigidbody>();           
                stone.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                numWhiteCapturedStones++;
    //Destroy(GameObject.Find("whiteStoneAt(" + remove.x + "," + remove.y + ")"));
}   
        } else if (whichPlayerOwns(pos) == "black")
        {
            Debug.Log("black group removed, has "+ blackPlays.Count+ " pieces on the board");
            List<Vector2> removeThese = getPiecesThatConnectWith(pos);

            foreach (Vector2 remove in removeThese)
            {
                blackPlays.Remove(remove);
                exp.transform.position = new Vector3(remove.x, 0.2f, remove.y);
                Instantiate(exp);
                GameObject stone = GameObject.Find("blackStoneAt(" + remove.x + "," + remove.y + ")");
                stone.AddComponent<Rigidbody>();
                stone.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                //Destroy(GameObject.Find("blackStoneAt(" + remove.x + "," + remove.y + ")"));
                numBlackCapturedStones++;
            }
        }
    }
    public void setGameType(GType type)
    {
        if (type == GType.ONE_PLAYER_EASY)
            GameType = GType.ONE_PLAYER_EASY;
        else if (type == GType.ONE_PLAYER_MEDIUM)
            GameType = GType.ONE_PLAYER_MEDIUM;
        else if (type == GType.ONE_PLAYER_HARD)
            GameType = GType.ONE_PLAYER_HARD;
        else if (type == GType.TWO_PLAYER)
            GameType = GType.TWO_PLAYER;
        else if (type == GType.TWO_PLAYER_NETWORK)
            GameType = GType.TWO_PLAYER_NETWORK;
        else 
            GameType = GType.TWO_PLAYER;
        
    }
    public void newGame()
    {
        timesPassed = 0;
        numWhiteCapturedStones = 0;
        numBlackCapturedStones = 0;
        canAddPiecesOn = true;
        isGameOver = false;
        clearBoard();
    }
    public void removeTempPiece()
    {
        if (tempDisplayStone != null)
            DestroyImmediate(tempDisplayStone);

        tempDisplayStone = null;
    }
    public void drawTempPiece(Vector3 pos)
    {
       
        if (!isGameOver && canAddPiecesOn)
        {
            Vector2 posXY = new Vector2(pos.x, pos.z);

            DestroyImmediate(tempDisplayStone);

            if (whichPlayerOwns(posXY) == "empty")
            {
                if (isBlacksTurn)
                {
                    tempDisplayStone = (GameObject)Instantiate(Resources.Load("blackStone-outline"));
                    tempDisplayStone.transform.position = new Vector3(0, 0, 0) + offset;
                    tempDisplayStone.transform.position = pos + offset;
                    tempDisplayStone.name = "tempStone(" + pos.x + "," + pos.z + ")";

                }
                else
                if (!isBlacksTurn)
                {
                    tempDisplayStone = (GameObject)Instantiate(Resources.Load("whiteStone-outline"));

                    tempDisplayStone.transform.position = new Vector3(0, 0, 0) + offset;
                    tempDisplayStone.transform.position = pos + offset;
                    tempDisplayStone.name = "tempStone(" + pos.x + "," + pos.z + ")";
                }
            }
        }
    }
    private List<List<Vector2>> getEmptyGroups()
    {
       
        List<Vector2> list = new List<Vector2>();
        List<List<Vector2>> emptyGroups = new List<List<Vector2>>();


        for (int x = 1; x < boardSize; x++)
            for (int y = 1; y < boardSize; y++)
            {
                if(whichPlayerOwns(new Vector2(x, y)) == "empty")
                    list.Add(new Vector2(x, y));
                
            }

        
        while (list.Count > 0 && list[0] != null)
        {
                 
            Vector2 spotToCheck = list[0];
           // list.Remove(spotToCheck);
            List<Vector2> group = getConnectedEmptySpots(spotToCheck);
           // group.Sort();
            emptyGroups.Add(group);

            foreach (Vector2 item in group)
                if (list.Contains(item))
                    list.Remove(item);

            
            Debug.Log("");  
           
        }
        Debug.Log("Done");
        return emptyGroups;
    }
    public int calculateScore(bool forBlack)
    {
        int numStones = 0;
        int numTerrirtory = 0;
        int numCaptured = 0;

        

        //number of black territory + number of black stones - number of black stones captured
        if (forBlack)
        {
            numStones = blackPlays.Count;
            numTerrirtory = 0;
            numCaptured = 0;
        }else
        {
            numStones = whitePlays.Count;
            numTerrirtory = 0;
            numCaptured = 0;
        }

        //getEmptyGroups();

        return numStones;
    }
    private void playSoundEffect()
    {
        //TODO add sound effects

        float throwSpeed = 2000f;
        AudioSource source;
        float volLowRange = .5f;
        float volHighRange = 1.0f;
        //  Debug.Log("Wav");
        //  AudioClip wav = Resources.Load("click.wav") as AudioClip;
        // float vol = UnityEngine.Random.Range(volLowRange, volHighRange);
        // source = GetComponent<AudioSource>();
        // source.PlayOneShot(wav);

        //source.PlayOneShot(shootSound, vol);
    }


    public void getSnapShotOfBoard()
    {

    }
    bool isRecreatingPreviousBoardLayout()
    {


        return false;
    }

    public bool addGamePiece(Vector3 gamePeiceLocation)
    {

        if (gamePeiceLocation == null || !canAddPiecesOn)
            return false;

       

        timesPassed = 0;
        Vector2 onboardLocation = new Vector2(gamePeiceLocation.x, gamePeiceLocation.z);
        removeTempPiece();

        if (isBlacksTurn && whichPlayerOwns(onboardLocation) == "empty")
        {
            allGameObjects.Add(InstansitateGamePeice(gamePeiceLocation, isBlacksTurn));           
            blackPlays.Add(onboardLocation);
            playSoundEffect();

            //remove spots with no liberties
            List<Vector2> toRemove = new List<Vector2>();
            foreach (Vector2 spot in getConnectingSpots(onboardLocation))
                if (whichPlayerOwns(spot) == "white" && numberOfLiberties(getPiecesThatConnectWith(spot)) == 0)
                    removePiece(spot);

            List<Vector2> list = getPiecesThatConnectWith(onboardLocation);
            int numLiberties = numberOfLiberties(list);

            Debug.Log("Black placed peice at "+ onboardLocation+  " forming a group of size " + list.Count + " and has " + numLiberties + " liberties");
            
            //kill your self
            if (numLiberties <= 0)                        
                foreach (Vector2 spot in list)
                    removePiece(spot);

        }
        else if (!isBlacksTurn && whichPlayerOwns(onboardLocation) == "empty")
        {
            allGameObjects.Add(InstansitateGamePeice(gamePeiceLocation, isBlacksTurn));
            whitePlays.Add(onboardLocation);
            playSoundEffect();        
            
            foreach (Vector2 spot in getConnectingSpots(onboardLocation))
            if (whichPlayerOwns(spot) == "black" && numberOfLiberties(getPiecesThatConnectWith(spot)) == 0)
                removePiece(spot);

            List<Vector2> list = getPiecesThatConnectWith(onboardLocation);
            int numLiberties = numberOfLiberties(list);
            Debug.Log("White placed peice at " + onboardLocation + " forming a group of size " + list.Count + " and has " + numLiberties + " liberties");

            //kill your self
            if (numLiberties <= 0)          
                foreach (Vector2 spot in list)
                    removePiece(spot);                   
        }
        else
        {
            return false;
        }

        playerSwitch();
        //Debug.Log("Pos" + gamePeiceLocation + ", normalized " + gamePeiceLocation.normalized + ", onboardLocation: " + onboardLocation + ", Black: " + blackPlays.Count + ", White: " + whitePlays.Count);
        if (GameType == GType.ONE_PLAYER_EASY && !isBlacksTurn)
                playAI(GameType);
 
        return true;
    }    
    public bool whoseTurn()
    {
        return isBlacksTurn;
    }

    public void Update()
    {

    }
    public void updateUI()
    {
        blackScore.text = "Black: "+ calculateScore(true);
        whiteScore.text = "White: " + calculateScore(false);
        if (isBlacksTurn)
        {
            blackScore.color = Color.green;
            whiteScore.color = Color.white;        
        } else
        {
            whiteScore.color = Color.green;
            blackScore.color = Color.white;          
        }
        //else
        //  scoreText.text = "White";

    }
    public void playerSwitch()
    {
        //white will always be the computer player    
     

        if (isBlacksTurn)
                isBlacksTurn = false;
        else
            isBlacksTurn = true;

        if (onlyBlack)
            isBlacksTurn = true;

        updateUI();
    }
    private void playAI(GType gtype)
    {
        //get random empty spot
        System.Random random = new System.Random();
        Vector2 play = new Vector2(-1, -1);
        do
        {
            int randomNumber = random.Next(1, boardSize);
            play = new Vector2(random.Next(1, boardSize), random.Next(1, boardSize));


        } while (whichPlayerOwns(play) != "empty");

        addGamePiece(new Vector3(play.x, -0.1f, play.y));             
    }
    public void clickBoardSection()
    {
        Debug.Log("board section clicked");
    }
    public bool pass()
    {    
        timesPassed++;
        if (timesPassed >= 2)
        {
            Debug.Log("GameOver");
            isGameOver = true;
        }

        playerSwitch();

        if (isGameOver)
        {
            int scoreB = 0;
            int scoreW = 0;
            int numBlackStones = calculateScore(true);
            int numWhiteStones = calculateScore(false);
            int terrB = 0;
            int terrW = 0;
           // EndGameScoresPanel.GetComponent<Text>().
            List<List<Vector2>> groups = getEmptyGroups();
            foreach(List<Vector2> group in groups)
            {
                int whiteCount = 0;
                int blackCount = 0;
                foreach(Vector2 stone in group)
                {
                    foreach(Vector2 spot in getConnectingSpots(stone))
                    {
                        if (whichPlayerOwns(spot) == "black")
                            blackCount++;
                        if (whichPlayerOwns(spot) == "white")
                            whiteCount++;
                    }
                   

                   
                }
                //if only one color of stones has incremented then that terriroty belongs to that color
                if (whiteCount > 0 && blackCount == 0)
                    terrW = terrW + group.Count;
                if (blackCount > 0 && whiteCount == 0)
                    terrB = terrB + group.Count;
            }

            string output = "";
            scoreB = terrB + numBlackStones;
            scoreW = terrW + numWhiteStones;
            if (scoreB > scoreW)
                output = "Winner is Black\n\n";
            else if (scoreW > scoreB)
                output = "Winner is White\n\n";
            else if (scoreW == scoreB)
                output = "It's a tie\n\n";
            else output = "UNKNOWN\n\n";

            output = output + "Black: "+ scoreB+"\n";
            output = output + "-stones: " + calculateScore(true) + "\n";
            output = output + "-terroritory: "+ terrB + " \n";
            output = output + "-captured pieces: " + numBlackCapturedStones+ " \n";
            output = output + "White: "+scoreW+"\n";
            output = output + "-stones: " + calculateScore(false) + "\n";
            output = output + "-terroritory: " + terrW + " \n";
            output = output + "-captured pieces: " + numWhiteCapturedStones + " \n";

            endGameScore.GetComponent<Text>().text = output;
            return true;
        }
        else
            return false;

       
    }  
}




    