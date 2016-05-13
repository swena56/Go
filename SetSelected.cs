using UnityEngine;
using System.Collections;

public class SetSelected : MonoBehaviour {

     string selected = "";
    public UnityEngine.UI.Text selectTextObject;
	// Use this for initialization
	void Start () {
	
	}
	
    public void setSelected()
    {
        Debug.Log("user clicked");
        GameObject respawns = GameObject.FindGameObjectWithTag("selected");

        if (respawns != null)
        {
            selected = respawns.name.ToString();
            //respawns.GetComponent<UnityEngine.UI.Text>().color
            if (selectTextObject != null)
                selectTextObject.text = selected;
            respawns.tag = "Untagged";
            Debug.Log("" + selected);
        }
        
    }
     
	// Update is called once per frame
	void Update () {

        
    }


}
