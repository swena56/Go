
using GameSlyce;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ListManager : MonoBehaviour
    {
   

    public Scrollbar scrollbarV;
        //public GameObject loadingPanel;
        public Sprite[] stateSprites;
   

        List<GameObject> gsList = new List<GameObject>();
        // List containers that list Items - (Dynamically Increasing ListView <Custom>)
        public GameObject GS_ListContainer;
        //Prefabs that holds items that will be places in the containers.
        
        public GameObject gsItemPrefab;
       
        void Start()
        {
            gsList.Clear();
            SetScrollBarVisibility(false);
        }


       
        
        //Method to add item to the custom invitable dynamically scrollable list
        public void AddItem(GameObject i)
        {
            GameObject item = Instantiate(i) as GameObject;
            item.transform.SetParent(GS_ListContainer.transform, false);
            gsList.Add(item);

            //if (gsList.Count > 3)
            //{
            //    SetScrollBarVisibility(true);
            //}
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

   
}
