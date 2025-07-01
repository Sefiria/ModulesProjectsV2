using GeonBit.UI.Entities;
using Microsoft.Xna.Framework.Graphics;
using System;
using Tools.Animations;

namespace Project7.Source.Entities
{
    public class Quest
    {
        Game1 Context => Game1.Instance;

        public Guid ID;
        public string Name;
        public bool Success = false;
        public RichParagraph TargetUI;
        public Action<Quest> ConditionChecker;

        public Quest(Action<Quest> ConditionChecker, RichParagraph TargetUI, string name = null)
        {
            ID = Guid.NewGuid();
            Name = name;
            this.TargetUI = TargetUI;
            this.ConditionChecker = ConditionChecker;
            Context.QuestManager.Quests.Add(this);
        }
        public void Update()
        {
            ConditionChecker?.Invoke(this);
        }
        public void SetText(string newText)
        {
            if(newText != null)
                TargetUI.Text = newText;
        }
        public void Validate(string newText = null)
        {
            if (Success)
                return;
            Success = true;
            SetText(newText);
            SFX.SFX.PlaySoundAsync(Game1.Instance.SE_QUEST_SUCCESS);
        }
    }
}
