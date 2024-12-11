using gs.chef.game.Scripts.Item;
using gs.chef.game.Scripts.Objects;
using gs.chef.game.Scripts.Others;
using gs.chef.game.Scripts.Presenters;
using gs.chef.game.Scripts.ScriptableObjects;
using gs.chef.game.Controllers;
using gs.chef.game.enums;
using gs.chef.game.Events;
using gs.chef.game.Managers;
using gs.chef.game.models;
using gs.chef.game.Views;
using gs.chef.onnectnext.timer;
using gs.chef.vcontainer.extensions;
using gs.chef.vcontainer.menu;
using Lean.Touch;
using MessagePipe;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;

namespace gs.chef.game.LifeTimeScope
{
    public class etime : AbsLifeTimeScope
    {
        [Header("UI")]
        [SerializeField] private MenuManager _menuManager;
        [SerializeField] private GamePlayView _gamePlayView;
        [SerializeField] private WinMenuView _winMenuView;
        [SerializeField] private FailPanelView _failPanelView;
        [SerializeField] private SettingsMenuView _settingsMenuView;
        [SerializeField] private TutorialCanvas _tutorialCanvas;
        
        [FormerlySerializedAs("baseCellItemPrefab")]
        [FormerlySerializedAs("_cellItemPrefab")]
        [Header("ItemPrefab")]
        [SerializeField] private CellItem cellItemPrefab;
        [SerializeField] private GoalItem goalItemPrefab;
        [SerializeField] private SlotItem slotItemPrefab;
        [SerializeField] private GridItem gridItemPrefab;
        
        
        [Header("Component")]
        [SerializeField] private LeanSelectByFinger _selectByFinger;
        [SerializeField] private Containers containers;
        
        [Header("ScriptableObject")]
        [SerializeField] private GameHelper _gameHelper;
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);


            #region Instance

            #endregion


            #region Component

            builder.RegisterComponent(_failPanelView);
            builder.RegisterComponent(_gamePlayView);
            builder.RegisterComponent(containers);
            builder.RegisterComponent(_winMenuView);
            builder.RegisterComponent(_settingsMenuView);
            builder.RegisterComponent(_tutorialCanvas);
            builder.RegisterComponent(_selectByFinger);
            builder.RegisterComponent(_gameHelper);

            #endregion


            #region LevelEvents

            builder.RegisterMessageBroker<CreateGamePlaySceneEvent>(MessagePipeOptions);
            builder.RegisterMessageBroker<FeelingEvent>(MessagePipeOptions);
            builder.RegisterMessageBroker<RestartButtonClickEvent>(MessagePipeOptions);
            builder.RegisterMessageBroker<LevelFailEvent>(MessagePipeOptions);
            builder.RegisterMessageBroker<LevelCompleteEvent>(MessagePipeOptions);
            builder.RegisterMessageBroker<LevelDestroyEvent>(MessagePipeOptions);
            builder.RegisterMessageBroker<LevelCreatedEvent>(MessagePipeOptions);
            builder.RegisterMessageBroker<LevelModelCreatEvent>(MessagePipeOptions);
            builder.RegisterMessageBroker<TapITableItemEvent>(MessagePipeOptions);
            builder.RegisterMessageBroker<TapBoosterButtonEvent>(MessagePipeOptions);
            builder.RegisterMessageBroker<UseBoosterEvent>(MessagePipeOptions);
            
            builder.RegisterMessageBroker<LevelModelCreatedEvent>(MessagePipeOptions);
            builder.RegisterMessageBroker<PlayOnButtonClickEvent>(MessagePipeOptions);
            builder.RegisterMessageBroker<RewardPlayOnButtonClickEvent>(MessagePipeOptions);

            #endregion


            #region Spawner

            builder.RegisterObjectSpawner<Transform, BaseItemModel, CellItem>(cellItemPrefab, Lifetime.Scoped, true);
            builder.RegisterObjectSpawner<Transform, BaseItemModel, GoalItem>(goalItemPrefab, Lifetime.Scoped, true);
            builder.RegisterObjectSpawner<Transform, BaseItemModel, SlotItem>(slotItemPrefab, Lifetime.Scoped, true);
            builder.RegisterObjectSpawner<Transform, BaseItemModel, GridItem>(gridItemPrefab, Lifetime.Scoped, true);

            #endregion


            builder.Register<GameModel>(Lifetime.Singleton);
            builder.Register<ChefTimerFactory>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.RegisterEntryPoint<AppManager>(Lifetime.Singleton);
            builder.RegisterSubscribable<LevelManager>(Lifetime.Singleton).AsSelf();
            builder.RegisterSubscribable<PoolManager>(Lifetime.Singleton).AsSelf();
            builder.RegisterSubscribable<InputController>(Lifetime.Singleton).AsSelf();
            builder.RegisterSubscribable<ProcessSystemController>(Lifetime.Singleton).AsSelf();
            builder.RegisterSubscribable<LevelModelController>(Lifetime.Singleton).AsSelf();
            builder.RegisterSubscribable<HapticManager>(Lifetime.Singleton).AsSelf();
            builder.RegisterSubscribable<SoundManager>(Lifetime.Singleton).AsSelf();
            builder.RegisterSubscribable<TutorialManager>(Lifetime.Singleton).AsSelf();


            #region Menu

            builder.RegisterMessageBroker<OpenMenuEvent>(MessagePipeOptions);
            builder.RegisterMessageBroker<CloseMenuEvent>(MessagePipeOptions);
            builder.RegisterMessageBroker<CloseOtherMenuEvent>(MessagePipeOptions);


            builder.RegisterComponent(_menuManager);

            builder.RegisterSubscribable<GamePlayPresenter>(Lifetime.Singleton).AsSelf();
            builder.RegisterSubscribable<WinMenuPresenter>(Lifetime.Singleton).AsSelf();
            builder.RegisterSubscribable<SettingsMenuPresenter>(Lifetime.Singleton).AsSelf();
            builder.RegisterSubscribable<FailPanelPresenter>(Lifetime.Singleton).AsSelf();


            builder.RegisterMenuFactory<MenuNames, GamePlayView, GamePlayPresenter>(_gamePlayView,
                _menuManager.MenuRoot, Lifetime.Scoped);

            builder.RegisterMenuFactory<MenuNames, WinMenuView, WinMenuPresenter>(_winMenuView, _menuManager.MenuRoot,
                Lifetime.Scoped);
            builder.RegisterMenuFactory<MenuNames, SettingsMenuView, SettingsMenuPresenter>(_settingsMenuView,
                _menuManager.MenuRoot, Lifetime.Scoped);
            builder.RegisterMenuFactory<MenuNames, FailPanelView, FailPanelPresenter>(_failPanelView,
                _menuManager.MenuRoot, Lifetime.Scoped);

            #endregion
        }
    }
}