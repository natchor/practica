//UserRoleModel FindById(int IdRole, int IdUser);
//int Guardar(UserRoleModel rol);
//Task<List<UserRoleModel>> GetAll();
using AutoMapper;
using Dato.Entities;
using Dato.Interfaces.Repositories;
using Entidad.Interfaz.Models.UserRoleModels;
using Microsoft.EntityFrameworkCore;
using Negocio.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Negocio.Services
{
    public class UserRoleService : IUserRoleService
    {

        private readonly IUserRoleRepository _repoUserRole;
        private readonly IMapper _mapper;


        public UserRoleService(IUserRoleRepository userRoleRepository, IMapper mapper)
        {
            _repoUserRole = userRoleRepository;
            _mapper = mapper;

        }

        public async Task<List<UserRoleModel>> GetAll()
        {
            //Task.FromResult(result.ToList());
            var user = await _repoUserRole.Query().ToListAsync();
            var userRoleModel = _mapper.Map<List<UserRoleModel>>(user);

            return userRoleModel;
        }

        public UserRoleModel FindById(int roleId)
        {

            var user = _repoUserRole.Query().Where(u => u.RoleId == roleId).FirstOrDefault();
            var userRoleModel = _mapper.Map<UserRoleModel>(user);

            return userRoleModel;
        }

        public UserRoleModel FindByUserId(int userId)
        {

            var user = _repoUserRole.Query().Where(u => u.UserId == userId)
                .Include(s => s.Role)
                .FirstOrDefault();
            var userRoleModel = _mapper.Map<UserRoleModel>(user);

            return userRoleModel;
        }

        public int Guardar(UserRoleModel userRole)
        {
            int ret = 0;
            var userro = _repoUserRole.Query().FirstOrDefault(e => e.UserId == userRole.UserId);

            try
            {
                if (userro == null)
                {
                    ret = insertar(userRole);
                }
                else
                {
                    ret = editar(userRole);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ret;
        }

        private int editar(UserRoleModel userRole)
        {
            try
            {
                var userro = _repoUserRole.Query().FirstOrDefault(e => e.UserId == userRole.UserId); // busca el registro del rol en función del id

                if(userro == null)
                {
                    userro = new UserRole();
                    userro.UserId = userRole.UserId;
                    _repoUserRole.Update(userro); //agrega el nuevo registro
                }
                _mapper.Map<UserRoleModel, UserRole>(userRole, userro); //Trasfiere los datos de UserRoleModel a UserRole en la bdd
                _repoUserRole.SaveChanges();

                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }

        }

        private int insertar(UserRoleModel UserRole)
        {

            var userR = _mapper.Map<UserRole>(UserRole);
            _repoUserRole.Add(userR);
            _repoUserRole.SaveChanges();

            return userR.UserId;
        }

    }
}


