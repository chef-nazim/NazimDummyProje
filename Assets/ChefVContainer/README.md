# ChefVContainer

Icindekiler:

- [Setup](#setup)
  - [1. Prerequests](#1-prerequests)
  - [2. ChefVContainer](#2-chefvcontainer)
  - [3. Create Your LifeTimeScope](#3-create-your-lifetimescope)
- [GameModel](#gamemodel)
- [MenuManager & Menus](#menumanager--menus)
  - [Menu Setup](#menu-setup)
    - [Menu Names Setup](#menu-names-setup)
    - [Menu Manager Setup](#menu-manager-setup)
    - [Menu View Setup](#menu-view-setup)
    - [Menu Data Setup](#menu-data-setup)
    - [Menu Presenter Setup](#menu-presenter-setup)
    - [Menu Register](#menu-register)
    - [Menu Events](#menu-events)
  - [Menu Calisma Prensipleri](#menu-calisma-prensipleri)
- [AppManager](#appmanager)
- [Events (MessagePipe)](#events-messagepipe)
- [BaseSubscribable](#basesubscribable)
- [ObjectSpawner](#objectspawner)
- [SpawnPooling](#spawnpooling)
- [GameProcess](#gameprocess)

## Setup

#### 1. Prerequests

- [VContainer](https://vcontainer.hadashikick.jp) [OpenUPM](https://openupm.com/packages/jp.hadashikick.vcontainer/)
- [UniTask](https://github.com/Cysharp/UniTask) [OpenUPM](https://openupm.com/packages/com.cysharp.unitask/)
- [MessagePipe](https://github.com/Cysharp/MessagePipe) [OpenUPM](https://openupm.com/packages/com.cysharp.messagepipe/) [OpenUPM VContainer](https://openupm.com/packages/com.cysharp.messagepipe.vcontainer/)
- [JSON.NET](https://openupm.com/packages/jillejr.newtonsoft.json-for-unity/#close)

```Packages>manifest.json```'a asagidakileri ekleyiniz.

```json
{
  "dependencies": {
    "jp.hadashikick.vcontainer": "1.15.4",
    "com.cysharp.messagepipe": "1.8.1",
    "com.cysharp.messagepipe.vcontainer": "1.8.1",
    "com.cysharp.unitask": "2.5.4",
    "jillejr.newtonsoft.json-for-unity": "13.0.102"
  },
  "scopedRegistries": [
    {
      "name": "package.openupm.com",
      "url": "https://package.openupm.com",
      "scopes": [
        "com.cysharp",
        "jp.hadashikick.vcontainer",
        "jillejr.newtonsoft.json-for-unity"
      ]
    }
  ]
}
```

#### 2. ChefVContainer

Submodule olrak projeye eklenir. Eklenecek dizin : ```Assets/ChefVContainer``` olmalidir.

SSH linkini url kismina giriniz.
```git@github.com:chefgamestudio/ChefVContainer.git```

#### 3. Create Your LifeTimeScope

```Assets/ChefGame/Scripts/LifeTime``` altinda kendi lifetimescope'unuzu olusturunuz.
Olusturdugunuz LifeTimeScope class'i ```AbsLifeTimeScope``` class'indan turetilmelidir.

Scene'de LifeTimeScope olusturulup bu class component olarak eklenmelidir.

```csharp
using gs.chef.vcontainer.extensions;
using MessagePipe;
using VContainer;

namespace gs.chef.game.lifetime
{
  public class ChefLifeTimeScope : AbsLifeTimeScope
  {
  }
}
```

## GameModel

Oyun ici ses, haptic, music gibi kullanim bilgileri tutan modeldir.

```Assets/ChefGame/Scripts/Model/GameModel``` altinda olacak sekilde yeni bir class yaratilir.
LevelIndex, LevelId gibi bilgileri bu yaratmis oldugumuz class icinde tutariz.

```BaseGameModel``` class'indan turetilmelidir.

```csharp
using gs.chef.vcontainer.core.model;

namespace gs.chef.game.model
{
    public class GameModel : BaseGameModel
    {
    }
}
```

```GameModel``` i ```ChefLifeTimeScope``` icinde register edilir.

```csharp
namespace gs.chef.game.lifetime
{
    public class ChefLifeTimeScope : AbsLifeTimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<GameModel>(Lifetime.Singleton);
        }
    }
}
```

## MenuManager & Menus

### Menu Setup

#### Menu Names Setup

Oncelikle Menu isimlerini tutacagimiz bir enum olusturuyoruz.

```csharp
namespace gs.chef.game.menu
{
    public enum MenuNames
    {
        MainMenu,
        SettingsMenu,
    }
}
```

#### Menu Manager Setup

Menulerimizi yonetecek ```MenuManager.cs``` yaratiyoruz. Scenede Emptiy Object yaratip bu class'i component olarak
ekliyoruz.

```csharp
using gs.chef.game.menu;
using gs.chef.vcontainer.menu;

namespace gs.chef.game.managers
{
    public class MenuManager : AbsMenuManager<MenuNames>
    {
        protected override void MessagePipeSubscribes()
        {
        
        }

        protected override void OpenMenu(MenuNames menuName, IMenuData menuData)
        {
        
        }
    }
}
```

#### Menu View Setup

Menumuz icin view class'ini yaratiyoruz.

```csharp
using System;
using gs.chef.vcontainer.menu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace gs.chef.game.menu
{
    public class MainMenuView : BaseMenuView
    {
        [SerializeField] private TextMeshProUGUI _titleTxt;
        [SerializeField] private Button _button;
    
        public event Action<string> OnChangeTitle; 

        public TextMeshProUGUI TitleTxt => _titleTxt;
    
        public Button Button => _button;
    
        public void SetTitle(string title)
        {
            _titleTxt.text = title;
            OnChangeTitle?.Invoke(title);
        }
    }
}
```

#### Menu Data Setup

Menumuze data tasiyacak sinifi yaratiyoruz.

```csharp
using gs.chef.vcontainer.menu;

namespace gs.chef.game.menu
{
    public class MainMenuData : MenuData
    {
        public string Message { get; private set; }
    
        public MainMenuData(string message)
        {
            Message = message;
        }
    }
}
```

#### Menu Presenter Setup

Menumuzun logic islemlerini yapacak, view'e aktaracak, menu presenter class'imizi
yaratiyoruz. ```MainMenuPresenter : BaseMenuPresenter<MenuNames, MainMenuData, MainMenuView>```

* ```MenuMode```Menunun oyun icerisindeki davranisini tanimlar. Scene lerin davranisini taklit etmesini saglar.
* ```Single``` menuler cagirildiginda ekranda bulunan tum menuleri kapatir ve sadece o menu ekranda gorunur olur.
  Menunun modunu presenter clasinda mutlaka vermeliyiz Verilmezse default olarak degeri ```Additive``` olur.
* ```Additive``` menuler popup davranisi sergiler. En son cagrilan ```Additive``` menu her zaman en ustte konumlanacak
  sekilde acildir. BIr menunun ustunde baska bir menu gorunsun isteniyorsa o menu ```Additive``` olarak ayarlanmalidir.
* ```MenuName``` Menuyu sisteme tanitacagimiz menunun ismidir. Mutlaka Presenter classinda tanimlanmalidir.
* Her presenter class'i ```BaseMenuPresenter<TMenuName, TMenuData, TMenuView>``` dan turetilir.
  - ```TMenuName``` Menuisimlerini kaydettigimiz enum type'dir. Buradaki ornekte ```MenuNames```.
  - ```TMenuData``` Her presenter icin yarattimiz bu menuye ozel data class'i. Buradaki ornekte ```MainMenuData```.
  - ```TMenuView``` Presenter class`imizin view lerini barindiran view class'imiz. Buradaki
    ornekte ```MainMenuView```.

```csharp
using gs.chef.vcontainer.menu;

namespace gs.chef.game.menu
{
    public class MainMenuPresenter : BaseMenuPresenter<MenuNames, MainMenuData, MainMenuView>
    {
        public override MenuMode MenuMode => MenuMode.Single;
        public override MenuNames MenuName => MenuNames.MainMenu;

        protected override void OnShow(MainMenuData menuData)
        {
        }

        protected override void OnHide()
        {
        }
    }
}
```

#### Menu Register

* Yazdigimiz ```LifeTimeScope``` class'inda ```MenuManager```, ```MainMenuPresenter```  register edilir. Register islemi sirasinda ```MainMenuView```'in
  prefabina ihtiyac duyulur.
* Managerimiz sahnemizde bulunan ```MonoBehaviour``` oldugu icin. ```MenuManager```'i component olarak register
  ediyoruz.
* Presenter'imizde eventlesi dinleme ihtiyacimiz olabilir. Bu durumda ```Subscriptions``` override edilerek gerekli eventleri subscribe edebilirsiniz. Bknz: [BaseSubscribable](#basesubscribable)
* Presenter'imiz ```builder.RegisterSubscribable<MainMenuPresenter>(Lifetime.Singleton).AsSelf();``` seklinde register edilir.
* ```RegisterMenuFactory``` ile ```MenuView```'in factory'si register edilir. Bu register islemi ile ```Presenter```
  hangi ```View```'i, prefablar arasindan instantiate edecegini bilir.
* Yeni bir menuyu register etmek istedigimizde view, data presenter ve name bilgileri ve prefab'ini olusturup benzer
  sekilde burada register ederiz.

```csharp
namespace gs.chef.game.lifetime
{
    public class ChefLifeTimeScope : AbsLifeTimeScope
    {
        [SerializeField] private MenuManager _menuManager;
        [SerializeField] private MainMenuView _mainMenuView;

        protected override void Configure(IContainerBuilder builder)
        {
            // Register Menu Manager
            builder.RegisterComponent(_menuManager);
            // Register MenuPresenters
            builder.RegisterSubscribable<MainMenuPresenter>(Lifetime.Singleton).AsSelf();

            // Register MenuFactories
            builder.RegisterMenuFactory<MenuNames, MainMenuView, MainMenuPresenter>(_mainMenuView, _menuManager.MenuRoot, Lifetime.Scoped);
        }
    }
}
```

#### Menu Events

* Menuler'i acma ve/veya kapama islemlerini ```MenuManager``` uzerinden yazacagimiz menu eventleri ile yapariz.
* Bu islemler icin kullanacagimiz 3 tane eventimiz olacak, Bunlar'i yaratalim.

```csharp
using gs.chef.vcontainer.menu;

namespace gs.chef.game.menu
{
    public class OpenMenuEvent : BaseOpenMenuEvent<MenuNames, IMenuData>
    {
        public OpenMenuEvent(MenuNames menuName, IMenuData menuData) : base(menuName, menuData)
        {
        }
    }
  
    public class CloseMenuEvent : BaseCloseMenuEvent<MenuNames>
    {
        public CloseMenuEvent(MenuNames menuName) : base(menuName)
        {
        }
    }

    public class CloseOtherMenuEvent : BaseCloseOthersMenuEvent<MenuNames>
    {
        public MenuNames[] KeepMenuNames { get; private set; }
    
        public CloseOtherMenuEvent(params MenuNames[] keepMenuNames) : base(keepMenuNames)
        {
            KeepMenuNames = keepMenuNames;
        }
    }
}
```

* Menu Event'lerini ```Publish``` ve ```Subscribe``` edebilmemiz icin Register etmemiz gerekiyor.
* ```ChefLifeTimeScope``` classinda register ediyoruz.

```csharp
namespace gs.chef.game.lifetime
{
    public class ChefLifeTimeScope : AbsLifeTimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // Register Open Menu Event
            builder.RegisterMessageBroker<OpenMenuEvent>(MessagePipeOptions);
            // Register Close Menu Event
            builder.RegisterMessageBroker<CloseMenuEvent>(MessagePipeOptions);
            // Register Close Others Menu Event
            builder.RegisterMessageBroker<CloseOthersMenuEvent>(MessagePipeOptions);
        }
    }
}
```

* Bu eventlerimizi artik ```MenuManager``` uzerinden dinleyerek menu acma/kapatma islemlerini yapabiliriz.
* Register edilmis olan bu eventleri dinlemek icin ```MenuManager```'imizda ```ISubscibe``` ile bu
  eventleri ```Inject``` edebiliriz.

```csharp
namespace gs.chef.game.managers
{
    public class MenuManager : AbsMenuManager<MenuNames>
    {
        [Inject] private readonly ISubscriber<OpenMenuEvent> _openMenuSubscriber;
        [Inject] private readonly ISubscriber<CloseMenuEvent> _closeMenuSubscriber;
        [Inject] private readonly ISubscriber<CloseOthersMenuEvent> _closeOthersMenuSubscriber;

        protected override void MessagePipeSubscribes()
        {
            _openMenuSubscriber.Subscribe(OpenMenuHandler).AddTo(_disposableBagBuilder);
            _closeMenuSubscriber.Subscribe(CloseMenuHandler).AddTo(_disposableBagBuilder);
            _closeOthersMenuSubscriber.Subscribe(CloseOthersMenuHandler).AddTo(_disposableBagBuilder);
        }

        protected override void OpenMenu(MenuNames menuName, IMenuData menuData)
        {
        
        }
    
        private void OpenMenuHandler(OpenMenuEvent openMenuEvent)
        {
            OpenMenu(openMenuEvent.MenuName, openMenuEvent.MenuData);
        }
    
        private void CloseMenuHandler(CloseMenuEvent closeMenuEvent)
        {
            CloseMenu(closeMenuEvent.MenuName);
        }

        private void CloseOthersMenuHandler(CloseOthersMenuEvent closeOthersMenuEvent)
        {
            CloseOthers(closeOthersMenuEvent.KeepMenuNames);
        }
    }
}
```

* ```MenuManager``` menulerin acma ve kapatma islemini yapacagi icin, Presenter'larimizi ```MenuManager```'in bilmesi
  gerekiyor. ```ChefLifeTimeScope``` icerinde register ettigimiz Presenterlarimizi artik ```MenuManager``` icerisine *
  *Inject** edebilir ve menu acma/kapama islemlerini yaptirabiliriz.
* ```MenuManager``` class'imiz MainMenu icin asagidaki seklini almis olacaktir. Yeni menuler eklendigimizde yeni menunun
  presenter'ini yine buraya inject edip, ```OpenMenu``` methodu icerisinde switch case ile kontrol ederek acma islemini
  yapabiliriz.

```csharp
namespace gs.chef.game.managers
{
    public class MenuManager : AbsMenuManager<MenuNames>
    {
        [Inject] private readonly ISubscriber<OpenMenuEvent> _openMenuSubscriber;
        [Inject] private readonly ISubscriber<CloseMenuEvent> _closeMenuSubscriber;
        [Inject] private readonly ISubscriber<CloseOthersMenuEvent> _closeOthersMenuSubscriber;
    
        [Inject] private readonly MainMenuPresenter _mainMenuPresenter;

        protected override void MessagePipeSubscribes()
        {
            _openMenuSubscriber.Subscribe(OpenMenuHandler).AddTo(_disposableBagBuilder);
            _closeMenuSubscriber.Subscribe(CloseMenuHandler).AddTo(_disposableBagBuilder);
            _closeOthersMenuSubscriber.Subscribe(CloseOthersMenuHandler).AddTo(_disposableBagBuilder);
        }

        protected override void OpenMenu(MenuNames menuName, IMenuData menuData)
        {
            switch (menuName)
            {
                case MenuNames.MainMenu:
                    Open<MainMenuPresenter, MainMenuData>(_mainMenuPresenter, menuData);
                    break;
            }
        }
    
        private void OpenMenuHandler(OpenMenuEvent openMenuEvent)
        {
            OpenMenu(openMenuEvent.MenuName, openMenuEvent.MenuData);
        }
    
        private void CloseMenuHandler(CloseMenuEvent closeMenuEvent)
        {
            CloseMenu(closeMenuEvent.MenuName);
        }

        private void CloseOthersMenuHandler(CloseOthersMenuEvent closeOthersMenuEvent)
        {
            CloseOthers(closeOthersMenuEvent.KeepMenuNames);
        }
    }
}
```

### Menu Calisma Prensipleri

* Bir menuyu acmak istedigimizde register ettigimiz open event'i, cagirmak istedigimiz yerde oncelikle ```IPublisher```
  olarak **Inject** edip, cagirmak istedigimiz anda ```Publish``` ediyoruz.

> Dikkat: **Inject** islemi sadece VContainer tarafindan register edilmis ya da VContainer tarafindan resolve edilebilir
> class/component'lerde yapilabilir.

```csharp
public class SomeRegisteredClass
{
    [Inject] private readonly IPublisher<OpenMenuEvent> _openMenuEventPublisher;
  
    public void OpenMenu()
    {
        _openMenuEventPublisher.Publish(new OpenMenuEvent(MenuNames.MainMenu, new MainMenuData("Hello World")));
    }
}
```

* **Publish** ettigimiz bu event, ```MenuManager```'imizde register ettigimiz ```ISubscriber``` ile dinlenir
  ve ```OpenMenu``` methodu calisir.
* ```MenuManager``` icerisinden ilgili presenter (burada ```MainMenuPresenter```) araciligi ile kendi View'ini acar (
  Kullanima gore, **Instantiate** eder ya da **SetActive** ini true yapar).
* **View**'in ```DestroyWhenClosed``` ozelligi true ise, menu kapatildiginda **View** destroy edilir. Bu ozellik false
  ise, **View** disable edilir.
* Presenter icerisindeki ```OnShow``` methodu menu scenede active olmadan once, ```OnHide``` methodu menu kapanmadan
  once calisir. ornegin ```OnShow``` icerisinde gelen data ile menunun view'inde guncelleme yapilabilir, buttonlar
  ve/veya ```event Action``` lar icin event listenerlar eklenebilir. ```OnHide``` icerisinde ekledigimiz event
  handlerlar kaldirilabilir.
* Asagidaki ornekte ```MainMenuView``` butonunun click'inde kendisini kapatmak istedigimizi
  dusunerek, ```MainMenuPresenter```'i duzenleyebiliriz.

```csharp
namespace gs.chef.game.menu
{
    public class MainMenuPresenter : BaseMenuPresenter<MenuNames, MainMenuData, MainMenuView>
    {
        [Inject] private readonly IPublisher<CloseMenuEvent> _closeMenuEventPublisher;

        public override MenuMode MenuMode => MenuMode.Single;
        public override MenuNames MenuName => MenuNames.MainMenu;

        protected override void OnShow(MainMenuData menuData)
        {
            View.OnChangeTitle += OnChangeTitle;
            View?.SetTitle(menuData.Message);
            View?.Button.onClick.AddListener(OnClickButton);
        }
    
        protected override void OnHide()
        {
            View.OnChangeTitle -= OnChangeTitle;
            View?.Button.onClick.RemoveAllListeners();
        }

        private void OnClickButton()
        {
            // Do something
            _closeMenuEventPublisher.Publish(new CloseMenuEvent(MenuName));
        }

        private void OnChangeTitle(string obj)
        {
            //Do something
        }
    }
}
```

## AppManager

* ```ChefLifeTimeScope``` da register islemlerimiz bittikten sonra, artik oyunun baslangic adimini baslatabiliriz. Bunun
  icin kendimize bir manager yaratiyoruz.
* Buradaki ```InitializeGame``` methodu, oyunun baslangicinda calisacak islemleri yaptigimiz methoddur. Burada oyun icin
  gerekli initialize islemleri yapilabilir.
* Initialize islemleri bittikten sonra ***ChefVContainer*** da tanimli ve register edilmis olan ```AppReadyEvent```'i arka planda
  ```InitializeGame``` deki islemler tamamlandiginda otomatik olrak **Publish** edilir ve boylece oyunun baslangic adimini baslatiriz.
* **AppManager** **BaseAppManager**'dan turetilmistir. **BaseAppManager**'da **AppReadyEvent**'i dinleyen bir *
  *ISubscriber** ve **Publish** eden bir **IPublisher** vardir. Bu sayede **AppManager** icinde **AppReadyEvent**'i
  dinleyebiliriz.
* **AppManager**'da **AppReadyEvent**'i **OnAppReady** de dinleyerek, oyunun baslangic adimini baslatiriz.

```csharp
namespace gs.chef.game.managers
{
    public class AppManager : BaseAppManager
    {
  
        protected override UniTask InitializeGame(CancellationToken token)
        {
            //TODO: Initialize Game Process
            //(Example: Load Game Data, Facebook Initialize, GameAnalytics Initialize, GoogleAds Initialize, etc.)
            return UniTask.CompletedTask;
        }

        protected override void OnAppReady(AppReadyEvent appReadyEvent)
        {
            //TODO: Start Game Process (Example: Load Level Event, Prepare Level Event, etc.)
        }
    }
}
```

* **AppManager**'i **ChefLifeTimeScope**'a register ediyoruz.

```csharp
namespace gs.chef.game.lifetime
{
    public class ChefLifeTimeScope : AbsLifeTimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterSubscribable<AppManager>(Lifetime.Singleton).AsSelf();
        }
    }
}
```

## Events (MessagePipe)

* **MessagePipe**'i kullanarak, oyun icerisindeki event'leri kolayca dinleyebilir ve yayabiliriz.
* Ornek bir senaryo uzerinden gidelim.
* Oyunumuz mobile cihazda acildiginda 2. level'in yuklenmesini istiyoruz. Bunun icin kendimize **LoadLevelEvent**
  yaratalim ve oyunumuz mobile cihazda ilk acildiginda **AppManager**'imizden **LoadLevelEvent**'i **Publish** edelim.
  Bu durumda oyunumuzda levellarimizi yonetecek bir manager'a da ihtiyacimiz olacak. **LevelManager** **LoadLevelEvent
  **'i dinleyerek, level yukleme islemlerini yurutecek. Yuklenme islemleri tamamlaninca da yuklenen level'in hazir
  oldugunu bildiren bir event gonderecek.
* Ornek olarak ```LevelManager``` ```LevelReadyEvent```i hem subscribe hem de publish edebilsin.
* ```LevelManager``` level ready oldugunda ```MainMenu```yu acsin;
* Ayni zamanda ```MainMenuPresenter``` da ```LevelReadyEvent```i subscribe edebilsin.
* Bu durumda asagidakilere ihtiyacimiz var ve bunlari ```ChefLifeTimeScope```'a register etmemiz gerekiyor.
  * ```LoadLevelEvent```
  * ```LevelManager```
  * ```LevelReadyEvent```

```csharp
namespace gs.chef.game.level.events
{
    public struct LevelLoadEvent
    {
        public int LevelIndex { get; private set; }
    
        public LevelLoadEvent(int levelIndex)
        {
            LevelIndex = levelIndex;
        }
    }

    public struct LevelReadyEvent
    {
        public int LevelIndex { get; private set; }
    
        public LevelReadyEvent(int levelIndex)
        {
            LevelIndex = levelIndex;
        }
    }
}

namespace gs.chef.game.managers
{
    public class LevelManager : IStartable, IDisposable
    {
        [Inject] private readonly ISubscriber<LevelLoadEvent> _levelLoadEventSubscriber;
        [Inject] private readonly ISubscriber<LevelReadyEvent> _levelReadyEventSubscriber;
    
        [Inject] private readonly IPublisher<LevelReadyEvent> _levelReadyEventPublisher;
        [Inject] private readonly IPublisher<OpenMenuEvent> _openMenuEventPublisher;

        private IDisposable _subscription;

        public void Start()
        {
            var bag = DisposableBag.CreateBuilder();
            _levelLoadEventSubscriber.Subscribe(OnLevelLoad).AddTo(bag);
            _levelReadyEventSubscriber.Subscribe(OnLevelReady).AddTo(bag);
            _subscription = bag.Build();
        }

        private void OnLevelLoad(LevelLoadEvent levelLoadEvent)
        {
            LoadLevel(levelLoadEvent.LevelIndex);
        }
    
        private void OnLevelReady(LevelReadyEvent levelReadyEvent)
        {
            //TODO: Do something
            Debug.LogWarning($"OnLevelReady LevelManager {levelReadyEvent.LevelIndex}");
        }

        private void LoadLevel(int levelIndex)
        {
            //TODO: Do something to load level
            _openMenuEventPublisher.Publish(new OpenMenuEvent(MenuNames.MainMenu, new MainMenuData("Hello World")));
            _levelReadyEventPublisher.Publish(new LevelReadyEvent(levelIndex));
        }

        public void Dispose()
        {
            _subscription.Dispose();
        }
    }
}
```

```LevelManager```, ```LevelLoadEvent```, ```LevelReadyEvent```'i ```ChefLifeTimeScope```'a register ediyoruz.

```csharp
namespace gs.chef.game.lifetime
{
    public class ChefLifeTimeScope : AbsLifeTimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<LevelManager>();
    
            builder.RegisterMessageBroker<LevelLoadEvent>(MessagePipeOptions);
            builder.RegisterMessageBroker<LevelReadyEvent>(MessagePipeOptions);
        }
    }
}
```

* Artik ```AppManager```dan ```LoadLevelEvent```'i publish edebiliriz.

```csharp
namespace gs.chef.game.managers
{
    public class AppManager : BaseAppManager
    {
        [Inject] private readonly IPublisher<LevelLoadEvent> _levelLoadEventPublisher;

        public override UniTask StartAsync(CancellationToken cancellation)
        {
            //TODO: Initialize Game Process
            //(Example: Load Game Data, Facebook Initialize, GameAnalytics Initialize, GoogleAds Initialize, etc.)
            AppReadyEventPublisher.Publish(new AppReadyEvent());
            return UniTask.CompletedTask;
        }

        protected override void OnAppReady(AppReadyEvent appReadyEvent)
        {
            //TODO: Start Game Process (Example: Load Level Event, Prepare Level Event, etc.)
            _levelLoadEventPublisher.Publish(new LevelLoadEvent(2));
        }
    }
}
```

* ```MainMenuPresenter```'da ```LevelReadyEvent```'i subscribe edelim.

```csharp
namespace gs.chef.game.menu
{
    public class MainMenuPresenter : BaseMenuPresenter<MenuNames, MainMenuData, MainMenuView>
    {
        [Inject] private readonly ISubscriber<LevelReadyEvent> _levelReadyEventSubscriber;

        public override MenuMode MenuMode => MenuMode.Single;
        public override MenuNames MenuName => MenuNames.MainMenu;

        private IDisposable _subscription;
    
        protected override void OnShow(MainMenuData menuData)
        {
            var bag = DisposableBag.CreateBuilder();
            _levelReadyEventSubscriber.Subscribe(@event => OnLevelReady(@event)).AddTo(bag);
            _subscription = bag.Build();
        }

        private void OnLevelReady(LevelReadyEvent @event)
        {
            //TODO: Do something
            Debug.LogWarning($"OnLevelReady MainMenuPresenter {@event.LevelIndex}");
            View?.SetTitle($"Level {@event.LevelIndex}");
        }

        protected override void OnHide()
        {
            _subscription?.Dispose();
        }
    }
}
```

## BaseSubscribable

Events basliginda bahsettigimize ek olarak **ChefVContainer**'a event leri dinlemek islemlerini kolaylastirmak icin eklenmis bir ozelliktir.
Eventleri Subscribe etmek istedigimiz bir class'i **BaseSubscribable** dan turetebiliriz.
**BaseSubscribable** class'i **IInitializable, IDisposable** interfacelerini kendisine implemente etmistir. Dinlemek istedigimiz eventleri subscribe etmek icin bize ```Subscriptions``` metodunu kullanima sunmustur.
Ayrica ```Init``` metodu ile resolve sirasinda yapmak isteyebileceginiz Initialize islemlerini isterseniz ekleyebilirsiniz.
Bu durumda **LevelManager** clasi asagidaki gibi olacaktir.

Bu sayede MessagePipe icin bagbuilder yaratma ve dispose islemlerini kolaylastirmis olunacaktir. Yukarida Events basliginda anlatilanlar eventleri subscribe islemleri icin temel bilgidir. **BaseSubscribable** bu temel bilgi kullanilarak insa edilmistir.

```C#
namespace gs.chef.game.managers
{
    public class LevelManager : BaseSubscribable
    {
        [Inject] private readonly ISubscriber<AppReadyEvent> _appReadyEventSubscriber;
        [Inject] private readonly IPublisher<LevelLoadEvent> _levelLoadEventPublisher;
        [Inject] private readonly ISubscriber<LevelLoadEvent> _levelLoadEventSubscriber;
        [Inject] private readonly ISubscriber<LevelReadyEvent> _levelReadyEventSubscriber;
    
        [Inject] private readonly IPublisher<OpenMenuEvent> _openMenuEventPublisher;

        [Inject] private readonly LevelController _levelController;

        protected override void Subscriptions()
        {
            _appReadyEventSubscriber.Subscribe(OnAppReady).AddTo(_bagBuilder);
            _levelLoadEventSubscriber.Subscribe(OnLevelLoad).AddTo(_bagBuilder);
            _levelReadyEventSubscriber.Subscribe(e=> OnLevelReady(e.LevelIndex)).AddTo(_bagBuilder);
        }

        private void OnLevelReady(int levelIndex)
        {
            _openMenuEventPublisher.Publish(new OpenMenuEvent(MenuNames.GamePlayMenu, new GamePlayMenuData(levelIndex)));
        }

        public LevelManager()
        {
            Debug.Log("LevelManager Constructor");
        }

        private void OnAppReady(AppReadyEvent appReadyEvent)
        {
        
        }

        private void OnLevelLoad(LevelLoadEvent levelLoadEvent)
        {
            _levelController.LoadLevel(levelLoadEvent.LevelIndex);
        }
   }
}
```

**BaseSubscribable** dan turetilen bir class asagidaki gibi register edilmelidir.

```C#
builder.RegisterSubscribable<LevelManager>(Lifetime.Singleton).AsSelf();
```

## ObjectSpawner

Oyun icerisinde ```ChefLifeTimeScope``` da register ettigimiz herhangibir prefab'i istedigimiz yerde ve zamanda spawn
etmek icin kullaniriz.
Bunun icin iki farkli strateji kurulabilir. Spawn edecegimiz objelerin belirli bir ```transform``` altinda olmasini veya
olmamasini isteyebiliriz.

Prefabi istedigimiz herhangi bir ```transform```'un altinda spawn etmek.
Spawn etmek istedigimiz objenin scriptini ve model'ini yazalim.

```csharp
namespace ChefGame.Scripts.Level
{
    public class CellItem : MonoBehaviour, ISpawnItem<CellItemModel>
    {
        [SerializeField] private CellItemModel _itemModel;


        public void Dispose()
        {
            Destroy(gameObject);
        }

        public CellItemModel ItemModel
        {
            get => _itemModel;
            set => _itemModel = value;
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void ReInitialize(CellItemModel itemModel)
        {
            _itemModel = itemModel;
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            transform.rotation = Quaternion.identity;
            SetActive(true);
            SetPosition(_itemModel.Position);
        }

        private void SetPosition(Vector3 position)
        {
            transform.localPosition = position;
        }
    }

    [Serializable]
    public class CellItemModel : ISpawnItemModel
    {
        [field: SerializeField] public int Id { get; private set; }

        public Vector3 Position;

        public CellItemModel(int id)
        {
            Id = id;
        }
    }
}
```

```CellItem```'i ekledigimiz objeyi prefab yapalim, ve ```ChefLifeTimeScope```'da bu prefab ile objeye ozel
spawner'imizi register edelim.

```csharp
namespace gs.chef.game.lifetime
{
    public class ChefLifeTimeScope : AbsLifeTimeScope
    {
        [SerializeField] private CellItem _cellItemPrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            // Belirli bir transform altinda Spawn edilecekse
            builder.RegisterObjectSpawner<Transform, CellItemModel, CellItem>(_cellItemPrefab, Lifetime.Scoped);
            // Belirli bir transform altinda Spawn EDILMEYECEKSE
            builder.RegisterObjectSpawner<CellItemModel, CellItem>(_cellItemPrefab, Lifetime.Scoped);
        }
    }
}
```

> ```builder.RegisterObjectSpawner```da istenen son parametre olan ```isInjectable```'in **default** degeri **true**'
> dir.
> Bu su anlama gelir. Spawn ettigimiz ```CellItem``` icersinde injection yapilabilir (```[Inject]```). Injecttion yapmak
> istenmiyorsa asagidaki gibi yazilmalidir.
>
> ```csharp
> builder.RegisterObjectSpawner<Transform, CellItemModel, CellItem>(_cellItemPrefab, Lifetime.Scoped, false);
> ```

Mesela Scenede bir transform'un altida spawn edeceksek, spawn edecek Script'imizin bu transformu bilmesi gerekir. Bu objeyi ```ChefLifetimeScope```'da Component olarak register edip iecerisinde spawn islemini yapabiliriz; ya da register ettigimiz bu objeyi Spawn edecegimiz Script icerisinde **Inject** ederek trasnformu bildirebiliriz.

Ornek olrak Scene'de Bos bir **GameObject** yaratalim ve asagidaki Script'i bu objemize ekleyelim.

```csharp
namespace gs.chef.game.level
{
    public class LevelContainer : MonoBehaviour
    {
    }
}
```

```LevelContainer```'i register edelim.

```csharp
namespace gs.chef.game.lifetime
{
    public class ChefLifeTimeScope : AbsLifeTimeScope
    {
        [SerializeField] private LevelContainer _levelContainer;
        [SerializeField] private CellItem _cellItemPrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_levelContainer);
    
            builder.RegisterEntryPoint<LevelManager>();
    
            // Belirli bir transform altinda Spawn edilecekse
            builder.RegisterObjectSpawner<Transform, CellItemModel, CellItem>(_cellItemPrefab, Lifetime.Scoped);
            // Belirli bir transform altinda Spawn EDILMEYECEKSE
            builder.RegisterObjectSpawner<CellItemModel, CellItem>(_cellItemPrefab, Lifetime.Scoped);
        }
    }
}
```

Artik ```LevelManager```'a **CellItem**'i spawn edecegimiz **transform**'u **Inject** edebilir ve Spawn islemleri icin register ettigimiz methodlarimizi da **Inject** edebilirz.

```csharp
namespace gs.chef.game.managers
{
    public class LevelManager : BaseSubscribable
    {
        [Inject] private readonly ISubscriber<LevelLoadEvent> _levelLoadEventSubscriber;
        [Inject] private readonly IPublisher<LevelReadyEvent> _levelReadyEventPublisher;
    
        // Spawn edecegimiz objelerin parent transform'u
        [Inject] private readonly LevelContainer _levelContainer;
    
        // Belirli bir objenin child'i olarak spawn etmek istersek register ettigimiz metodu buraya inject ediyoruz.
        [Inject] private readonly Func<Transform, CellItemModel, CellItem> _cellItemWithTransformSpawner;
    
        // World'e spawn etmek icin register ettigimiz metodu buraya register ediyoruz.
        [Inject] private readonly Func<CellItemModel, CellItem> _cellItemSpawner; 

        protected override void Subscriptions()
        {
            _levelLoadEventSubscriber.Subscribe(OnLevelLoad).AddTo(_bagBuilder);
        }

        private void OnLevelLoad(LevelLoadEvent levelLoadEvent)
        {
            LoadLevel(levelLoadEvent.LevelIndex);
        }
    
        private void LoadLevel(int levelIndex)
        {
            // Belirli bir objenin child'i olarak spawn eder.
            var cellItem = _cellItemWithTransformSpawner.Invoke(_levelContainer.transform, new CellItemModel(15){Position = Vector3.zero});
       
            // World e spawn eder.
            cellItem = _cellItemSpawner.Invoke(new CellItemModel(20){Position = Vector3.one});
        
            _levelReadyEventPublisher.Publish(new LevelReadyEvent(levelIndex));
        
            //TODO: Do something to load level
            //_openMenuEventPublisher.Publish(new OpenMenuEvent(MenuNames.MainMenu, new MainMenuData("Hello World")));   
        }
   }
}
```

> ```_cellItemSpawner.Invoke``` cagirildigi zaman sistem arka tarafta **prefab**'i instantiate eder ve **CellItem**'in ReInitialize methodunu cagirir. Bu method icerisinde istedigimiz ozellikleri ekleyebiliriz.

## SpawnPooling

Yine bir prefabimizdan pooling yapmak istersek, asagidaki gibi bir Pooling Script'imizi yaratabiliriz. [ObjectSpawner](#objectspawner) da oldugu gibi verecegimiz bir transformun icinde chil olarak kullanip kullanmayacagimiza gore iki farkli Pooling Script'i yaratabiliriz.

```csharp
namespace ChefGame.Scripts.Level
{
    // Belirli bir transform icersinde pooling icin kullanilir.
    public class CellItemPoolingWithTransform : BaseSpawnPool<Transform, CellItemModel, CellItem>
    {
        public CellItemPoolingWithTransform(Func<Transform, CellItemModel, CellItem> spawnFunc, int poolSize,
            Transform poolTarget,
            bool willGrow = false) : base(spawnFunc, poolSize, poolTarget, willGrow)
        {
        }
    }

    // World da pooling icin kullanilir.
    public class CellItemPooling : BaseSpawnPool<CellItemModel, CellItem>
    {
        public CellItemPooling(Func<CellItemModel, CellItem> spawnFunc, int poolSize, bool willGrow = false) : base(
            spawnFunc, poolSize, willGrow)
        {
        }
    }
}
```

Ornek olarak ```LevelManager```'da Pooling'i asagidaki gibi uygulayaibliriz.

```csharp
namespace gs.chef.game.managers
{
    public class LevelManager : BaseSubscribable
    {
        [Inject] private readonly ISubscriber<LevelLoadEvent> _levelLoadEventSubscriber;
        [Inject] private readonly IPublisher<LevelReadyEvent> _levelReadyEventPublisher;
    
        // Spawn edecegimiz objelerin parent transform'u
        [Inject] private readonly LevelContainer _levelContainer;
    
        // Belirli bir objenin child'i olarak spawn etmek istersek register ettigimiz metodu buraya inject ediyoruz.
        [Inject] private readonly Func<Transform, CellItemModel, CellItem> _cellItemWithTransformSpawner;
    
        // World'e spawn etmek icin register ettigimiz metodu buraya register ediyoruz.
        [Inject] private readonly Func<CellItemModel, CellItem> _cellItemSpawner; 
    
        private CellItemWithTransformPooling _cellItemWithTransformPooling;
        private CellItemPooling _cellItemPooling;

        protected override void Subscriptions()
        {
            _levelLoadEventSubscriber.Subscribe(OnLevelLoad).AddTo(_bagBuilder);
        }

        private void OnLevelLoad(LevelLoadEvent levelLoadEvent)
        {
            LoadLevel(levelLoadEvent.LevelIndex);
        }
    
        private void LoadLevel(int levelIndex)
        {
            _cellItemWithTransformPooling ??= new CellItemWithTransformPooling(_cellItemWithTransformSpawner, 10, _levelContainer.transform, true);
            _cellItemPooling ??= new CellItemPooling(_cellItemSpawner, 10, true);
        
            _cellItemWithTransformPooling.HideAllObjects();
            _cellItemPooling.HideAllObjects();
    
            for (int i = 0; i < 5; i++)
            {
                var cellItem0 = _cellItemWithTransformPooling.GetItem(new CellItemModel(i){Position = new Vector3(i, 0, 0)});
                var cellItem1 = _cellItemPooling.GetItem(new CellItemModel(i){Position = new Vector3(i, 0, 1)});
            }
        
            _levelReadyEventPublisher.Publish(new LevelReadyEvent(levelIndex));
        
            //TODO: Do something to load level
            //_openMenuEventPublisher.Publish(new OpenMenuEvent(MenuNames.MainMenu, new MainMenuData("Hello World")));   
        }
    }
}
```
## GameProcess

Oyun icinde sirali bir sekilde yapilmasi gereken islemler varsa, bu islemleri **GameProcess** ile yonetebiliriz.

GameProcess sistemini kullanabilmemis icin oncelikle LifeTime Scope icerisinde ```GameProcessProvider```'i register etmemiz gerekmektedir.

```csharp
namespace gs.chef.game.lifetime
{
    public class ChefLifeTimeScope : AbsLifeTimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // GameProcessProvider register edilir.
            builder.Register<GameProcessProvider>(Lifetime.Singleton);
        }
    }
}
```
Diyelimki oyunuzda Sayilarin Toplamini yapacak bir processiniz var ve her sayi toplaminda 1 Frame bekletmek istiyorum. Bu processi asagidaki gibi yaratabiliriz.

```csharp
public struct SumIntProcessArgs : IProcessArgs
{
    public readonly List<int> Numbers;
    
    public SumIntProcessArgs(List<int> numbers)
    {
        Numbers = numbers;
    }
}

public class SumIntProcess : BaseProcess<SumIntProcessArgs>
{
    protected override void Initialize()
    {
    }
    
    public async override UniTask<IGameProcess> AsyncExecute(CancellationToken ctx = default)
    {
        Debug.Log("SumIntProcess Start");
        int sum = 0;
        foreach (var number in Args.Numbers)
        {
            sum += number;
            await UniTask.DelayFrame(1, cancellationToken: ctx).AttachExternalCancellation(cancellationToken);
        }
        Debug.Log($"SumIntProcess End. Sum: {sum}");
        return this;
    }
}
```
Yarattigimiz bu processi LifeTime Scope icerisinde register edelim.

```csharp
namespace gs.chef.game.lifetime
{
    public class ChefLifeTimeScope : AbsLifeTimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // GameProcessProvider register edilir.
            builder.Register<GameProcessProvider>(Lifetime.Singleton);
            // SumIntProcess register edilir.
            builder.RegisterGameProcessFactory<SumIntProcessArgs, SumIntProcess>(Lifetime.Transient);
        }
    }
}
```

Bu processi calistirmak icin asagidaki gibi bir ornek olusturabiliriz.

```csharp

public class LevelManager : IDisposable
{
    [Inject] private readonly Func<SumIntProcessArgs, SumIntProcess> _sumIntProcessFactory;
    
    private CancellationTokenSource _levelCTS;
    
    public void OnLevelCreate()
    {
        _levelCTS = new CancellationTokenSource();
    }
    
    public void DoSomething()
    {
        var numbers = new List<int>{1, 2, 3, 4, 5};
        var sumIntProcess = _sumIntProcessFactory.Invoke(new SumIntProcessArgs(numbers));
        sumIntProcess.Execute(_levelCTS.Token);
    }
    
    public void OnLevelDestroy()
    {
        _levelCTS?.Cancel();
        _levelCTS?.Dispose();
        _levelCTS = null;
    }
    
    public void Dispose()
    {
        OnLevelDestroy();
    }
}
```
Bu ornekte en basit anlamiyla bir processin calistirilmasi gosterilmistir. 
LevelManager icerisinde her level yaratildiginda bir CancellationTokenSource olusturulur ve level yokedildiginde bu token cancel edilir ve dispose edilir. 
DoSomething methodu cagirildiginda SumIntProcess calistirilir. 
Process, icersinde verdigimiz isi yapana kadar arka planda calisir ve islem bittiginde kendisini dispose eder. 
Yarattiginiz her processin ayrica kendine ait bir cancellationToken i vardir.
Oyunun her hangibir aninda bu ornek icin LevelManager Dispose olursa veya level bittiginde veya level destroy edildiginde yarattigimiz bu ```_levelCTS``` cancel edilirse veya telofonunuzda oyunu kapattiginizda bu token lar otomatik olarak dispose olacagi icin processin arka planda calismadigini garanti etmis olursunuz.

Yarattiginiz bir processin complete durumunu handle etmek isterseniz asagidaki orneklerde oldugu gibi yapabilirsiniz.

```csharp
public void DoSomething() {
    var sumIntProcess = _sumIntProcessFactory.Invoke(new SumIntProcessArgs(numbers))
        .OnComplete( process => 
        {    
            Debug.Log("SumIntProcess Complete"); 
        });
}

public void DoSomething() {
    var numbers = new List<int>{1, 2, 3, 4, 5};
    var sumIntProcess = _sumIntProcessFactory.Invoke(new SumIntProcessArgs(numbers))
        .OnComplete(OnCompleteSumIntProcess);
    sumIntProcess.Execute(_levelCTS.Token);
}

private void OnCompleteSumIntProcess(IGameProcess process)
{
    var sumIntProcess = process as SumIntProcess;
}
```

Diyelimki bu processin toplam sonucunu OnComlete de almak istiyoruz. Bu durumda ```SumIntProcess``` icerisinde bir property OnComplete de bu degeri alabiliriz.

```csharp
public class SumIntProcess : BaseProcess<SumIntProcessArgs>
{
    public int Sum { get; private set; }
    
    protected override void Initialize()
    {
    }
    
    public async override UniTask<IGameProcess> AsyncExecute(CancellationToken ctx = default)
    {
        Debug.Log("SumIntProcess Start");
        int sum = 0;
        foreach (var number in Args.Numbers)
        {
            sum += number;
            await UniTask.DelayFrame(1, cancellationToken: ctx).AttachExternalCancellation(cancellationToken);
        }
        Debug.Log($"SumIntProcess End. Sum: {sum}");
        Sum = sum;
        return this;
    }
}

public void DoSomething() {
    var numbers = new List<int>{1, 2, 3, 4, 5};
    var sumIntProcess = _sumIntProcessFactory.Invoke(new SumIntProcessArgs(numbers))
        .OnComplete(OnCompleteSumIntProcess);
    sumIntProcess.Execute(_levelCTS.Token);
}

private void OnCompleteSumIntProcess(IGameProcess process)
{
    var sumIntProcess = process as SumIntProcess;
    Debug.Log($"SumIntProcess Complete. Sum: {sumIntProcess.Sum}");
}
```
* Yaratmis oldugunuz her process birbirinden bagimsiz sekilde calisir. 
* Ayni process'den **N** tane yaratabilirsiniz ve her biri birbirinden bagimsiz olarak calisir.

Bir process'in bitimine ayri bir process'i veya kendisini Append edebilirsiniz. 
Bu durumda Execute ettiginiz process tamamlandiginda kendisine append edilmis processi siraya alip onuda calistirir ve ilk calisan process ona Append ettiginiz ikinci Process'i de icerir ve hepsi bittiginde OnComplete ini alirsiniz.

```csharp
public struct AvarageIntProcessArgs : IProcessArgs
{
    public readonly int Sum;
    public readonly int Count;
    
    public AvarageIntProcessArgs(int sum, int count)
    {
        Sum = sum;
        Count = count;
    }
}

public class AvarageIntProcess : BaseProcess<AvarageIntProcessArgs>
{
    public float Avarage { get; private set; }
    
    protected override void Initialize()
    {
    }
    
    public async override UniTask<IGameProcess> AsyncExecute(CancellationToken ctx = default)
    {
        Avarage = (float)Args.Sum / Args.Count;
        return this;
    }
}


public class SumIntProcess : BaseProcess<SumIntProcessArgs>
{
    [Inject] private readonly Func<AvarageIntProcessArgs, AvarageIntProcess> _avarageIntProcessFactory;
    public int Sum { get; private set; }
    
    protected override void Initialize()
    {
    }
    
    public async override UniTask<IGameProcess> AsyncExecute(CancellationToken ctx = default)
    {
        Debug.Log("SumIntProcess Start");
        int sum = 0;
        foreach (var number in Args.Numbers)
        {
            sum += number;
            await UniTask.DelayFrame(1, cancellationToken: ctx).AttachExternalCancellation(cancellationToken);
        }
        Debug.Log($"SumIntProcess End. Sum: {sum}");
        Sum = sum;
        
        var avarageIntProcess = _avarageIntProcessFactory.Invoke(new AvarageIntProcessArgs(Sum, Args.Numbers.Count));
        Append(avarageIntProcess);
        
        return this;
    }
}
```

Bir process`in icersine **N** tane process ekleyebilirsiniz. Bu durumda ilk process calistirildiginda icersindeki processler sirasiyla calisir ve bitiminde ilk processin OnComplete ini alirsiniz.
Yukaridaki ornek gibi bir processde toplama islemleri bittikten sonra avarage hesaplama islemi yapilir ve bu process de bitince SumIntProcess'in OnComplete ini dinlediginiz yerden alirsiniz. 
Burada ne zaman Append edilmis olduguna dikkat ediniz. Calisma sirasi yukaridan asagiya dogru olacaktir.

DOSomething metodunda Execute etmeden oncede Append edebilirsiniz.

```csharp
public struct MedianIntProcessArgs : IProcessArgs
{
    public readonly List<int> Numbers;
    
    public MedianIntProcessArgs(List<int> numbers)
    {
        Numbers = numbers;
    }
}

public class MedianIntProcess : BaseProcess<MedianIntProcessArgs>
{
    public float Median { get; private set; }
    
    protected override void Initialize()
    {
    }
    
    public async override UniTask<IGameProcess> AsyncExecute(CancellationToken ctx = default)
    {
        Args.Numbers.Sort();
        Median = Args.Numbers[Args.Numbers.Count / 2];
        return this;
    }
}


public void DoSomething() {
    var numbers = new List<int>{1, 2, 3, 4, 5};
    var sumIntProcess = _sumIntProcessFactory.Invoke(new SumIntProcessArgs(numbers))
        .Append(_medianIntProcessFactory.Invoke(new MedianIntProcessArgs(numbers)))
        .OnComplete(OnCompleteSumIntProcess);
    sumIntProcess.Execute(_levelCTS.Token);
}
```

Yukaridaki ornekte ilk olarak SumIntProcess calisir ve bitiminde MedianIntProcess calisir ardindan da SumIntProcess icersinde Append edilmis olan AvarageIntProcess calisir ve en sonunda OnCompleteSumIntProcess calisir.
Kisacasi  Append edilen processler programatik olarak listeye eklenis sirasina gore birbirlerinin bitmesini bekleyerek sirayla calisir.

Processler icersine yapacaginiz ise ozel Tasklar yazarak bunlari ekleyebilirsiniz.

Ornek olarak Tum bu processlerin icerigini Tasklar ile yazalim.

```csharp
public struct SumIntTask : IGameTask<SumIntTask>
{
    public readonly List<int> Numbers;
    public int Sum { get; private set; }
    
    public SumIntTask(List<int> numbers)
    {
        Numbers = numbers;
    }
    
    public async UniTask<SumIntTask> Execute(CancellationToken ctx = default)
    {
        Debug.Log("SumIntTask Start");
        Sum = 0;
        foreach (var number in Numbers)
        {
            Sum += number;
            await UniTask.DelayFrame(1, cancellationToken: ctx).AttachExternalCancellation(cancellationToken);
        }
        
        Debug.Log($"SumIntTask End. Sum: {sum}");
        return this;
    }
}

public struct MedianIntTask : IGameTask<MedianIntTask>
{
    public readonly List<int> Numbers;
    public float Median { get; private set; }
    
    public MedianIntTask(List<int> numbers)
    {
        Numbers = numbers;
    }
    
    public async UniTask<MedianIntTask> Execute(CancellationToken ctx = default)
    {
        Debug.Log("MedianIntTask Start");
        Numbers.Sort();
        Median = Numbers[Numbers.Count / 2];
        Debug.Log($"MedianIntTask End. Median: {Median}");
        return this;
    }
}

public class IntProcess : BaseProcess<IntProcessArgs>
{
    [Inject] private readonly Func<AvarageIntProcessArgs, AvarageIntProcess> _avarageIntProcessFactory;
    public int Sum { get; private set; }
    public float Median { get; private set; }
    
    protected override void Initialize()
    {
    }
    
    public async override UniTask<IGameProcess> AsyncExecute(CancellationToken ctx = default)
    {
        Debug.Log("SumIntProcess Start");
        var sumIntTask = await new SumIntTask(Args.Numbers).Execute(ctx).AttachExternalCancellation(cancellationToken);
        Sum = sumIntTask.Sum;
        
        /*
        var medianIntTask = new MedianIntTask(Args.Numbers);
        await medianIntTask.Execute(ctx).AttachExternalCancellation(cancellationToken);
        Median = medianIntTask.Median;
        */
        
        var medianTask = await new MedianIntTask(Args.Numbers).Execute(ctx).AttachExternalCancellation(cancellationToken);
        Median = medianTask.Median;
        
        var avarageIntProcess = _avarageIntProcessFactory.Invoke(new AvarageIntProcessArgs(Sum, Args.Numbers.Count));
        Append(avarageIntProcess);
        
        return this;
    }
}
```

Yarattiginiz Processler icersinde serbest bir sekilde ```[Inject]``` kullanabilirsiniz.
Bunu tasklar icin yapamazsiniz. Eger Task icersinde Resolve etmek istediginiz bir obje varsa bunu Task'in constructor'inda parametre olarak almaniz gerekmektedir.

```csharp
public struct MedianIntTask : IGameTask<MedianIntTask>
{
    private readonly IObjectResolver _resolver;
    private readonly GameModel _gameModel;
    private readonly IPublisher<LevelReadyEvent> _levelReadyEventPublisher;
    public readonly List<int> Numbers;
    public float Median { get; private set; }
    
    public MedianIntTask(IObjectResolver resolver, List<int> numbers)
    {
        _resolver = resolver;
        _gameModel = _resolver.Resolve<GameModel>();
        _levelReadyEventPublisher = _resolver.Resolve<IPublisher<LevelReadyEvent>>();
        Numbers = numbers;
    }
    
    public async UniTask<MedianIntTask> Execute(CancellationToken ctx = default)
    {
        Debug.Log("MedianIntTask Start");
        Numbers.Sort();
        Median = Numbers[Numbers.Count / 2];
        Debug.Log($"MedianIntTask End. Median: {Median}");
        _levelReadyEventPublisher.Publish(new LevelReadyEvent(_gameModel.CurrentLevel));
        return this;
    }
}
```

