namespace OutOfOffice.BLL.ViewModels.Project
{
    public class TableProjectViewModel
    {
        public int Id { get; set; }

        public string ProjectType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int ProjectManagerId { get; set; }

        public string Comment { get; set; } = null!;

        public string Status { get; set; }
    }
}
