using UnityEngine;
using UnityEngine.UI;

public class UIMana : MonoBehaviour
{
    public Slider manaSlider;

    public float maxMana = 100f;
    public float currentMana = 100f;

    void Start()
    {
        manaSlider.maxValue = maxMana;
        manaSlider.value = currentMana;
    }

    void Update()
    {
        RecoverManaOverTime();
    }

    void RecoverManaOverTime()
    {
        RecoverMana(10f * Time.deltaTime); // 10 mana mỗi giây
    }


    public void UseMana(float amount)
    {
        currentMana -= amount;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        manaSlider.value = currentMana;
    }

    public void RecoverMana(float amount)
    {
        currentMana += amount;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        manaSlider.value = currentMana;
    }


}
