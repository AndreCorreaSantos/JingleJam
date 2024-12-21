using UnityEngine;

public class ElfChoirManager : MinigameManager
{
    [Header("Elf Choir References")]
    [SerializeField] private Transform choirParent;
    [SerializeField] private CandyCaneConductor candyCane;
    
    [Header("Maestro Settings")]
    [SerializeField] private float inputCooldown = 0.1f; // Previne inputs muito rápidos
    private float lastInputTime;

    public override void PerfectHit()
    {
        base.PerfectHit();
        AnimateAllElves(currentMultiplier);
    }

    public override void EarlyHit()
    {
        base.EarlyHit();
        AnimateAllElves(currentMultiplier*0.75f);
    }

    public override void LateHit()
    {
        base.LateHit();
        AnimateAllElves(currentMultiplier*0.75f);
    }

    public override void NoteMissed()
    {
        base.NoteMissed();
        AnimateAllElves(0.1f);
    }

    protected override void Update()
    {
        base.Update();
        
        // Só processa inputs se passou o cooldown
        if (Time.time - lastInputTime < inputCooldown) return;

        Vector2 direction = Vector2.zero;

        // Checa inputs das setas
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction = Vector2.right;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = Vector2.left;
        }

        // Se houve input, inclina os elfos
        if (direction != Vector2.zero)
        {
            TiltAllElves(direction);
            if (candyCane != null)
            {
                candyCane.TiltToDirection(direction);
            }
            lastInputTime = Time.time;
        }
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

    private void TiltAllElves(Vector2 direction)
    {
        if (choirParent == null) return;

        foreach (Transform elfTransform in choirParent)
        {
            ElfSinger elf = elfTransform.GetComponent<ElfSinger>();
            if (elf != null)
            {
                elf.TiltToDirection(direction);
            }
        }
    }
}