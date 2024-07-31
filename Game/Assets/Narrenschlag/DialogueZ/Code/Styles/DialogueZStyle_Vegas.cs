using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace narrenschlag.dialoguez
{
    public class DialogueZStyle_Vegas : DialogueZStyle
    {
    public Text title;

        public override void DrawMessage(DialogueZElement e)
        {
            title.text = e.title;
            base.DrawMessage(e);
        }

        public override void SetParts_Message(bool b)
        {
            message.transform.parent.gameObject.SetActive(b);

            base.SetParts_Message(b);
        }
        public override void SetParts_Choice(bool b)
        {
            froot.parent.gameObject.SetActive(b);

            base.SetParts_Choice(b);
        }
    }
}