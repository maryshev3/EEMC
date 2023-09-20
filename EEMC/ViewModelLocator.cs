using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EEMC.ViewModels;
using EEMC.Models;
using Microsoft.Extensions.DependencyInjection;
using EEMC.Services;

namespace EEMC
{
    public class ViewModelLocator
    {
        private static ServiceProvider _provider;

        public MainWindowVM MainWindowVM => _provider.GetRequiredService<MainWindowVM>();
        public CourseWindowVM CourseWindowVM => _provider.GetRequiredService<CourseWindowVM>();

        public static void Init() 
        {
            var services = new ServiceCollection();

            services.AddTransient<CourseWindowVM>();
            services.AddSingleton<MainWindowVM>();

            services.AddSingleton<Course>();

            services.AddSingleton<MessageBus>();

            _provider = services.BuildServiceProvider();
            foreach (var service in services) 
            {
                _provider.GetRequiredService(service.ServiceType);
            }
        }
    }
}
