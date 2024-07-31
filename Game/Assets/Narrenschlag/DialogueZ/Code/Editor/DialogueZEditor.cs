using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using narrenschlag.extension;

namespace narrenschlag.dialoguez
{
    [CustomEditor(typeof(DialogueZ))]
    public class DialogueZEditor : Editor
    {
        DialogueZ d;
        Color old;

        SerializedProperty events;
        private void OnEnable()
        {
            d = target as DialogueZ;
            old = GUI.backgroundColor;

            events = serializedObject.FindProperty("events");
        }

        public override void OnInspectorGUI()
        {
            Fe.h_begin();
            GUILayout.Label("Database", GUILayout.Width(DialogueZBaseEditor.w));
            d.db = EditorGUILayout.ObjectField(d.db, typeof(DialogueZBase), false) as DialogueZBase;
            Fe.h_end();

            Fe.space(10f);
            #region Draw Events
            GUILayout.Label("Callable Events", EditorStyles.boldLabel);
            Fe.section_begin();

            if (d.events == null) d.events = new Event[0];
            int i = 0;
            serializedObject.Update();
            foreach (Event e in d.events)
            {
                Fe.h_begin();
                Fe.DrawButton("", e.c, old, 10f);
                if (Fe.DrawButton(e.idi + ": " + e.ids, e.eie ? Color.black : old, old)) e.eie = !e.eie;
                if (Fe.DrawButton("X", Fe.GetButtonColor(Color.red), old, 25f))
                {
                    d.events = d.events.Remove(e);
                    continue;
                }
                Fe.h_end();

                if (!e.eie) continue;

                Fe.h_begin();
                GUILayout.Label("Id string", GUILayout.Width(DialogueZBaseEditor.w - 10f));
                e.ids = EditorGUILayout.TextField(e.ids).ToLower();
                GUILayout.Label(e.idi.ToString(), GUILayout.Width(30f));
                Fe.h_end();

                EditorGUILayout.PropertyField(events.GetArrayElementAtIndex(i).FindPropertyRelative("onCast"));

                i++;
            }
            serializedObject.ApplyModifiedProperties();
            Fe.section_end();

            if (Fe.DrawButton("Add Event", Fe.GetButtonColor(Color.yellow), old))
            {
                i = d.HighestIdi() + 1;
                d.events = d.events.Add(new Event()
                {
                    ids = "event_" + i,
                    idi = i,

                    c = Fe.GetButtonColor(new Color(Random.Range(0, .99f), Random.Range(0, .99f), Random.Range(0, .99f), 1))
                });
                return;
            }
            #endregion
        }
    }
}