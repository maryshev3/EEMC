using EEMC.ViewModels;
using EEMC.Models;
using Microsoft.Extensions.DependencyInjection;
using EEMC.Services;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using System;
using Newtonsoft.Json;
using System.Windows.Documents;
using System.Collections.Generic;
using System.IO;

namespace EEMC
{
    public class ViewModelLocator
    {
        private static ServiceProvider _provider;

        public MainWindowVM MainWindowVM => _provider.GetRequiredService<MainWindowVM>();
        public CourseWindowVM CourseWindowVM => _provider.GetRequiredService<CourseWindowVM>();
        public AddCourseVM AddCourseVM => _provider.GetRequiredService<AddCourseVM>();
        public RenameCourseVM RenameCourseVM => _provider.GetRequiredService<RenameCourseVM>();
        public RemoveCourseVM RemoveCourseVM => _provider.GetRequiredService<RemoveCourseVM>();
        public CoursesListVM CoursesListVM => _provider.GetRequiredService<CoursesListVM>();
        public AddFolderVM AddFolderVM => _provider.GetRequiredService<AddFolderVM>();
        public SwitcherCourseViewVM SwitcherCourseViewVM => _provider.GetRequiredService<SwitcherCourseViewVM>();
        public ThemesWindowVM ThemesWindowVM => _provider.GetRequiredService<ThemesWindowVM>();

        public static void AddVMs(ServiceCollection services)
        {
            services.AddTransient<CourseWindowVM>();
            services.AddSingleton<MainWindowVM>();
            services.AddSingleton<AddCourseVM>();
            services.AddTransient<RenameCourseVM>();
            services.AddTransient<RemoveCourseVM>();
            services.AddTransient<CoursesListVM>();
            services.AddTransient<AddFolderVM>();
            services.AddTransient<SwitcherCourseViewVM>();
            services.AddTransient<ThemesWindowVM>();
        }

        public static void AddTemplates(ServiceCollection services)
        {
            Templates templates = new();

            string json = File.ReadAllText("./templates.json");

            templates.TemplatesList = JsonConvert.DeserializeObject<List<string>>(json);

            services.AddSingleton<Templates>(templates);
        }

        public static void Init() 
        {
            var services = new ServiceCollection();

            AddVMs(services);

            AddTemplates(services);

            services.AddSingleton<Course>();

            services.AddSingleton<IWindowService, WindowService>();

            services.AddSingleton<MessageBus>();

            services.AddSingleton<IServiceProvider>(sp => sp);

            _provider = services.BuildServiceProvider();

            foreach (var service in services) 
            {
                _provider.GetRequiredService(service.ServiceType);
            }
        }
    }
}
