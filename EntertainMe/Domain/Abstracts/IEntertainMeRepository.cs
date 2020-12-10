using System;
using System.Collections.Generic;
using System.Text;

using EntertainMe.Domain.ValueObjects;

namespace EntertainMe.Domain.Abstracts
{
    interface IEntertainMeRepository
    {
        MigrationResults MigrateDatabase(bool calledFromAutoUpgrade = false);
    }
}
