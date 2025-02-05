using System.Collections.Generic;
using AllObject;
using AllObject.Entity;
using DefaultNamespace;
using Manager.Generics;
using SkillSystem;
using UnityEngine;

namespace Manager
{
    public class BattleManager : MonoSingleton<BattleManager>
    {
        //테스트용으로 일단 직접적으로 참조하게 함.
        [SerializeField] private EntityView player;
        private EntityModel _playerModel;
        private EntityPresenter _turnOwnerPresenter;

        public void Start()
        {
            _playerModel = new EntityModel();
            _turnOwnerPresenter = new EntityPresenter(_playerModel, player);
        }

        public void Attack(IStat defender) => Attack(_playerModel, defender);
        public void Attack(IStat attacker, IStat defender)
        {
            defender.hp.Value -= attacker.atk.Value - defender.def.Value;

            if (defender.hp.Value <= 0)
            {
                
            }
            else
            {
                attacker.hp.Value -= defender.atk.Value - attacker.def.Value;
            }
        }

        public void UsedSkill(List<Skill> skillList, IStat defender)
        {
            foreach (var skill in skillList)
            {
                skill.AddTriggerAction(defender);
            }

            TriggerManager.Instance.ExecuteTrigger(TriggerType.SkillStart);
        }

        /// <summary>
        /// 타겟이 없는 스킬
        /// </summary>
        /// <param name="skillList"></param>
        public void UsedSkill(List<Skill> skillList)
        {
            UsedSkill(skillList, null);
        }

        public EntityModel GetPlayer() => _playerModel;
    }
}