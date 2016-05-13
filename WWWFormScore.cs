using UnityEngine;
using System.Collections;

using UnityEngine.UI;

using SimpleJSON;
using System.Net;
using System.Collections.Generic;

public class WWWFormScore : MonoBehaviour
{
    public string playerName = "Player 1";
    public bool connected = false;

    //objects passed in by networkAccess unity gameobject
    public GameObject onlinePlayers;
    public GameObject onlineStatus;
    public InputField playerNameInputField;
    public InputField passField;   
    public Text status;
    public GameObject loginRequest;

    //public GameObject listOfOnlineUsers;
    public GameObject templatePlayer;

    private JSONNode currentDataSet;
    ListManager listManager;
    public WWWFormScore() { }

    public Scrollbar scrollbarV;
    //public GameObject loadingPanel;
    public Sprite[] stateSprites;


    List<GameObject> gsList = new List<GameObject>();
    // List containers that list Items - (Dynamically Increasing ListView <Custom>)
    public GameObject GS_ListContainer;
    //Prefabs that holds items that will be places in the containers.

   

    public void updateOnlineUsers()
    {
        onlinePlayers.GetComponent<ScrollRect>();
    }

    public void updatePlayerStatus()
    {
        onlineStatus.GetComponent<Text>().text = playerName + " ranking of 0";
    }

    public void login()
    {
        
        if (playerNameInputField.text != "" && passField.text != "")
        {
            status.text = "Processing...";
            WWWForm form = new WWWForm();
            playerName = playerNameInputField.text;
            form.AddField("username", playerName);
            form.AddField("hashcode", "c32ad802df5abca941d784abf642e7fd");
            form.AddField("password", passField.text);

            WWW usersForm = new WWW("http://swena56.ddns.net/sendData.php", form);
            StartCoroutine(registerWWW(usersForm));
        }
        else
            status.text = "Enter a username and password";
    }

    //works but the post method in getOnlineusers works too.
    public void upload()
    {
        string url = "http://swena56.ddns.net/sendData.php";
        string data = "wooooo! test!!";

        using (WebClient client = new WebClient())
        {
            client.UploadString(url, data);
        }

    }

    IEnumerator registerWWW(WWW www)
    {
        yield return www;
        Debug.Log(www.text);
        
        currentDataSet = JSONNode.Parse(www.text);        
        updateUserList();     
    }

    void Start()
    {
        gsList.Clear();
        SetScrollBarVisibility(false);
    }
   
    private void SetScrollBarVisibility(bool status)
    {
        scrollbarV.gameObject.SetActive(status);
    }

    public void OnScrollRectValueChanged()
    {
        scrollbarV.gameObject.SetActive(true);
        Invoke("HideScrollbar", 2);
    }

    void HideScrollbar()
    {
        SetScrollBarVisibility(false);
    }
    //Click Handler for Back Button
    public void BackMain()
    {
        //Application.LoadLevel(0);
    }
    public void updateUserList()
    {
        int numUsersOnline = currentDataSet["online"].Count;
        var b = currentDataSet["online"]["array"].Value;

        JSONArray onlineArray = currentDataSet["online"].AsArray;
        JSONArray playerArray = currentDataSet["player"].AsArray;
        loginRequest.SetActive(false);

        for (int i = 0; i < numUsersOnline; i++)
        {
            string p = onlineArray[i][1].Value.ToString() + " rank of " + onlineArray[i][2].Value.ToString();

            GameObject a = Instantiate(templatePlayer) as GameObject;
            a.transform.SetParent(GS_ListContainer.transform, false);
            a.GetComponent<UnityEngine.UI.Text>().text = p;
            gsList.Add(a);
            
            
           


            Debug.Log("added online user to list: " + p);

        }
    }

    void isConnected()
    {
        connected = true;

        //close window
        Debug.Log("Is active:" + loginRequest.activeInHierarchy);

    }

    public void addUser()
    {
        Debug.Log("Add User");
        playerName = playerNameInputField.text;
        WWWForm form = new WWWForm(); //here you create a new form connection
        //form.AddField("myform_hash", hash); //add your hash code to the field myform_hash, check that this variable name is the same as in PHP file
        form.AddField("username", playerName);

        playerName = playerNameInputField.text;
        WWW addUserForm = new WWW("http://swena56.ddns.net/addUser.php",form);
        StartCoroutine(registerWWW(addUserForm));
    }
}