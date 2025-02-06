using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfNovelEngine
{
    public class ChoiceButton
    {
        public string Text { get; set; }  // Текст на кнопке
        public int StorylineID { get; set; }  // ID сюжетной линии

        public ChoiceButton(string text, int storylineID)
        {
            Text = text;
            StorylineID = storylineID;
        }
    }
}
