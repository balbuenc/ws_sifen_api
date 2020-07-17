using GoldenGateAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenGateAPI.Data
{
    public interface IUsuarioRepo
    {
        IEnumerable<Usuario> GetAllUsuarios();

        Usuario GetUsuarioByUserName(string UserName);


    }
}
