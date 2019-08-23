using Autofac;
using ChessEngine.Services;
using ChessEngine.Services.Contracts;
using ChessEngine.ViewModels;
using System;
using System.Collections.Generic;
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

            //builder.RegisterType<ChessGridViewModel>().AsSelf();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("ViewModel"))
                .AsSelf();
            //builder.RegisterType<BoardGeneratorService>()
            //    .As<IBoardGeneratorService>();

            Container = builder.Build();
        }
    }
}
