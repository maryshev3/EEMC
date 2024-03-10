using EEMC.ViewModels;
using EEMC.Models;
using Microsoft.Extensions.DependencyInjection;
using EEMC.Services;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;

namespace EEMC
{
    public class ViewModelLocator
    {
        private static ServiceProvider _provider;

        public MainWindowVM MainWindowVM => _provider.GetRequiredService<MainWindowVM>();
        public CourseWindowVM CourseWindowVM => _provider.GetRequiredService<CourseWindowVM>();
        public AddCourseVM AddCourseVM => _provider.GetRequiredService<AddCourseVM>();

        public static void Init() 
        {
            var services = new ServiceCollection();

            services.AddTransient<CourseWindowVM>();
            services.AddSingleton<MainWindowVM>();
            services.AddSingleton<AddCourseVM>();

            services.AddSingleton<Course>();

            services.AddSingleton<IWindowService, WindowService>();

            services.AddSingleton<MessageBus>();

            _provider = services.BuildServiceProvider();

            foreach (var service in services) 
            {
                _provider.GetRequiredService(service.ServiceType);
            }
        }
    }
}
