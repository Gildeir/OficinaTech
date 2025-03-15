using OficinaTech.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OficinaTech.Domain.Entities
{
    public class ServiceOrder
    {
        public int Id { get; set; }

        public String Number { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public int OrcamentoId { get; set; }
        public Orcamento Orcamento { get; set; }
        public EStatusServiceOrder StatusServiceOrder { get; set; }
    }
}
