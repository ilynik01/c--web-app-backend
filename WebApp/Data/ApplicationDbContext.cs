﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    // Specify entities here
    // Better to add them here to easily find them
    public DbSet<Caesar> Caesars { get; set; }
    public DbSet<Vigenere> Vigeneres { get; set; }

    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    
}