using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Lodkod;
using GameEvents;
using System;
using DG.Tweening;

public class UIM
{

    private static UIM instance = null;
    public static void NewGame()
    {
        if (UIM.instance != null)
            UIM.instance = null;

        UIM.instance = new UIM();

        UIM.instance._root = GameObject.Find("MainCanvas").GetComponent<Canvas>();
        UIM.instance._crect = UIM.instance._root.GetComponent<RectTransform>();

        UIM.instance._allMenus = new Dictionary<string, MenuEx>();

        foreach (MenuEx temp in UIM.instance._root.GetComponentsInChildren<MenuEx>(true))
        {
            temp.HardSet();
            UIM.instance._allMenus.Add(temp.ID, temp);
        }

        UIM.instance._tooltip = UIM.instance._allMenus["Tooltips"] as Tooltips;
        UIM.instance._contextMenu = UIM.instance._allMenus["ContextMenu"] as ContextMenu;
        UIM.instance._questMenu = UIM.instance._allMenus["QuestNotificationMenu"] as QuestNotificationMenu;
    }

    private bool _completeInit = false;
    private Vector2 screenDimension;
    private Vector2 invertedScreenDimension;
    public static Vector2 ScreenDimension
    {
        get
        {
            if (!UIM.instance._completeInit)
                InitSceenDemension();
                    
            return UIM.instance.screenDimension;
        }
    }
    public static Vector2 InvertedScreenDimension { get { return UIM.instance.invertedScreenDimension; } }
    public static void InitSceenDemension()
    {
        if (UIM.instance._completeInit)
            return;

        UIM.instance.screenDimension = new Vector2(Screen.width, Screen.height);
        UIM.instance.invertedScreenDimension = new Vector2(1f / Screen.width, 1f / Screen.height);
    }

    Canvas _root;
    public static Canvas Root
    {
        get { return UIM.instance._root; }
    }

    RectTransform _crect;
    public static float ScreenScale
    {
        get { return UIM.instance._root.GetComponent<CanvasScaler>().scaleFactor; }
    }

    float _screenWidth;
    public static float Width
    {
        get { return UIM.instance._screenWidth; }
    }

    float _screenHeight;
    public static float Height
    {
        get { return UIM.instance._screenHeight; }
    }


    #region Standart Menu Work
    Dictionary<string, MenuEx> _allMenus;

    public static void CallFunc(string ID, string func)
    {
        UIM.instance._allMenus[ID].SafeCall(func);
    }

    public static void HideAllMenu()
    {
        foreach(var menu in UIM.instance._allMenus)
        {
            if (!menu.Key.Equals("Black"))
                menu.Value.Visible = false;
        }
    }

    public static void ShowAllMenu()
    {
        foreach (var menu in UIM.instance._allMenus)
        {
            if(!menu.Key.Equals("Black"))
                menu.Value.Visible = true;
        }
    }

    public static void ShowMenu(string ID)
    {
        if (UIM.instance._allMenus.ContainsKey(ID) == false)
        {
            Debug.LogError("No such menu: " + ID);
            return;
        }

        UIM.instance._allMenus[ID].Visible = true;
    }

    public static void HideMenu(string ID)
    {
        if (UIM.instance._allMenus.ContainsKey(ID) == false)
        {
            Debug.LogError("No such menu: " + ID);
            return;
        }

        UIM.instance._allMenus[ID].Visible = false;
    }

    public static void OpenMenu(string ID)
    {
        if(UIM.instance._allMenus.ContainsKey(ID) == false)
        {
            Debug.LogError("No such menu: " + ID);
            return;
        }

        if(ID.Equals("GameInterfaceMenu") == false)
            UIM.instance._allMenus["GameInterfaceMenu"].Close();

        UIM.instance._allMenus[ID].Open();
    }

    public static void CloseMenu(string ID)
    {
        if (UIM.instance._allMenus.ContainsKey(ID) == false)
        {
            Debug.LogError("No such menu: " + ID);
            return;
        }

        if (ID.Equals("GameInterfaceMenu") == false)
            UIM.instance._allMenus["GameInterfaceMenu"].Open();

        UIM.instance._allMenus[ID].Close();
    }

    public static void AddMenuItem(string ID, UIItem item, string PanelID = null)
    {
        if (UIM.instance._allMenus.ContainsKey(ID) == false)
        {
            Debug.LogError("No such menu: " + ID);
            return;
        }

        UIM.instance._allMenus[ID].AddItem(item, PanelID);
    }

    public static void RemoveMenuItem(string ID, UIItem item, string PanelID = null)
    {
        if (UIM.instance._allMenus.ContainsKey(ID) == false)
        {
            Debug.LogError("No such menu: " + ID);
            return;
        }

        UIM.instance._allMenus[ID].RemoveItem(item, PanelID);
    }

