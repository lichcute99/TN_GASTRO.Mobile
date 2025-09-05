using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN_GASTRO.Mobile.Services.Tables
{
    public sealed class DummyTableService : ITableService
    {
        public Task<IReadOnlyList<Table>> GetTablesAsync(string areaId, CancellationToken ct)
        {
            var rnd = new Random();
            var list = new List<Table>();

            for (int i = 1; i <= 30; i++)
            {
                var id = i.ToString("000");
                var occupied = rnd.NextDouble() < 0.45;

                list.Add(new Table
                {
                    Id = id,
                    AreaId = areaId,
                    Status = occupied ? TableStatus.Occupied : TableStatus.Empty,
                    GuestName = occupied ? (rnd.Next(0, 2) == 0 ? "Anna" : "Alan Kin") : null,
                    Amount = occupied ? Math.Round((decimal)rnd.NextDouble() * 60m, 2) : null,
                    Elapsed = occupied ? TimeSpan.FromMinutes(rnd.Next(1, 240)) : null,
                });
            }

            return Task.FromResult<IReadOnlyList<Table>>(list);
        }
    }
}
