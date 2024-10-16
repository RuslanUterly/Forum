using Application.DTOs.Topics;
using Mapster;
using Microsoft.Data.SqlClient;
using Persistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings;

public class RegisterMapper
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<Topic, TopicDetailsRequest>()
            .Map(dest => dest.Tags, src => src.ThreadTags.Select(tt => tt.Tag))
            .RequireDestinationMemberSource(true);

    }
}
