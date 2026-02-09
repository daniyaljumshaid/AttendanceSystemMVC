

namespace AttendanceSystemMVC.Models // <-- same namespace as your other models
{
    public class ErrorViewModel
    {
        public string ?RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
