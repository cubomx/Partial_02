using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuButtons : MonoBehaviour
{
    public GameObject panelCombo, panelMenu;
    public GameObject handleInstruments;

    public GameObject resumeButton;
    public GameObject handler;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7)){
            changeMenus( );
        }
    }

    

    public void changeMenus( ){
        panelCombo.SetActive( !panelCombo.activeInHierarchy );
        panelMenu.SetActive( !panelMenu.activeInHierarchy );
        if ( panelMenu.activeInHierarchy){
            resumeButton.GetComponent<Button>().Select( );
        }
    }

    public void Restart( ){
        changeMenus( );
        handler.GetComponent<Chords>().stopChord( );
        GameObject.Find("HandleInsturments").GetComponent<HandleIntruments>().RestartEverything( true );
    }
}
