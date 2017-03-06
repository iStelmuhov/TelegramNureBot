using System;
using System.Linq;
using System.Reflection;
using Dependencies.Aspects;
using Dependencies.Logging;
using Microsoft.Practices.EnterpriseLibrary.Validation.PolicyInjection;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Repository.EF;
using Syn.Bot.Oscova;
using Telegram.Bot;


namespace Dependencies
{
    public static class ContainerBoostraper
    {
        public static void RegisterTypes(IUnityContainer container, NureBotDbContext dbContext,string telegramTolken)
        {
            Console.WriteLine("Configuring dependencies container...");
            container.AddNewExtension<Interception>();

            container.RegisterInstance<NureBotDbContext>(dbContext);

            RegisterLogFacilities(container);
            RegisterServices(container);
            RegisterRepositories(container);

            container.RegisterInstance<OscovaBot>(new OscovaBot());
            container.RegisterInstance<TelegramBotClient>(new TelegramBotClient(telegramTolken));
            
            PrintContainerDebuggingInfo( container );
            Console.WriteLine();
        }

        private static void RegisterLogFacilities(IUnityContainer container)
        {
            container.RegisterInstance<BotEventSource>(new BotEventSource());
            container.RegisterType(
                typeof(LogListener),
                new ContainerControlledLifetimeManager()
            );

            var logListener = container.Resolve<LogListener>();
            logListener.OnStartup();
        }

        private static void RegisterServices(IUnityContainer container)
        {
            container.Configure<Interception>()
                .AddPolicy("ValidationPolicy")
                    .AddMatchingRule<NamespaceMatchingRule>(
                        new InjectionConstructor("Service.Impl", true)
                    )
                    .AddCallHandler(
                        new ValidationCallHandler("", SpecificationSource.Both)
                    )
                    ;

            container.RegisterTypes(
                AllClasses.FromAssemblies(
                    new Assembly[] {
                        Assembly.Load( "Service.Impl" ),
                    }
                ),
                WithMappings.FromMatchingInterface,
                WithName.Default,
                WithLifetime.ContainerControlled,
                getInjectionMembers: t => new InjectionMember[]
                {
                    new Interceptor< InterfaceInterceptor >(),
                    new InterceptionBehavior< ExceptionInterceptionBehavior >(),
                    new InterceptionBehavior< SemanticLoggingInterceptionBehavior >(),
                    new InterceptionBehavior< TransactionInterceptionBehavior< NureBotDbContext > >(),
                    new InterceptionBehavior< PolicyInjectionBehavior >( "ValidationPolicy" )
                }
            );

            container.RegisterTypes(
                AllClasses.FromAssemblies(
                    new Assembly[] {
                        Assembly.Load( "TelegramNureBot" )
                    }
                ),
                WithMappings.FromMatchingInterface,
                WithName.Default,
                WithLifetime.ContainerControlled,
                getInjectionMembers: t => new InjectionMember[]
                {
                    new Interceptor< InterfaceInterceptor >(),
                    new InterceptionBehavior< ExceptionInterceptionBehavior >(),
                    new InterceptionBehavior< SemanticLoggingInterceptionBehavior >(),
                    new InterceptionBehavior< PolicyInjectionBehavior >( "ValidationPolicy" )
                }
            );
        }

        private static void RegisterRepositories(IUnityContainer container)
        {
            container.RegisterTypes(
                AllClasses.FromAssemblies(
                    new Assembly[] {
                        Assembly.Load( "Repository.EF" )
                    }
                ).Where(t => t != typeof(NureBotDbContext)),

                WithMappings.FromMatchingInterface,
                WithName.Default,
                WithLifetime.ContainerControlled
            );

        }

        private static void PrintContainerDebuggingInfo(IUnityContainer container)
        {
            Console.WriteLine("Container has {0} Registrations:", container.Registrations.Count());
            foreach (ContainerRegistration item in container.Registrations)
                System.Console.WriteLine(item.GetMappingAsString());
        }

        private static string GetMappingAsString(this ContainerRegistration registration)
        {
            string regName, regType, mapTo, lifetime;

            var r = registration.RegisteredType;
            regType = r.Name + GetGenericArgumentsList(r);

            var m = registration.MappedToType;
            mapTo = m.Name + GetGenericArgumentsList(m);

            regName = registration.Name ?? "[default]";

            lifetime = registration.LifetimeManagerType.Name;
            if (mapTo != regType)
                mapTo = " -> " + mapTo;
            else
                mapTo = string.Empty;

            lifetime = lifetime.Substring(0, lifetime.Length - "LifetimeManager".Length);
            return String.Format("+ {0}{1}  '{2}'  {3}", regType, mapTo, regName, lifetime);
        }

        private static string GetGenericArgumentsList(Type type)
        {
            if (type.GetGenericArguments().Length == 0)
                return string.Empty;

            string arglist = string.Empty;
            bool first = true;
            foreach (Type t in type.GetGenericArguments())
            {
                arglist += first ? t.Name : ", " + t.Name;
                first = false;
                if (t.GetGenericArguments().Length > 0)
                    arglist += GetGenericArgumentsList(t);
            }
            return "<" + arglist + ">";
        }
    }
}
