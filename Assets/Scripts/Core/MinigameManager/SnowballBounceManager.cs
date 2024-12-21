using UnityEngine;

public class SnowballBounceManager : MinigameManager
{
    [Header("Snowball Bounce References")]
    [SerializeField] private SnowmanThrower snowman;
    
    public override void NoteSpawned()
    {
        base.NoteSpawned();
        
        if (snowman != null)
        {
            snowman.ThrowAnimation();
        }
    }


}