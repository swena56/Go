
using System.Collections.Generic;
using UnityEngine;


public class AnimatedBackdrop : MonoBehaviour {

    GameObject randomStones;
    static int numberOfStones = 100;
    List<GameObject> stones;
   
    // Use this for initialization
    void Start () {
          randomStones = (GameObject)Instantiate(Resources.Load("randomStones"));

        /*
        stones = new List<GameObject>();

        for (int i = 0; i < numberOfStones; i++)
        {
            System.Random random = new System.Random();

           // int randomNumber = random.Next(0, 10);
            Vector3 position = new Vector3(Random.Range(-10.0F, 8.0F), Random.Range(16F, 20.0F), Random.Range(1.0F, 12.0F)) + new Vector3(-842, 6, -516.1f);

            if (i < (numberOfStones/2))
            {
                GameObject t = (GameObject)Instantiate(Resources.Load("whiteStone"), new Vector3(-842, 6, -516.1f), new Quaternion(0, 0, 0, 0));
               t.AddComponent<Rigidbody>();
                t.transform.position = position;
            }
            else
            {
                GameObject t = (GameObject)Instantiate(Resources.Load("blackStone"), new Vector3(-842, 6, -516.1f), new Quaternion(0, 0, 0, 0));
               t.AddComponent<Rigidbody>();
                t.transform.position = position;
            }


        }
        */


       


    }

    void createRandomNewStone()
    {

    }
   
    // Update is called once per frame
    void Update () {
      
        Debug.Log(randomStones.transform.position);
        /*
       if (randomStones.transform.position.y <= 0)
       {
            Destroy(randomStones);
            randomStones = (GameObject)Instantiate(Resources.Load("randomStones"));
        }
        */

	}
}
