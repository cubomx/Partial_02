using System.Collections;
using System.Collections.Generic;
using UnityEngine;  
using UnityEngine.Networking;
using SimpleJSON;
using System.IO;
public class Chords : MonoBehaviour
{
    // Start is called before the first frame update
    public TextAsset text, drumsClips;
    public List<string> nameSongs;

    public TextAsset comboFile;
    public Combo combos;
    public AudioSource audioSource;
    public AudioClip myClip;

    private string id = "scapiobjid3";
    private string _class = "scales_chords_api";
    private string chord = "D#m(maj9)";
    public string instrument = "piano";
    private string output = "sound";
    private string chordLink = "";
    private string songsData = "";
    private  Dictionary<string, List<string>> chordSequence;
    private List<Chord> chords;
    private Dictionary<string, DrumChord> drumChords;

    public int sequenceIndex;
    public  bool isReady, finishedSong;

    public string nameSong;

    public bool setInstruction = false;
    public GameObject guitar, piano, drums;

    private Dictionary<string, AudioClip> clips;

    private bool drumColored;
    private DrumManager drumManager;
    KeyCode [] codes;

    void Start()
    {
        if ( instrument == "guitar" )
            guitar.SetActive( true );
        else if ( instrument == "piano" )
            piano.SetActive( true );
        else
            drums.SetActive( true );

        gameObject.AddComponent<Combo>( );
        combos = gameObject.GetComponent<Combo>( );
        combos.createCombos( comboFile );
        sequenceIndex = 0;
        finishedSong = false;
        chords = new List<Chord>( );
        clips = new Dictionary<string, AudioClip>( );
        chordSequence = new Dictionary<string, List<string>>();
        audioSource = GetComponent<AudioSource>( );
        drumChords = new Dictionary<string, DrumChord>( );
        readChordSequenceFile( );
        if (instrument == "guitar")  codes = combos.KeyCodes;
        else codes = combos.KeyBoardKeyCodes;
    }
    

    // Update is called once per frame
    void Update()
    {
        if (!finishedSong ){
            if ( instrument == "drums")
                drumPlaying();

        else if ( combos.isCreated && !setInstruction ){
                combos.actualCombo = combos.comboSystems[chordSequence[nameSong][sequenceIndex]];
                isReady = true;
            }

            else if ( !isReady && setInstruction ){
                for (int index = 0; index < codes.Length; index++) {
                    KeyCode kcode = codes[index];
                    if (Input.GetKeyDown(kcode))
                        sequenceCombo( kcode );
                }
            }
        }
        else{
            combos.isCreated = true;
            setInstruction = false;
            GameObject.Find("HandleInsturments").GetComponent<HandleIntruments>().RestartEverything( true );
        }
            
    }
    /* Tell the objects that are part of the actual combo to be of a different color */
    void colorDrum(string nameGameObject ){
        GameObject drumGO = GameObject.Find(nameGameObject);
        drumGO.GetComponent<DrumManager>().changeMaterials( true );
        drumGO.GetComponent<DrumManager>().wasTouched = false;
    }

