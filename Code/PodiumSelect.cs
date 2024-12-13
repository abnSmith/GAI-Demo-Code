using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class PodiumSelect : MonoBehaviour
{

    GameObject podiumIcon;
    static public GameObject podContent;
    [HideInInspector]
    public PlayerMovement CNA;
    private IEnumerator CNA_turnTo;
    //bool isSelected = false;
    private Text slide_Title;
    private Text slide_sectionHeader;
    private Text slideBullets;
    private static podiumTXTSetup pInformation;
    private RectTransform slideTitle_rect;
    private float slideTitle_width;
    public GameObject bulletObj;
    private Transform bulletCNTR;
    public Transform podTXTCNTR;
    private GameObject podiumCanvas;
    private List<GameObject> allPodiumNavLocations = new List<GameObject>();
    private Transform navTRANS;
    private List<string> podiumNames = new List<string>()
    {
        "Integrated Logistics Support",
        "Earned Value Management and Financial Analysis",
        "Engineering Design and Documentation",
        "System Integration, Testing, and Evaluation",
        "Software Development and Testing",
        "Field Installation and Lab Management"
    };
    private Dictionary<string, float> lowestPTs = new Dictionary<string, float>()
    {
        {"Integrated Logistics Support", -295f},
        {"Earned Value Management and Financial Analysis", -76f},
        {"Engineering Design and Documentation", -143f},
        {"System Integration, Testing, and Evaluation", -77f},
        {"Software Development and Testing", -117f},
        {"Field Installation and Lab Management", -426f}
    };
    private Transform navBTTNContainer;
    [SerializeField] private CanvasGroup myUIGRP;
    //public GameObject fadeOVRLY;
    [HideInInspector]
    public float ilsY = 0f;
    private GameObject scndryPopup;
    private GameObject scrollGO;
    private float defCNTRPos = -0.4330936f;
    private int defCNTRHeight = 700;
    public Sprite podiumImg;
    


    // Start is called before the first frame update
    public void Awake()
    {
        podiumCanvas = GameObject.Find("podiumCanvas").gameObject;
       
        podContent = podiumCanvas.transform.GetChild(0).gameObject;
        podTXTCNTR = podContent.transform.GetChild(2);
        slide_Title = podTXTCNTR.GetChild(0).GetComponent<Text>();
        slideTitle_rect = podTXTCNTR.GetChild(0).GetComponent<RectTransform>();
        slideTitle_width = slideTitle_rect.rect.width;
        slide_sectionHeader = podTXTCNTR.GetChild(1).GetComponent<Text>();
        navTRANS = GameObject.Find("Navigation").transform;
        pInformation = navTRANS.gameObject.GetComponent<podiumTXTSetup>();
        bulletCNTR = podTXTCNTR.GetChild(1).GetChild(0);
        //podiumCanvas.SetActive(false);
        CNA = GameObject.Find("Main Camera").GetComponent<PlayerMovement>();
        for(int x = 0; x < navTRANS.childCount; x++)
        {
            string currChildName = navTRANS.GetChild(x).gameObject.name;
            if (podiumNames.Contains(currChildName)) allPodiumNavLocations.Add(navTRANS.GetChild(x).gameObject);
           
        }
        navBTTNContainer = podiumCanvas.transform.GetChild(1);
        scndryPopup = GameObject.Find("Main Canvas").transform.GetChild(4).gameObject;
        scrollGO = podTXTCNTR.GetChild(1).gameObject;
        podiumIcon = podContent.transform.GetChild(1).gameObject;
        


    }

    //TURNS THE PODIUM CONTENT ON OR OFF DEPENDING ON THE BOOL
    public void Select(bool isSelected)
    {
        podiumCanvas.SetActive(isSelected);
        scrollGO.GetComponent<ScrollRect>().enabled = false;
        bool needsSroll = false;
        bulletCNTR.transform.localPosition = new Vector3(bulletCNTR.transform.localPosition.x, -defCNTRPos, bulletCNTR.transform.localPosition.z);//RESETS THE POSITION OF THE BLLT CONTAINER IF IT WAS MOVED DUE TO SCROLLING
        bulletCNTR.GetComponent<RectTransform>().sizeDelta = new Vector2(bulletCNTR.GetComponent<RectTransform>().sizeDelta.x, defCNTRHeight);//RESETS CONTAINER HEIGHT IF IT WAS CHANGED DUE TO SCROLLING
        //fadeOVRLY.SetActive(false);
        foreach (Transform child in bulletCNTR)
        {
            if(child.gameObject.name != "Scrollbar")Destroy(child.gameObject);
        }

        if (isSelected)
        {
            string currentPodium = this.transform.parent.gameObject.name;
            StartCoroutine(Fade());
            
            Vector3 podiumPOSFINAL = new Vector3();
            Vector3 podiumPOSINITIAL = new Vector3();
            Vector3 podiumROT = new Vector3();          
            //this is a worldspace canvas object that shares screenspace with objects that change size so the canvas pos is hard coded in per podium
            switch (currentPodium)
            {
                case "Integrated Logistics Support":
                    podiumPOSINITIAL = new Vector3(-13.525f, 0.451f, 7.404f);
                    podiumPOSFINAL = new Vector3(-13.499f, 0.453f, 7.381f);
                    podiumROT = new Vector3(2.762f, -40.051f, -0.814f);
                    break;
                case "Earned Value Management and Financial Analysis":
                    podiumPOSFINAL = new Vector3(-14.17f, 0.527f,6.415f);
                    podiumROT = new Vector3(2.839f, -56.612f, -0.188f);
                    break;
                case "Engineering Design and Documentation":
                    podiumPOSFINAL = new Vector3(-14.725f, 0.462f, 5.236f);
                    podiumROT = new Vector3(3.862f, -81.285f, 0.488f);
                    break;
                case "System Integration, Testing, and Evaluation":
                    podiumPOSFINAL = new Vector3(-14.822f, 0.466f, 4.073f);
                    podiumROT = new Vector3(10.043f, -107.117f, 0.707f);
                    break;
                case "Software Development and Testing":
                    podiumPOSFINAL = new Vector3(-14.354f, 0.393f, 2.791f);
                    podiumROT = new Vector3(6.731f, -135.19f,0.809f);
                    break;
                case "Field Installation and Lab Management":
                    podiumPOSFINAL = new Vector3(-13.268f, 0.428f, 1.883f);
                    podiumROT = new Vector3(6.875f, -160.989f, 0.841f);
                    break;
            }
            //changes canvas position based on switch above
            podiumCanvas.transform.localPosition = podiumPOSFINAL;
            podiumCanvas.transform.localRotation = Quaternion.Euler(podiumROT.x, podiumROT.y, podiumROT.z);

            slide_Title.text = pInformation.podiums[currentPodium].pName.ToUpper();
            List<string> tmpList = new List<string>();
            tmpList = pInformation.podiums[currentPodium].bullets;
            float prefWidth = slide_Title.preferredWidth;
            int z = 0;
            float blltSpacing = 0f;

            //creates each bullet based on pre-existing gameobject
            foreach(string LI in tmpList)
            {
                GameObject newLI = Instantiate(bulletObj, bulletCNTR);
                //gets the new GO transform
                Transform liTRNS =  newLI.transform;
                //if this is the first bullet the liY is a predetermined number vs one that needs to be lower on the y then bullets above it
                float liY = (z == 0)? liTRNS.localPosition.y :blltSpacing;
                //if liY is taller then the canvas, need to add scroll functionality
                if(liY < lowestPTs[pInformation.podiums[currentPodium].pName]) needsSroll = true;
                //move newLI to new y position
                liTRNS.localPosition = new Vector3(liTRNS.position.x, liY, liTRNS.localPosition.z);
                //get the text object in the newLI
                Text currTXTBox = liTRNS.GetChild(1).GetChild(0).GetComponent<Text>();
                //sets its text to the LI string
                currTXTBox.text = LI;
                //some math to determine of the bullet needs to wrap/ the next bullet needs to be lower because the previous bullet wrapped
                float bulletPrefWidth = currTXTBox.preferredHeight;
                float bulletActWidth = liTRNS.GetChild(1).GetComponent<RectTransform>().rect.height;                
                blltSpacing = (bulletPrefWidth > bulletActWidth)? (liTRNS.localPosition.y - (bulletPrefWidth / 2) - 35f) : (liTRNS.localPosition.y - 50f);
                //adjustment to bullet pos/spacing for styling child bullets
                if(pInformation.podiums[currentPodium].isExpandableList && (pInformation.podiums[currentPodium].expandedList.ContainsKey(LI)))
                {
                    List<string> tmpILSList = pInformation.podiums[currentPodium].expandedList[LI];
                    float currY = -45;
                    foreach (string bullet in tmpILSList)
                    {
                        GameObject nBullet = Instantiate(bulletObj, liTRNS);
                        nBullet.transform.localPosition = new Vector3(-122f, currY, nBullet.transform.localPosition.z);
                        nBullet.GetComponent<RectTransform>().sizeDelta = new Vector2(1275.68f, nBullet.GetComponent<RectTransform>().sizeDelta.y);
                        nBullet.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = bullet;
                        nBullet.transform.GetChild(1).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(1856, 80);
                        nBullet.transform.GetChild(1).GetChild(0).localPosition = new Vector3(356,0,0);
                        nBullet.SetActive(false);
                        currY -= 45f;
                       
                    }
                   
                }
                z++;
                
            }
        }
        else
        {
           CNA.exitPodiumLocation(true);
            scndryPopup.SetActive(false);
        }

        if (needsSroll || pInformation.podiums[this.transform.parent.gameObject.name].isExpandableList)enableUserScroll();//IF THIS CONTENT IS LOWER THEN THE ALLOWED HEIGHT OR CONTENT THAT UNSPOOLS ENABLE THE SCROLL
        podiumIcon.GetComponent<Image>().sprite = podiumImg;
    }

    public void enableUserScroll()
    {
        scrollGO.GetComponent<ScrollRect>().enabled = true;
        
    }
    IEnumerator Fade(/*Vector3 start, Vector3 end*/)
    {
       
            for (float cAlpha = 0f; cAlpha <= 1f; cAlpha += 0.1f)
            {
                myUIGRP.alpha = cAlpha;
                yield return new WaitForSeconds(.05f);
            }
 
    }
}
