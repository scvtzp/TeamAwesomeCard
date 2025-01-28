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
        private EntityModel _turnOwner;
        private EntityPresenter _turnOwnerPresenter;

        public void Start()
        {
            _turnOwner = new EntityModel();
            _turnOwnerPresenter = new EntityPresenter(_turnOwner, player);
        }

        public void Attack(IStat defender) => Attack(_turnOwner, defender);
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
                skill.StartSkill(defender);
            }
        }
    }
}