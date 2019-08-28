using Autofac;
using ChessEngine.Services;
using ChessEngine.Services.Contracts;
using ChessEngine.ViewModels;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public static class Bootstraper
    {
        public static IContainer Container { get; set; }

        public static void Init()
        {
            var builder = new ContainerBuilder();

            string servicesAssemblyPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ChessEngine.Services.dll");
            Assembly assembly = Assembly.LoadFile(servicesAssemblyPath);

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance(); 

            builder.RegisterAssemblyTypes(assembly)
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("ViewModel"))
                .AsSelf();

            Container = builder.Build();
        }
    }
}
