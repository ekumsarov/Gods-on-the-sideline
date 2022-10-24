using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarController : UIImage
{
    [SerializeField] SimpleText _countText;
    [SerializeField] SimpleHealthBar _bar;
    [SerializeField] bool _showCount = true;

    public override void Setting()
    {
        base.Setting();

        if (_showCount)
            _countText.Visible = true;
        else
            _countText.Visible = false;
    }

    public void UpdateBar(int curValue, int maxValue)
    {
        _bar.UpdateBar(curValue, maxValue);

        if (_showCount)
            _countText.Text = string.Format("{0}/{1}", curValue, maxValue);
    }
}
