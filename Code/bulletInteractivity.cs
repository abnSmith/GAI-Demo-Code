using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bulletInteractivity : MonoBehaviour
{
    //TEST COMMENT
    [HideInInspector]
    public bool hasInterctivity = true;
    private GameObject secondPU;
    private List<string> expandableSlides = new List<string>()
    {
        "INTEGRATED LOGISTICS SUPPORT",
        "FIELD INSTALLATION AND LAB MANAGEMENT"
    };
        



    // Start is called before the first frame update
    public void Awake()
    {
        secondPU = GameObject.Find("Main Canvas").transform.GetChild(4).gameObject;
    }
    public void DoThing()
    {
        //if this slide is a slide that had embedded bullets 
        if (expandableSlides.Contains(this.transform.parent.parent.parent.GetChild(0).GetComponent<Text>().text))
        {
            Transform containerObj = this.gameObject.transform.parent;
            string blltLabel = this.transform.GetChild(1).GetChild(0).GetComponent<Text>().text;
            int allHeaders = containerObj.childCount;
            bool isActive;
            float refY = 0;
            for (int x = 0; x < allHeaders; x++)
            {
                Transform currHeader = containerObj.GetChild(x);
                isActive = (currHeader.GetChild(1).GetChild(0).GetComponent<Text>().text != blltLabel || this.transform.GetChild(3).gameObject.activeInHierarchy) ? false : true;
                int numChildren = currHeader.childCount;
                for (int y = 0; y < numChildren; y++)
                {
                    GameObject subBLLT = currHeader.GetChild(y).gameObject;
                    
                    if (subBLLT.name == "bullet(Clone)") toggleGO(subBLLT, isActive);
                }
                //adjusts position of list to accomodate if multiple embedded lists are visible
                if(x > 0)
                {
                    Transform prevChild = containerObj.GetChild(x - 1);
                    refY = (prevChild.GetChild(3).gameObject.activeInHierarchy)? prevChild.GetChild((prevChild.childCount - 1)).position.y : prevChild.position.y;
                    refY -= 0.075f;
                    containerObj.GetChild(x).transform.position = new Vector3(currHeader.position.x, refY, currHeader.position.z);
                }
                
            }
        }
       
    //sets subBLLT(embeded bullet to active)
    public void toggleGO(GameObject GO, bool b)
    {
        GO.SetActive(b);
    }
    //resets slide to default without expanded bullets
    public void resetSlide()
    {   float firstY = 0;
        if (expandableSlides.Contains(this.transform.parent.GetChild(0).GetComponent<Text>().text))
        {
            for(int i = 0; i < this.transform.childCount; i++)
            {
                firstY = (i == 0) ? this.transform.GetChild(0).position.y : firstY - 0.075f;
                for(int j = 0; j < this.transform.GetChild(i).childCount; j++)
                {
                    GameObject nOBJ = this.transform.GetChild(i).GetChild(j).gameObject;
                    if (nOBJ.name == "bullet(Clone)") toggleGO(nOBJ, false);
                }
                this.transform.GetChild(i).position = new Vector3(this.transform.GetChild(i).position.x, firstY, this.transform.GetChild(i).position.z);
            }

        }
    }
}
