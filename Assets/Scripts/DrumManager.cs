using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrumManager : MonoBehaviour
{
    public AudioClip audioClip;
    AudioSource audioSource;
    public Material press;
    public Material [] initialMaterials;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        initialMaterials = GetComponent<Renderer>().materials;
    }
    // Update is called once per frame
    void Update()
    {
        

    }

    public void changeMaterials( ){
        Material [ ] materials = this.gameObject.GetComponent<Renderer>().materials;
        for (int i = 0; i < materials.Length; i++){
            materials[i] = press;
        }
        this.gameObject.GetComponent<Renderer>().materials = materials;
    }

    IEnumerator restartColor (GameObject gameObject ){
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<Renderer>().materials = initialMaterials;
    }
}
