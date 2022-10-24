using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEffectItem : UIItem
{

    [UnityEngine.SerializeField]
    UIImage ico;

    [SerializeField] SimpleText effectAmount;

    public override void Setting()
    {
        if (ico == null)
            ico = this.gameObject.GetComponentInChildren<UIImage>();

        ico.Image = "info";
        this.ItemTag = "BattleEffect";

        this.SelectedAction = true;

        base.Setting();
    }

    public void SetEffect(BattleEffects.BattleEffect _ac)
    {
        this.ico.Image = BattleEffects.BattleEffect.GetEffectIcon(_ac.EffectType.ToString());
        this.effectAmount.Text = _ac.AmountCount.ToString();
    }
}