using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShieldUIHandler : MonoBehaviour
{
    public Sprite[] shieldSprites;
    public Image shieldImage;
    public TextMeshProUGUI shieldText;

    public void UpdateShieldUI(StatusManager sm)
    {
        int value = sm.ShieldHp;
        shieldImage.sprite = shieldSprites[Mathf.Clamp(value,0, shieldSprites.Length-1)];
        shieldText.text = ""+value;
    }
}
