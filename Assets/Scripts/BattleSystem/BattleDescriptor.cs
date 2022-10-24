using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using BattleEngine;
using SimpleJSON;

[CreateAssetMenu(fileName = "BattleStat", menuName = "Descriptor")]
public class BattleDescriptor : ScriptableObject
{
    [SerializeField] private GroupSetup _playerSetupType = GroupSetup.Simple;
    public GroupSetup PlayerSetup => _playerSetupType;

    [SerializeField] private List<string> _playerSetups;
    public List<string> PlayerSetups => _playerSetups;

    [SerializeField] private GroupSetup _enemySetupType;
    public GroupSetup EnemySetup => _enemySetupType;

    [SerializeField] private List<string> _enemySetups;
    public List<string> EnemySetups => _enemySetups;

    [SerializeField] private string _battleEnd = "SimpleEnd";
    public string BattleEnd => _battleEnd;

    public class DescriptorBuilder
    {
        private BattleDescriptor _descriptor;
        public static DescriptorBuilder Get()
        {
            DescriptorBuilder temp = new DescriptorBuilder();
            temp._descriptor = new BattleDescriptor();
            return temp;
        }

        public void SetPlayerSetupType(GroupSetup type)
        {
            _descriptor._playerSetupType = type;
        }

        public void SetPlayerSetupType(string type)
        {
            GroupSetup convert;
            try
            {
                GroupSetup test = (GroupSetup)Enum.Parse(typeof(GroupSetup), type);
                if (Enum.IsDefined(typeof(GroupSetup), test))
                    convert = test;
                else
                {
                    Debug.LogError("No such group setup type:" + type);
                    convert = GroupSetup.Simple;
                }
            }
            catch
            {
                Debug.LogError("No such group setup type:" + type);
                convert = GroupSetup.Simple;
            }

            _descriptor._playerSetupType = convert;
        }

        public void AddPlayerStack(string id)
        {
            if (_descriptor._playerSetups == null)
                _descriptor._playerSetups = new List<string>();

            _descriptor._playerSetupType = GroupSetup.Special;
            _descriptor._playerSetups.Add(id);
        }

        public void AddPlayerStack(List<string> list)
        {
            if (_descriptor._playerSetups == null)
                _descriptor._playerSetups = new List<string>();

            _descriptor._playerSetupType = GroupSetup.Special;
            _descriptor._playerSetups.AddRange(list);
        }

        public void SetEnemySetupType(GroupSetup type)
        {
            _descriptor._enemySetupType = type;
        }

        public void SetEnemySetupType(string type)
        {
            GroupSetup convert;
            try
            {
                GroupSetup test = (GroupSetup)Enum.Parse(typeof(GroupSetup), type);
                if (Enum.IsDefined(typeof(GroupSetup), test))
                    convert = test;
                else
                {
                    Debug.LogError("No such group setup type:" + type);
                    convert = GroupSetup.Simple;
                }
            }
            catch
            {
                Debug.LogError("No such group setup type:" + type);
                convert = GroupSetup.Simple;
            }

            _descriptor._enemySetupType = convert;
        }

        public void AddEnemyStack(string id)
        {
            if (_descriptor._enemySetups == null)
                _descriptor._enemySetups = new List<string>();

            _descriptor._enemySetupType = GroupSetup.Special;
            _descriptor._enemySetups.Add(id);
        }

        public void AddEnemyStack(List<string> list)
        {
            if (_descriptor._enemySetups == null)
                _descriptor._enemySetups = new List<string>();

            _descriptor._enemySetupType = GroupSetup.Special;
            _descriptor._enemySetups.AddRange(list);
        }

        public BattleDescriptor GetDescriptor()
        {
            return _descriptor;
        }
    }
}
