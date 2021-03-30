using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SysDVW.Models.Entities;

namespace SysDVW.Models
{
    public partial class InvSysContext : IdentityDbContext
    {
        public InvSysContext(DbContextOptions<InvSysContext> options) : base(options)
        {
        }

        //Personas
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<UsuarioTable> UserTable { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Empleado> Empleados { get; set; }

        //Comprobantes
        public DbSet<Comprobantes> Comprobante { get; set; }
        public DbSet<ncf> Ncfs { get; set; }
        public DbSet<NCFGenerados> nCFGenerados { get; set; }

        //Caja
        public DbSet<Pagos> Pago { get; set; }
        public DbSet<Gastos> Gastos { get; set; }
        public DbSet<Caja> Cajas { get; set; }
        public DbSet<Cuadre> Cuadre { get; set; }

        //Ventas
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetalleVentas { get; set; }
        public DbSet<Tipo_factura> Tipo_Facturas { get; set; }
        public DbSet<Tipo_trabajo> Tipo_trabajo { get; set; }

        //Productos
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> categorias { get; set; }
        public DbSet<tipoGOma> tipoGOma { get; set; }

        //Empresa
        public DbSet<Licencia> Licencia { get; set; }
        public DbSet<NomEmp> NomEmps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Personas
            modelBuilder.Entity<Cargo>().ToTable("Cargo");
            modelBuilder.Entity<Empleado>().ToTable("Empleado");
            modelBuilder.Entity<UsuarioTable>().ToTable("Usuario");
            modelBuilder.Entity<Cliente>().ToTable("Cliente");

            //Comprobantes
            modelBuilder.Entity<Comprobantes>().ToTable("Comprobantes");
            modelBuilder.Entity<ncf>().ToTable("ncf");
            modelBuilder.Entity<NCFGenerados>().ToTable("NCFGenerados");

            //Caja
            modelBuilder.Entity<Caja>().ToTable("Caja");
            modelBuilder.Entity<Pagos>().ToTable("Pagos");
            modelBuilder.Entity<Gastos>().ToTable("Gastos");
            modelBuilder.Entity<Cuadre>().ToTable("Cuadre");

            //Ventas
            modelBuilder.Entity<Venta>().ToTable("Venta");
            modelBuilder.Entity<DetalleVenta>().ToTable("DetalleVenta");
            modelBuilder.Entity<Tipo_trabajo>().ToTable("Tipo_trabajo");
            modelBuilder.Entity<Tipo_factura>().ToTable("Tipo_factura");

            //Productos
            modelBuilder.Entity<Categoria>().ToTable("Categoria");
            modelBuilder.Entity<Producto>().ToTable("Producto");
            modelBuilder.Entity<tipoGOma>().ToTable("tipoGOma");

            //Empresa
            modelBuilder.Entity<Licencia>().ToTable("Licencia");
            modelBuilder.Entity<NomEmp>().ToTable("NomEmp");
        }
    }
}