    /* Detect the touches on the Drums and if the user touches a part of the drums
        that it requires to play in that moment.
    */
    void drumPlaying( ){
        if (!drumColored){
            /* Colored it the drums of the actual sequence */
            for (int i = 0; i < drumChords[nameSong].chords[sequenceIndex].Count; i++){
                colorDrum(drumChords[nameSong].chords[sequenceIndex][i]);
            }
            drumChords[nameSong].leftTouch = drumChords[nameSong].chords[sequenceIndex].Count;
            drumColored = true;
        }

        foreach (Touch touch in Input.touches)
        {
            Ray raycast = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit) && touch.phase == TouchPhase.Began){
                GameObject hitGO = raycastHit.transform.gameObject;
                DrumManager drumManager = hitGO.GetComponent<DrumManager>();
                if ( hitGO.tag != "Untagged"){
                    /* Detect if the part touched is on the sequence */
                    bool itPlayed = hitGO.GetComponent<DrumManager>().PlayClip( );
                    if ( itPlayed ){
                        drumChords[nameSong].leftTouch--;
                        if (drumChords[nameSong].leftTouch == 0){
                            sequenceIndex++;
                            drumColored = false;
                            if ( sequenceIndex == drumChords[nameSong].chords.Count ){
                                Debug.Log("Finished song");
                                finishedSong = true;
                            }
                        }
                    } 
                }
            }
        }
    }

    /* Check the kcode pressed in relation to the actual combo */
    void sequenceCombo(KeyCode kcode ){
        if ( combos.checkCombo( kcode, combos.actualCombo, instrument )) {
            // if the combo is done, play the corresponding clip
            audioSource.clip = clips[ combos.actualCombo.name ];
            audioSource.Play( );
            Debug.Log( "Done combo");
            sequenceIndex++;
            if ( sequenceIndex +1 >= chordSequence[nameSong].Count ){
                Debug.Log( "Finished song");
                finishedSong = true;
            }
            else{
                combos.actualCombo = combos.comboSystems[chordSequence[nameSong][sequenceIndex]];
                setInstruction = false;
                isReady = true;
            }
            
        }
    }

    /* Get the .mp3 file from the API, depends on the chord and the instrument */
    IEnumerator GetAudioClip(int index )
    {
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(chordLink, AudioType.MPEG)){
            yield return www.SendWebRequest();
 
            if (www.isNetworkError){
                Debug.Log(www.error);
            }
            else{
                myClip = DownloadHandlerAudioClip.GetContent(www);
                clips.Add(chords[index].chord, myClip);
                if ( index + 1 == chords.Count )
                    StartCoroutine( wait ( ) );
            }
        }
    }

    /* Get the link from the API according to the instrument and chord needed s*/
    IEnumerator GetChord(int index)
    {
            Debug.Log(chords[index].chord +  instrument );
            WWWForm form = new WWWForm();
            form.AddField("id", id);
            form.AddField("class", _class);
            form.AddField("chord", chords[index].chord);
            form.AddField("instrument", instrument);
            form.AddField("output", output);


            UnityWebRequest www = UnityWebRequest.Post("https://www.scales-chords.com/api/scapi.1.3.php", form);
            yield return www.SendWebRequest( );

            if (www.result != UnityWebRequest.Result.Success)
                Debug.Log(www.error);
            else{
                string response = www.downloadHandler.text;
                string[] values = splitString("<source src=\"", response);
                /* Manipulate the string to get the URL for the .mp3 file */
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

    /* Get the sequence of the drums (it is different from the piano/guitar
        because it can play many parts of the drums)
    */
    public List<List<string>> GetDrumsChord(int idxSong, JSONNode song ){
        List<List<string>> chordsDrum = new List<List<string>>( );
        for (int i = 0; i < song[idxSong]["sequence"].Count; i++ ){
            List<string> newChord = new List<string>();
            for (int j = 0; j < song[idxSong]["sequence"][i].Count; j++){
                string word = splitString("\"", song[idxSong]["sequence"][i][j])[0];
                 newChord.Add(word);
            }
            chordsDrum.Add(newChord);
        }
        return chordsDrum;
    }

    /* Get the whole sequence of the songs (only guitar/piano)*/
    public void getChordsSequence(int idxSong, JSONNode song){
        List<string> songChordsSequence =  new List<string>();
        for (int i = 0; i < song[idxSong]["sequence"].Count; i++ ){
                songChordsSequence.Add(splitString("\"", song[idxSong]["sequence"][i])[0]);
            }
            chordSequence.Add(song[idxSong]["name"], songChordsSequence);
    }

    /* Get all the song sequence from the JSON file*/
    public void readChordSequenceFile ( ){
        Dictionary<string, string> alreadyFoundChords = new Dictionary<string, string>( );
        songsData =  text.text ;

        JSONNode data = JSON.Parse( songsData );
        JSONNode song = data["songs"];
        // Get every song sequence
        for(int idxSong = 0; idxSong < song.Count; idxSong++){
            
            if (song[idxSong]["instrument"] == "drums")
                drumChords.Add(song[idxSong]["name"], new DrumChord(GetDrumsChord( idxSong, song ))); 
            else
                getChordsSequence(idxSong, song);
            
            for (int i = 0; i < song[idxSong]["chords"].Count; i++){
                string newChord = splitString("\"", song[idxSong]["chords"][i])[0];
                /* Save all the chords to be downloaded or loaded */
                if ( !alreadyFoundChords.ContainsKey( newChord ) ){
                    chords.Add( new Chord(newChord, song[idxSong]["instrument"]) );
                    alreadyFoundChords.Add(newChord, newChord);
                }
            } 
        }
        GetAllChords( );
    }

    /* Get all the clips needed to reproduce the chords*/
    public void GetAllChords ( ){
        GetDrumsClips();
        for (int i = 0; i < chords.Count; i++){
            if ( chords[i].instrument != "drums")
                StartCoroutine( GetChord (i) );
        }
    }

    /* Get the drums clips that are on the Resources folder */
    public void GetDrumsClips( ){
        JSONNode data = JSON.Parse( drumsClips.text)["chords"];
        for (int i = 0; i < data.Count; i++){
            clips.Add(data[i]["name"], Resources.Load<AudioClip>(data[i]["path"]));
        }
    }

    IEnumerator wait ( ){
        yield return new  WaitForSeconds(1.0f);
    }

}


public class Chord {
    public string chord;
    public string instrument;
    public Chord (string chord, string instrument){
        this.chord = chord;
        this.instrument = instrument;
    }
}

public class DrumChord {
    public List<List<string>> chords;
    public int leftTouch = 0;
    public DrumChord(List<List<string>> chords){
        this.chords = chords;
    }
}
