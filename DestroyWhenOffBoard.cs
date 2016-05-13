using UnityEngine;
using System.Collections;

public class DestroyWhenOffBoard : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y >= 5)
        {
            Destroy(gameObject);
        }
    }
}
