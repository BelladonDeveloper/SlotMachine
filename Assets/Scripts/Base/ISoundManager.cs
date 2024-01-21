namespace Base
{
    public interface ISoundManager : IManager
    {
        void PlaySound(SoundName soundName);
    }
    
    public enum SoundName
    {
        None,
        TurnHandle,
        ReelSpin,
        RewardFlying,
    }
}