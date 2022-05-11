using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerManagementApi.Data.Models.CompanyWorkerDtos
{
    public class PatchCompanyWorkerDto
    {
        public long IdCompany { get; set; }
        public long IdWorker { get; set; }
    }
}
