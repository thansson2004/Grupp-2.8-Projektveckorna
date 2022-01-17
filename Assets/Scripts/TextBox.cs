using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBox : MonoBehaviour
{
    [SerializeField] List<string> lines;
         
        public List<string> Lines {
        get { return lines; }
    }
}
