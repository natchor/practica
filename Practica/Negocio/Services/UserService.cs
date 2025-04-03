using AutoMapper;
using Dato.Entities;
using Dato.Interfaces.Repositories;
using Entidad.Interfaz;
using Entidad.Interfaz.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using Negocio.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Negocio.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _repoUser;
        private readonly IUserRoleRepository _repoUserRole;
        private readonly IMapper _mapper;


        public UserService(IUserRepository userRepository, IUserRoleRepository userRoleRepository, IMapper mapper)
        {
            _repoUser = userRepository;
            _repoUserRole = userRoleRepository;
            _mapper = mapper;

        }

        public async Task<List<UserTablaModel>> GetAllUser()
        {
            //Task.FromResult(result.ToList());
            var user = await _repoUser.Query()
                .Include(e => e.Cargo)
                .Include(u => u.Sector)
                .ToListAsync();
            var userModel = _mapper.Map<List<UserTablaModel>>(user);

            return userModel;
        }

        public UserModel GetByUserId(int Id)
        {

            var user = _repoUser.Query().Where(e => e.Id == Id).Include(u => u.Cargo).FirstOrDefault();
            var userModel = _mapper.Map<UserModel>(user);

            return userModel;
        }

        public async Task<List<SelectUserModel>> GetForSelect()
        {
            //Task.FromResult(result.ToList());
            var user = await _repoUser.Query().Where(e => e.Estado == true).OrderBy(e => e.Nombre).ToListAsync(); //.ThenBy(e => e.Apellido)
            var userModel = _mapper.Map<List<SelectUserModel>>(user);

            return userModel;
        }

        public string ValidarUsuario(string mail)
        {
            var user = _repoUser.Query().Where(u => u.Email == mail).FirstOrDefault();
            string mensaje = (user == null) ? "nuevo" : "Usuario ya existe";
            //var userModel = _mapper.Map<UserModel>(user);

            return mensaje;
        }

        public UserModel Login(string userName, string password)
        {
            var user = _repoUser.Query().Where(u => u.UserName == userName && u.Password == password && u.Estado)
                .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .Include(u => u.Sector)
                .Include(u => u.Cargo).FirstOrDefault();

            var userModel = _mapper.Map<UserModel>(user);

            return userModel;
        }



        public int Guardar(UserModel User) //guarda información de UserModel en User de la base de datos
        {
            int ret = 0;

            try
            {
                if (User.Id == 0)
                {
                    ret = insertar(User);
                }
                else
                {
                    ret = editar(User);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ret;
        }

        private int editar(UserModel user) //trae la información que se desea editar en la variable user de userModel
        {

            var mon = _repoUser.Query().FirstOrDefault(e => e.Id == user.Id); //trae la información del usuario de la bdd y la deja en la variable mon

            if (mon.UserName != user.UserName)
            {
                user.UserName = mon.UserName; //intercambia y reemplaza el username de la base de datos en user 
                _mapper.Map<UserModel, User>(user, mon); //Enlaza información ingresada en UserModel y va hacia User en la Base de datos
                _repoUser.Update(mon);
                _repoUser.SaveChanges();

            }

            else
            {
                _mapper.Map<UserModel, User>(user, mon); //Enlaza información ingresada en UserModel y va hacia User en la Base de datos
                _repoUser.Update(mon);
                _repoUser.SaveChanges();
            }

            return user.Id;
        }

        private int insertar(UserModel User)
        {

            var solic = _mapper.Map<User>(User);
            _repoUser.Add(solic);
            _repoUser.SaveChanges();

            return solic.Id;
        }

        public UserModel ExistUser(string userName)
        {
            var user = _repoUser.Query().Where(u => u.UserName == userName && u.Estado)
                .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .Include(u => u.Sector)
                .Include(u => u.Cargo)
                .FirstOrDefault();

            var userModel = _mapper.Map<UserModel>(user);

            return userModel;
        }

        public async Task<List<UserModel>> GetByUserCompra()
        {
            //    // TODO: 3 y 229 corresponden a Tecnico	DEPARTAMENTO DE ADMINISTRACIÓN en ambiente QA, podria ser una configuracion por BBDD que indique los ids o que corresponden a usuarios de compra
            //    var user = await _repoUser.Query().Where(e => e.CargoId == 3 && e.SectorId == 229 ).ToListAsync();
            var user = await _repoUserRole.Query().Where(ur => ur.User.Estado == true && (ur.RoleId == Roles.AnalistaCompra || ur.RoleId == Roles.AsignadorSolicitud))
                .Include(ur => ur.User)
                .Select(ur => ur.User).ToListAsync();
            var userModel = _mapper.Map<List<UserModel>>(user);

            return userModel;
        }

        public async Task<List<UserModel>> GetPresupuestoUserIds()
        {
            var user = await _repoUser.Query().Where(e => e.CargoId == 2 && e.SectorId == 231 && e.Estado == true).ToListAsync();
            var userModel = _mapper.Map<List<UserModel>>(user);

            return userModel;
        }

        public async Task<bool> EsPresupuesto(int userId)
        {
            var user = await _repoUser.Query().Where(e => (e.CargoId == 2 || e.CargoId == 14) && e.SectorId == 231 && e.Estado == true)
                .Select(u => u.Id).ToListAsync();


            return user.Exists(id => id == userId);
        }

        public int? BuscaJefe(int? sectorId)
        {
            var user = _repoUser.Query().Where(e => (e.CargoId == 10 || e.CargoId == 14 || e.CargoId == 5) && e.SectorId == sectorId && e.Estado == true)
                        .FirstOrDefault();

            var userModel = _mapper.Map<UserModel>(user);

            return userModel.Id;
        }


    }
}
