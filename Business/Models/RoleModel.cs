using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
    public class RoleModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }
}
