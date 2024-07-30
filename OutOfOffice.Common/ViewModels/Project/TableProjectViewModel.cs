﻿using OutOfOffice.Common.ViewModels.Employee;

namespace OutOfOffice.Common.ViewModels.Project
{
    public class TableProjectViewModel
    {
        public int Id { get; set; }

        public string ProjectType { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public BriefEmployeeViewModel ProjectManager { get; set; } = null!;

        public string Comment { get; set; } = null!;

        public string Status { get; set; } = null!;
    }
}
