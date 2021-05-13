using System.Collections;
using System.Collections.Generic;
using UnityEngine;  
using UnityEngine.Networking;
using SimpleJSON;
using System.IO;
public class Chords : MonoBehaviour
{
    // Start is called before the first frame update
    public string response;
    public TextAsset text;
    public List<string> nameSongs;

    public TextAsset comboFile;
    public Combo combos;
    AudioSource audioSource;
    AudioClip myClip;

    private string id = "scapiobjid3";
    private string _class = "scales_chords_api";
    private string chord = "D#m(maj9)";
    public string instrument = "piano";
    private string output = "sound";

    private string chordLink = "";
    private string songsData = "";
    private  List<List<string>> chordSequence;

    private List<string> chords;

    private int sequenceIndex;
    private int songIndex;
    public  bool isReady;

    public bool setInstruction = false;
    public GameObject guitar, piano;

    private Dictionary<string, AudioClip> clips;


    void Start()
    {
        if ( instrument == "guitar" ){
            guitar.SetActive( true );
        }
        else if ( instrument == "piano" ){
            piano.SetActive( true );
        }
        gameObject.AddComponent<Combo>( );
        combos = gameObject.GetComponent<Combo>( );
        combos.createCombos( comboFile );
        sequenceIndex = 0;
        songIndex = 0;
        chords = new List<string>( );
        clips = new Dictionary<string, AudioClip>( );
        chordSequence = new List<List<string>>( );
        audioSource = GetComponent<AudioSource>( );
        readChordSequenceFile( );
    }

    // Update is called once per frame
    void Update()
    {

        if ( combos.isCreated && !setInstruction ){
            combos.actualCombo = combos.comboSystems[chordSequence[songIndex][sequenceIndex]];
            isReady = true;
        }

        if ( !isReady && setInstruction ){
            KeyCode [] codes;
            if (instrument == "guitar")  codes = combos.KeyCodes;
            else codes = combos.KeyBoardKeyCodes;
             
            for (int index = 0; index < codes.Length; index++) {
                KeyCode kcode = codes[index];
                if (Input.GetKeyDown(kcode)) {
                           if ( combos.checkCombo( kcode, combos.actualCombo, instrument )) {
                               audioSource.clip = clips[ combos.actualCombo.name];
                               audioSource.Play( );
                               Debug.Log( "Done combo");
                               sequenceIndex++;
                               if ( sequenceIndex +1 >= chordSequence[songIndex].Count ){
                                   Debug.Log( "Finished song");
                               }
                               else{
                                   combos.actualCombo = combos.comboSystems[chordSequence[songIndex][sequenceIndex]];
                                    setInstruction = false;
                                    isReady = true;
                               }
                               
                           }
                }
            }
        }
    }

    IEnumerator GetAudioClip(int index )
    {
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(chordLink, AudioType.MPEG))
        {
            yield return www.SendWebRequest();
 
            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                myClip = DownloadHandlerAudioClip.GetContent(www);
                clips.Add(chords[index], myClip);
                if ( index + 1 == chords.Count ){
                    StartCoroutine( wait ( ) );
                    
                }
            }
    
        }
        
    }

    IEnumerator GetChord(int index)
    {
            WWWForm form = new WWWForm();
            form.AddField("id", id);
            form.AddField("class", _class);
            form.AddField("chord", chords[index]);
            form.AddField("instrument", instrument);
            form.AddField("output", output);


            UnityWebRequest www = UnityWebRequest.Post("https://www.scales-chords.com/api/scapi.1.3.php", form);
            yield return www.SendWebRequest( );

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                response = www.downloadHandler.text;
                string[] values = splitString("<source src=\"", response);
                //displayArray(values);

                for (int i = 0; i < values.Length; i++ ){
                    if ( values[i].Contains("type") && values[i].Contains("mp3")){
                        chordLink = splitString("\" type", values[i])[0];
                        StartCoroutine( GetAudioClip(index ) );
                    }
                }
            
            }
        
    }
     

    public string[] splitString(string needle, string haystack) {
        return haystack.Split(new string[] {needle}, System.StringSplitOptions.RemoveEmptyEntries);
    }

    public void readChordSequenceFile ( ){
        Dictionary<string, string> alreadyFoundChords = new Dictionary<string, string>( );
        songsData =  text.text ;

        JSONNode data = JSON.Parse( songsData );
        JSONNode song = data["songs"];
        for(int idxSong = 0; idxSong < song.Count; idxSong++){
            List<string> songChordsSequence =  new List<string>();
            for (int i = 0; i < song[idxSong]["sequence"].Count; i++ ){
                songChordsSequence.Add(splitString("\"", song[idxSong]["sequence"][i])[0]);
            }
            chordSequence.Add( songChordsSequence );
            
            for (int i = 0; i < song[idxSong]["chords"].Count; i++){
                string newChord = splitString("\"", song[idxSong]["chords"][i])[0];

                if ( !alreadyFoundChords.ContainsKey( newChord ) ){
                    chords.Add( newChord );
                    alreadyFoundChords.Add(newChord, newChord);
                }
            }
            
        }
        
        
        GetAllChords( );
    }

    public void GetAllChords ( ){
        for (int i = 0; i < chords.Count; i++){
                StartCoroutine( GetChord (i) );
            
        }
    }

    IEnumerator wait ( ){
        yield return new  WaitForSeconds(1.0f);
    }

}
