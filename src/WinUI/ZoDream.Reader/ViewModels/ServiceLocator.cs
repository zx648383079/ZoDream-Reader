using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.Reader.ViewModels
{
    /// <summary>
    /// Service locator, used to obtain the container for dependency injection.
    /// </summary>
    public class ServiceLocator
    {
        public ServiceLocator(): this(new ServiceCollection()) {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLocator"/> class.
        /// </summary>
        /// <param name="serviceCollection">Service provider instance.</param>
        public ServiceLocator(IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection;
            Instance = this;
        }

        /// <summary>
        /// Instance of <see cref="ServiceLocator"/>.
        /// </summary>
        public static ServiceLocator Instance { get; private set; }


        private IServiceProvider serviceProvider;
        public IServiceProvider ServiceProvider 
        { 
            get { 
                serviceProvider ??= ServiceCollection.BuildServiceProvider();
                return serviceProvider;
            }
        }

        /// <summary>
        /// Service collection instance.
        /// </summary>
        public IServiceCollection ServiceCollection { get; }

        /// <summary>
        /// Get registered service.
        /// </summary>
        /// <typeparam name="T">Service registration type.</typeparam>
        /// <returns>Service.</returns>
        public T GetService<T>()
            => ServiceProvider != null ? ServiceProvider.GetService<T>() : default;

        /// <summary>
        /// Get registered service.
        /// </summary>
        /// <param name="typeName">Service type name.</param>
        /// <typeparam name="T">Service registration type.</typeparam>
        /// <returns>Service.</returns>
        public T GetService<T>(string typeName)
            => ServiceProvider != null ? ServiceProvider.GetServices<T>().FirstOrDefault(p => p.GetType().Name == typeName) : default;

        /// <summary>
        /// Try to load the service.
        /// </summary>
        /// <typeparam name="T">Service registration type.</typeparam>
        /// <param name="defineService">Definition of need to load the service.</param>
        /// <returns>Whether the loading is successful.</returns>
        public ServiceLocator LoadService<T>(out T defineService)
        {
            if (ServiceProvider == null)
            {
                defineService = default;
            }
            else
            {
                var service = GetService<T>();
                defineService = service;
            }

            return this;
        }
    }
}
