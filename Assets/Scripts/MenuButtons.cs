using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    public GameObject panelCombo, panelMenu;
    public GameObject handleInstruments;

    public GameObject handler;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKeyDown(KeyCode.Escape)){
            changeMenus( );
        }
    }

    public void changeMenus( ){
        panelCombo.SetActive( !panelCombo.activeInHierarchy );
        panelMenu.SetActive( !panelMenu.activeInHierarchy );
    }

    public void Restart( ){
        changeMenus( );
        handler.GetComponent<Chords>().audioSource.Pause( );
        GameObject.Find("HandleInsturments").GetComponent<HandleIntruments>().RestartEverything( true );
    }
}
