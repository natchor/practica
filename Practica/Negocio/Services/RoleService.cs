using AutoMapper;
using Dato.Entities;
using Dato.Respositories;
using Entidad.Interfaz.Models.RoleModels;
using Microsoft.EntityFrameworkCore;
using Negocio.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Negocio.Services
{
    public class RoleService : IRoleService
    {

        private readonly IRoleRepository _repoRole;
        private readonly IMapper _mapper;


        public RoleService(IRoleRepository roleRepository, IMapper mapper)
        {
            _repoRole = roleRepository;
            _mapper = mapper;

        }

        public async Task<List<RoleModel>> GetAll()
        {
            //Task.FromResult(result.ToList());
            var role = await _repoRole.Query().ToListAsync();
            var RoleModel = _mapper.Map<List<RoleModel>>(role);

            return RoleModel;
        }

        public RoleModel FindById(int Id)
        {

            var Role = _repoRole.Query().Where(e => e.Id == Id).FirstOrDefault();
            var RoleModel = _mapper.Map<RoleModel>(Role);

            return RoleModel;
        }

        public async Task<List<SelectRoleModel>> GetForSelect()
        {
            //Task.FromResult(result.ToList());
            var Role = await _repoRole.Query().ToListAsync();
            var RoleModel = _mapper.Map<List<SelectRoleModel>>(Role);

            return RoleModel;
        }

        public int Guardar(RoleModel Role)
        {
            int ret = 0;

            try
            {
                if (Role.Id == 0)
                {
                    ret = insertar(Role);
                }
                else
                {
                    ret = editar(Role);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ret;
        }

        private int editar(RoleModel role)
        {

            var rol = _repoRole.Query().FirstOrDefault(e => e.Id == role.Id);

            _mapper.Map<RoleModel, Role>(role, rol);

            _repoRole.Update(rol);
            _repoRole.SaveChanges();

            return role.Id;

        }

        private int insertar(RoleModel Role)
        {

            var solic = _mapper.Map<Role>(Role);
            _repoRole.Add(solic);
            _repoRole.SaveChanges();

            return solic.Id;
        }

    }
}


//RoleModel FindById(int Id);
//int Guardar(RoleModel rol);
//Task<List<SelectRoleModel>> GetForSelect();
//Task<List<RoleModel>> GetAll();