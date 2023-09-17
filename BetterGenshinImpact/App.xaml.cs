﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Serilog;
using static Vanara.PInvoke.Kernel32;
using System.IO;
using Microsoft.Extensions.Logging;
using OpenCvSharp;
using Vision.Recognition;

namespace BetterGenshinImpact
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Services = ConfigureServices();

            this.InitializeComponent();
        }

        /// <summary>
        /// Gets the current <see cref="App"/> instance in use
        /// </summary>
        public new static App Current => (App)Application.Current;

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
        /// </summary>
        public IServiceProvider Services { get; }

        /// <summary>
        /// Configures the services for the application.W
        /// </summary>
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();


            var logFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"log");
            Directory.CreateDirectory(logFolder);
            var logFile = Path.Combine(logFolder, $"better-genshin-impact.log");

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(path: logFile, outputTemplate: "[{Timestamp:HH:mm:ss.fff}] [{Level:u3}] {SourceContext}{NewLine}{Message}{NewLine}{Exception}{NewLine}", rollingInterval: RollingInterval.Day)
                .WriteTo.RichTextBox(MaskWindow.Instance().LogBox, outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
            services.AddLogging(c => c.AddSerilog());


            //services.AddSingleton<ISettingsService, SettingsService>();

            return services.BuildServiceProvider();
        }

        public static ILogger<T> GetLogger<T>()
        {
            return Current.Services.GetService<ILogger<T>>()!;
        }
    }
}