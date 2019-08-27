﻿namespace ChessEngine
{
    using Autofac;
    using ChessEngine.Services.BoardGenerator;
    using ChessEngine.Services.BoardGenerator.Contracts;
    using ChessEngine.Services.Checkmate;
    using ChessEngine.Services.Checkmate.Contracts;
    using ChessEngine.Services.Engine;
    using ChessEngine.Services.Engine.Contracts;
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    public static class Bootstraper
    {
        public static IContainer Container { get; set; }

        public static void Init()
        {
            var builder = new ContainerBuilder();
            string servicesAssemblyPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ChessEngine.Services.dll");
            Assembly assembly = Assembly.LoadFile(servicesAssemblyPath);

            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("ViewModel"))
                .AsSelf();

            Container = builder.Build();
        }
    }
}
