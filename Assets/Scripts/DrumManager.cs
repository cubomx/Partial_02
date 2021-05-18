using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrumManager : MonoBehaviour
{
    public AudioClip audioClip;
    AudioSource audioSource;
    public Material normal, press;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        normal = GetComponent<Renderer>().material;
    }
    // Update is called once per frame
    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            Ray raycast = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit raycastHit;
            Debug.Log(Physics.Raycast(raycast, out raycastHit));
            if (Physics.Raycast(raycast, out raycastHit) && touch.phase == TouchPhase.Began)
            {
                GameObject gameObject = raycastHit.transform.gameObject;
                string GameObjectHitName = raycastHit.transform.gameObject.name;
                if (GameObjectHitName.Equals(gameObject.name))
                {
                    gameObject.GetComponent<Renderer>().material = press;
                    StartCoroutine(restartColor(gameObject));
                    audioSource.Play();
                }
            }
        }

    }

    IEnumerator restartColor (GameObject gameObject ){
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<Renderer>().material = normal;
    }
}
