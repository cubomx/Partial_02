using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;

public enum combo{
        A,
        B,
        X,
        Y,
        LEFT, 
        RIGHT,
        DOWN, 
        UP
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

    public KeyCode []  KeyBoardKeyCodes =  new KeyCode [] {
        KeyCode.DownArrow,
        KeyCode.UpArrow,
        KeyCode.RightArrow,
        KeyCode.LeftArrow
    };

    // Update is called once per frame
    void Update( ) {
      
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
            case "DOWN":
                return combo.DOWN;
            case "UP":
                return combo.UP;
            case "LEFT":
                return combo.LEFT;
            case "RIGHT":
                return combo.RIGHT;
            default:
                return combo.A;

        }
    }

    public List<combo> obtainInstrumentChords( string instrument, JSONNode data ){
            JSONNode dataInstrument = data[instrument];
            List<combo>comboSequence = new List<combo>();
            for(int comboIdx = 0; comboIdx < dataInstrument.Count; comboIdx++){
                string chordSeq =  splitString("\"", dataInstrument[comboIdx])[0];
                comboSequence.Add( getComboValue( chordSeq ) );
            }
           
            return comboSequence; 
    }

    public void createCombos ( TextAsset comboFile ){
        comboSystems = new Dictionary<string, ComboSystem>( ); 
        namesCombo = new List<string>( );
        string chordData = comboFile.text;
        JSONNode data = JSON.Parse( chordData )["chords"];
        
        for (int i = 0; i < data.Count; i++ ){
            ComboSystem comboSys = new ComboSystem( );
            Dictionary<string, List<combo>> _Combos =  new Dictionary<string, List<combo>>();
            _Combos.Add("guitar", obtainInstrumentChords( "guitar", data[i]) ) ;
            _Combos.Add("piano", obtainInstrumentChords( "piano", data[i]) ) ;

            comboSys.combo = _Combos;
            comboSys.name = data[i]["chord"];

            comboSystems.Add( data[i]["chord"], comboSys );
        }
        
        isCreated = true;
    }

    /* Checking each combo, sending to the object to look at */
    public bool checkCombo( KeyCode kcode, ComboSystem comboSys, string insrument ) {
        bool activateCombo = comboSys.check( kcode.ToString( ), insrument );
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
    public Dictionary<string, List <combo>> combo;
    private Hashtable controls = new Hashtable( );
    public string name;
    public int index = 0;

    public ComboSystem( ) {
        controls.Add("A", "JoystickButton0");
        controls.Add("B", "JoystickButton1");
        controls.Add("X", "JoystickButton2");
        controls.Add("Y", "JoystickButton3");
        controls.Add("DOWN", "DownArrow");
        controls.Add("UP", "UpArrow");
        controls.Add("LEFT", "LeftArrow");
        controls.Add("RIGHT", "RightArrow");
    }

    /* Check with the comboList if the sequence is going the correct order */
    public bool check (string _keyCode, string instrument) {
        if (_keyCode == controls[ combo[instrument][index].ToString( ) ].ToString( ) ){
            index += 1;
            if (index == combo[instrument].Count){
                index = 0;
                return true;   
            }
        }
        else
            index = 0;
        return false;
    }
}
