using Microsoft.AspNetCore.Identity;
using SysDVW.Models.Entities;
using SysDVW.Utilities;

namespace SysDVW.ViewModels
{
    public class User : IdentityUser<string>
    {
        #region Members

        bool _IsEnabled;

        #endregion


        #region Propeties

        public Cliente Clientes { get; set; }
        public Empleado Empleados { get; set; }
        public UsuarioTable Usuario { get; set; }
        public bool changeUser { get; set; }
        //public virtual ICollection<UserRole> Roles { get; } = new List<UserRole>();


        //public virtual ICollection<Claim> Claims { get; } = new List<Claim>();


        //public virtual ICollection<Login> Logins { get; } = new List<Login>();



        #endregion


        public User()
        {
            Id = IdentityGenerator.NewSequentialGuid().ToString();
            Clientes = new Cliente();
            Empleados = new Empleado();
            Usuario = new UsuarioTable();
        }


        public bool IsEnabled
        {
            get
            {
                return _IsEnabled;
            }
            private set
            {
                _IsEnabled = value;
            }
        }

        #region Public Methods


        /// <summary>
        /// Disable entity
        /// </summary>
        public void Disable()
        {
            if (IsEnabled)
                this._IsEnabled = false;
        }

        /// <summary>
        /// Enable entity
        /// </summary>
        public void Enable()
        {
            if (!IsEnabled)
                this._IsEnabled = true;
        }

        #endregion
    }
}
