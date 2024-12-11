using TMPro;
using UnityEngine;

namespace gs.chef.vcontainer.utility
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class AppVersionText : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<TextMeshProUGUI>().text = $"v {Application.version}";
        }
    }
}