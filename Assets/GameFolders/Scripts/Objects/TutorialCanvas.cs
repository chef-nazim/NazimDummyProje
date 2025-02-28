using gs.chef.bakerymerge.Objects;
using UnityEngine;

namespace NCG.template.Scripts.Objects
{
    public class TutorialCanvas : MonoBehaviour
    {
        [SerializeField] private TutorialHand _tutorialHand;
        [SerializeField] private GameObject _tutorialText_Level1;
        [SerializeField] private GameObject _tutorialText_Level2_Text1;
        [SerializeField] private GameObject _tutorialText_Level2_Text2;
        [SerializeField] private GameObject _ratateGameObject;
        [SerializeField] private GameObject _ratateHand;
        
        public TutorialHand TutorialHand => _tutorialHand;
        public GameObject TutorialText_Level1 => _tutorialText_Level1;
        public GameObject TutorialText_Level2_Text1 => _tutorialText_Level2_Text1;
        public GameObject TutorialText_Level2_Text2 => _tutorialText_Level2_Text2;
        
        public GameObject RatateGameObject => _ratateGameObject;
        public GameObject RatateHand => _ratateHand;
        
    }
}