using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

using narrenschlag.extension;
using UnityEditor;

namespace narrenschlag.dialoguez
{
    public class DialogueZScaler : MonoBehaviour
    {
        public DialogueZScaleType type;

        public bool scale_x, scale_y;
        public bool pos_x, pos_y;
        public Vector2 offset;

        public RectTransform get;
        public RectTransform apply;

        // Message
        public Text text;
        public Vector2 extra = new Vector2(25f, 25f);

        // Follow
        public LayoutGroup lg;
        public Vector2 pos_factor = new Vector2(.5f, .5f);

        private void Awake()
        {
            switch (type)
            {
                case DialogueZScaleType.title:
                    DialogueZStyle.onInit += InitTitle;
                    break;
                
                case DialogueZScaleType.message:
                    DialogueZStyle.onInit += InitMessage;
                    break;

                case DialogueZScaleType.follow:
                    DialogueZStyle.onFollowInit += InitFollow;
                    break;

                default:
                    break;
            }
        }

        void InitTitle(DialogueZStyle st, DialogueZElement e)
        {
            if (!apply || !text) return;

            #region Setup
            Vector2 start = apply.anchoredPosition;
            start.x += -apply.sizeDelta.x / 2;
            if (start_pos_assigned) start = start_pos;
            else
            {
                start_pos = start;
                start_pos_assigned = true;
            }
            #endregion

            float width = text.preferredWidth; // textGen.GetPreferredWidth(text.text, generationSettings);
            width += extra.x;

            // Debug.Log(width + ": " + text.text);

            apply.sizeDelta = new Vector2(width, apply.sizeDelta.y);
            apply.anchoredPosition = start_pos + new Vector2(width / 2, 0) + offset;
        }

        void InitMessage(DialogueZStyle st, DialogueZElement e)
        {
            if (!apply || !text) return;

            // Size
            TextGenerator textGen = new TextGenerator();
            TextGenerationSettings generationSettings = text.GetGenerationSettings(text.rectTransform.rect.size);

            bool irrelevant;
            Vector2 size = new Vector2(apply.rect.width, apply.rect.height);
            string s = st.Modify(e.message, st.message_always_up, out irrelevant, false);
            if (scale_x)
            {
                size.x = textGen.GetPreferredWidth(s, generationSettings);
                size.x += extra.x;
            }
            if (scale_y)
            {
                size.y = textGen.GetPreferredHeight(s, generationSettings);
                size.y += extra.y;
            }
            apply.sizeDelta = size;

            // Position
            // -> none... yet...
        }

        Vector2 start_pos;
        bool start_pos_assigned;
        void InitFollow(DialogueZStyle st, DialogueZElement e)
        {
            if ((!get && !apply) || !lg) return;

            if (!get) get = apply;
            else if (!apply) apply = get;

            int count;
            Vector2 children = get.GetChildSize(out count, true);

            // Apply position
            float x = children.x;
            x += lg.padding.left + lg.padding.right;

            float y = children.y;
            y += lg.padding.bottom + lg.padding.top;

            Vector2 size = new Vector2(apply.rect.width, get.rect.height);
            if (scale_x) size.x = x;
            if (scale_y) size.y = y;
            size += extra;
            apply.sizeDelta = size;

            // Apply position
            Vector2 pos = apply.transform.position;
            if (start_pos_assigned) pos = start_pos;
            else
            {
                start_pos = pos;
                start_pos_assigned = true;
            }

            if (pos_x) pos.x += offset.x + size.x * pos_factor.x;
            if (pos_y) pos.y += offset.y + size.y * pos_factor.y;
            pos += offset;

            apply.position = (Vector3)pos;
        }
    }

    public enum DialogueZScaleType { title, message, follow }

    [CustomEditor(typeof(DialogueZScaler))]
    public class DialogueZScalerEditor : Editor
    {
        DialogueZScaler ds;
        public void OnEnable() { ds = target as DialogueZScaler; }

