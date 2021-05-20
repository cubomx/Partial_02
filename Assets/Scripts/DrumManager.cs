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

    /* If this part of the drum is present in the sequence of the chords, change its color */
    public void changeMaterials(bool colorIt ){
        Material [ ] materials = this.gameObject.GetComponent<Renderer>().materials;
        for (int i = 0; i < materials.Length; i++){
            materials[i] = ( colorIt ) ? press : initialMaterials[i] ;
        }
        this.gameObject.GetComponent<Renderer>().materials = materials;
    }

    /* Check if this object is the one that it is needed to play at this moment */
    public bool PlayClip( ){
        if ( !this.wasTouched ){
            changeMaterials( false );
            audioSource.Play( );
            this.wasTouched = true;
            return true;
        }
        return false;
    }

}
