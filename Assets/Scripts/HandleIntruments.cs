﻿using System.Collections;
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
    public GameObject textMessage;
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

    /* Get the selected instrument in the DropDown and change to the
        selection of the song.
    */
   public void getSelectedInstrument( ){
       string value = instrumentDrop.options[instrumentDrop.value].text;
       songDrop.ClearOptions();
       if (value == "Guitar" || value == "Piano")
           songDrop.AddOptions(guitarPianoSongs);
       else
            songDrop.AddOptions(drumsSongs);

       chords.instrument = value.ToLower();
        ChangeInitialUI( false );
        songSelect.SetActive(true);
        songBtnConfirm.SetActive(true);
   }

   /* Get the selected song in the dropdown and starting the gameplay */
   public void getSong( ){
       string nameSong = songDrop.options[songDrop.value].text;
       chords.nameSong = nameSong;
       RestartEverything( false );
        songBtnConfirm.SetActive(false);
   }
    /* Change the DropDown and the Button required */
    public void ChangeInitialUI (bool isRestart ){
        instrumentSelect.SetActive(isRestart);
        instrumentBtnConfirm.SetActive(isRestart);
        songSelect.SetActive(!isRestart);
        songBtnConfirm.SetActive(!isRestart);
    }

    /* Change the scene UI depending if is the start or not */

   public void RestartEverything(bool isRestart ){
       textMessage.SetActive( isRestart );
       songSelect.SetActive(!isRestart);
       songBtnConfirm.SetActive(!isRestart);
       canvasSelection.SetActive( isRestart );
       StartCoroutine( waitForMessage( isRestart ) );
   }


    /* Changing the UI. Appear or dissapear objects and handle the appearing of the
        song finish message.
    */
   IEnumerator waitForMessage(bool showMessage){
       yield return new WaitForSeconds( showMessage ? 3.0f : 0.0f);
       handler.SetActive(!showMessage);
        if ( chords.instrument != "drums" || showMessage ){
           menuHandler.SetActive(!showMessage);
       }
       textMessage.SetActive( false );
       canvasButtons.SetActive(!showMessage);
       ChangeInitialUI( showMessage );
   }
}
