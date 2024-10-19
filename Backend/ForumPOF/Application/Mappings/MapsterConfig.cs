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

public class MapsterConfig
{
    public MapsterConfig()
    {
        TypeAdapterConfig<Topic, TopicDetailsRequest>
            .NewConfig()
            .Map(dest => dest.Tags, src => src.ThreadTags.Select(tt => tt.Tag))
            .RequireDestinationMemberSource(true);
    }
}
