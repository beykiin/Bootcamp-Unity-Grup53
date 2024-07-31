using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace narrenschlag.dialoguez
{
    public class DialogueZStyle_Dan : DialogueZStyle
    {
        public Text title;

        public override void SetParts_Message(bool b)
        {
            base.SetParts_Message(b);

            // Title stuff
            title.gameObject.SetActive(b);
            title.transform.parent.gameObject.SetActive(b); // Sets the title scaler
        }

        public override void DrawMessage(DialogueZElement e)
        {
            bool b;
            title.text = Modify(e.title, message_always_up, out b);
            base.DrawMessage(e);
        }
    }
}