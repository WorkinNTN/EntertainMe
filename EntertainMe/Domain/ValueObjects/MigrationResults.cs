﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EntertainMe.Domain.ValueObjects
{
    public class MigrationResults
    {
        public bool Success { get; set; }
        public bool MigrationOccurred { get; set; }
        public string Message { get; set; }

        public MigrationResults()
        {
            Success = true;
            MigrationOccurred = false;
            Message = "";
        }
    }
}
