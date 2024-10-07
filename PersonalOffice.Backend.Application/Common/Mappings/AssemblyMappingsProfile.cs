using AutoMapper;
using System.Reflection;

namespace PersonalOffice.Backend.Application.Common.Mappings
{
    /// <summary>
    /// Профиль мапинга
    /// </summary>
    public class AssemblyMappingsProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly">assembly типов мапинга</param>
        public AssemblyMappingsProfile(Assembly assembly)
        {
            ApplyMappingsFromAssembly(assembly);
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(type => type.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapWith<>)))
                .ToList();

            foreach (var type in types)
            {
                var instance  = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping");
                methodInfo?.Invoke(instance, new object[] {this});
            }
        }
    }
}
