using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using narrenschlag.extension;

namespace narrenschlag.dialoguez
{
    [CustomEditor(typeof(DialogueZBase))]
    public class DialogueZBaseEditor : Editor
    {
        public const float w = 65f;

        DialogueZBase db;

        Color old;
        Color red;
        Color gold;
        Color green;

        bool auto_saved, auto_loaded;
        private void OnEnable()
        {
            db = target as DialogueZBase;
            old = GUI.backgroundColor;

            red = Color.red.GetButtonColor();
            gold = Color.yellow.GetButtonColor();
            green = Color.green.GetButtonColor();

            // Auto Load if auto_save is true
            if(db.auto_save) Load(true);
        }

        #region dictionaries
        Dictionary<int, int> idi_indx;
        Dictionary<int, int> indx_idi;
        Dictionary<int, string> idi_ids;
        Dictionary<string, int> ids_idi;

        List<string> ids = new List<string>();
        string[] idz;

        //
        // Does what it says it does
        void UpdateDictionaries()
        {
            db.GetDictionaries(out idi_indx, out indx_idi, out idi_ids, out ids_idi);

            ids = ids_idi.Keys.ToList();
            idz = ids.ToArray();
        }
        #endregion

        public override void OnInspectorGUI()
        {
            if (db.elements == null) db.elements = new DialogueZElement[0];

            Fe.h_begin();
            GUILayout.Label("Auto Save/Load", EditorStyles.boldLabel);
            if (Fe.DrawButton(db.auto_save ? "active" : "inactive", db.auto_save ? green : Color.black, old)) db.auto_save = !db.auto_save;
            Fe.h_end();

            Fe.h_begin();
            if (Fe.DrawButton("Open", Color.gray, old)) foreach (DialogueZElement e in db.elements) e.eie = true;
            if (Fe.DrawButton("Close", Color.gray, old)) foreach (DialogueZElement e in db.elements) e.eie = false;
            if (Fe.DrawButton("Sort>ABC", Color.white, old)) Sort_Name();
            if (Fe.DrawButton("Sort>123", Color.white, old)) Sort_Id();
            if (Fe.DrawButton("Reload", Fe.GetButtonColor(Color.magenta), old, 50f) || reload_wip) Reload();
            Fe.h_end();

            UpdateDictionaries();
            foreach (DialogueZElement e in db.elements)
                if (!DrawElement(e)) return;

            #region add element
            if (Fe.DrawButton("Add Element", gold, old))
            {
                int highest = db.HighestIdi();
                db.elements = db.elements.Add(new DialogueZElement()
                {
                    ids = "dialogue_" + (highest + 1),
                    idi = highest + 1,

                    title = "Narrator",
                    message = "Type your message here!",
                    color_type = Color.green,

                    follows = new DialogueZFollow[0],
                    auto_next_idi = highest
                });
            }
            #endregion

            // *Not integrated yet
            Fe.h_begin();
            if (Fe.DrawButton("Save as txt", Color.cyan, old)) Save();

            if (Fe.DrawButton("Load from txt", Color.green, old)) Load();
            Fe.h_end();

            EditorUtility.SetDirty(target);
        }

        void Save(bool is_auto = false)
        {
            if (is_auto)
                if (auto_saved) return;
                else auto_saved = true;

            Debug.Log("Saving " + db);
            db.SaveAsTxt();
        }

        void Load(bool is_auto = false)
        {
            if (is_auto)
                if (auto_loaded) return;
                else auto_loaded = true;

            Debug.Log("Loading " + db);
            DialogueZElement[] elements;
            if (db.LoadFromTxt(out elements)) db.elements = elements;
        }

        #region Assistant Functions
        bool reload_wip;
        List<int> reload_indx_active = new List<int>();
        public void Reload()
        {
            if (!reload_wip)
            {
                int i = 0;
                reload_indx_active.Clear();
                foreach (DialogueZElement e in db.elements)
                {
                    if (e.eie) reload_indx_active.Add(i);
                    e.eie = true;
                    i++;
                }

                reload_wip = true;
            }
            else
            {
                int i = 0;
                foreach (DialogueZElement e in db.elements)
                {
                    e.eie = reload_indx_active.Contains(i);
                    i++;
                }

                reload_wip = false;
            }
        }

        public void Sort_Name()
        {
            Dictionary<string, DialogueZElement> dic = new Dictionary<string, DialogueZElement>();
            foreach (DialogueZElement e in db.elements) dic.Add(e.ids, e);

            List<string> idss = dic.Keys.ToList();
            idss.Sort();

            List<DialogueZElement> elements = new List<DialogueZElement>();
            foreach (string i in idss) elements.Add(dic[i]);
            db.elements = elements.ToArray();
        }

        public void Sort_Id()
        {
            Dictionary<int, DialogueZElement> dic = new Dictionary<int, DialogueZElement>();
            foreach (DialogueZElement e in db.elements) dic.Add(e.idi, e);

            List<int> idis = dic.Keys.ToList();
            idis.Sort();

            List<DialogueZElement> elements = new List<DialogueZElement>();
            foreach (int i in idis) elements.Add(dic[i]);
            db.elements = elements.ToArray();
        }
        #endregion

