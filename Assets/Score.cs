using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Score : MonoBehaviour
{

    public string name;
    public float score;
    public string _id;
    public string username;


    public Score(string name, float score, string _id, string username)
    {
        this.name = name;
        this.score = score;
        this._id = _id;
        this.username = username;
    }
}
