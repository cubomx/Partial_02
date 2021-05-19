using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class HandleIntruments : MonoBehaviour
{
    public GameObject instrumentSelect;
    public GameObject songSelect;

    public GameObject handler;
    public GameObject menuHandler;

    public GameObject canvasSelection, canvasButtons;
    public GameObject instrumentBtnConfirm, songBtnConfirm; 
    public List<string> guitarPianoSongs, drumsSongs;
    private Chords chords;

    private TMP_Dropdown instrumentDrop, songDrop;

    // Start is called before the first frame update
    void Start()
    {
        chords = handler.GetComponent<Chords>();
        instrumentDrop = instrumentSelect.GetComponent<TMP_Dropdown>();
        songDrop = songSelect.GetComponent<TMP_Dropdown>();
    }



    // Update is called once per frame
   public void getSelectedInstrument( ){
       string value = instrumentDrop.options[instrumentDrop.value].text;
       songDrop.ClearOptions();
       if (value == "Guitar" || value == "Piano")
           songDrop.AddOptions(guitarPianoSongs);
       else
            songDrop.AddOptions(drumsSongs);

       chords.instrument = value.ToLower();
        instrumentSelect.SetActive(false);
        songSelect.SetActive(true);
        instrumentBtnConfirm.SetActive(false);
        songBtnConfirm.SetActive(true);
   }

   public void getSong( ){
       string nameSong = songDrop.options[songDrop.value].text;
       chords.nameSong = nameSong;
       handler.SetActive(true);
       if ( chords.instrument != "drums"){
           menuHandler.SetActive(true);
       }
        
        canvasSelection.SetActive(false);
        canvasButtons.SetActive(true);
        songBtnConfirm.SetActive(false);
   }
}
