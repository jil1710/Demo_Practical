using System.ComponentModel.DataAnnotations.Schema;

namespace Deixar.Domain.Entities
{
    public class Leave
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column(TypeName = "nvarchar(200)")]
        public string Reason { get; set; }

        public DateTime LeaveDate { get; set; }

        public bool IsPaidLeave { get; set; }

        public bool IsApproved { get; set; } = false;

        [Column(TypeName = "nvarchar(400)")]
        public string? Notes { get; set; }

        [ForeignKey("CreatedByUser")]
        public int CreatedBy { get; set; }

        [ForeignKey("StatusUpdatedByUser")]
        public int? StatusUpdatedBy { get; set; }

        //Relational property
        public User CreatedByUser { get; set; }
        public User? StatusUpdatedByUser { get; set; }
    }
}
