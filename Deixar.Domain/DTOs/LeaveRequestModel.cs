namespace Deixar.Domain.DTOs
{
    public class LeaveRequestModel
    {
        public string Reason { get; set; }

        public DateTime LeaveDate { get; set; }

        public bool IsPaidLeave { get; set; }
    }
}
