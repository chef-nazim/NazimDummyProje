using gs.chef.vcontainer.plugins.attauth;
using gs.chef.vcontainer.plugins.googleads;
using UnityEditor;
using UnityEngine;

namespace gs.chef.vcontainer.editor
{
    public static class ChefMenuOptions
    {
        [MenuItem("GameObject/CHEF GAME/Chef Google Ads Controller")]
        private static void CreateChefGoogleAdsController()
        {
            GameObject googleAdsManager = new GameObject("ChefGoogleAdsController1");
            var component = googleAdsManager.AddComponent<ChefGoogleAdsController>();
            googleAdsManager.transform.localPosition = Vector3.zero;
            
        }
        
        [MenuItem("GameObject/CHEF GAME/Chef ATTAuth Manager")]
        private static void CreateChefATTAuthManager()
        {
            GameObject attAuthManager = new GameObject("ChefATTAuthManager");
            var component = attAuthManager.AddComponent<CHEF_ATTAuth_Manager>();
            attAuthManager.transform.localPosition = Vector3.zero;
        }
    }
}