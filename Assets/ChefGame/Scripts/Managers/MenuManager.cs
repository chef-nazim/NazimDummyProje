using System;
using Cysharp.Threading.Tasks;
using gs.chef.game.Scripts.Presenters;
using gs.chef.game.enums;
using gs.chef.game.Events;
using gs.chef.game.Views;
using gs.chef.vcontainer.menu;
using MessagePipe;
using UnityEngine;
using VContainer;

namespace gs.chef.game.Managers
{
    public class MenuManager : AbsMenuManager<MenuNames>
    {
        [Inject] private readonly GamePlayPresenter _gamePlayPresenter;
        [Inject] private readonly WinMenuPresenter _winMenuPresenter;
        [Inject] private readonly SettingsMenuPresenter _settingsMenuPresenter;
        [Inject] private readonly FailPanelPresenter _failMenuPresenter;
        [Inject] private readonly ISubscriber<OpenMenuEvent> _openMenuSubscriber;
        [Inject] private readonly ISubscriber<CloseMenuEvent> _closeMenuSubscriber;
        [Inject] private readonly ISubscriber<CloseOtherMenuEvent> _closeOthersMenuSubscriber;
        [Inject] private readonly ISubscriber<LevelModelCreatedEvent> _levelModelCreatedEventSubscriber;
        protected override void MessagePipeSubscribes()
        {
            _openMenuSubscriber.Subscribe(OpenMenuHandler).AddTo(_disposableBagBuilder);
            _closeMenuSubscriber.Subscribe(CloseMenuHandler).AddTo(_disposableBagBuilder);
            _closeOthersMenuSubscriber.Subscribe(CloseOthersMenuHandler).AddTo(_disposableBagBuilder);
            _levelModelCreatedEventSubscriber.Subscribe(LevelModelCreated).AddTo(_disposableBagBuilder);
            
        }

        public void LevelModelCreated(LevelModelCreatedEvent e)
        {
            OpenMenu(MenuNames.GamePlayMenu, new GamePlayViewData());
        }


        protected override void OpenMenu(MenuNames menuName, IMenuData menuData)
        {
            switch (menuName)
            {
                case MenuNames.GamePlayMenu:
                    Open<GamePlayPresenter, GamePlayViewData>(_gamePlayPresenter, menuData);
                    break;
                case MenuNames.WinMenu:
                    ShowWinMenu((WinMenuViewData)menuData);
                    break;
                case MenuNames.SettingsMenu:
                    Open<SettingsMenuPresenter, SettingsMenuViewData>(_settingsMenuPresenter, menuData);
                    break;
                case MenuNames.FailMenu:
                    ShowFailMenu((FailPanelViewData)menuData);
                    break;
            }
        }

        private async void ShowFailMenu(FailPanelViewData menuData)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(.5f));
            Open<FailPanelPresenter, FailPanelViewData>(_failMenuPresenter, menuData);
        }

        private async void ShowWinMenu(WinMenuViewData menuData)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(.5f));
            Open<WinMenuPresenter, WinMenuViewData>(_winMenuPresenter, menuData);
        }

        public void OpenMenuHandler(OpenMenuEvent openMenuEvent)
        {
            OpenMenu(openMenuEvent.MenuName, openMenuEvent.MenuData);
        }

        public void CloseMenuHandler(CloseMenuEvent closeMenuEvent)
        {
          
            CloseMenu(closeMenuEvent.MenuName);
        }

        public void CloseOthersMenuHandler(CloseOtherMenuEvent closeOthersMenuEvent)
        {
            CloseOthers(closeOthersMenuEvent.KeepMenuNames);
        }
    }
}