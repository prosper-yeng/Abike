using System;
using Microsoft.EntityFrameworkCore;
using Abike.Model;

namespace Abike.Data;

public class ApplicationDBContext: DbContext
 {
      public ApplicationDBContext(DbContextOptions dbContextOptions)
       :base(dbContextOptions)
       {
        
       } 
       public DbSet<ServiceType>ServiceTypes{get;set;}
       public DbSet<BikeBrand>BikeBrands{get;set;}
       public DbSet<OrderService>OrderServices{get;set;}
    }

