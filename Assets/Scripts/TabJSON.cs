using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Songs
{
    public Song song;
}
[Serializable]
public class Song
{
    public string name;
    public string[] chords;
    public string[][] sequence;
}

public class TabJSON : MonoBehaviour
{
    public TextAsset jsonFile;
    private Dictionary<string, DrumManager> drums;
    private string[][] sequence;
    private int counter;
    // Start is called before the first frame update
    void Start()
    {
        drums = new Dictionary<string, DrumManager>();
        sequence = new string[100][];
        counter = 0;
        Songs songsJSON = JsonUtility.FromJson<Songs>(jsonFile.text);
        Song songJSON = songsJSON.song;
        foreach (string chord in songJSON.chords)
        {
            GameObject drum = null;
            try
            {
                drum = GameObject.FindGameObjectWithTag(chord);
                Debug.Log(gameObject.name);
            }
            catch (System.Exception)
            {
                if (chord == "MediumCrashCymbal")
                {
                    drum = GameObject.FindGameObjectWithTag("CrashCymbal");
                }
                else if (chord == "ClosedHiHat")
                {
                    drum = GameObject.FindGameObjectWithTag("HiHats");
                }
                else if (chord == "HandClap")
                {
                    drum = GameObject.FindGameObjectWithTag("MidTom");
                }
            }
            if (drum != null)
            {
                drums.Add(chord, drum.GetComponent<DrumManager>());
            }
        }
        sequence = songJSON.sequence;
        Debug.Log(sequence);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
