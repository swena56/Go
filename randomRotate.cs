using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class randomRotate : MonoBehaviour {


    GameObject black;
    GameObject white;
    List<GameObject> stones;
    // Use this for initialization
    Camera camera;
	
    private Vector3 getRandomVector(Vector3 orig)
    {

        //return new Vector3(Random.Range(0.0f, 10.0f), Random.Range(0.0f, 10.0f), Random.Range(0.0f, 10.0f));
        return orig + new Vector3(Random.Range(-110.0f, -190.0f), Random.Range(-110.0f, -120.0f), Random.Range(-110.0f, -190.0f));

    }
    private void drawStone(Vector3 orig)
    {
        for (int i = 0; i < 100; i++)
        {
            GameObject go;
            int blackOrWhite = Random.Range(0, 2);
            if(blackOrWhite == 1 )
                go = (GameObject)Instantiate(Resources.Load("blackStone"));
            else
                go = (GameObject)Instantiate(Resources.Load("whiteStone"));

            //go = (GameObject)Instantiate(Resources.Load("blackStone"));
            Vector3 pos = getRandomVector(orig);
            go.transform.position = pos;
            
            //stones.Add(go);
        }
       
        // go.transform.position = pos + offset;
        //go.name = "blackStoneAt(" + pos.x + "," + pos.z + ")";
    }
    void Start()
    {
        camera = GetComponent<Camera>();

        transform.LookAt(new Vector3(-45.07155f, 1.056732f, 3.455505f));
        stones = new List<GameObject>();
        drawStone(camera.transform.position);
        //black = (GameObject)Instantiate(Resources.Load("blackStone"));
        // white = (GameObject)Instantiate(Resources.Load("whiteStone"));
    }
    // Update is called once per frame
    void Update () {
        //(Screen.height /4)
        Debug.Log("camera: " + camera.transform.position.ToString());
        drawStone(camera.transform.position);
        transform.Translate(Vector3.back * 0.01f); 
       
        
    }
}