    public static void RemoveMenuItem(string ID, string item, string PanelID = null)
    {
        if (UIM.instance._allMenus.ContainsKey(ID) == false)
        {
            Debug.LogError("No such menu: " + ID);
            return;
        }

        UIM.instance._allMenus[ID].RemoveItem(item, PanelID);
    }
    #endregion

    #region Get Menus UIeX objects

    public static SkyObject GetItem(string ID)
    {
        foreach(var menu in UIM.instance._allMenus)
        {
            if (menu.Value.GetItem(ID) != null)
                return menu.Value.GetItem(ID);
        }

        return null;
    }

    public static void EnableItem(bool enable, string Menu, string item = "All")
    {
        if(!UIM.instance._allMenus.ContainsKey(Menu))
        {
            Debug.LogError("No such menu: " + Menu);
            return;
        }

        UIM.instance._allMenus[Menu].EnableItem(enable, item);
    }

    #endregion

    #region QuestMenu
    QuestNotificationMenu _questMenu;
    public static QuestNotificationMenu QuestNotification
    {
        get { return UIM.instance._questMenu; }
    }

    #endregion

    #region Tooltips

    Tooltips _tooltip;
    public static void ShowTooltip(SkyObject obj, TooltipFit fit, TooltipTimeMode timeMode, TooltipFillMode fillMode, TooltipObject objectMode, string Text, GameEvent gEvent = null, Action callback = null, float time = 1.0f, int lSize = 0)
    {
        UIM.instance._tooltip.ShowTooltip(obj, fit, timeMode, fillMode, objectMode, Text, gEvent, callback, time, lSize);
    }

    public static void ShowTooltip(Vector3 target, TooltipFit fit, TooltipTimeMode timeMode, TooltipFillMode fillMode, TooltipObject objectMode, string Text, GameEvent gEvent = null, Action callback = null, float time = 1.0f, int lSize = 0)
    {
        UIM.instance._tooltip.ShowTooltip(target, fit, timeMode, fillMode, objectMode, Text, gEvent, callback, time, lSize);
    }

    public static void HideTooltip(SkyObject obj)
    {
        UIM.instance._tooltip.HideTooltip(obj);
    }

    #endregion

    #region ContextMenu
    ContextMenu _contextMenu;

    #endregion

    #region BlackScreen
    BlackScreen _blackScreen;

    public static void FastFade(bool _in = true)
    {
        if (UIM.instance._blackScreen == null)
            UIM.instance._blackScreen = UIM.instance._allMenus["Black"] as BlackScreen;

        UIM.instance._blackScreen.Visible = _in;
    }

    public static void Fade(bool _in = true, GameEvent gEvent = null, Action callF = null, float speed = 1.5f)
    {
        if (UIM.instance._blackScreen == null)
            UIM.instance._blackScreen = UIM.instance._allMenus["Black"] as BlackScreen;

        UIM.instance._blackScreen.StartFade(_in, gEvent, callF, speed);
    }

    #endregion

    #region UIAnimation

    public static Vector2 GetCenter(RectTransform transform)
    {
        if (!UIM.instance._completeInit)
            InitSceenDemension();

        return Vector2.Scale(transform.position, UIM.instance.invertedScreenDimension);
    }

    public static Vector3 ScreenCenter
    {
        get
        {
            return new Vector3(UIM.instance.screenDimension.x / 2, UIM.instance.screenDimension.y / 2, 0f);
        }
    }

    public static Vector3 RandomScreenPoint
    {
        get
        {
            return new Vector3(UnityEngine.Random.Range(-UIM.instance.screenDimension.x / 2 + 100f, UIM.instance.screenDimension.x / 2 - 100f), UnityEngine.Random.Range(-UIM.instance.screenDimension.y / 2 + 50f, UIM.instance.screenDimension.y / 2 - 50f), 0f);
        }
    }

    #region MOVE_ANIMATION

    /*  Move*
     *      Movement animations.
     *
     *      It creates an animation to move <transform>
     *      from <origin> to <target> in <duration>
     *      seconds.
     *      - MOVE TO: 				From (current position) to (Vector2 target).
     *      - MOVE HORIZONTAL: 		From (float origin) to (float target) with fixed y.
     *      - MOVE HORIZONTAL TO: 	From (current position) to (float target) with fixed y.
     *      - MOVE HORIZONTAL: 		From (float origin) to (float target) with fixed x.
     *      - MOVE HORIZONTAL TO: 	From (current position) to (float target) with fixed x.
     *      - MOVE OFFSET: 			From (current position) to (current position + Vector2 offset).
     */

    public static Tween Move(UIeX obj, Vector2 origin, Vector2 target, float duration)
    {
        return obj.Rect.DOMove(target, duration).From(origin);
    }

