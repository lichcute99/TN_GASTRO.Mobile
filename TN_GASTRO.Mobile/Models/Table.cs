using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN_GASTRO.Mobile.Models;

public sealed class Table
{
    public string Id { get; set; } = "";         // "001", "101", ...
    public TableStatus Status { get; set; }

    // Thông tin khi có khách
    public string? GuestName { get; set; }       // "Alan Kin"
    public decimal? Amount { get; set; }         // 26.90
    public TimeSpan? Elapsed { get; set; }       // 00:15:30

    // Có thể mở rộng: AreaId, Floor, Flags...
    public string AreaId { get; set; } = "restaurant";
}
