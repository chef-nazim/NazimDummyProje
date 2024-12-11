using System;
using System.Collections.Generic;
using System.Linq;
using MessagePipe;
using UnityEngine;
using VContainer;

namespace gs.chef.vcontainer.menu
{
    public abstract class AbsMenuManager<TMenuName> : MonoBehaviour, IMenuManager<TMenuName> where TMenuName : Enum
    {
        [SerializeField] private Transform _menuRoot;

        protected IDisposable _subscription;
        protected DisposableBagBuilder _disposableBagBuilder;

        //private List<IMenu<TMenuName>> menuLinkedList = new List<IMenu<TMenuName>>();

        private List<IMenuPresenter<TMenuName>> _menuList;

        public Transform MenuRoot => _menuRoot;

        [Inject]
        protected virtual void Construct()
        {
            _menuList ??= new List<IMenuPresenter<TMenuName>>();
            _menuList.Clear();

            if (_menuRoot != null)
            {
                foreach (Transform o in MenuRoot)
                {
                    Destroy(o.gameObject);
                }
            }

            _disposableBagBuilder = DisposableBag.CreateBuilder();
            MessagePipeSubscribes();
            _subscription = _disposableBagBuilder.Build();
        }

        protected abstract void MessagePipeSubscribes();

        protected void OnDisable()
        {
            _subscription?.Dispose();
        }

        protected void Open<TPresenter, TMenuData>(TPresenter menuPresenter, IMenuData menuData = null)
            where TMenuData : MenuData
            //where TMenu : MenuView<TMenuName>
            where TPresenter : IMenuPresenter<TMenuName>
        {
            if (menuPresenter.MenuMode == MenuMode.Single)
                CloseOthers();

            var anyPresenter = _menuList.FirstOrDefault(s =>
                Equals(s.MenuName, menuPresenter.MenuName));

            if (anyPresenter == null)
            {
                _menuList.Add(menuPresenter);
                menuPresenter.Open(menuData as TMenuData);
            }
            else
            {
                anyPresenter.Open(menuData as TMenuData);
            }
        }

        protected abstract void OpenMenu(TMenuName menuName, IMenuData menuData);


        protected void CloseMenu(TMenuName menuName)
        {
            var presenter = _menuList.FirstOrDefault(s =>
                s.IsAlreadyOpened && Equals(s.MenuName, menuName));

            if (presenter != null)
            {
                presenter.Close();
            }
        }

        protected void CloseOthers()
        {
            var anyMenu = _menuList.Select(s => s.IsAlreadyOpened);

            if (!anyMenu.Any())
                return;

            var list = _menuList.Select(s => s).ToList();

            foreach (var presenter in list)
            {
                if (presenter.IsAlreadyOpened == true)
                {
                    presenter.Close();
                }
                else
                {
                    _menuList.Remove(presenter);
                }
            }
        }

        protected void CloseOthers(params TMenuName[] menuNames)
        {
            var anyMenu = _menuList.Select(s => s.IsAlreadyOpened == true);

            if (!anyMenu.Any())
                return;

            var list = _menuList.Select(s => s).ToList();

            foreach (var presenter in list)
            {
                if (presenter.IsAlreadyOpened && !menuNames.Contains(presenter.MenuName))
                {
                    
                    presenter.Close();
                }
            }
        }
    }
}