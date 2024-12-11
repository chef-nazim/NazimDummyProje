using System;

namespace gs.chef.vcontainer.menu
{
    public interface IMenu : IDisposable
    {
        void SetActive(bool active);
        
        bool ActiveSelf { get; }
        
        bool DestroyWhenClosed { get; }
        void InitializeView();
        
        void UpdateView();
        
        void Open();
        
        void Close();

        void Dispose(bool disposing);

    }
}