        #region draw calls
        //
        // Draw Element in the inspector
        bool DrawElement(DialogueZElement e)
        {
            // Button and exposed in editor check
            Fe.h_begin();
            Color ctype = Fe.GetButtonColor(e.color_type);
            Fe.DrawButton("", ctype, old, 10f);
            if (Fe.DrawButton(e.idi + ": " + e.ids + " -> " + e.follows.Length, e.eie ? Color.black : old, old)) e.eie = !e.eie;
            Fe.DrawButton("", ctype, old, 10f);
            Fe.h_end();
            if (!e.eie) return true;

            // Message to check if reloading
            // if (reload_wip) Debug.Log("Reloading: " + e.idi + ":" + e.ids);

            #region Element Data
            Fe.section_begin(13f);

            #region filter color and delete button
            Fe.h_begin();
            GUILayout.Label("Type Color", GUILayout.Width(w));
            Color c = EditorGUILayout.ColorField(e.color_type);
            c.a = 1;
            e.color_type = c;

            if (Fe.DrawButton("Delete Element", red, old))
            {
                db.elements = db.elements.Remove(e);
                return false;
            }
            Fe.h_end();
            #endregion

            #region idi and ids
            Fe.h_begin();
            GUILayout.Label("Id string", GUILayout.Width(w));
            string old_ids = e.ids;
            e.ids = EditorGUILayout.TextField(e.ids);
            if (old_ids != e.ids) UpdateDictionaries(); // Update dictionaries on change of id

            GUILayout.Label(e.idi.ToString(), EditorStyles.boldLabel, GUILayout.Width(35f));
            Fe.h_end();
            #endregion

            Fe.space(10f);
            #region title, message and audio
            Fe.h_begin();
            GUILayout.Label("Title", GUILayout.Width(w));
            e.title = EditorGUILayout.TextField(e.title);
            Fe.h_end();

            Fe.h_begin();
            GUILayout.Label("Message", GUILayout.Width(w));
            e.message = EditorGUILayout.TextArea(e.message, GUILayout.MinHeight(25f));
            Fe.h_end();

            Fe.h_begin();
            GUILayout.Label("Audio", GUILayout.Width(w));
            e.audio = EditorGUILayout.ObjectField(e.audio, typeof(AudioClip), false) as AudioClip;
            Fe.h_end();
            #endregion

            Fe.space(10f);
            #region follow ups and auto next
            if (db.elements.Length > 1)
            {
                bool has_follows = e.follows != null && e.follows.Length > 0;
                if (has_follows)
                {
                    GUILayout.Label("Follows", EditorStyles.boldLabel);
                    Fe.section_begin();
                    foreach (DialogueZFollow f in e.follows)
                        if (!DrawFollow(e, f)) return true;
                    Fe.section_end();
                }
                else
                {
                    Fe.h_begin();
                    GUILayout.Label("Following", GUILayout.Width(w));
                    e.auto_next_idi = IdiSelection(e.auto_next_idi, e.idi); // -> is set to itself when the target is not existing
                    Fe.h_end();
                }

                #region add follow/choice
                if (Fe.DrawButton("Add Choice", green, old))
                {
                    e.follows = e.follows.Add(new DialogueZFollow()
                    {
                        next_idi = e.idi,
                        message = "Choice " + e.follows.Length
                    });

                    return true;
                }
                #endregion
            }
            #endregion

            Fe.section_end();
            #endregion

            return true;
        }

        bool DrawFollow(DialogueZElement e, DialogueZFollow f)
        {
            Fe.h_begin();
            f.next_idi = IdiSelection(f.next_idi, f.next_idi, w);
            if (Fe.DrawButton("?", f.er ? Color.black : Fe.GetButtonColor(Color.cyan), old, 25f)) f.er = !f.er;
            f.message = EditorGUILayout.TextField(f.message);
            if (Fe.DrawButton("X", red, old, 25f))
            {
                e.follows = e.follows.Remove(f);
                return false;
            }

            Fe.h_end();

            if (f.er)
            {
                Fe.h_begin();
                GUILayout.Label("Requirements:", GUILayout.Width(85f));
                f.requ = EditorGUILayout.TextArea(f.requ, GUILayout.MinHeight(25f));
                Fe.h_end();
            }

            return true;
        }

        int IdiSelection(int idi, int alternative = -1, float width = -1)
        {
            int GetAlternative() { return alternative < 0 || !idi_indx.ContainsKey(alternative) ? db.elements.Length - 1 : idi_indx[alternative]; }

            // Set y to alternative int if the old element does not exist anymore
            int y = idi_indx.ContainsKey(idi) ? idi_indx[idi] : GetAlternative();

            if (width < 0) y = EditorGUILayout.Popup(y, idz);
            else y = EditorGUILayout.Popup(y, idz, GUILayout.Width(width));

            return indx_idi[y];
        }
        #endregion

        public void OnDestroy()
        {
            // Auto Save if auto_save is true
            if (db.auto_save) Save(true);
        }
    }
}
