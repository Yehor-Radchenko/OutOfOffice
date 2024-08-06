namespace OutOfOffice.BlazorUI.Components.Other
{
    public static class StatusCssHelper
    {
        public static string GetStatusCssClass(string approveStatus)
        {
            return approveStatus switch
            {
                "New" => "new-status",
                "Pending" => "pending-status",
                "Submitted" => "submitted-status",
                "Canceled" => "canceled-status",
                "Approved" => "approved-status",
                "Rejected" => "rejected-status",
                _ => "unknown-status",
            };
        }
    }
}
