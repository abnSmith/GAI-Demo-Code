using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml;
using UnityEngine;

public class XMLLoader 
{
    // _fileName = the name of the XML file including extension.
    
    static public bool Load(string _fileName, out XmlDocument _xmlDoc)
    {
        string tempStr = "xml/" + _fileName;
        //attempts to load the requisit xml file
        TextAsset txtXmlAsset = Resources.Load(tempStr) as TextAsset;

        _xmlDoc = new XmlDocument();
        if (txtXmlAsset != null)
        {   //if the TextAsset was succesfully loaded, get its text
            _xmlDoc.LoadXml(txtXmlAsset.text);
        }
        else
        {   //else give error
            Debug.LogError("XMLLoader :: Load() - Failed to load: " + tempStr);
        }
        return _xmlDoc.InnerXml.Length != 0;
    }
}