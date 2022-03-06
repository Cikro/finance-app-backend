using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace finance_app.Types.Repositories
{
    public interface IUserIdResource
    {
        public uint User_Id { get; set; }
    }
}
