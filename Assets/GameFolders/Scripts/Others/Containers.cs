using FluffyUnderware.Curvy.Generator.Modules;
using NCG.template.extensions;
using NCG.template.Scripts.ScriptableObjects;
using UnityEngine;

namespace NCG.template.Scripts.Others
{
    public class Containers : Singleton<Containers>
    {
        public GameObject CellItemContainer;
        public GameObject GridItemContainer;
        public GameObject SelectGrid;
        public GameObject MatchGrid;
        public GameObject GoalItemContainer;

        public AudioSource AudioSource;
        
        [SerializeField] private GameHelper _gameHelper;
        public GameHelper GameHelper => _gameHelper;
    }
}