    public static Tween MoveTo(UIeX obj, Vector2 target, float duration)
    {
        return obj.Rect.DOMove(target, duration);
    }

    /*
    public static UIPositionAnimation MoveHorizontal(UIeX obj, float origin, float target, float duration)
    {
        return Move(obj, new Vector2(origin, GetCenter(obj.Rect).y), new Vector2(target, GetCenter(obj.Rect).y), duration);
    }

    public static UIPositionAnimation MoveHorizontalTo(UIeX obj, float target, float duration)
    {
        return Move(obj, GetCenter(obj.Rect), new Vector2(target, GetCenter(obj.Rect).y), duration);
    }

    public static UIPositionAnimation MoveVertical(UIeX obj, float origin, float target, float duration)
    {
        return Move(obj, new Vector2(GetCenter(obj.Rect).x, origin), new Vector2(GetCenter(obj.Rect).x, target), duration);
    }

    public static UIPositionAnimation MoveVerticalTo(UIeX obj, float target, float duration)
    {
        return Move(obj, GetCenter(obj.Rect), new Vector2(GetCenter(obj.Rect).x, target), duration);
    }

    public static UIPositionAnimation MoveOffset(UIeX obj, Vector2 offset, float duration)
    {
        return Move(obj, GetCenter(obj.Rect), GetCenter(obj.Rect) + offset, duration);
    }

    public static UIPositionAnimation MoveBezier(UIeX obj, Vector2 origin, Vector2 target, Vector2 p1, Vector2 p2, float duration)
    {
        return new UIBezierAnimation(
            obj: obj,
            origin: origin,
            target: target,
            p1: p1,
            p2: p2,
            duration: duration
        );
    }

    public static UIPositionAnimation MoveBezier(UIeX obj, Vector2 origin, Vector2 target, Vector2 p1, float duration)
    {
        return new UIBezierAnimation(
            obj: obj,
            origin: origin,
            target: target,
            p1: p1,
            duration: duration
        );
    }*/

    #endregion MOVE_ANIMATION

    #region SCALE_ANIMATION

    /*  Scale*
     *      Scale animations.
     *
     *      It creates an animation to scale <transform>
     *      from <origin> to <target> in <duration>
     *      seconds.
     *      - SCALE TO: 			From (current scale) to (Vector3 target).
     *      - SCALE OFFSET: 		From (current scale) to (current scale + Vector3 offset).
     */

    public static Tween Scale(UIeX obj, Vector3 origin, Vector3 target, float duration)
    {
        return obj.Rect.DOScale(target, duration).From(origin);
    }

    public static Tween ScaleTo(UIeX obj, Vector3 target, float duration)
    {
        return obj.Rect.DOScale(target, duration);
    }

    public static Tween ScaleOffset(UIeX obj, Vector3 offset, float duration)
    {
        return obj.Rect.DOScale(obj.Rect.localScale + offset, duration).From(obj.Rect.localScale);
    }

    #endregion SCALE_ANIMATION

    #region ROTATION_ANIMATION

    /*  Rotation*
     *      Rotation animations.
     *
     *      It creates an animation to rotate <transform>
     *      from <origin> to <target> in <duration>
     *      seconds.
     *      - ROTATE TO: 		From (current rotation) to (Quaternion target).
     *      - ROTATE OFFSET: 	From (current rotation) to (current rotation + Quaterion offset).
     */

    public static Tween Rotate(UIeX obj, Quaternion origin, Quaternion target, float duration)
    {
        return obj.Rect.DORotateQuaternion(target, duration).From(origin);
    }

    public static Tween RotateTo(UIeX obj, Quaternion target, float duration)
    {
        return obj.Rect.DORotateQuaternion(target, duration);
    }
    #endregion ROTATION_ANIMATION

    #region IMAGE_ANIMATION

    /*  ChangeColor*, Fade*
     *      Image animations.
     *
     *      It creates an animation to change color
     *      of <image> from <originColor> to <targetColor>
     *      in <duration> seconds.
     *      - CHANGE COLOR TO: 		From (current color) to (Color targetColor).
     *      - FADE IN:              Creates a fade in effect.
     *      - FADE OUT:             Creates a fade out effect.
     */

    public static Tween ColorAnimation(UIeX obj, Color origin, Color target, float duration, float delay = 0, Action callback = null)
    {
        return DOTween.To(() => obj.Color, x => obj.Color = x, target, duration).From(origin);
    }

    public static Tween ColorAnimationTo(UIeX obj, Color target, float duration, float delay = 0, Action callback = null)
    {
        return DOTween.To(() => obj.Color, x => obj.Color = x, target, duration);
    }

    #endregion IMAGE_ANIMATION

    #endregion

}