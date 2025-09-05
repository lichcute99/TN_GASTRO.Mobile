using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN_GASTRO.Mobile.Services.Tables
{
    public interface ITableService
    {
        Task<IReadOnlyList<Table>> GetTablesAsync(string areaId, CancellationToken ct);
    }
}
