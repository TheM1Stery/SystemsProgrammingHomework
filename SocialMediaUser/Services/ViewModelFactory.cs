using System;
using System.Collections.Generic;
using SocialMediaUser.ViewModels;

namespace SocialMediaUser.Services;

public class ViewModelFactory<TBase> : IViewModelFactory<TBase>
{
    private readonly Dictionary<Type, Func<TBase>> _factories;

    public ViewModelFactory(Dictionary<Type, Func<TBase>> factories)
    {
        _factories = factories;
    }
    
    
    public TBase Create<TViewModel>() where TViewModel : TBase
    {
        if (!_factories.TryGetValue(typeof(TViewModel), out var factory))
        {
            throw new ArgumentException("Invalid ViewModel");
        }

        return factory();
    }
}