using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using T2_Videoteca_RevillaFernandezHectorRolando.Models;

namespace T2_Videoteca_RevillaFernandezHectorRolando.Data
{
    public class T2_Videoteca_RevillaFernandezHectorRolandoContext : DbContext
    {
        public T2_Videoteca_RevillaFernandezHectorRolandoContext (DbContextOptions<T2_Videoteca_RevillaFernandezHectorRolandoContext> options)
            : base(options)
        {
        }

        public DbSet<T2_Videoteca_RevillaFernandezHectorRolando.Models.Video> Video { get; set; } = default!;
    }
}
