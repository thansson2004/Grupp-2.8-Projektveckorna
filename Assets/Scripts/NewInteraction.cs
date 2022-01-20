using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



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
        if (Input.GetKeyDown(KeyCode.E) && Inrange)  // checkar så att man trycker på tangenten och är tillräckligt nära. Victor
        {
            if(InteractBox.activeInHierarchy)
            {
                InteractBox.SetActive(false);
            }
            else
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
            Debug.Log("japp");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Inrange = false;
            InteractBox.SetActive(false);
            Debug.Log("nähä du");
        }
    }
}
