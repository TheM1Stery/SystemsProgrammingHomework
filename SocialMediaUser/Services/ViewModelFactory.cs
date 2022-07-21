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
    
    
    public BaseViewModel Create<TViewModel>() where TViewModel : BaseViewModel
    {
        if (!_factories.TryGetValue(typeof(TViewModel), out var factory))
        {
            throw new ArgumentException("Invalid ViewModel");
        }

        return factory();
    }
}