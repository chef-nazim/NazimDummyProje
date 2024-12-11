using gs.chef.vcontainer.core.events;
using gs.chef.vcontainer.menu;
using gs.chef.vcontainer.plugins.googleads.events;
using gs.chef.vcontainer.spawner;
using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;
#if SELF_PUBLISHING
using gs.chef.vcontainer.plugins.attauth;
using gs.chef.vcontainer.plugins.googleads;
#endif

namespace gs.chef.vcontainer.extensions
{
    public abstract class AbsLifeTimeScope : LifetimeScope
    {
#if SELF_PUBLISHING
        [SerializeField] protected ChefGoogleAdsController _chefGoogleAdsController;
        [SerializeField] protected CHEF_ATTAuth_Manager _attAuthManager;
#endif
        protected MessagePipeOptions MessagePipeOptions { get; private set; }

        protected override void Configure(IContainerBuilder builder)
        {
#if UNITY_EDITOR || GOOGLEADS_TESTDEVICE || CHEF_DEBUG
            Debug.unityLogger.logEnabled = true;
#else
            Debug.unityLogger.logEnabled = false;
#endif

            Application.targetFrameRate = 60;

            Application.targetFrameRate = 60;

            MessagePipeOptions = builder.RegisterMessagePipe();

            RegisterSelfPublishing(builder);

            builder.RegisterMessageBroker<AppReadyEvent>(MessagePipeOptions);

            builder.Register<MenuFactoryService>(Lifetime.Singleton);
        }

        private void RegisterSelfPublishing(IContainerBuilder builder)
        {
#if SELF_PUBLISHING
            builder.RegisterComponent(_chefGoogleAdsController);
            builder.RegisterComponent(_attAuthManager);
#endif
            builder.RegisterMessageBroker<AdsEventStatus, AdsEvent>(MessagePipeOptions);
        }
    }
}