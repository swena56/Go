using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MessagingSystem : MonoBehaviour {

    EventSystem system;
    public InputField chatInputField;

    public bool showChatWindow = true;
    public GameObject chatWindow;
    public GameObject messageItemObject;
    public GameObject messageContainer;
    public GameObject scrollRect;
    public GameObject scrollBar;
    public Font chatFont;
    List<GameObject> messageList = new List<GameObject>(); 

    public void sendMessage()
    {
        if (chatInputField == null || chatInputField.text == "")
        {
            Debug.Log("chat input field is not found or field blank");
            return;
        }

        if (messageItemObject == null || messageContainer == null)
        {
            Debug.Log("message template object is null");
            return;
        }


        //send message
        Debug.Log("send message: "+chatInputField.text);
       

        //set message to message object
        GameObject messageItemTemp = Instantiate(messageItemObject) as GameObject;
        

        if (chatFont != null)
            messageItemObject.GetComponent<Text>().font = chatFont;


        messageItemTemp.GetComponent<UnityEngine.UI.Text>().text = "username: " +chatInputField.text;
        messageItemTemp.transform.SetParent(messageContainer.transform, true);
        messageList.Add(messageItemTemp);

        chatInputField.text = "";

        //keep the cursor in the chat window
        if (chatInputField != null)
            chatInputField.OnPointerClick(new PointerEventData(system));

        scrollBar.GetComponent<Scrollbar>().value = scrollBar.GetComponent<Scrollbar>().value - 0.01f;
        if (scrollRect != null)
        {
           
        }
        
        //add button

        //status of message

        //add it to messaging queue
    }

	// Use this for initialization
	void Start () {
        bool isLoggedIn = true;
        //am i logged in
        //chatWindow.SetActive(true);
            if (!isLoggedIn)
            {

            }

	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.BackQuote))
        {
            Debug.Log("Chat Window toggle");

            if (showChatWindow)
                showChatWindow = false;
            else
                showChatWindow = true;
        }

       




        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("Tab pressed");
            //chatInputField.
             if (chatInputField != null)
                    chatInputField.OnPointerClick(new PointerEventData(system));

            
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Enter pressed");
            sendMessage();
        }
        /*
        if (chatWindow != null)
            if (!showChatWindow)
                chatWindow.SetActive(false);     //hide chat window
            else
                chatWindow.SetActive(true);     //hide chat window
                */
    }
}
