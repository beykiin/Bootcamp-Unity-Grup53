using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace narrenschlag.dialoguez
{
    public class DialogueZFollowInstance : MonoBehaviour
    {
        public Text text;

        DialogueZStyle s;
        DialogueZFollow f;
        bool is_quit;

        public void Init(DialogueZFollow follow, DialogueZStyle style)
        {
            f = follow;
            s = style;

            if (text) text.text = style.Modify(follow.message, style.follow_always_up, out is_quit);
        }

        public void Select()
        {
            DialogueZElement e;
            if (is_quit) s.Quit();
            else if (DialogueZ.TryGetDialogue_Idi(f.next_idi, out e)) s.Init(e);
        }
    }
}