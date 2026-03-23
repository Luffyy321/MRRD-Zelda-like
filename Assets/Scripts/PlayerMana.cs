using UnityEngine;
using UnityEngine.UI;

public class PlayerMana : MonoBehaviour
{
    public float maxMana = 8;
    public float currentMana;

    public Image manaBarImage;
    public Sprite[] manaSprites;

    void Start()
    {
        currentMana = 0;
        UpdateManaBar();
    }

    public void AddMana(int amount)
    {
        currentMana += amount;

        if (currentMana > maxMana)
            currentMana = maxMana;

        UpdateManaBar();
    }

    public bool UseMana()
    {
        if (currentMana >= maxMana)
        {
            currentMana = 0;
            UpdateManaBar();
            return true;
        }

        return false;
    }

    public void UpdateManaBar()
    {
        currentMana = Mathf.Clamp(currentMana, 0f, maxMana);

        int spriteIndex = Mathf.RoundToInt(currentMana);

        spriteIndex = Mathf.Clamp(spriteIndex, 0, manaSprites.Length - 1);

        manaBarImage.sprite = manaSprites[spriteIndex];
    }
}