using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SimpleJSON;
using System.Collections.Generic;
using System.Net;

public class Login : MonoBehaviour
{

    EventSystem system;
    public Button loginButton;

    public string playerName = "Player 1";
    public bool connected = false;

    //objects passed in by networkAccess unity gameobject  
    public InputField playerNameInputField;
    public InputField passField;
    public Text status;
    public GameObject loginRequest;
    public GameObject onlineLobbyUI;
    public GameObject chatUI;
    public GameObject templatePlayer;
    public GameObject logoutButton;

    private JSONNode currentDataSet;
    public GameObject scrollbarV;
    public Sprite[] stateSprites;
    List<GameObject> gsList = new List<GameObject>();  // List containers that list Items - (Dynamically Increasing ListView <Custom>)
    public GameObject GS_ListContainer;     //Prefabs that holds items that will be places in the containers.


    // Use this for initialization
    void Start()
    {
        system = EventSystem.current;

        //set cursor to user name
        if (playerNameInputField != null)
            playerNameInputField.OnPointerClick(new PointerEventData(system));


        //clear list
        gsList.Clear();

    }

    void getNextSelectable()
    {
        Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
        // which field is selected
        if (next != null)
        {
            InputField inputField = next.GetComponent<InputField>();
            if (inputField != null)
                inputField.OnPointerClick(new PointerEventData(system));

            system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
        }
    }

    // This function is called when the object becomes enabled and active
    public void OnEnable()
    {

        
        //set cursor to user name
        if (playerNameInputField != null && passField != null)
        {
            playerNameInputField.text = "";
            passField.text = "";
            playerNameInputField.OnPointerClick(new PointerEventData(system));
        }

        foreach (Transform child in GS_ListContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }


        //clear list
        gsList.Clear();

    }

    public void OnDisable()
    {
        //chatUI.SetActive(false);
    }



    // Sent to all game objects before the application is quit
    public void OnApplicationQuit()
    {

    }

    // Awake is called when the script instance is being loaded
    public void Awake()
    {

    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("Tab pressed");
            getNextSelectable();

        }

        if (!connected)
        {
            //chatUI.SetActive(false);
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("Enter pressed");

                //loginButton.
                login();

            }
        }
    }

    public void logOut()
    {
        connected = false;

        //turn off logout button
        if (logoutButton != null)
            logoutButton.SetActive(false);

        onlineLobbyUI.SetActive(false);

        //send disconnect signal
    }

    public void login()
    {
        chatUI.SetActive(true);
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

    IEnumerator registerWWW(WWW www)
    {
        yield return www;
        Debug.Log(www.text);

        currentDataSet = JSONNode.Parse(www.text);
        loginRequest.SetActive(false);

    }


    public void updateUserList()
    {
        int numUsersOnline = currentDataSet["online"].Count;
        var b = currentDataSet["online"]["array"].Value;

        JSONArray onlineArray = currentDataSet["online"].AsArray;
        JSONArray playerArray = currentDataSet["player"].AsArray;
        

        for (int i = 0; i < numUsersOnline; i++)
        {
            string p = onlineArray[i][1].Value.ToString() + " rank of " + onlineArray[i][2].Value.ToString();

            GameObject a = Instantiate(templatePlayer) as GameObject;
            a.transform.SetParent(GS_ListContainer.transform, true);
            a.GetComponent<UnityEngine.UI.Text>().text = p;
            gsList.Add(a);

            Debug.Log("added online user to list: " + p);
        }
        isConnected();
    }
    void isConnected()
    {
        connected = true;

        if (logoutButton != null)
            logoutButton.SetActive(true);
        //close window
        Debug.Log("Is active:" + loginRequest.activeInHierarchy);

        //update users
    }
    public void addUser()
    {
        Debug.Log("Add User");
        playerName = playerNameInputField.text;
        WWWForm form = new WWWForm(); //here you create a new form connection
        //form.AddField("myform_hash", hash); //add your hash code to the field myform_hash, check that this variable name is the same as in PHP file
        form.AddField("username", playerName);

        playerName = playerNameInputField.text;
        WWW addUserForm = new WWW("http://swena56.ddns.net/addUser.php", form);
        StartCoroutine(registerWWW(addUserForm));
    }
}
