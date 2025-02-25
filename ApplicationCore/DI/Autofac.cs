using Autofac;
using Autofac.Core.Activators.Reflection;
using System.Reflection;
using ApplicationCore.DataAccess.Identity;
using ApplicationCore.DataAccess.IT;
using ApplicationCore.DataAccess.Doc3;

namespace ApplicationCore.DI;

public class ApplicationCoreModule : Autofac.Module
{
   protected override void Load(ContainerBuilder builder)
   {
      builder.RegisterGeneric(typeof(IdentityRepository<>)).As(typeof(IIdentityRepository<>)).InstancePerLifetimeScope();
      builder.RegisterGeneric(typeof(ITContextRepository<>)).As(typeof(IITContextRepository<>)).InstancePerLifetimeScope();
      builder.RegisterGeneric(typeof(Doc3ContextRepository<>)).As(typeof(IDoc3ContextRepository<>)).InstancePerLifetimeScope();

      builder.RegisterAssemblyTypes(GetAssemblyByName("ApplicationCore"))
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();
   }

   public static Assembly GetAssemblyByName(String AssemblyName) => Assembly.Load(AssemblyName);

}



public class InternalConstructorFinder : IConstructorFinder
{
   public ConstructorInfo[] FindConstructors(Type targetType)
         => targetType.GetTypeInfo().DeclaredConstructors.Where(c => !c.IsPrivate && !c.IsPublic).ToArray();
}
