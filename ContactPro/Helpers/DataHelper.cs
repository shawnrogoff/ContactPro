﻿using ContactPro.Data;
using Microsoft.EntityFrameworkCore;

namespace ContactPro.Helpers;

public static class DataHelper
{
    public static async Task ManageDataAsync(IServiceProvider svcProvider)
    {
        var dbContextSvc = svcProvider.GetRequiredService<ApplicationDbContext>();

        await dbContextSvc.Database.MigrateAsync();
    }
}
