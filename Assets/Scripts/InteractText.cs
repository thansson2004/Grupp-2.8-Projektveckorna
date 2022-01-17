using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractText : MonoBehaviour
{   
    [SerializeField] GameObject dialogbox;
    [SerializeField] Text dialogtext;
    [SerializeField] int letterspersecond;

    public void ShowDialog(TextBox dialog)
    {
        dialogbox.SetActive(true);
        dialogtext.text = dialog.Lines[0];
    }
    public IEnumerator TypeDialog(string dialog)
    {
        dialogtext.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            dialogtext.text += letter;
            yield return new WaitForSeconds(1f / letterspersecond);
        }
      
    }
}


 