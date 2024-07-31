using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using narrenschlag.extension;
using UnityEditor;

namespace narrenschlag.dialoguez
{
    [CustomEditor(typeof(DialogueZStyle))]
    public class DialogueZStyleEditor : Editor
    {
        DialogueZStyle s;
        private void OnEnable()
        {
            s = target as DialogueZStyle;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}