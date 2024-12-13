using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class podiumTXTSetup : MonoBehaviour
{
    public Dictionary<string, PodiumInfo> podiums = new Dictionary<string, PodiumInfo>();
    //Class set up to be filled in from xml data
    public class PodiumInfo
    {
        public string pName;
        public List<string> bullets = new List<string>();
        public bool isExpandableList;
        public Dictionary<string, List<string>> expandedList = new Dictionary<string, List<string>>();
    }
    public void Awake()
    {
        XmlDocument xmlDoc;
        bool bSuccess = XMLLoader.Load("podiumTXT", out xmlDoc);
        if (bSuccess)
        {   //each page is a different podium
            XmlNodeList pages = xmlDoc.GetElementsByTagName("page");
            foreach (XmlNode page in pages)
            {
                PodiumInfo pInfo = new PodiumInfo();
                string pgTitle = page.Attributes["title"].Value;
                //if the xml has "hasSubHeaders" attribute which means the bullet has child bullets
                bool hasSubheaders = (page.Attributes["hasSubHeaders"].Value == "true") ? true : false;
                //get the number of bullets total in the xml
                int childCount = page.ChildNodes.Count;
                List<string> tmpBLLTList = new List<string>();
                Dictionary<string, List<string>> tmpDict = new Dictionary<string, List<string>>();
                for(int x = 0; x < childCount; x++)
                {   //"entry" denotes a bullet with embedded/child bullets 
                    if((hasSubheaders && page.ChildNodes[x].LocalName == "entry") || (!hasSubheaders && page.ChildNodes[x].LocalName == "bullet"))
                    {
                        //extra parsing that needs to be done for pages that have embedded bullets
                        if(hasSubheaders)
                        {
                            XmlNode entryNode = page.ChildNodes[x];
                            string header = entryNode.Attributes["title"].Value;
                            int actBulletCNT = entryNode.ChildNodes.Count;
                            List<string> tmpList = new List<string>();

                            for(int y = 0; y < actBulletCNT; y++)
                            {
                                string currBllt = entryNode.ChildNodes[y].Attributes["text"].Value;
                                tmpList.Add(currBllt);
                            }
                            tmpDict.Add(header, tmpList);

                        }
                        //grabs the title or the text depending on if the bullet is a parent/bullet that has child bullets
                        string bullTXT = (hasSubheaders)? page.ChildNodes[x].Attributes["title"].Value : page.ChildNodes[x].Attributes["text"].Value;
                        
                        tmpBLLTList.Add(bullTXT);
                    }

                }
                //filling in PodiumInfo object  attributes to grab when slide is created
                pInfo.pName = pgTitle;
                pInfo.bullets = tmpBLLTList;
                pInfo.isExpandableList = hasSubheaders;
                pInfo.expandedList = tmpDict;
                podiums.Add(pgTitle, pInfo);
            }
        }
        else
        {
            Debug.Log("Unable to load podiumTXT xml file");
        }
    }
    
}
