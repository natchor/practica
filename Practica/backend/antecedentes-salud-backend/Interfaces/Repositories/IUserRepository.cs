﻿using antecedentes_salud_backend.Models;
using Dato.Interfaces.Repositories;

namespace antecedentes_salud_backend.Interfaces.Repositories
{
    public interface IUserRepository : IRepository < User, int>
    {
    }
}
