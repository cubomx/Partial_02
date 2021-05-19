using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrumManager : MonoBehaviour
{
    public AudioClip audioClip;
    AudioSource audioSource;
    public Material press;
    public Material [] initialMaterials;
    public bool wasTouched;
    void Start()
    {
        wasTouched = true;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        initialMaterials = GetComponent<Renderer>().materials;
    }
    // Update is called once per frame
    void Update()
    {
        

    }

    public void changeMaterials(bool colorIt ){
        Material [ ] materials = this.gameObject.GetComponent<Renderer>().materials;
        for (int i = 0; i < materials.Length; i++){
            materials[i] = ( colorIt ) ? press : initialMaterials[i] ;
        }
        this.gameObject.GetComponent<Renderer>().materials = materials;
    }

    public bool PlayClip( ){
        if ( !this.wasTouched ){
            changeMaterials( false );
            audioSource.Play( );
            this.wasTouched = true;
            return true;
        }
        return false;
    }

    IEnumerator restartColor (GameObject gameObject ){
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<Renderer>().materials = initialMaterials;
    }
}
