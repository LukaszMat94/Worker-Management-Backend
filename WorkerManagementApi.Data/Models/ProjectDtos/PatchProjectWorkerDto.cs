﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerManagementApi.Data.Models.ProjectDtos
{
    public class PatchProjectWorkerDto
    {
        public long IdProject { get; set; }
        public long IdWorker { get; set; }
    }
}
