using System;
using System.Runtime.CompilerServices;
using gs.chef.vcontainer.core.managers;
using gs.chef.vcontainer.processes;
using gs.chef.vcontainer.spawner;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace gs.chef.vcontainer.extensions
{
    public static class ChefLifeTimeExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RegistrationBuilder RegisterSubscribable<T>(this IContainerBuilder builder, Lifetime lifetime)
            where T : BaseSubscribable
        {
            return builder.Register<T>(lifetime).AsImplementedInterfaces();
        }
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RegisterObjectSpawner<TTransform, TItemData, TItem>(
            this IContainerBuilder builder, TItem item, Lifetime lifetime, bool isInjectable = true)
            where TTransform : Transform
            where TItemData : ISpawnItemModel
            where TItem : Component, ISpawnItem<TItemData>
        {
            builder.RegisterFactory(
                container => RegisterSpawner<TTransform, TItemData, TItem>(container, item, isInjectable), lifetime);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RegisterObjectSpawner<TItemData, TItem>(
            this IContainerBuilder builder, TItem item, Lifetime lifetime, bool isInjectable = true)
            where TItemData : ISpawnItemModel
            where TItem : Component, ISpawnItem<TItemData>
        {
            builder.RegisterFactory(
                container => RegisterSpawner<TItemData, TItem>(container, item, isInjectable), lifetime);
        }

        private static Func<TTransform, TItemModel, TItem>
            RegisterSpawner<TTransform, TItemModel, TItem>(IObjectResolver resolver, TItem item, bool isInjectable)
            where TTransform : Transform
            where TItemModel : ISpawnItemModel
            where TItem : Component, ISpawnItem<TItemModel>
        {
            return (parent, data) =>
            {
                var cloneItem = isInjectable
                    ? resolver.Instantiate(item, parent)
                    : UnityEngine.Object.Instantiate(item, parent);
                if (data != null)
                    cloneItem.ReInitialize(data);
                else
                    cloneItem.SetActive(false);

                return cloneItem;
            };
        }

        private static Func<TItemModel, TItem>
            RegisterSpawner<TItemModel, TItem>(IObjectResolver resolver, TItem item, bool isInjectable)
            where TItemModel : ISpawnItemModel where TItem : Component, ISpawnItem<TItemModel>
        {
            return (model) =>
            {
                var cloneItem = isInjectable
                    ? resolver.Instantiate(item)
                    : UnityEngine.Object.Instantiate(item);
                if (model != null)
                    cloneItem.ReInitialize(model);
                else
                    cloneItem.SetActive(false);
                return cloneItem;
            };
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RegisterGameProcessFactory<TProcessArgs, TProcess>(this IContainerBuilder builder, Lifetime lifeTime)
            where TProcessArgs : IProcessArgs
            where TProcess : IGameProcess<TProcessArgs>, new()
        {
            builder.RegisterFactory<TProcessArgs, TProcess>(container =>
            {
                var provider = container.Resolve<GameProcessProvider>();
                return (args) =>
                {
                    return provider.CreateProcess<TProcessArgs, TProcess>(args);
                    
                };
            }, lifeTime);
        }
    }
}