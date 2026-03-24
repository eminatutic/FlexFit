using FlexFit.Domain.Interfaces.Repositories;
using FlexFit.Infrastructure.Data;
using Neo4j.Driver;

namespace FlexFit.Infrastructure.Repositories
{
    public class MemberGraphRepository : IMemberGraphRepository
    {
        private readonly Neo4jContext _context;

        public MemberGraphRepository(Neo4jContext context)
        {
            _context = context;
        }

        public async Task RecordVisitAsync(string memberId, int fitnessObjectId, string memberName = null, string objectName = null)
        {
            using var session = _context.GetSession();
            await session.ExecuteWriteAsync(async tx =>
            {
                await tx.RunAsync(@"
                    MERGE (m:Member {id: $mId})
                    ON CREATE SET m.name = $mName
                    MERGE (o:FitnessObject {id: $oId})
                    ON CREATE SET o.name = $oName
                    MERGE (m)-[v:VISITED]->(o)
                    ON CREATE SET v.count = 1, v.lastVisit = datetime()
                    ON MATCH SET v.count = v.count + 1, v.lastVisit = datetime()", 
                    new { mId = memberId, mName = memberName, oId = fitnessObjectId.ToString(), oName = objectName });
            });
        }

        public async Task RecordReservationAsync(string memberId, int resourceId, string memberName = null, string resourceType = null)
        {
            using var session = _context.GetSession();
            await session.ExecuteWriteAsync(async tx =>
            {
                await tx.RunAsync(@"
                    MERGE (m:Member {id: $mId})
                    ON CREATE SET m.name = $mName
                    MERGE (r:Resource {id: $rId})
                    ON CREATE SET r.type = $rType
                    MERGE (m)-[:RESERVED]->(r)", 
                    new { mId = memberId, mName = memberName, rId = resourceId.ToString(), rType = resourceType });
            });
        }

        public async Task<IEnumerable<string>> GetRecommendedObjectsAsync(string memberId)
        {
            // NEW RECOMMENDATION: Find objects that have similar resources to the ones this member uses most
            using var session = _context.GetSession();
            return await session.ExecuteReadAsync(async tx =>
            {
                var result = await tx.RunAsync(@"
                    MATCH (me:Member {id: $id})-[:RESERVED]->(r:Resource)
                    WITH me, r.type AS favType, count(r) AS usageCount
                    ORDER BY usageCount DESC LIMIT 1
                    MATCH (otherO:FitnessObject)<-[:BELONGS_TO]-(otherR:Resource {type: favType})
                    WHERE NOT (me)-[:VISITED]->(otherO)
                    RETURN DISTINCT otherO.name AS name", 
                    new { id = memberId });

                var list = new List<string>();
                while (await result.FetchAsync())
                {
                    list.Add(result.Current["name"].As<string>());
                }
                return list;
            });
        }
    }
}
