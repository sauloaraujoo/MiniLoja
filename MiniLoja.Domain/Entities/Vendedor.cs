﻿using Microsoft.AspNetCore.Identity;

namespace MiniLoja.Domain.Entities
{
    public class Vendedor : Entity
    {
        public string AspnetUserId { get; set; }
        public IdentityUser AspnetUser { get; set; }

    }
}
