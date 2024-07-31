using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using UnityEngine;

using narrenschlag.extension;

namespace narrenschlag.dialoguez
{ 
    public class DialogueZ : MonoBehaviour
    {
        public DialogueZBase db;
        public Event[] events;

        #region Dictionaries
        public Dictionary<string, int> ids_idi = new Dictionary<string, int>();
        public Dictionary<int, DialogueZElement> idi_element = new Dictionary<int, DialogueZElement>();
        bool SetupDictionaries()
        {
            if (!db)
            {
                Debug.LogError("No Database assigned to " + this);
                return false;
            }

            // Clear the dictionaries
            ids_idi.Clear();
            idi_element.Clear();

            // Assign values to them
            if (events != null)
                foreach (DialogueZElement e in db.elements)
                {
                    // Add idi -> elements
                    if (!idi_element.ContainsKey(e.idi)) idi_element.Add(e.idi, e);
                    // Add ids -> idi
                    string ids = e.ids.Trim();
                    if (!ids_idi.ContainsKey(ids)) ids_idi.Add(ids, e.idi);
                }

            return true;
        }
        #endregion

        public static void Init(DialogueZElement e, DialogueZBase db = null) { DialogueZStyle.singleton.Init(e, db); }

        //
        // Use this to add Databases to characters 
        // and initiate those databases from the characters
        public static void SetDatabase(DialogueZBase db)
        {
            DialogueZ s = singleton;

            // Return if there is no singleton
            if (!singleton) return;

            // Return if Database is null or same
            if (db == null || db == s.db) return;

            s.db = db;
            s.SetupDictionaries();
        }

        public static bool TryGetDialogue_Ids(string ids, out DialogueZElement res)
        {
            DialogueZ s = singleton;
            res = null;

            // Return if there is no singleton
            if (!singleton) return false;

            if (s.ids_idi.Count < 1)
                if (!s.SetupDictionaries()) return false;

            ids = ids.Trim();
            if (s.ids_idi.ContainsKey(ids)) res = s.idi_element[s.ids_idi[ids]];

            return res != null;
        }

        public static bool TryGetDialogue_Idi(int idi, out DialogueZElement res)
        {
            DialogueZ s = singleton;
            res = null;

            // Return if there is no singleton
            if (!singleton) return false;

            if (s.idi_element.Count < 1)
                if (!s.SetupDictionaries()) return false;

            if (s.idi_element.ContainsKey(idi)) res = s.idi_element[idi];

            return res != null;
        }

        public static bool TryGetEvent(string ids, out Event res)
        {
            DialogueZ s = singleton;
            res = null;

            // Return if there is no singleton
            if (!singleton) return false;

            if (s.events != null)
                foreach (Event e in s.events)
                {
                    if (e.ids != ids) continue;

                    res = e;
                    break;
                }

            return res != null;
        }

        #region internal functions
        public int ElementLength() { return events != null ? events.Length : 0; }

        public int HighestIdi()
        {
            if (ElementLength() < 1) return 0;

            int h = 0;
            foreach (Event e in events)
                if (e.idi > h) h = e.idi;
            return h;
        }
        #endregion

        public void Print(string s) { Debug.Log(s); }

        #region singleton
        public static DialogueZ singleton;
        private void Awake()
        {
            singleton = this;
        }
        #endregion
    }

    [System.Serializable]
    public class Event
    {
        public string ids;
        public int idi;

        public Color c = Color.green;
        public bool eie;

        public UnityEvent onCast;
    } 
}