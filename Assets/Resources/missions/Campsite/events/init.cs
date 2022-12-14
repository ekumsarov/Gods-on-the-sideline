using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventPackSystem;

namespace EventPackSystemCampsite
{
    public class init : EventPack
    {
        public override void Create()
        {
            this.events = new List<SimpleJSON.JSONNode>()
            {
               Make("{ 'Base':'InitCanvas' }"),
               Make("{ 'Base':'HideMenu' }"),
               Make("{ 'Base':'PerpareCutSene' }"),
               Make("{ 'Base':'PlayerInit', 'City':'false', 'HeroIsland':'island_14'}"),
               Make("{ 'Base':'ShowMenu' }"),
               Make("{ 'Base':'CallEditorPacks' }"),
               Make("{ 'Base':'OpenMenu', 'MenuID':'HeroesMenu' }"),
               Make("{ 'Base':'HideMenu', 'MenuID':'StatMenu' }"),
               Make("{ 'Base':'HideMenu', 'MenuID':'Black' }")
               
            }; 
        }
    }
}