using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Menu : MonoBehaviour

{
    public GameObject handler;
    public GameObject generalPanel, individualButtonPanel;
    public GameObject aButton, bButton, xButton, yButton;
    public GameObject leftButton, rightButton, upButton, downButton;

    private GameObject newPanel;
    
    private Combo _Combo;
    private Chords _Chords;

    private Transform transform;

    private GameObject [] buttons ;
    // Start is called before the first frame update
    void Start()
    {
        buttons = new GameObject [3];
        _Chords =  handler.GetComponent<Chords>( );
        
        StartCoroutine(WaitToStart());
        
        transform = generalPanel.GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        if ( _Chords != null && _Combo != null && _Chords.isReady ){
            createInstructions ( );
        }
    }
    /* Getting the GO with the correspond image button */
    GameObject selectImage(string buttonName ){
        if ( buttonName == "A" ) return aButton;
        if ( buttonName == "B" ) return bButton;
        if ( buttonName == "X" ) return xButton;
        if ( buttonName == "Y" ) return yButton;

        if ( buttonName == "UP" ) return upButton;
        if ( buttonName == "DOWN" ) return downButton;
        if ( buttonName == "LEFT" ) return leftButton;
        if ( buttonName == "RIGHT" ) return rightButton;
        
        return null;
    }

    /* Change the title corresponding to the action that the 
        button sequence is related to.
    */
    void changeTextInstruction( GameObject panel){
            GameObject child = panel.transform.GetChild(0).gameObject;
            child.GetComponent<Text>().text = _Combo.actualCombo.name;
    }

    /* Create the panel that groups the image sequence of buttons */
    void createInstructions( ){
        if ( newPanel != null){
            Destroy (newPanel);
            for (int sequenceIdx = 0; sequenceIdx < buttons.Length; sequenceIdx++){
                Destroy( buttons[sequenceIdx] );
            }
        }
        Debug.Log( _Chords.instrument );
        List<combo> _ComboData = _Combo.actualCombo.combo[_Chords.instrument];
        
        newPanel = Instantiate ( individualButtonPanel, 
        new Vector3(transform.position.x,transform.position.y, transform.position.z), Quaternion.identity, transform.parent );
        Vector3 panelGroupPos = newPanel.transform.position;
        panelGroupPos += new Vector3(0.0f, 225.0f, 0.0f);
        newPanel.transform.position = panelGroupPos;
        changeTextInstruction( newPanel );
        for (int sequenceIdx = 0; sequenceIdx < _ComboData.Count; sequenceIdx++){
            
            GameObject imageBtn = selectImage( _ComboData[sequenceIdx].ToString( ) );
            Transform trans = newPanel.transform;
            buttons[sequenceIdx] = Instantiate( imageBtn, 
            new Vector3(trans.position.x, trans.position.y, trans.position.z ), Quaternion.identity, trans.parent);
            Vector3 imgPos = buttons[sequenceIdx] .transform.position;
            imgPos += new Vector3(-250.0f + 350.0f*sequenceIdx, 0.0f, 0.0f);
            buttons[sequenceIdx] .transform.position = imgPos;
        }
         _Chords.isReady = false;
         _Chords.setInstruction = true;
    }
    /* Wait a little bit to the other codes to have the combos ready */
    IEnumerator WaitToStart( ) {
        
        yield return new WaitForSeconds(1.0f);
        _Combo = _Chords.GetComponent<Combo>( );
        createInstructions( );
    }
}
