using System;
using System.Collections.Generic;
using SocialMediaUser.ViewModels;

namespace SocialMediaUser.Services;

public class ViewModelFactory : IViewModelFactory
{
    private readonly Dictionary<Type, Func<BaseViewModel>> _factories;

    public ViewModelFactory(Dictionary<Type, Func<BaseViewModel>> factories)
    {
        _factories = factories;
    }
    
    
    public BaseViewModel Create<T>() where T : BaseViewModel
    {
        if (!_factories.TryGetValue(typeof(T), out var factory))
        {
            throw new ArgumentException("Invalid ViewModel");
        }

        return factory();
    }
}