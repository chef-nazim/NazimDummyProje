using FluffyUnderware.Curvy.Generator.Modules;
using NCG.template.extensions;
using UnityEngine;

namespace NCG.template.Scripts.Others
{
    public class Containers : Singleton<Containers>
    {
        public GameObject CellItemPoolContainer;
        public GameObject SlotItemPoolContainer;
        public GameObject GoalItemPoolContainer;
        public GameObject GridItemPoolContainer;


        public AudioSource AudioSource;
    }
}