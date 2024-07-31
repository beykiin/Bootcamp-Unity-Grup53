using System.Collections.Generic;
using System.Collections;
using UnityEngine;

using narrenschlag.extension;
using UnityEditor;

namespace narrenschlag.dialoguez
{
    [CreateAssetMenu(fileName = "DialogueZ Base", menuName = "Narrenschlag/DialogueZ Base")]
    public class DialogueZBase : ScriptableObject
    {
        [SerializeField] public DialogueZElement[] elements;
        [SerializeField] public bool auto_save = false;

        #region internal functions
        public void GetDictionaries(out Dictionary<int, int> idi_indx, out Dictionary<int, int> indx_idi, out Dictionary<int, string> idi_ids, out Dictionary<string, int> ids_idi)
        {
            idi_ids = new Dictionary<int, string>();
            ids_idi = new Dictionary<string, int>();
            idi_indx = new Dictionary<int, int>();
            indx_idi = new Dictionary<int, int>();

            int indx = 0;
            if (elements != null)
                foreach (DialogueZElement e in elements)
                {
                    //idi_ids
                    idi_ids.Add(e.idi, e.ids);
                    //ids_idi
                    if (!ids_idi.ContainsKey(e.ids)) ids_idi.Add(e.ids, e.idi);
                    // idi_indx
                    idi_indx.Add(e.idi, indx);
                    // indx_idi
                    indx_idi.Add(indx, e.idi);

                    indx++;
                }

            return;
        }

        public int ElementLength() { return elements != null ? elements.Length : 0; }

        public int HighestIdi()
        {
            if (ElementLength() < 1) return 0;

            int h = 0;
            foreach (DialogueZElement e in elements)
                if (e.idi > h) h = e.idi;
            return h;
        }
        #endregion
    }

    [System.Serializable]
    public class DialogueZElement
    {
        [SerializeField] public string ids;                  // string   id
        [SerializeField] public int idi;                     // int      id

        [SerializeField] public Color color_type;            // color    type used for filtering
        [SerializeField] public bool eie;                    // exposed in editor

        [SerializeField] public string title;                // title of the guy that talks
        [SerializeField] public string message;              // message of the dialogue element
        [SerializeField] public AudioClip audio;             // audio played while text is writing

        [SerializeField] public DialogueZFollow[] follows;  // follow ups
        [SerializeField] public int auto_next_idi;           // follow up if follow ups empty
    }

    [System.Serializable]
    public class DialogueZFollow
    {
        [SerializeField] public int next_idi;                // idi that is loaded when 

        [SerializeField] public string message;              // message displayed

        [SerializeField] public string requ;                 // requirements
        [SerializeField] public bool er;                     // exposed requirements
    }
} 