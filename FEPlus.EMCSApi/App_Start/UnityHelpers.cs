using FEPlus.Contract;
using FEPlus.Contract.EMCS;
using FEPlus.Models;
using FEPlus.Pattern.DataContext;
using FEPlus.Pattern.Factory;
using FEPlus.Pattern.Repositories;
using FEPlus.Pattern.UnitOfWork;
using FEPlus.Service.Pattern;
using FEPlus.Services;
using FEPlus.Services.EMCS;
using FEPlus.Utility.Attributes;
using Microsoft.Owin.Hosting;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace FEPlus.EMCSApi.App_Start
{
    public static class UnityHelpers
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            container
                     .RegisterType<IDataContextAsync, EMCSContext>(new PerResolveLifetimeManager())
                     .RegisterType<IUnitOfWorkAsync, UnitOfWork>(new PerResolveLifetimeManager())

                     .RegisterType<IRepositoryAsync<Equipment>, Repository<Equipment>>()
                     .RegisterType<IRepositoryAsync<Manual>, Repository<Manual>>()
                     .RegisterType<IRepositoryAsync<Method>, Repository<Method>>()
                     .RegisterType<IRepositoryAsync<Equipment>, Repository<Equipment>>()
                     .RegisterType<IRepositoryAsync<Manual>, Repository<Manual>>()
                     .RegisterType<IRepositoryAsync<Method>, Repository<Method>>()
                     .RegisterType<IRepositoryAsync<Plans>, Repository<Plans>>()
                     .RegisterType<IRepositoryAsync<PlanTimeJob>, Repository<PlanTimeJob>>()
                     .RegisterType<IRepositoryAsync<PlanTimeJob_Items>, Repository<PlanTimeJob_Items>>()
                     .RegisterType<IRepositoryAsync<Profile>, Repository<Profile>>()
                     .RegisterType<IRepositoryAsync<Requisition>, Repository<Requisition>>()

                     //Bussiess for Admin
                     .RegisterType<IEmployeeService, EmployeeService>()
                     //Bussiness Class Dependency 
                     .RegisterType<IEquipmentService, EquipmentService>()
                     .RegisterType<IPlanScheduleService, PlanScheduleService>()
                     .RegisterType<IVoucherService, VoucherService>()
                     .RegisterType<IGateCheckerService, GateCheckerService>();



            RegisterTypes(container);
            return container;
        });

        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        //private static readonly Type[] EmptyTypes = new Type[0];

        public static IEnumerable<Type> GetTypesWithCustomAttribute<T>(Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.GetCustomAttributes(typeof(T), true).Length > 0)
                    {
                        yield return type;
                    }
                }
            }
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            var myAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith("FEPlus")).ToArray();

            container.RegisterType(typeof(Startup));

            container.RegisterTypes(
                UnityHelpers.GetTypesWithCustomAttribute<UnityIoCContainerControlledAttribute>(myAssemblies),
                WithMappings.FromMatchingInterface,
                WithName.Default,
                WithLifetime.ContainerControlled,
                null
                ).RegisterTypes(
                         UnityHelpers.GetTypesWithCustomAttribute<UnityIoCTransientLifetimeAttribute>(myAssemblies),
                         WithMappings.FromMatchingInterface,
                         WithName.Default,
                         WithLifetime.Transient);

        }
    }
}
