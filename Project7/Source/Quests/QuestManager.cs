using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Project7.Source.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project7.Source.Particles
{
    public class QuestManager
    {
        public Panel panelQuests;
        public List<Quest> Quests;

        int plan_clear_quests_in = -1;

        public QuestManager()
        {
            Quests = new List<Quest>();

            panelQuests = new Panel(new Vector2(500, 95), PanelSkin.Simple, Anchor.TopLeft, new Vector2(10, 10));
            panelQuests.Draggable = true;
            panelQuests.AddChild(new Header("Quests"));
            panelQuests.AddChild(new HorizontalLine());
            UserInterface.Active.AddEntity(panelQuests);
        }
        public void AddQuest(string text, Action<Quest> conditionChecker)
        {
            var para = new RichParagraph(text);
            Quests.Add(new Quest(conditionChecker, para));
            panelQuests.AddChild(para);
            panelQuests.Size = new Vector2(panelQuests.Size.X, panelQuests.Size.Y + 35);
            plan_clear_quests_in = -1;
            panelQuests.Visible = true;
        }
        public void Update()
        {
            Quests.ForEach(quest => quest.Update());
            if (plan_clear_quests_in == -1 && Quests.All(q => q.Success))
                plan_clear_quests_in = 300;
            if (Quests.Count > 0 && Quests.All(q => q.Success) && plan_clear_quests_in == 0)
            {
                Quests.Clear();
                plan_clear_quests_in = -1;
            }
            else if (plan_clear_quests_in > 0)
                plan_clear_quests_in--;
            panelQuests.Visible = Quests.Count > 0;
        }
    }
}
