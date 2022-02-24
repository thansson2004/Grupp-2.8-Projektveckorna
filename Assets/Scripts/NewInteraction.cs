using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Victor


public class NewInteraction : MonoBehaviour
{
    public GameObject InteractBox;
    public Text InteractText;
    public string Interaction;
    public bool Inrange;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Inrange)  // checkar så att man trycker på tangenten och är tillräckligt nära.
        {
            if(InteractBox.activeInHierarchy)
            {
                InteractBox.SetActive(false);
            }
            else                                                   //Stänger av boxen ifall den redan är aktiv, och aktiverar den som den inte är aktiv
            {          
                InteractBox.SetActive(true);
                InteractText.text = Interaction;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Inrange = true;
            Debug.Log("japp");     //Gör så att man kan aktivera textboxen
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Inrange = false;                           //Gör så att boxen försvinner om man går utanför objektets område
            InteractBox.SetActive(false);
            Debug.Log("nähä du");
        }
    }
}
