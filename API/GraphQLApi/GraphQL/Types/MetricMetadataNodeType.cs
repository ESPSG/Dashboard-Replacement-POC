using GraphQL.Types;
using GraphQLApi.Contracts;
using GraphQLApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLApi.GraphQL.Types
{
    public class MetricMetadataNodeType : ObjectGraphType<MetricMetadataNode>
    {
        public MetricMetadataNodeType()
        {
            Field(x => x.MetricId);
            Field(x => x.Name);
            Field(x => x.Url);
            Field(x => x.DomainEntityType);
            Field(x => x.Description);
            Field(x => x.DisplayName);
            Field(x => x.DisplayOrder);
            Field(x => x.Enabled);
            Field(x => x.MetricTypeId);
            Field(x => x.ShortName);
            Field(x => x.Tooltip);
            Field<ListGraphType<MetricMetadataNodeType>>(
            "Children",
                resolve: context => 
                {
                    var metadataNode = context.Source as MetricMetadataNode;
                    return metadataNode.Children;
                }
             );
        }
    }
}
