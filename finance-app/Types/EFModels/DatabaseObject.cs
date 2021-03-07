using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace finance_app.Types.EFModels
{
    public class DatabaseObject
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public uint Id { get; set; }

        public DateTime Date_Created { get; set; }
        public DateTime Date_Last_Edited { get; set; }
    }
}
