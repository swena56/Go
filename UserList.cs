using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using SimpleJSON;

public class UserList : MonoBehaviour
{
    EventSystem system;


    public bool showUserListWindow = true;

    public GameObject userWindow;
    public GameObject userItemObject;
    public GameObject userContainer;
    public GameObject scrollRect;
    public GameObject scrollBar;
    public Font userFont;


    string selected = "";
    public UnityEngine.UI.Text selectTextObject;
    JSONNode currentDataSet;
     GameObject userListObject;
    //user Image pictures

    List<GameObject> userList = new List<GameObject>();

    void Start()
    {
        bool isLoggedIn = true;
        //am i logged in

        getUserList();
        if (showUserListWindow)
        { }
        if (!isLoggedIn)
        {

        }


    }
    void getUserList()
    {
       
           
            WWWForm form = new WWWForm();
            //playerName = playerNameInputField.text;
          //  form.AddField("username", playerName);
            form.AddField("hashcode", "c32ad802df5abca941d784abf642e7fd");
           // form.AddField("password", passField.text);

            WWW usersForm = new WWW("http://swena56.ddns.net/getUsers.php", form);
            StartCoroutine(registerWWW(usersForm));
       
       
    }

   

    IEnumerator registerWWW(WWW www)
    {
        yield return www;
        Debug.Log(www.text);

        currentDataSet = JSONNode.Parse(www.text);
        updateUserList();
        
    }


    public void updateUserList()
    {
        int numUsersOnline = currentDataSet["online"].Count;
        var b = currentDataSet["online"]["array"].Value;

        JSONArray onlineArray = currentDataSet["online"].AsArray;
       
       

        for (int i = 0; i < numUsersOnline; i++)
        {
            string p =  "      " + onlineArray[i][1].Value.ToString() + " rank of " + onlineArray[i][2].Value.ToString();

            GameObject a = Instantiate(userItemObject) as GameObject;
            a.transform.SetParent(userContainer.transform, true);
            a.GetComponent<UnityEngine.UI.Text>().text = p;
            a.tag = "Untagged";
            //a.GetComponent<Selectable>();
            a.name = p;
            //a.GetComponent<Button>().onClick.AddListener(() => setClick(p));

            
            a.GetComponent<Button>().onClick.AddListener(() => {

                Debug.Log("user clicked");
                
                selected = a.name;
                
                selectTextObject.text = selected;

            });
            
//            a.GetComponent<Button>().
            userList.Add(a);

           // Debug.Log("added online user to list: " + p);
        }
       
    }
   
    

    void setClick(string index)
    {
        Debug.Log("clicked");
    }

    public void addUser()
    {
      
        //set message to message object
        GameObject userItemTemp = Instantiate(userItemObject) as GameObject;


        if (userFont != null)
            userItemTemp.GetComponent<Text>().font = userFont;


        userItemTemp.GetComponent<UnityEngine.UI.Text>().text = "";
        userItemTemp.transform.SetParent(userContainer.transform, true);
        userList.Add(userItemTemp);

       

        

        scrollBar.GetComponent<Scrollbar>().value = scrollBar.GetComponent<Scrollbar>().value - 0.01f;
       
    }

    // Use this for initialization
   

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.BackQuote))
       
    }


}
