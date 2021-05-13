using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;

public enum combo{
        A,
        B,
        X,
        Y
    }
public class Combo : MonoBehaviour
{
    public Dictionary<string, ComboSystem> comboSystems;
    public List<string> namesCombo;
    public ComboSystem actualCombo;
    public int index = 0;

    public bool isCreated;

    // Start is called before the first frame update
    void Start() {

    }

    public KeyCode []  KeyCodes =  new KeyCode [] {
        KeyCode.JoystickButton0,
        KeyCode.JoystickButton1,
        KeyCode.JoystickButton2,
        KeyCode.JoystickButton3,
        KeyCode.JoystickButton4,
        KeyCode.JoystickButton5,
        KeyCode.JoystickButton6,
        KeyCode.JoystickButton7,
        KeyCode.JoystickButton8,
        KeyCode.JoystickButton9,
        KeyCode.JoystickButton10,
        KeyCode.JoystickButton11,
        KeyCode.JoystickButton12,
        KeyCode.JoystickButton13,
        KeyCode.JoystickButton14,
        KeyCode.JoystickButton15,
        KeyCode.JoystickButton16,
        KeyCode.JoystickButton17,
        KeyCode.JoystickButton18,
        KeyCode.JoystickButton19
    };

    // Update is called once per frame
    void Update( ) {
        if ( isCreated ){
            
        }

            /*for (int index = 0; index < KeyCodes.Length; index++) {
                KeyCode kcode = KeyCodes[index];
                if (Input.GetKeyDown(kcode)) {
                     Checking all combos 
                           checkCombo( kcode, actualCombo); 
                }
            }*/
        
      
        }

    combo getComboValue ( string value ) {
        switch (value){
            case "X":
                return combo.X;
            case "Y":
                return combo.Y;
            case "A":
                return combo.A;
            case "B":
                return combo.B;
            default:
                return combo.A;

        }
    }

    public void createCombos ( TextAsset comboFile, string instrument ){
        comboSystems = new Dictionary<string, ComboSystem>( ); 
        namesCombo = new List<string>( );
        string chordData = comboFile.text;
        JSONNode data = JSON.Parse( chordData )[instrument];
        
        for (int i = 0; i < data.Count; i++ ){
            ComboSystem comboSys = new ComboSystem( );
            List<combo>comboSequence = new List<combo>();
            JSONNode comboData =data[i];
            for(int comboIdx = 0; comboIdx < comboData["combo"].Count; comboIdx++){
                string chordSeq =  splitString("\"", comboData["combo"][comboIdx])[0];
                comboSequence.Add( getComboValue( chordSeq ) );
            }
            comboSys.name = comboData["chord"] ;
            comboSys.combo = comboSequence;
            comboSystems.Add( comboData["chord"], comboSys );
        }
        
        isCreated = true;
    }

    /* Checking each combo, sending to the object to look at */
    public bool checkCombo( KeyCode kcode, ComboSystem comboSys ) {
        bool activateCombo = comboSys.check( kcode.ToString( ) );
                if (activateCombo){
                    return true;
                }
                return false;
    }
    
    /* Restart the index of all the combos */
    void RestartCombos ( ) {
        for( int idx = 0; idx < namesCombo.Count; idx++)
            comboSystems[namesCombo[idx]].index = 0;
    }

     public string[] splitString(string needle, string haystack) {
        return haystack.Split(new string[] {needle}, System.StringSplitOptions.RemoveEmptyEntries);
    }
    
}

public class ComboSystem {
    public List <combo> combo;
    private Hashtable controls = new Hashtable( );
    public string name;
    public int index = 0;

    public ComboSystem( ) {
        controls.Add("A", "JoystickButton0");
        controls.Add("B", "JoystickButton1");
        controls.Add("X", "JoystickButton2");
        controls.Add("Y", "JoystickButton3");
    }

    /* Check with the comboList if the sequence is going the correct order */
    public bool check (string _keyCode) {
        if (_keyCode == controls[ combo[index].ToString( ) ].ToString( ) ){
            index += 1;
            if (index == combo.Count){
                index = 0;
                return true;   
            }
        }
        else
            index = 0;
        return false;
    }
}
