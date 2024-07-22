using TMPro;
using UnityEngine;

public class CharEquipZone : MonoBehaviour
{
    [SerializeField] protected int myCharIndex;
    [SerializeField] protected TextMeshProUGUI charName;
    [SerializeField] protected TextMeshProUGUI stat_0, stat_1, stat_2;
    private CharacterData cash;

    private void OnEnable()
    {
        if (cash == null)
            cash = CharacterData.GetSGT();

        RefreshText();
    }

    public void RefreshText()
    {
        GameCharactor cm = cash.GetCharData(myCharIndex);

        charName.text = "Hp : " + cm.hp.ToString("0.00") + " / " + cm.maxHp.ToString("0.00");
        stat_0.text = cm.ability.ToString("0.00") + " ";
        stat_1.text = cm.resist.ToString("0.00") + " ";
        stat_2.text = cm.speed.ToString("0.00") + " ";
    }
}
