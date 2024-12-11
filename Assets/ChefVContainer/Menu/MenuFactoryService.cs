using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using VContainer;

namespace gs.chef.vcontainer.menu
{
    public class MenuFactoryService
    {
        private IObjectResolver _resolver;

        public MenuFactoryService(IObjectResolver resolver)
        {
            _resolver = resolver;
        }

        public TMenu SpawnMenu<TMenuName, TMenu, TPresenter>(TMenu prefab, Transform parent)
            where TMenuName : Enum
            where TMenu : MonoBehaviour, IMenu
            where TPresenter : IMenuPresenter<TMenuName>
        {
            var presenter = _resolver.Resolve<TPresenter>();
            var mainMenu = presenter.GetView<TMenu>();
            if (mainMenu == null)
            {
                mainMenu = UnityEngine.Object.Instantiate<TMenu>(prefab, parent);
                //presenter.SetView(mainMenu);
            }


            return mainMenu;
        }
    }

    public static class MenuFactoryExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RegisterMenuFactory<TMenuName, TMenu, TPresenter>(this IContainerBuilder builder,
            TMenu prefab, Transform parent, Lifetime lifeTime)
            where TMenuName : Enum
            where TMenu : MonoBehaviour, IMenu
            where TPresenter : IMenuPresenter<TMenuName>
        {
            builder.RegisterFactory<TMenu>(container =>
            {
                return () =>
                {
                    var menuFactoryService = container.Resolve<MenuFactoryService>();
                    var menu = menuFactoryService.SpawnMenu<TMenuName, TMenu, TPresenter>(prefab, parent);

                    return menu;
                };
            }, lifeTime);
        }
    }
}