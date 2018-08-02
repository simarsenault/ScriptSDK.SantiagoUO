namespace ScriptSDK.SantiagoUO.Utilities.SkillGainTracker
{
    public class DiscordSkillChangeEventHandler : ISkillChangeEventHandler
    {
        public void Handle(SkillChangeEvent skillChangeEvent)
        {
            DiscordChannelSender.SendMessage("[Skill gain] " + skillChangeEvent.Skill.Value + " of " + skillChangeEvent.Player.Name + " " + (skillChangeEvent.OldValue < skillChangeEvent.NewValue ? "increased" : "decreased") + " from " + skillChangeEvent.OldValue + " to " + skillChangeEvent.NewValue);
        }
    }
}
