using UnityEngine;

public class CandyPainterManager : MinigameManager
{
    [Header("Candy Painter References")]
    [SerializeField] private CandyCaneBeatScroller beatScroller;
    
    protected override void Start()
    {
        base.Start();
    }

}