﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DialogueLoader {
    public static TwineStory loadStory(TextAsset jsonFile)
    {
        TwineStory story = JsonUtility.FromJson<TwineStory>(jsonFile.text);
        //Debug.Log(JsonUtility.ToJson(story));
        return story;
    }
}

[System.Serializable]
public class TwineStory : System.Object
{
    public Passage[] passages;
    public string name;
    public string startnode;
    public string creator;
    public string creator_version;
    public string ifid;
}

[System.Serializable]
public class Passage : System.Object
{
    public string text;
    public Link[] links;
    public string name;
    public int pid;
    public NodePosition position;

}

[System.Serializable]
public class Link : System.Object
{
    public string name;
    public string link;
    public int pid;
}

[System.Serializable]
public class NodePosition : System.Object
{
    public int x;
    public int y;
}


/*Link tempLink = new Link();
        tempLink.name = "linkname";
        tempLink.link = "linklink";
        tempLink.pid = 999;
        Passage temp = new Passage();
        temp.links = new Link[] { tempLink };
        temp.text = "passage text ";
        temp.name = "passage name";
        temp.pid = 999;
        temp.position = new NodePosition();
        temp.position.x = 1;
        temp.position.y = 1;
        TwineStory test = new TwineStory();
        test.creator = "creator";
        test.creator_version = "creator version";
        test.ifid = "0";
        test.name = "name";
        test.passages = new Passage[] { temp };
        test.startnode = "start node";
        //Debug.Log(JsonUtility.ToJson(test));*/
