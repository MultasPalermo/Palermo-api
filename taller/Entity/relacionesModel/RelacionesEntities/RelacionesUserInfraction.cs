using Entity.ConfigurationsBase;
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.relacionesModel.RelacionesEntities
{
    // 6. UserInfraction Configuration
    public class RelacionesUserInfraction : IEntityTypeConfiguration<UserInfraction>
    {
        public void Configure(EntityTypeBuilder<UserInfraction> builder)
        {
            builder.ToTable("userInfraction", schema: "Entities");
            builder.ConfigureBaseModel();
        }
    }
}

