namespace ScriptSDK.SantiagoUO.Utilities.SkillGainTracker
{
    public interface ISkillChangeEventHandler
    {
        void Handle(SkillChangeEvent skillChangeEvent);
    }
}
