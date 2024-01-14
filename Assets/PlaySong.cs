using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaySong : MonoBehaviour
{
    public GameObject MusicBox;
    public AudioSource source;

    private string SongName;

    public void OnClickButton()
    {
        Transform musicBoxTransform = MusicBox.transform;

        Transform[] allChildren = musicBoxTransform.GetComponentsInChildren<Transform>(true);

        if (allChildren.Length > 0)
        {
            Transform TextChild = allChildren[4];

            Text textComponentForName = TextChild.GetComponent<Text>();

            if (textComponentForName != null)
            {
                string Name = textComponentForName.text;
                Debug.Log(Name);

                SongName = Name;
                PredvajajMuziko();
            }
            else
            {
                Debug.Log("The last child Transform does not have a Text component.");
            }
        }
        else
        {
            Debug.Log("There are no child Transforms in the array.");
        }
    }


    void PredvajajMuziko()
    {

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
