using System;
using TextCipher.ViewModels;

namespace TextCipher.Factories;

public class TabFactory : ITabFactory
{
    private readonly Func<TabInfoViewModel> _factory;

    public TabFactory(Func<TabInfoViewModel> factory)
    {
        _factory = factory;
    }
    
    public TabInfoViewModel Create()
    {
        return _factory();
    }
}