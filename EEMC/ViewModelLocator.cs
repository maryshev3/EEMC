﻿using EEMC.ViewModels;
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
using EEMC.ToXPSConverteres;

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
        public AddThemeVM AddThemeVM => _provider.GetRequiredService<AddThemeVM>();
        public RenameThemeVM RenameThemeVM => _provider.GetRequiredService<RenameThemeVM>();
        public ChangeDescriptionThemeVM ChangeDescriptionThemeVM => _provider.GetRequiredService<ChangeDescriptionThemeVM>();
        public RemoveThemeVM RemoveThemeVM => _provider.GetRequiredService<RemoveThemeVM>();
        public DocumentViewVM DocumentViewVM => _provider.GetRequiredService<DocumentViewVM>();
        public VideoViewVM VideoViewVM => _provider.GetRequiredService<VideoViewVM>();
        public ImageViewVM ImageViewVM => _provider.GetRequiredService<ImageViewVM>();
        public PdfViewVM PdfViewVM => _provider.GetRequiredService<PdfViewVM>();
        public ExportWindowVM ExportWindowVM => _provider.GetRequiredService<ExportWindowVM>();
        public VersionsViewVM VersionsViewVM => _provider.GetRequiredService<VersionsViewVM>();
        public CreateTestVM CreateTestVM => _provider.GetRequiredService<CreateTestVM>();
        public EnterTestNameVM EnterTestNameVM => _provider.GetRequiredService<EnterTestNameVM>();
        public TestViewVM TestViewVM => _provider.GetRequiredService<TestViewVM>();
        public EnterTotalTestNameVM EnterTotalTestNameVM => _provider.GetRequiredService<EnterTotalTestNameVM>();
        public AddTotalTestVM AddTotalTestVM => _provider.GetRequiredService<AddTotalTestVM>();
        public EditTotalTestVM EditTotalTestVM => _provider.GetRequiredService<EditTotalTestVM>();

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
            services.AddTransient<AddThemeVM>();
            services.AddTransient<RenameThemeVM>();
            services.AddTransient<ChangeDescriptionThemeVM>();
            services.AddTransient<RemoveThemeVM>();
            services.AddTransient<DocumentViewVM>();
            services.AddTransient<VideoViewVM>();
            services.AddTransient<ImageViewVM>();
            services.AddTransient<PdfViewVM>();
            services.AddTransient<ExportWindowVM>();
            services.AddTransient<VersionsViewVM>();
            services.AddTransient<CreateTestVM>();
            services.AddTransient<EnterTestNameVM>();
            services.AddTransient<TestViewVM>();
            services.AddTransient<EnterTotalTestNameVM>();
            services.AddTransient<AddTotalTestVM>();
            services.AddTransient<EditTotalTestVM>();
        }

        public static void AddTemplates(ServiceCollection services)
        {
            Templates templates = new();

            string json = File.ReadAllText("./templates.json");

            templates.TemplatesList = JsonConvert.DeserializeObject<List<string>>(json);

            services.AddSingleton<Templates>(templates);
        }

        public static void AddConverteres(ServiceCollection services)
        {
            services.AddSingleton<PptConverter>();
            services.AddSingleton<TxtConverter>();
            services.AddSingleton<WordConverter>();

            services.AddSingleton<ConverterUtils>();
        }

        public static void Init() 
        {
            var services = new ServiceCollection();

            AddVMs(services);

            AddTemplates(services);

            AddConverteres(services);

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
