﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SocialMediaUser.Views;

public partial class RegisterView : UserControl
{
    public RegisterView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}