using UnityEngine;

public class ElfChoirManager : MinigameManager
{
    [Header("Elf Choir References")]
    [SerializeField] private Transform choirParent;
    //[SerializeField] private AudioSource elfChoir; // Som do coral

    public override void PerfectHit()
    {
        base.PerfectHit();
        
        // Anima todos os elfos com intensidade máxima
        AnimateAllElves(1f);
        

    }

    public override void EarlyHit()
    {
        base.EarlyHit();
        AnimateAllElves(0.5f);
    }

    public override void LateHit()
    {
        base.LateHit();
        AnimateAllElves(0.5f);
    }

    public override void NoteMissed()
    {
        base.NoteMissed();
        AnimateAllElves(0.1f);
    }

    private void AnimateAllElves(float intensity)
    {
        if (choirParent == null) return;

        foreach (Transform elfTransform in choirParent)
        {
            ElfSinger elf = elfTransform.GetComponent<ElfSinger>();
            if (elf != null)
            {
                elf.Sing(intensity);
            }
        }
    }
}