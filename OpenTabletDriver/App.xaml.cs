using System;
using System.Linq;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using Avalonia.Styling;
using System.Collections.Generic;
using OpenTabletDriver.Windows;

namespace OpenTabletDriver
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override async void OnFrameworkInitializationCompleted()
        {
            Styles.CollectionChanged += (sender, e) => RefreshAllStyles();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Program.UnhandledException);

                desktop.MainWindow = new MainWindow();
                desktop.MainWindow.Show();
                await (desktop.MainWindow as MainWindow).ViewModel.Initialize();
            }
            
            base.OnFrameworkInitializationCompleted();
        }

        private static readonly IStyle DummyStyle = new Style();

        public static void SetTheme(StyleInclude style)
        {
            App.Current.Styles[1] = style;
        }

        public void RefreshAllStyles()
        {
            foreach (var window in ((IClassicDesktopStyleApplicationLifetime)ApplicationLifetime).Windows)
            {
                window.Styles.Add(DummyStyle);
                window.Styles.Remove(DummyStyle);
            }
        }
   }
}