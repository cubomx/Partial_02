using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class HandleIntruments : MonoBehaviour
{
    public GameObject instrumentSelect;
    public GameObject songSelect;
    public GameObject resumeQuitMenu;
    public GameObject handler;
    public GameObject menuHandler;
    public GameObject textMessage;
    public GameObject canvasSelection, canvasButtons;
    public GameObject instrumentBtnConfirm, songBtnConfirm; 
    public List<string> guitarPianoSongs, drumsSongs;
    public Sprite escapeIcon, menuIcon;

    public GameObject menuImage;
    private Chords chords;

    private TMP_Dropdown instrumentDrop, songDrop;

    private TMP_Dropdown actualDrop;
    private Button actualButton;

    private bool dropSelected = true;
    private bool instrumentSelection = true;
    

    // Start is called before the first frame update
    void Start()
    {
        chords = handler.GetComponent<Chords>();
        instrumentDrop = instrumentSelect.GetComponent<TMP_Dropdown>();
        songDrop = songSelect.GetComponent<TMP_Dropdown>();
        changeActualDropAndButton( );
    }

    /* Get the selected instrument in the DropDown and change to the
        selection of the song.
    */

   
    /* The elements to move around with the controller */
    void changeActualDropAndButton( ){
        actualDrop = instrumentSelection ? instrumentDrop : songDrop;
        actualButton = instrumentSelection 
        ? instrumentBtnConfirm.GetComponent<Button>() 
        : songBtnConfirm.GetComponent<Button>();
        actualDrop.Select( );
    }

    /* Get the selected instrument by the user in the dropdown */
   public void getSelectedInstrument( ){
       string value = instrumentDrop.options[instrumentDrop.value].text;
       songDrop.ClearOptions();
       if (value == "Guitar" || value == "Piano")
           songDrop.AddOptions(guitarPianoSongs);
       else
            songDrop.AddOptions(drumsSongs);

       chords.instrument = value.ToLower();
       changeOpenMenuIcon( chords.instrument );
        ChangeInitialUI( false );
        songSelect.SetActive(true);
        songBtnConfirm.SetActive(true);

        instrumentSelection = false;
        changeActualDropAndButton( );
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
       instrumentSelection = isRestart;
       changeActualDropAndButton( );
       handler.GetComponent<Chords>().sequenceIndex = 0;
       handler.GetComponent<Chords>().finishedSong = false;
       handler.GetComponent<Chords>().setInstruction = isRestart;
       
       if ( handler.GetComponent<Chords>().GetComponent<Combo>() ){
           handler.GetComponent<Chords>().GetComponent<Combo>().isCreated = !isRestart;
       }
       
       textMessage.SetActive( isRestart );
       songSelect.SetActive(!isRestart);
       songBtnConfirm.SetActive(!isRestart);
       canvasSelection.SetActive( isRestart );
       resumeQuitMenu.SetActive( !isRestart );
       StartCoroutine( waitForMessage( isRestart ) );
   }

   public void changeOpenMenuIcon(string instrument){
       if (instrument == "piano"){
           menuImage.GetComponent<Image>().sprite = escapeIcon;
       }
       else{
           menuImage.GetComponent<Image>().sprite = menuIcon;
       }
    }


    /* Changing the UI. Appear or dissapear objects and handle the appearing of the
        song finish message.
    */
   IEnumerator waitForMessage(bool showMessage){
       
       yield return new WaitForSeconds( showMessage ? 3.0f : 0.0f);
       
       handler.SetActive(!showMessage);
       if ( !showMessage ){
           menuHandler.GetComponent<Menu>().DestroyPanel( );
           handler.GetComponent<Chords>().restartInstruments( );
           handler.GetComponent<Chords>().initializeValues( );
       }
        if ( chords.instrument != "drums" || showMessage ){
           menuHandler.SetActive(!showMessage);
       }
       textMessage.SetActive( false );
       canvasButtons.SetActive(!showMessage);
       ChangeInitialUI( showMessage );
   }
}
