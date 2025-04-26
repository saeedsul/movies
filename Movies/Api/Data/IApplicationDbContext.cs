using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public interface IApplicationDbContext
    { 
        public DbSet<Movie> Movies { get; set; } 
         
    }
}
