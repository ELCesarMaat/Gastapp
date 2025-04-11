using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GastappApi;

public partial class GastappDbContext : IdentityDbContext
{

    public GastappDbContext(DbContextOptions options)
        : base(options)
    {
    }
}
