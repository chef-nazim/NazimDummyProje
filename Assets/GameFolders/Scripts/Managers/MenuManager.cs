using System;
using Cysharp.Threading.Tasks;
using NCG.template._NCG.Core.AllEvents;
using NCG.template._NCG.Core.Model;
using NCG.template.enums;
using NCG.template.EventBus;
using NCG.template.Events;
using NCG.template.extensions;
using NCG.template.Views;
using UnityEngine;

namespace NCG.template.Managers
{
    public class MenuManager : Singleton<MenuManager>
    {
        [SerializeField] GamePlayView _gamePlayView;
        [SerializeField] WinMenuView _winMenuView;
        [SerializeField] SettingsMenuView _settingsMenuView;
        [SerializeField] FailPanelView _failMenuView;


        

        private void OnEnable()
        {
            MessagePipeSubscribes();
            
        }


        void MessagePipeSubscribes()
        {
            
            EventBus<OpenMenuEvent>.Subscriber(OpenMenuHandler);

            
            EventBus<CloseMenuEvent>.Subscriber(CloseMenuHandler);

            
            EventBus<CloseOtherMenuEvent>.Subscriber(CloseOthersMenuHandler);

            
            EventBus<LevelModelCreatedEvent>.Subscriber(LevelModelCreated);
        }

        public void LevelModelCreated(LevelModelCreatedEvent e)
        {
            OpenMenu(MenuNames.GamePlayMenu, new GamePlayViewData());
        }


        void OpenMenu(MenuNames menuName, MenuData menuData)
        {
            switch (menuName)
            {
                case MenuNames.GamePlayMenu:
                    _gamePlayView.Show(menuData);
                    break;
                case MenuNames.WinMenu:
                    ShowWinMenu((WinMenuViewData)menuData);
                    break;
                case MenuNames.SettingsMenu:
                    _settingsMenuView.Show(menuData);
                    break;
                case MenuNames.FailMenu:
                    ShowFailMenu((FailPanelViewData)menuData);
                    break;
            }
        }

        private async void ShowFailMenu(FailPanelViewData menuData)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(.5f));
            _failMenuView.Show(menuData);
        }

        private async void ShowWinMenu(WinMenuViewData menuData)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(.5f));
            _winMenuView.Show(menuData);
        }

        public void OpenMenuHandler(OpenMenuEvent openMenuEvent)
        {
            OpenMenu(openMenuEvent._MenuName, openMenuEvent._MenuData);
        }

        public void CloseMenuHandler(CloseMenuEvent closeMenuEvent)
        {
            CloseMenu(closeMenuEvent._MenuName);
        }

        private void CloseMenu(MenuNames menuName)
        {
            switch (menuName)
            {
                case MenuNames.GamePlayMenu:
                    _gamePlayView.Hide();
                    break;
                case MenuNames.WinMenu:
                    _winMenuView.Hide();
                    break;
                case MenuNames.SettingsMenu:
                    _settingsMenuView.Hide();
                    break;
                case MenuNames.FailMenu:
                    _failMenuView.Hide();
                    break;
            }
        }


        public void CloseOthersMenuHandler(CloseOtherMenuEvent closeOthersMenuEvent)
        {
            foreach (var menu in Enum.GetValues(typeof(MenuNames)))
            {
                if (Array.IndexOf(closeOthersMenuEvent.KeepMenuNames, (MenuNames)menu) == -1)
                {
                    CloseMenu((MenuNames)menu);
                }
            }
        }
    }
}