        List<string> list = new List<string>();
        string[] x = new string[2] { "x_no", "x_yes" };
        string[] y = new string[2] { "y_no", "y_yes" };
        public override void OnInspectorGUI()
        {
            Fe.DrawButton("", Color.black, GUI.backgroundColor);

            Fe.h_begin();
            GUILayout.Label("Type", EditorStyles.boldLabel, GUILayout.Width(65f));

            list = new List<string>();
            foreach (DialogueZScaleType t in System.Enum.GetValues(typeof(DialogueZScaleType))) list.Add(t.ToString());
            ds.type = (DialogueZScaleType)EditorGUILayout.Popup((int)ds.type,list.ToArray());
            Fe.h_end();

            if (ds.type == DialogueZScaleType.follow) DrawFollow();
            else DrawMessage();

            Fe.space(10);
            Fe.h_begin();
            GUILayout.Label("Extras", GUILayout.Width(90f));
            ds.extra = EditorGUILayout.Vector2Field("", ds.extra);
            Fe.h_end();
        }

        void DrawMessage()
        {
            Fe.space(10);
            Fe.h_begin();
            GUILayout.Label("Apply to", GUILayout.Width(65f));
            ds.apply = EditorGUILayout.ObjectField(ds.apply, typeof(RectTransform), true) as RectTransform;
            Fe.h_end();

            if (ds.type != DialogueZScaleType.title)
            {
                Fe.space(10);
                GUILayout.Label("Apply", EditorStyles.boldLabel);

                Fe.h_begin();
                GUILayout.Label("Scale", GUILayout.Width(65f));
                ds.scale_x = EditorGUILayout.Popup(ds.scale_x.ToInt(), x).ToBool();
                ds.scale_y = EditorGUILayout.Popup(ds.scale_y.ToInt(), y).ToBool();
                Fe.h_end();
            }

            Fe.space(10);
            Fe.h_begin();
            GUILayout.Label("Text", GUILayout.Width(65f));
            ds.text = EditorGUILayout.ObjectField(ds.text, typeof(Text), true) as Text;
            Fe.h_end();

            if (ds.type != DialogueZScaleType.message)
            {
                Fe.h_begin();
                GUILayout.Label("Offset", GUILayout.Width(90f));
                ds.offset = EditorGUILayout.Vector2Field("", ds.offset);
                Fe.h_end();
            }
        }

        void DrawFollow()
        {
            Fe.space(10);
            Fe.h_begin();
            GUILayout.Label("Apply to", GUILayout.Width(65f));
            ds.apply = EditorGUILayout.ObjectField(ds.apply, typeof(RectTransform), true) as RectTransform;
            GUILayout.Label("<- Child", GUILayout.Width(55f));
            ds.get = EditorGUILayout.ObjectField(ds.get, typeof(RectTransform), true) as RectTransform;
            Fe.h_end();

            Fe.space(10);
            GUILayout.Label("Apply", EditorStyles.boldLabel);

            Fe.h_begin();
            GUILayout.Label("Scale", GUILayout.Width(65f));
            ds.scale_x = EditorGUILayout.Popup(ds.scale_x.ToInt(), x).ToBool();
            ds.scale_y = EditorGUILayout.Popup(ds.scale_y.ToInt(), y).ToBool();
            Fe.h_end();

            Fe.h_begin();
            GUILayout.Label("Position", GUILayout.Width(65f));
            ds.pos_x = EditorGUILayout.Popup(ds.pos_x.ToInt(), x).ToBool();
            ds.pos_y = EditorGUILayout.Popup(ds.pos_y.ToInt(), y).ToBool();
            Fe.h_end();

            Fe.space(10);
            Fe.h_begin();
            GUILayout.Label("Layout Group", GUILayout.Width(85f));
            ds.lg = EditorGUILayout.ObjectField(ds.lg, typeof(LayoutGroup), true) as LayoutGroup;
            Fe.h_end();

            Fe.h_begin();
            GUILayout.Label("Position Factor", GUILayout.Width(90f));
            ds.pos_factor = EditorGUILayout.Vector2Field("", ds.pos_factor);
            Fe.h_end();

            Fe.h_begin();
            GUILayout.Label("Offset", GUILayout.Width(90f));
            ds.offset = EditorGUILayout.Vector2Field("", ds.offset);
            Fe.h_end();
        }
    }
}