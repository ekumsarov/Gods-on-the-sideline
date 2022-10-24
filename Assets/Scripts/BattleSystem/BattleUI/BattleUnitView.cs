using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleEngine;
using DG.Tweening;

public class BattleUnitView : UIItem
{
    #region Fields

    [SerializeField] private BarController _healthBar;
    [SerializeField] private UIImage _icon;
    [SerializeField] private List<BattleEffectItem> _effects;
    private iBattleUnit _bindedUnit;

    #endregion

    public override void Setting()
    {
        base.Setting();

        Sequence seq = DOTween.Sequence();
        seq.Pause();
        seq.Append(UIM.ColorAnimationTo(this._icon, Color.red, 0.5f));
        seq.Append(UIM.ColorAnimationTo(this._icon, this._icon.Color, 0.5f));
        seq.OnComplete(AnimationComplete);
        this._icon.AddAnimation(seq, "Damage");

        Sequence seq2 = DOTween.Sequence();
        seq2.Pause();
        seq2.Append(UIM.ColorAnimationTo(this._icon, Color.green, 0.5f));
        seq2.Append(UIM.ColorAnimationTo(this._icon, this._icon.Color, 0.5f));
        seq2.OnComplete(AnimationComplete);
        this._icon.AddAnimation(seq2, "Heal");

        for(int i = 0; i < this._effects.Count; i++)
        {
            this._effects[i].Visible = false;
        }

        this._icon.Color = Color.white;
    }

    public void BindUnit(iBattleUnit unit)
    {
        _bindedUnit = unit;
        _bindedUnit.View = this;

        _icon.Image = unit.Icon;
        _healthBar.UpdateBar(unit.CurrentHP, unit.HP);
        _tooltipText = unit.Name;
        TooltipActive = true;

        _bindedUnit.EffectSystem.SetupEffectViews(this._effects);
    }

    public void PlayHit()
    {
        this._icon.PlayAnimation("Damage");
    }

    public void PlayHeal()
    {
        this._icon.PlayAnimation("Heal");
    }

    public override void AnimationComplete()
    {
        base.AnimationComplete();
        _bindedUnit.CompleteAnimation();
        _healthBar.UpdateBar(_bindedUnit.CurrentHP, _bindedUnit.HP);
    }

    public void UpdateItem()
    {
        _bindedUnit.EffectSystem.SetupEffectViews(this._effects);
    }

    public override void Selected(bool enter)
    {
        base.Selected(enter);

        if(enter)
        {
            BattleSystem.PointedUnit(this._bindedUnit);
            this._bindedUnit.ShowInfo();
        }
        else
        {
            BattleSystem.PointedUnit(null);
            this._bindedUnit.CloseInfo();
        }
    }

    public override void Pressed()
    {
        BattleSystem.OnUnitPoint(this._bindedUnit);
    }
}
